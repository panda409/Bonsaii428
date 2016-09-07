using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class MyController : BaseController
    {
        public MyController()
        {
        }
        // GET: My
        public ActionResult Index()
        {
            string name = base.UserName;
            return View();
        }
    }
}