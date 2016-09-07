using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Routing;
using System.Data.Entity;

namespace AppWebInterface.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        ///  连接字符串，从session中获取
        /// </summary>

        public string UserName
        {
            get;
            private set;
        }
        public ApplicationUserManager UserManager
        {
            get;
            set;
        }




        /// <summary>
        /// 在Controller的构造函数调用完成之前是不能够获取到Session这个对象的！因此要把获取有关Session数据的操作放到Initialize方法当中。
        /// Initialize方法调用的时候，Controller已经可以获取到Session对象了。
        /// </summary>
        public BaseController()
        {
        }

        /// <summary>
        /// 这个函数的调用顺序在：BaseController的构造函数和继承BaseController的构造函数调用之后才进行调用
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = UserManager.FindById(User.Identity.GetUserId());
            //既没有通过系统登录，也没有通过外部登录，返回给APP一个结果，通知没有登录
            if (user == null)
                Rederect(requestContext, Url.Action("Login", "Account"));
        }

        private void Rederect(RequestContext requestContext, string action)
        {
            requestContext.HttpContext.Response.Clear();
            requestContext.HttpContext.Response.Redirect(action);
            requestContext.HttpContext.Response.End();
        }
    }
}