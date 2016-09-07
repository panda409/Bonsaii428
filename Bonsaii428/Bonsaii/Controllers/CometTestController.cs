using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class CometTestController : BaseController
    {
        // GET: CometTest
        public ActionResult Index()
        {
            return View();
        }

        // <summary>
        //这个是死循环；简单但不实用
        //</summary>
      //  [HttpPost]
        public ActionResult Test()
        {
            //Response.Buffer = false;

            //for (int i = 0; i < 1; i++)
            //{
                /*Panda Add SystemMessage alert*/
                SystemDbContext db2 = new SystemDbContext();

                var count = (from p in db2.SystemMessages where (p.CompanyId) == this.CompanyId && (p.UserName == this.UserName) && (p.IsRead == false) select p).ToList();
                //ViewBag.Mess = count.Count();
                Response.Write(count.Count() + "|");
                //Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFF") + "|");
                //Thread.Sleep(500);
            //}
           // return Json(count.Count());
            return RedirectToAction("/Home/IndexMain");
            //return Json(JsonRequestBehavior.AllowGet);
            //跑不到这里的
            //return Content("");
        }
    
    }
}