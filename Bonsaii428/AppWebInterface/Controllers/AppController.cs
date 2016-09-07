using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebInterface.Controllers
{
    public class AppController : BaseController
    {
        // GET: App
        public JsonResult Rock()
        {
            return Json(new
            {
                Data = "Rock 'n' Roll"
            },JsonRequestBehavior.AllowGet);
        }
    }
}