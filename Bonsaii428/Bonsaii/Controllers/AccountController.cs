using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Bonsaii.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq.Expressions;
using System.Collections.Generic;
using Bonsaii.Models.App;
using HXComm;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using JpushApiExample;
using BonsaiiModels;
using System.Data.Entity;

namespace Bonsaii.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        public const string url_url = "http://211.149.199.42:88/files/";//http://192.168.0.19:8888图片的访问地址。
        public const string appClientID = "YXA6MfhwoG6mEeWbT0ef-BFVfw";
        public const string appClientSecret = "YXA6GUEzoaBTvze7xX3RmlJnF1wMtZA";
        public const string appName = "fuckaholic";
        public const string orgName = "fuckaholic";
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public VerifyCodeTool verifyCode { get; set; }


        public AccountController()
        {
            verifyCode = new VerifyCodeTool();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #region Web端用户管理代码区，好几百行呢！没事别瞎点开
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (SystemDbContext con = new SystemDbContext())
            {
                ApplicationUser user = UserManager.FindByName(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "用户名或密码错误！请重新登录");
                    return View(model);
                }
                if (!user.IsProved)
                {
                    ModelState.AddModelError("", "您所属的企业尚未通过审核。请与系统管理员联系。");
                    return View(model);
                }
                if (!user.IsAvailable)
                {
                    ModelState.AddModelError("", "抱歉，您已经被企业管理员禁用！请与企业管理员联系");
                    return View(model);
                }
            }

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = UserManager.FindByName(model.UserName);
                    //设置session
                    Session["ConnectionString"] = user.ConnectionString;
                    Session["CompanyId"] = user.CompanyId;
                    Session["UserName"] = user.UserName;
                    Session["CompanyFullName"] = user.CompanyFullName;
                    Session["IsProved"] = user.IsProved;

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "无效的登录尝试。");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Province = "北京";
            ViewBag.City = "东城区";
            SystemDbContext db = new SystemDbContext();
            List<SelectListItem> dimension = db.Dimensions.ToList().Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DimensionName
            }).ToList();
            ViewBag.dimensions = dimension;
            return View();
        }
         public JsonResult GenerateVerifyCode(string PhoneNumber)
        {
            //获取验证码成功
            int rst = verifyCode.sendVerifyCode(PhoneNumber);
            if (rst > 0)
                return this.packageJson(1, "获取验证码成功", null);
            else
                return this.packageJson(0, "验证码发送失败！错入代码=" + rst, null);
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            //由于省份和城市是由前台js事件控制的，没有利用MVC的Model Bind，所以要手动写入以前的状态
            ViewBag.Province = model.Province;
            ViewBag.City = model.City;
            SystemDbContext db = new SystemDbContext();
            List<SelectListItem> dimension = db.Dimensions.ToList().Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DimensionName
            }).ToList();
            ViewBag.dimensions = dimension;
            if (model.cbox == false)
            {
                ModelState.AddModelError("", "要创建高维度帐户，您必须同意网站的系统协议。");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                //首先核对用户的短信验证码是否合法
                using (var vCode = new SystemDbContext())
                {
                    var CurrentUserCode = vCode.VerifyCodes.Find(model.UserName);
                    DateTime CurTime = System.DateTime.Now;
                    if (CurTime > CurrentUserCode.OverTime)     //用户短信验证码超时
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码已经过期！");
                        return View(model);
                    }
                    else if (!CurrentUserCode.Code.Equals(model.Code))
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码输入错误！");
                        return View(model);
                    }
                }

                //验证企业全称是否已经被注册
                using (var vUser = new SystemDbContext())
                {
                    var tmp = vUser.Users.Where(p => p.CompanyFullName.Equals(model.CompanyFullName)).ToList();
                    if (tmp.Count != 0)
                    {
                        ModelState.AddModelError("", "抱歉，该企业全称已经被注册！");
                        return View(model);

                    }
                }
                var user = new ApplicationUser
                {
                    CompanyFullName = model.CompanyFullName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    Name = "系统管理员",   //用户注册的时候写入该名称
                    IsProved = false,           //是否审核的标志
                    IsAvailable = true,         //是否是可用的管理员
                    IsRoot = true                //注册企业号的人默认就是企业的超级管理员
                };
                //生成企业ID号
                user.CompanyId = Generate.GenerateCompanyId();
                string CompanyDbName = "Bonsaii" + user.CompanyId;
                user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var tmpUser = UserManager.FindByName(user.UserName);
                    //注册用户的权限就是超级管理员
                    UserManager.AddToRole(tmpUser.Id, "Admin");
                    //添加注册的企业信息到Companies数据表当中
                    using (SystemDbContext sys = new SystemDbContext())
                    {
                        Company company = new Company()
                        {
                            CompanyId = user.CompanyId,
                            FullName = user.CompanyFullName,
                            Address = model.Province + model.City + model.AddressDetail,
                            PersonNumber = model.PersonNumber,
                            TelNumber = user.PhoneNumber,
                            UserName = user.UserName,
                            RecordTime = DateTime.Now,
                            RecordPerson = user.CompanyFullName
                        };
                        sys.Companies.Add(company);
                        sys.SaveChanges();
                    }
                    /**
                     * 注册成功并不会为企业创建独有的数据库，只有系统平台的超级管理员通过相应用户的审核之后才会为用户创建数据库
                     * */
                    //        await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false); 
                    // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                    // 发送包含此链接的电子邮件
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户");
                    return RedirectToAction("RegisterSuccess", "Account");
                }
                AddErrors(result);
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单

            return View(model);
        }


        public ActionResult RegisterSuccess()
        {
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /*提交成功的页面*/
        [AllowAnonymous]
        public ActionResult ForgotPasswordInfo()
        {
            return View();
        }
        /*提交失败的页面*/
        [AllowAnonymous]
        public ActionResult ForgotPasswordInfoError()
        {
            return View();
        }

        /*忘记密码申请：跳转到找回密码页面之前需要验证身份*/
        // GET: /Account/ForgotPasswordInfo
        [AllowAnonymous]
        public ActionResult ForgotPasswordApply()
        {
            UserPasswordInfo view = new UserPasswordInfo();
            return View(view);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordApply(UserPasswordInfo info, HttpPostedFileBase image)
        {
            SystemDbContext db = new SystemDbContext();
            if (ModelState.IsValid)
            {
                info.AuditStatus = 0;
                db.UserPasswordInfos.Add(info);
                if (image != null)
                {
                    info.BusinessLicenseType = image.ContentType;//获取图片类型
                    info.BusinessLicense = new byte[image.ContentLength];
                    image.InputStream.Read(info.BusinessLicense, 0, image.ContentLength);
                }
                return RedirectToAction("ForgotPasswordInfo");
            }
            return View(info);
        }
        /*获取图片*/
        public FileContentResult GetImage()
        {
            //  SystemDbContext db = new SystemDbContext();
            UserPasswordInfo info = new UserPasswordInfo();
            if (info != null)
            {
                return File(info.BusinessLicense, info.BusinessLicenseType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }



        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //首先核对用户的短信验证码是否合法
                using (SystemDbContext vCode = new SystemDbContext())
                {
                    var CurrentUserCode = vCode.VerifyCodes.Find(model.PhoneNumber);
                    DateTime CurTime = System.DateTime.Now;
                    if (CurTime > CurrentUserCode.OverTime)     //用户短信验证码超时
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码已经过期！");
                        return View(model);
                    }
                    else if (!CurrentUserCode.Code.Equals(model.Code))
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码输入错误！");
                        return View(model);
                    }
                }


                ApplicationUser user = UserManager.FindByName(model.PhoneNumber);
                if (user == null)
                {
                    ModelState.AddModelError("", "用户不存在！");
                    return View(model);
                }
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = UserManager.ResetPassword(user.Id, code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }


                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }
          //显示用户协议
        public ActionResult Protocal()
        {
            return View();
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // 请不要显示该用户不存在
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        public ActionResult ModifyPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ModifyPassword(ModifyPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SystemDbContext con = new SystemDbContext())
            {
                ApplicationUser user = UserManager.FindByName(model.UserName);
                var result = UserManager.ChangePassword(user.Id, model.Password, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("ModifyPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码错误！");
                    return View(model);
                }
            }
        }

        public ActionResult ModifyPasswordConfirmation()
        {
            return View();
        }
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpGet]
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion


        #region APP端用户管理代码区
        /// <summary>
        /// 获取手机验证码（在目前测试阶段，发送的所有验证码都是1984）
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public JsonResult AppSendCode(string PhoneNumber)
        {
            //获取验证码成功
            int rst = verifyCode.sendVerifyCode(PhoneNumber);
            if (rst > 0)
                return this.packageJson(1, "获取验证码成功", new { Code = verifyCode.Code });
            else if (rst == -97899)
                return this.packageJson(0, "验证码获取失败。该手机号已经注册！", null);
            else
                return this.packageJson(0, "验证码发送失败！错入代码=" + rst, null);
        }
        public JsonResult NotLogin()
        {
            return this.packageJson(-1, "请先登录！", null);
        }

        /// <summary>
        /// APP调用的注册函数
        /// </summary>
        /// <param name="UserName">用户名，现在是用手机号</param>
        /// <param name="Password">密码</param>
        /// <param name="NickName">用户昵称，这个主要用来注册环信用</param>
        /// <param name="Code">手机验证码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult AppRegister(string UserName, string Password, string NickName, string Code)
        {
            SystemDbContext db = new SystemDbContext();

            //首先核对用户的短信验证码是否合法
            using (var vCode = new SystemDbContext())
            {
                var CurrentUserCode = vCode.VerifyCodes.Find(UserName);
                DateTime CurTime = System.DateTime.Now;
                if (CurTime > CurrentUserCode.OverTime)     //用户短信验证码超时
                {
                    ModelState.AddModelError("", "抱歉，您的验证码已经过期！");
                    return this.packageJson(0, "验证码已过期!", null);
                }
                else if (!CurrentUserCode.Code.Equals(Code))
                    return this.packageJson(0, "验证码错误！", null);
            }
            UserModels usermodel = (from um in db.Users where um.UserName == UserName select um).FirstOrDefault();
            if (usermodel != null)//当注册用户存在时，只修改密码
            {
                //ApplicationUser user = UserManager.FindByName(UserName);
                //var code = UserManager.GeneratePasswordResetToken(user.Id);
                //var result1 = UserManager.ResetPassword(user.Id, code, Password);
                //if (result1.Succeeded && usermodel.HuanTag != true)
                //{
                //    //在环信中注册
                //    EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
                //    string MobResult = myEaseMobDemo.AccountCreate(UserName, Password, NickName);
                //    usermodel.HuanTag = true;
                //    db.SaveChanges();
                //    return packageJson(1, "注册成功！", null);
                //}
                return packageJson(0, "Web系统中注册用户已经存在！如果你是本人，可以通过找回密码方式，注册用户和绑定公司", null);
            }
            else
            {
                ApplicationUser user = new ApplicationUser
                {
                    CompanyFullName = "GeneralStaff",//model.CompanyFullName,
                    PhoneNumber = UserName,
                    UserName = UserName,
                    NickName = NickName,
                    Name = "",   //用户注册的时候写入该名称
                    IsProved = false,           //是否审核的标志
                    IsAvailable = false,         //是否是可用的管理员
                    IsRoot = false                //注册企业号的人默认就是企业的超级管理员
                };
                //填写CompanyId，由于新注册的用户没有绑定企业，所以填写一个默认值
                user.CompanyId = "app-id";
                user.ConnectionString = "app-ConnectionString";

                var result = UserManager.Create(user, Password);
                if (result.Succeeded)
                {
                    UserModels usermodel1 = (from um in db.Users where um.UserName == UserName select um).FirstOrDefault();
                    usermodel1.HuanTag = true;

                    //在环信中注册
                    EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
                    string MobResult = myEaseMobDemo.AccountCreate(UserName, Password, NickName);
                    db.SaveChanges();

                    //JPushClient client = new JPushClient(JPushApiExample.app_key, JPushApiExample.master_secret);
                    //PushPayload payload = JPushApiExample.PushObject_new_user(UserName);//选择一种方式
                    //try
                    //{
                    //    var result1 = client.SendPush(payload);//推送


                    //}
                    //catch (APIRequestException e)//处理请求异常
                    //{
                    //    var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode + "Error Message: " + e.ErrorCode;
                    //    return Json(message, JsonRequestBehavior.AllowGet);
                    //}
                    //catch (APIConnectionException e)//处理连接异常
                    //{
                    //    return Json(e.Message, JsonRequestBehavior.AllowGet);
                    //}
                    return packageJson(1, "注册成功！", MobResult);
                }
                return packageJson(0, "Web系统中注册用户失败！", null);
            }
        }

        [AllowAnonymous]
        public JsonResult AppLogin(string UserName, string Password)
        {
            SystemDbContext con = new SystemDbContext();

            ApplicationUser user = UserManager.FindByName(UserName);

            if (user == null)
                return packageJson(0, "该用户不存在！", null);

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = SignInManager.PasswordSignIn(UserName, Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Failure:
                    return packageJson(0, "用户名或密码错误！", null);
            }
            UserModels user1 = (from sd in con.Users where sd.UserName == UserName select sd).FirstOrDefault();
            string user_url = null;
            if (user1.Head != null)
            {
                user_url = Picture(user1.Head, user.UserName);//头像的地址
            }
            //if (user1.HuanTag != true)
            //{
            //    //在环信中注册
            //    EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
            //    string MobResult = myEaseMobDemo.AccountCreate(UserName, Password, UserName);
            //    user1.HuanTag = true;
            //    con.SaveChanges();
            //}
            BindCode code = (from b in sdb.BindCodes where b.BindingCode == user1.BindingCode && b.BindTag == true && b.IsAvail == true select b).FirstOrDefault();
            if (code != null)//账户是否绑定公司
            {
                code.LastTime = DateTime.Now;
                sdb.SaveChanges();
                Company company = sdb.Companies.Find(user.CompanyId);
                BonsaiiDbContext db = new BonsaiiDbContext(user1.ConnectionString);
                Staff staff = (from s in db.Staffs where s.StaffNumber == user1.StaffNumber select s).FirstOrDefault();
                Department department = (from d in db.Departments where d.DepartmentId == staff.Department select d).FirstOrDefault();
                int id = Convert.ToInt16(company.PersonNumber);
                Dimension dimen = sdb.Dimensions.Find(id);
                return Json(new
                {
                    result = 2,
                    msg = "绑定公司",
                    company = new
                    {
                        companyId = company.CompanyId,
                        name = company.FullName,
                        telephone = company.TelNumber,
                        email = company.Email,
                        address = company.Address,
                        url = company.Url,
                        companyPersonNumber = dimen.DimensionName
                    },
                    personal = new
                    {
                        nickname = user1.NickName,
                        headUrl = user_url,
                        gender = user1.Gender,
                        address = user1.Address,
                        strict = staff.NativePlace,
                        personal = user1.Personal,
                        email = user1.Email
                    },
                    staff = new { staffnumber = staff.StaffNumber, department = department.Name, position = staff.Position, inoffice = "在职" }
                }, JsonRequestBehavior.AllowGet);//绑定公司
                // return packageJson(2, "登录成功", null);
            }
            else
            {
                return Json(new
                {
                    result = 1,
                    msg = "没有绑定公司",
                    personal = new
                    {
                        nickname = user1.NickName,
                        headUrl = user_url,
                        gender = user1.Gender,
                        address = user1.Address,
                        strict = user1.Strict,
                        personal = user1.Personal,
                        email = user1.Email
                    }
                }, JsonRequestBehavior.AllowGet);//没有绑定公司
                // return packageJson(1, "登录成功", null);
            }
        }
        //找回账户密码
        [AllowAnonymous]
        public JsonResult AppLookPassword(string userName, string password, string code)
        {
            SystemDbContext db = new SystemDbContext();
            SystemDbContext db1 = new SystemDbContext();
            UserModels usermodel = (from um in db.Users where um.UserName == userName select um).FirstOrDefault();//找到原来用户，解除绑定的信息和环信好友
            if (usermodel == null)//当用户不存在时
            {
                return this.backJson(false, "用户不存在，无法找回密码！请注册", null);
            }
            //首先核对用户的短信验证码是否合法
            using (var vCode = new SystemDbContext())
            {
                var CurrentUserCode = vCode.VerifyCodes.Find(userName);
                DateTime CurTime = System.DateTime.Now;
                if (CurTime > CurrentUserCode.OverTime)     //用户短信验证码超时
                {
                    ModelState.AddModelError("", "抱歉，您的验证码已经过期！");
                    return this.backJson(false, "验证码已过期!", null);
                }
                else if (!CurrentUserCode.Code.Equals(code))
                    return this.backJson(false, "验证码错误！", null);

            }

            ApplicationUser user = UserManager.FindByName(userName);
            if (user.IsRoot == true || user.IsAvailable == true || user.IsProved == true)//用户是企业超级管理员，就需要返回错误。
            {
                return this.backJson(false, "账户不能找回密码，请联系客服！", null);
            }
            var code1 = UserManager.GeneratePasswordResetToken(user.Id);
            var result1 = UserManager.ResetPassword(user.Id, code1, password);
            if (result1.Succeeded)
            {
                //在环信中注册
                EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
                string friends = myEaseMobDemo.AccountLookFriends(userName);
                JObject jo = (JObject)JsonConvert.DeserializeObject(friends);//解析Json数据
                string tt = jo["data"].ToString();//得到key对应的数据
                var ttt = JArray.Parse(tt);//对数据进行解析
                foreach (var temp in ttt)//取出每个字段
                {
                    myEaseMobDemo.AccountDelFriend(userName, temp.ToString());//删除原来账户的环信好友。
                }
                myEaseMobDemo.AccountResetPwd(userName, password);//环信用户设置当前密码
                // string MobResult = myEaseMobDemo.AccountCreate(userName, password1, userName);//创建环信用户
                usermodel.CompanyFullName = "GeneralStaff";//model.CompanyFullName,
                usermodel.PhoneNumber = userName;
                usermodel.UserName = userName;
                usermodel.NickName = userName;
                usermodel.Name = "";   //用户注册的时候写入该名称
                usermodel.IsProved = false;           //是否审核的标志
                usermodel.IsAvailable = false;         //是否是可用的管理员
                usermodel.IsRoot = false;               //注册企业号的人默认就是企业的超级管理员
                usermodel.CompanyId = "app-id";
                usermodel.ConnectionString = "app-ConnectionString";
                usermodel.HuanTag = true;
                usermodel.BindTag = false;//找回密码后就没有绑定公司
                usermodel.StaffNumber = null;
                usermodel.BindingCode = null;
                usermodel.Head = null;
                usermodel.HeadType = null;
                usermodel.Address = null;
                usermodel.Gender = null;
                usermodel.Strict = null;
                usermodel.Personal = null;

                db.SaveChanges();

                BindCode bind = (from b in db1.BindCodes where b.Phone == userName select b).FirstOrDefault();
                bind.BindTag = false;
                bind.BindingCode = GetBindCode();
                db1.Entry(bind).State = EntityState.Modified;
                db1.SaveChanges();
                return backJson(true, "账户找回密码成功！", null);
            }
            return this.backJson(false, "验证码错误！", null);
        }
        //个人信息设置
        public JsonResult PersonalInfoSet(string userName, HttpPostedFileBase photo, string nickname, string address, string gender, string strict, string personal, string email)
        {
            UserModels user = (from u in sdb.Users where u.UserName == userName select u).FirstOrDefault();
            // App_User app_user = sdb.App_Users.Find(userName);
            if (user != null)
            {
                if (photo != null)
                {
                    user.HeadType = photo.ContentType;
                    user.Head = new byte[photo.ContentLength];
                    photo.InputStream.Read(user.Head, 0, photo.ContentLength);
                }
                if (nickname == null)
                {
                    user.NickName = nickname;
                }
                else
                    user.NickName = nickname.Trim();
                if (address == null)
                {
                    user.Address = address;
                }
                else
                    user.Address = address.Trim();
                if (gender == null)
                {
                    user.Gender = gender;
                }
                else
                    user.Gender = gender.Trim();
                if (strict == null)
                {
                    user.Strict = strict;
                }
                else
                {
                    user.Strict = strict.Trim();
                }
                if (personal == null)
                {
                    user.Personal = personal;
                }
                else
                    user.Personal = personal.Trim();
                if (email == null)
                {
                    user.Email = email;
                }
                else
                    user.Email = email.Trim();

                //user.NickName = (nickname + " ").Trim();
                //user.Address = (address + " ").Trim();
                //user.Gender = (gender + " ").Trim();
                //user.Strict = (strict + " ").Trim();
                //user.Personal = (personal + " ").Trim();
                //user.Email = (email + " ").Trim();


                // app_user.Personal = personal;
                sdb.SaveChanges();
                return Json(new { result = true, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            { return Json(new { result = false, msg = "修改失败" }, JsonRequestBehavior.AllowGet); }
        }
        //修改账户密码(与环信账户同步)

        [AllowAnonymous]

        public JsonResult AppResetPassword(string userName, string password)
        {
            UserModels usermodel = (from um in sdb.Users where um.UserName == userName select um).FirstOrDefault();

            if (usermodel != null)//当注册用户存在时，只修改密码
            {
                ApplicationUser user = UserManager.FindByName(userName);
                var code = UserManager.GeneratePasswordResetToken(user.Id);
                var result1 = UserManager.ResetPassword(user.Id, code, password);
                if (result1.Succeeded)
                {
                    EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
                    myEaseMobDemo.AccountResetPwd(userName, password);
                    return Json(new { result = true, msg = "密码修改成功！" });
                }
                return Json(new { result = false, msg = "密码修改失败！" });
            }
            return Json(new { result = false, msg = "密码修改失败！" });
        }
        /// <summary>
        /// 封装接口调用要返回的Json对象
        /// </summary>
        /// <param name="result">结果值,0代表请求失败，1是成功，-1表示APP用户还没有登录</param>
        /// <param name="msg">执行的结果信息</param>
        /// <param name="data">执行的结果数据</param>
        /// <returns></returns>
        public JsonResult packageJson(int result, string msg, object data)
        {
            return Json(new
            {
                Result = result,
                Msg = msg,
                Data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult backJson(bool result, string msg, object data)
        {
            return Json(new
            {
                Result = result,
                Msg = msg,
                Data = data
            }, JsonRequestBehavior.AllowGet);
        }
        SystemDbContext sdb = new SystemDbContext();
        //获取绑定码
        public JsonResult AppBindCode(string userName)
        {

            UserModels user = (from sd in sdb.Users where sd.UserName == userName select sd).FirstOrDefault();
            if (user != null && user.BindingCode != null && user.CompanyId != "app-id")
            {

                return Json(new
                {
                    result = true,
                    bindCode = user.BindingCode
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = false,
                    msg = "用户没有在相关公司入职，无法绑定公司",

                }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 加密随机数生成器 生成随机种子
        /// </summary>
        /// <returns></returns>
        static int GetRandomSeed()
        {

            byte[] bytes = new byte[4];

            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();

            rng.GetBytes(bytes);

            return BitConverter.ToInt32(bytes, 0);

        }
        public string GetRandomCode(int numlen)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            string code = string.Empty;

            for (int i = 0; i < numlen; i++)
            {
                //这里是关键，传入一个seed参数即可保证生成的随机数不同            
                //Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
                Random rnd = new Random(GetRandomSeed());
                code += chars[rnd.Next(0, 61)].ToString();
            }

            return code;
        }
        public string GetBindCode()
        {
            string temp;
            int i = 0;
            do
            {
                temp = GetRandomCode(8);
                var count = (from s in sdb.BindCodes where s.BindingCode == temp select s).ToList();
                i = count.Count;
            } while (i != 0);
            return temp;
        }
        //用户自己解除绑定公司
        public JsonResult UnbindCompany(string userName, string companyId)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new
                {
                    result = false,
                    msg = "传入参数错误,绑定验证错误",

                }, JsonRequestBehavior.AllowGet);
            }

            BindCode code = (from b in sdb.BindCodes where b.CompanyId == companyId && b.StaffNumber == user.StaffNumber select b).FirstOrDefault();
            if (code == null)
            {
                return Json(new
                {
                    result = false,
                    msg = "传入参数错误,绑定码",

                }, JsonRequestBehavior.AllowGet);
            }
            code.BindTag = false;
            code.BindingCode = GetBindCode();
            sdb.SaveChanges();
            var friends = (from u in sdb.Users where u.CompanyId == companyId && u.UserName != userName select u).ToList();//获取该用户的企业好友。

            //在环信中注册
            EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);

            foreach (var temp1 in friends)//取出每个字段
            {
                myEaseMobDemo.AccountDelFriend(userName, temp1.UserName);//删除原来账户的环信好友。
            }
            user.CompanyFullName = "GeneralStaff";//model.CompanyFullName,

            user.Name = "";   //用户注册的时候写入该名称
            user.IsProved = false;           //是否审核的标志
            user.IsAvailable = false;         //是否是可用的管理员
            user.IsRoot = false;               //注册企业号的人默认就是企业的超级管理员
            user.CompanyId = "app-id";
            user.ConnectionString = "app-ConnectionString";
            user.HuanTag = true;
            user.BindTag = false;//找回密码后就没有绑定公司
            user.StaffNumber = null;
            user.BindingCode = null;
            sdb.SaveChanges();
            return Json(new { result = true, message = "解绑成功！" });
        }
        //用户和相应公司绑定
        public JsonResult AppBind(string userName, string bindCode)
        {
            UserModels user = (from sd in sdb.Users where sd.UserName == userName select sd).FirstOrDefault();
            BindCode code = (from c in sdb.BindCodes where c.BindingCode == bindCode select c).FirstOrDefault();
            if (code == null || user == null)
            {
                return Json(new
                {
                    result = false,
                    msg = "传入参数错误,绑定验证错误",

                }, JsonRequestBehavior.AllowGet);
            }
            EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);

            Company company = (from c in sdb.Companies where c.CompanyId == code.CompanyId select c).FirstOrDefault();
            user.CompanyId = code.CompanyId;
            user.ConnectionString = code.ConnectionString;
            user.CompanyFullName = company.FullName;
            user.StaffNumber = code.StaffNumber;
            user.Name = code.RealName;
            user.BindingCode = code.BindingCode;

            sdb.SaveChanges();

            var friends = (from u in sdb.Users where u.CompanyId == user.CompanyId && u.UserName != userName && u.IsRoot != true select u).ToList();//&&u.IsAvailable!=true&&u.IsProved!=true
            foreach (var temp in friends)
            {
                myEaseMobDemo.AccountAdd(userName, temp.UserName);
            }
            code.Phone = userName;
            code.BindTag = true;
            code.IsAvail = true;
            code.LastTime = DateTime.Now;
            sdb.SaveChanges();
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            Staff staff = (from s in db.Staffs where s.StaffNumber == user.StaffNumber select s).FirstOrDefault();
            Department department = (from d in db.Departments where d.DepartmentId == staff.Department select d).FirstOrDefault();
            // Company company = sdb.Companies.Find(user.CompanyId);
            int id = Convert.ToInt16(company.PersonNumber);
            Dimension dimen = sdb.Dimensions.Find(id);
            JPushClient client = new JPushClient(CompanyPushAPI.app_key, CompanyPushAPI.master_secret);
            PushPayload payload = CompanyPushAPI.PushObject_new_user(userName, company.FullName);//选择一种方式
            try
            {
                var result1 = client.SendPush(payload);//推送


            }
            catch (APIRequestException e)//处理请求异常
            {
                var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode + "Error Message: " + e.ErrorCode;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (APIConnectionException e)//处理连接异常
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = true,
                count = friends.Count,
                msg = "绑定公司成功",
                company = new
                {
                    companyId = company.CompanyId,
                    name = company.FullName,
                    telephone = company.TelNumber,
                    email = company.Email,
                    address = company.Address,
                    url = company.Url,
                    companyPersonNumber = dimen.DimensionName
                },
                staff = new { staffnumber = staff.StaffNumber, department = department.Name, position = staff.Position, inoffice = "在职" }
            }, JsonRequestBehavior.AllowGet);




        }
        //创建企业提供人数
        public JsonResult AppCompanyNumber()
        {
            var dimen = (from d in sdb.Dimensions select d).ToList();
            return Json(new { data = dimen }, JsonRequestBehavior.AllowGet);
        }
        //创建企业(App创建企业,可以登录平台和App？)
        public JsonResult AppCreateCompany(string userName, string name, string address, string phone, string personNumber)
        {

            UserModels user = (from u in sdb.Users where u.UserName == userName select u).First();
            var usercount = (from u in sdb.Users where u.UserName == name select u).ToList();
            if (usercount.Count != 0)
            {
                return Json(new { result = false, message = "抱歉，该企业全称已经被注册！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (user.CompanyId.Equals("app-id"))
                {
                    //user.CompanyFullName = name;
                    //user.PhoneNumber = phone;
                    //user.UserName = account;
                    //user.IsProved = false;
                    //user.IsAvailable = true;
                    //user.IsRoot = true;
                    //user.Name = "系统管理员";
                    ////生成企业ID号
                    //user.CompanyId = Generate.GenerateCompanyId();
                    //string CompanyDbName = "Bonsaii" + user.CompanyId;
                    //user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                    //sdb.SaveChanges();


                    user.CompanyFullName = name;
                    user.PhoneNumber = phone;
                    user.UserName = userName;
                    user.Name = "系统管理员";   //用户注册的时候写入该名称
                    user.IsProved = false;           //是否审核的标志
                    user.IsAvailable = true;         //是否是可用的管理员
                    user.IsRoot = true;             //注册企业号的人默认就是企业的超级管理员
                    //BindTag=true

                    //生成企业ID号
                    user.CompanyId = Generate.GenerateCompanyId();
                    string CompanyDbName = "Bonsaii" + user.CompanyId;
                    user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                    sdb.SaveChanges();
                    //var result = await UserManager.CreateAsync(user1, model.Password);

                    var tmpUser = UserManager.FindByName(user.UserName);
                    //注册用户的权限就是超级管理员
                    UserManager.AddToRole(tmpUser.Id, "Admin");
                    //添加注册的企业信息到Companies数据表当中
                    using (SystemDbContext sys = new SystemDbContext())
                    {
                        Company company = new Company()
                        {
                            CompanyId = user.CompanyId,
                            FullName = user.CompanyFullName,
                            Address = address,
                            PersonNumber = personNumber,
                            TelNumber = user.UserName,
                            UserName = user.UserName,
                            RecordTime = DateTime.Now,
                            RecordPerson = user.CompanyFullName
                        };
                        sys.Companies.Add(company);
                        sys.SaveChanges();
                    }


                    return Json(new { result = true, message = "创造企业成功！" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = false, message = "必须是未绑定企业的用户！" }, JsonRequestBehavior.AllowGet);
            }

        }
        //获取好友信息
        public JsonResult AppFriend(string jsonText)
        {
            if (jsonText != null)
            {
                //JArray ja = (JArray)JsonConvert.DeserializeObject(jsonText);
                var tt = JArray.Parse(jsonText);
                List<object> list = new List<object>();
                UserModels user;
                string temp;
                foreach (var temp1 in tt)
                {
                    string head_url = null;
                    temp = temp1.ToString();
                    user = (from u in sdb.Users where u.UserName == temp && u.IsRoot != true select u).FirstOrDefault();//&& u.IsAvailable != true && u.IsProved != true
                    if (user == null)
                    {
                        continue;
                    }
                    BindCode code = (from b in sdb.BindCodes where b.BindingCode == user.BindingCode && b.BindTag == true select b).FirstOrDefault();
                    BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);

                    if (user != null && user.Head != null)
                    {
                        head_url = Picture(user.Head, user.UserName);
                        if (code != null)
                        {
                            list.Add(new { head_url = head_url, gender = user.Gender, nickname = user.NickName, username = user.UserName, companyid = user.CompanyId, personal = user.Personal, email = user.Email, realName = user.Name });
                        }
                        else
                        {
                            list.Add(new { head_url = head_url, gender = user.Gender, nickname = user.NickName, username = user.UserName, companyid = "app-id", personal = user.Personal, email = user.Email, realName = user.Name });

                        }
                    }
                    else if (user != null && user.Head == null)
                    {
                        if (code != null)
                        {
                            list.Add(new { head_url = head_url, gender = user.Gender, nickname = user.NickName, username = user.UserName, companyid = user.CompanyId, personal = user.Personal, email = user.Email, realName = user.Name });
                        }
                        else
                        {
                            list.Add(new { head_url = head_url, gender = user.Gender, nickname = user.NickName, username = user.UserName, companyid = "app-id", personal = user.Personal, email = user.Email, realName = user.Name });

                        }
                    }

                }
                return Json(new { result = true, data = list, msg = "好友列表" }, JsonRequestBehavior.AllowGet);

            }
            else
                return Json(new { result = false, msg = "没有好友" }, JsonRequestBehavior.AllowGet);
        }
        //获取企业好友信息
        public JsonResult AppCompanyFriend(string companyId)
        {
            try
            {
                var user = (from u in sdb.Users where u.CompanyId == companyId select u).ToList();//&& u.IsAvailable != true && u.IsProved != true
                if (user.Count == 0)
                {
                    return Json(new { result = false, msg = "传入参数错误！" }, JsonRequestBehavior.AllowGet);
                }
                List<object> list1 = new List<object>();

                BonsaiiDbContext db = new BonsaiiDbContext(user.FirstOrDefault().ConnectionString);
                var department = (from d in db.Departments select d).ToList();

                foreach (var depart in department)
                {
                    var staffAll = (from s in db.Staffs where s.Department == depart.DepartmentId select s).ToList();
                    List<object> list = new List<object>();
                    foreach (var staffAll1 in staffAll)
                    {
                        //    foreach (var temp in user)
                        //    {
                        string head_url = null;
                        if (staffAll1.Head != null)
                        {
                            head_url = Picture(staffAll1.Head, companyId + staffAll1.StaffNumber);

                        }
                        UserModels user1 = (from u in sdb.Users where u.CompanyId == companyId && u.IsRoot != true && u.StaffNumber == staffAll1.StaffNumber select u).FirstOrDefault();
                        if (user1 != null)
                        {
                            list.Add(new
                            {
                                head_url = head_url,
                                gender = staffAll1.Gender,
                                nickname = user1.NickName,
                                realname = staffAll1.Name,
                                position = staffAll1.Position,
                                telphone = staffAll1.IndividualTelNumber,
                                staffnumber = staffAll1.StaffNumber,
                                username = user1.UserName,
                                email = staffAll1.Email
                            });


                        }
                        else
                        {
                            list.Add(new
                            {
                                head_url = head_url,
                                gender = staffAll1.Gender,
                                nickname = "",
                                realname = staffAll1.Name,
                                position = staffAll1.Position,
                                telphone = staffAll1.IndividualTelNumber,
                                staffnumber = staffAll1.StaffNumber,
                                username = "",
                                email = staffAll1.Email
                            });
                        }

                    }
                    list1.Add(new { department = depart.Name, person = list });

                } return Json(new { result = true, data = list1, msg = "企业好友信息" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee) { return Json(new { result = false, msg = ee.Message }, JsonRequestBehavior.AllowGet); }



        }

        //图片转换
        public string Picture(byte[] photo, string name)
        {

            string path = Server.MapPath("~/files/");//AppDomain.CurrentDomain.BaseDirectory + "files\\";//System.Environment.CurrentDirectory 
            System.IO.MemoryStream ms = new System.IO.MemoryStream(photo);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            //string uurl=Request.ApplicationPath;//获取服务器上ASP.NET应用程序的虚拟应用程序根目录
            string url = path + name + ".jpg";
            img.Save(url);
            url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/files/" + name + ".jpg";
            // url = url_url + name + ".jpg";
            return url;

        }
        public JsonResult test(string CompanyId)
        {
            Company company = sdb.Companies.Find(CompanyId);

            string path = AppDomain.CurrentDomain.BaseDirectory + "files\\";

            System.IO.MemoryStream ms = new System.IO.MemoryStream(company.Logo);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(path + company.FullName + ".jpg");
            string url = path + company.FullName + ".jpg";
            //Staff staff = db1.Staffs.Where(s => s.StaffNumber.Equals(user.StaffNumber)).FirstOrDefault();
            // Department department = db1.Departments.Where(d => d.DepartmentId.Equals(staff.Department)).FirstOrDefault();
            return Json(new { result = true, msg = "绑定公司成功", url = url }, JsonRequestBehavior.AllowGet);
        }
        public string Tt(string userName)
        {
            EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
            string friends = myEaseMobDemo.AccountLookFriends(userName);
            JObject jo = (JObject)JsonConvert.DeserializeObject(friends);//解析Json数据
            //var tt = JArray.Parse();
            string tt = jo["data"].ToString();//得到key对应的数据
            var ttt = JArray.Parse(tt);//对数据进行解析
            foreach (var temp in ttt)//取出每个字段
            {
                string aa = temp.ToString();
            }
            return "";
        }
        public string tet()
        {
            string uurl = Request.ApplicationPath;//获取服务器上ASP.NET应用程序的虚拟应用程序根目录
            string url = HttpRuntime.AppDomainAppVirtualPath; //项目虚拟路径
            string rl = System.Web.HttpContext.Current.Server.MapPath("~/");
            string l = Request.Url.Host; // string l = Request.Url.AbsoluteUri;
            int port = Request.Url.Port;
            return l + ":" + port;
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        //public FileContentResult GetImage(string CompanyId)
        //{
        //    Company com = db1.Companies.FirstOrDefault(p => p.CompanyId == CompanyId);
        //    if (com != null)
        //    {
        //        return File(com.Logo, com.LogoType);//File方法直接将二进制转化为指定类型了。
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion


        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("IndexMain", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }





            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}