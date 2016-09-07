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
using HXComm;

namespace AppWebInterface.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public const string appClientID = "YXA6MfhwoG6mEeWbT0ef-BFVfw";
        public const string appClientSecret = "YXA6GUEzoaBTvze7xX3RmlJnF1wMtZA";
        public const string appName = "fuckaholic";
        public const string orgName = "fuckaholic";
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
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

        /// <summary>
        /// APP调用的注册函数
        /// </summary>
        /// <param name="UserName">用户名，现在是用手机号</param>
        /// <param name="Password">密码</param>
        /// <param name="NickName">用户昵称，这个主要用来注册环信用</param>
        /// <param name="Code">手机验证码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult Register(string UserName, string Password, string NickName,string Code)
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
                    }
                    else if (!CurrentUserCode.Code.Equals(Code))
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码输入错误！");
                        this.PackageJson(0, "验证码错误！", null);
                    }
                }

                ApplicationUser user = new ApplicationUser
                {
                    CompanyFullName = "GeneralStaff",//model.CompanyFullName,
                    PhoneNumber = UserName,
                    UserName = UserName,
                    NickName = NickName,
                    Name = "普通员工",   //用户注册的时候写入该名称
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
                    //在环信中注册
                    EaseMobDemo myEaseMobDemo = new EaseMobDemo(appClientID, appClientSecret, appName, orgName);
                    string MobResult = myEaseMobDemo.AccountCreate(UserName, Password,NickName);
                    return PackageJson(1, "注册成功！",MobResult);
                }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return PackageJson(0,"Web系统中注册用户失败！",null);
        }
        //[AllowAnonymous]
        //public JsonResult AppLogin()
        //{
        //    return Json(new
        //    {
        //        Result = -1,
        //        Msg = "APP用户还没有登录"
        //    },JsonRequestBehavior.AllowGet);
        //}

        public ActionResult WebLogin()
        {
            return View();
        }
        [AllowAnonymous]
        public JsonResult Login(string UserName, string Password)
        {
            using (SystemDbContext con = new SystemDbContext())
            {
                ApplicationUser user = UserManager.FindByName(UserName);
                if (user == null)   
                    return PackageJson(0, "该用户不存在！", null);
            }
            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = SignInManager.PasswordSignIn(UserName, Password,true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Failure:
                    return PackageJson(0, "用户名或密码错误！", null);
            }
            return PackageJson(1, "登录成功", null);
        }

        /// <summary>
        /// 封装接口调用要返回的Json对象
        /// </summary>
        /// <param name="result">结果值,0代表请求失败，1是成功，-1表示APP用户还没有登录</param>
        /// <param name="msg">执行的结果信息</param>
        /// <param name="data">执行的结果数据</param>
        /// <returns></returns>
        public JsonResult PackageJson(int result, string msg, object data)
        {
            return Json(new
            {
                Result = result,
                Msg = msg,
                Data = data
            }, JsonRequestBehavior.AllowGet);
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

        #region Helpers
        // Used for XSRF protection when adding external logins
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
            return RedirectToAction("Index", "Home");
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