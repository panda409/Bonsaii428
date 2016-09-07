using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Routing;
using System.Threading.Tasks;
using BonsaiiModels;

namespace Bonsaii.Controllers
{
    public class SuperRootController : Controller
    {
        private SystemDbContext db = new SystemDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public SuperRootController()
        {
        }
        /*数据订阅功能*/
        /*作者：Panda*/
        /*日期：2016/02/26*/
        public ActionResult SubscribeListIndex()
        {
            return View(db.SubscribeLists.ToList());
        }
        public ActionResult SubscribeListDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            if (subscribeList == null)
            {
                return HttpNotFound();
            }
            return View(subscribeList);
        }
        public ActionResult SubscribeListCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubscribeListCreate(SubscribeList subscribeList)
        {
            if (ModelState.IsValid)
            {
                subscribeList.CreateDate = DateTime.Now;
           

                db.SubscribeLists.Add(subscribeList);
                db.SaveChanges();
                return RedirectToAction("SubscribeListIndex");
            }

            return View(subscribeList);
        }
        public ActionResult SubscribeListEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            if (subscribeList == null)
            {
                return HttpNotFound();
            }
            return View(subscribeList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubscribeListEdit(SubscribeList subscribeList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscribeList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SubscribeListIndex");
            }
            return View(subscribeList);
        }
        public ActionResult SubscribeListDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            if (subscribeList == null)
            {
                return HttpNotFound();
            }
            return View(subscribeList);
        }

        [HttpPost, ActionName("SubscribeListDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SubscribeListDeleteConfirmed(int id)
        {
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            db.SubscribeLists.Remove(subscribeList);
            db.SaveChanges();
            return RedirectToAction("SubscribeListIndex");
        }

        /*数据订阅功能*/
        
        /*意见反馈*/
        /*作者：Panda*/
        /*日期：不详*/
        public ActionResult IndexAdvice()
        {
            return View(db.AdviceBacks.ToList());
        }
        public SuperRootController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
        // GET: /SuperRoot/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /SuperRoot/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = UserManager.FindByName(model.UserName);
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
        public void CreateSuperRoot()
        {
            var user = new ApplicationUser
            {
                UserName = "administrator",
                CompanyId = "001",
                CompanyFullName = "System",
                IsProved = false,           //是否审核的标志
                IsAvailable = false,         //是否是可用的管理员
                IsRoot = false               //注册企业号的人默认就是企业的超级管理员
            };

            user.ConnectionString = ConfigurationManager.AppSettings["SystemDbConnectionString"];  
            
            var result =  UserManager.Create(user, "administrator");
        }

        //检查平台及管理员的登录状态
        public void CheckLoginStatus()
        {
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = UserManager.FindById(User.Identity.GetUserId());
            string tmp = Request.Url.AbsoluteUri;
            if (user == null)
            {
                Rederect(HttpContext.Request.RequestContext, Url.Action("Login", "SuperRoot"));
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "SuperRoot");
        }
        private void Rederect(RequestContext requestContext, string action)
        {
            requestContext.HttpContext.Response.Clear();
            requestContext.HttpContext.Response.Redirect(action);
            requestContext.HttpContext.Response.End();
        }
        // GET: SuperRoot
        public ActionResult Index()
        {
            CheckLoginStatus();
            return View(db.Users.Where(p => p.IsRoot == true));
        }

        // GET: SuperRoot/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Audit(string id)
        {
            /**
             * 生成企业的数据库名称
             * */
            string CompanyDbName = "Bonsaii" + id;
            var flag1 = CreateDbForCompany(CompanyDbName);
            var tmp = db.Users.Where(p => p.CompanyId.Equals(id)).Single();
            tmp.IsProved = true;
            db.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var flag2 = CreateCompanyTables(tmp.ConnectionString);

            //创建该企业用户的私有数据表
            if (flag1 && flag2)
                return RedirectToAction("Index");
            else
                return View("Error");
        }

        public ActionResult Close(string id)
        {
            using (SystemDbContext mydb = new SystemDbContext()) {
                List<UserModels> tmp = mydb.Users.Where(p => p.CompanyId.Equals(id)).ToList();
                foreach(UserModels user in tmp)
                    user.IsProved = false;
                mydb.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
                mydb.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult AuditCompany()
        {
            CheckLoginStatus();
            return View(db.Users.Where(p => p.IsRoot == true));
        }

        //添加找回密码审核：
        public ActionResult PasswordApply() 
        {
            var item = (from p in db.UserPasswordInfos select p).ToList();
            return View(item);
        }
        public ActionResult OfferHelp(int?id)
        {
           UserPasswordInfo info = db.UserPasswordInfos.Find(id);
           info.AuditStatus = 1;
           db.Entry(info).State = EntityState.Modified;
           db.SaveChanges();
           return View();
        }
        public ActionResult AdminIndex()
        {
            List<UserModels> users = new List<UserModels>();
            using(SystemDbContext db =  new SystemDbContext()){
                users = db.Users.Where(p => p.CompanyId.Equals("001")).ToList();
            }
            return View(users);
        }


        public ActionResult CreateAdmin()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdmin(string UserName)
        {
            var user = new ApplicationUser
            {
                CompanyFullName = "System",
                CompanyId = "001",
                UserName = UserName,
                ConnectionString = ConfigurationManager.AppSettings["SystemDbConnectionString"],
                IsProved = false,           //是否审核的标志
                IsAvailable = false,         //是否是可用的管理员
                IsRoot = false               //非企业号的注册人默认就是非企业的超级管理员
            };
            string Password = "admin";
            var result = await UserManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Admin", "SuperRoot");
            }
            AddErrors(result);
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        ///  为注册的企业用户创建数据库，数据库名称为企业ID
        /// </summary>
        /// <param name="CompanyId">企业ID</param>
        /// <returns></returns>
        public bool CreateDbForCompany(string CompanyDbName)
        {
            String str;
            string ConnString = ConfigurationManager.AppSettings["MasterDbConnectionString"];
            SqlConnection myConn = new SqlConnection(ConnString);
            str = "CREATE DATABASE " + CompanyDbName;
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                    myConn.Close();
            }
            return true;
        }

        /// <summary>
        /// 创建企业数据库所需要的所有数据表
        /// </summary>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns></returns>
        public bool CreateCompanyTables(string ConnectionString)
        {
            string SqlFilePath = ConfigurationManager.AppSettings["SQLFilePath"];
            string varFileName = HttpContext.Server.MapPath("~/App_Data/company.sql");
            if (!System.IO.File.Exists(varFileName))
            {
                return false;
            }

            StreamReader sr = System.IO.File.OpenText(varFileName);

            ArrayList alSql = new ArrayList();
            string commandText = "";
            string varLine = "";
            while (sr.Peek() > -1)
            {
                varLine = sr.ReadLine();
                if (varLine == "")
                {
                    continue;
                }
                if (varLine != "GO")
                {
                    commandText += varLine;
                    commandText += "\r\n";
                }
                else
                {
                    alSql.Add(commandText);
                    commandText = "";
                }
            }

            sr.Close();
            try
            {
                ExecuteCommand(alSql, ConnectionString);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static void ExecuteCommand(ArrayList varSqlList, string ConString)
        {
            SqlConnection MyConnection = new SqlConnection(ConString);
            MyConnection.Open();
            SqlTransaction varTrans = MyConnection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = MyConnection;
            command.Transaction = varTrans;
            try
            {
                foreach (string varcommandText in varSqlList)
                {
                    command.CommandText = varcommandText;
                    command.ExecuteNonQuery();
                }
                varTrans.Commit();
            }
            catch (Exception ex)
            {
                varTrans.Rollback();
                throw ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }
        public JsonResult Upload()
        {
            var tmp = Request;
            return null;
        }
        public ActionResult AppAds()
        {
            return View();
        }

        //广告图片暂时只使用jpg格式，需要其他的格式以后再扩展
        public JsonResult UploadPic()
        {
            //要获取物理路径才行
            string sFileName = System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\img\\" + Request["imgName"] + ".jpg";
            Request.Files[0].SaveAs(sFileName);
            return Json(sFileName);
        }





    }
}
