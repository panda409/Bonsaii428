using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bonsaii.Authorize
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //用作包含有关某个 HTTP 请求的 HTTP 特定信息的类的基类。
        /**
         * protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                //User：在派生类中重写时，获取或设置当前 HTTP 请求的安全信息。
                //具体的验证逻辑写在这里
                if (验证结果为假）
                   return false;
               
                return true;
            }
         * */

        public CustomAuthorizeAttribute()
        {
            Roles = GetRoles();
        }
        private string GetRoles()
        {
            return "Admin,User";
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //if (httpContext == null)
            //    throw new ArgumentNullException("httpContext");

            string[] roles = Roles.Split(',');
            var tmp = Users;

            var rd = httpContext.Request.RequestContext.RouteData;
            string action = rd.GetRequiredString("action");
            string controller = rd.GetRequiredString("controller");
       //     var controller = this.RouteData.Values["controller"].ToString();

            return false;
            ////if (!httpContext.User.Identity.IsAuthenticated)
            ////    return false;

            //return true;
        }

    }
}