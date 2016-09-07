using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class TestRoleController : Controller
    {
        // GET: TestRole
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}