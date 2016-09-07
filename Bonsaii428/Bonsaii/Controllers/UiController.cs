using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class UiController : Controller
    {
        // GET: Ui
        public ActionResult Index()
        {
            return View();
        }

         [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string tmp = "HDJSFJ";
            return View();
        }
    }
}