using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class SettingBGController : Controller
    {
        // GET: SettingBG
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SettingBG()
        {
            //SystemDbContext db = new SystemDbContext();
            //HttpRequest request = HttpContext.;   
            string ips = Request.UserHostAddress.ToString();
            return View();
        }
    }
}