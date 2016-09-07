using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels.GlobalStaticVaribles;
using Bonsaii.Models.Checking_in;

namespace Bonsaii.Controllers
{
    public class CheckingInParamsController : BaseController
    {


        public ActionResult HomeIndex()
        {
            ViewBag.OvertimeAheadMinutes = db.CheckingInParams.Find(1).Value;
            ViewBag.OvertimeBackMinutes = db.CheckingInParams.Find(2).Value;
            ViewBag.EvectionAheadMinutes = db.CheckingInParams.Find(3).Value;
            ViewBag.EvectionBackMinutes = db.CheckingInParams.Find(4).Value;
            ViewBag.LateMinutes = db.CheckingInParams.Find(5).Value;
            ViewBag.LateToHour = db.CheckingInParams.Find(6).Value;
            ViewBag.EarlyMinutes = db.CheckingInParams.Find(7).Value;
            ViewBag.EarlyToHour = db.CheckingInParams.Find(8).Value;
            //值班提前打卡分钟数
            ViewBag.OnDutyAheadMinutes = db.CheckingInParams.Find(11).Value;
            //值班打卡推后分钟数
            ViewBag.OnDutyBackMinutes = db.CheckingInParams.Find(12).Value;



            ViewBag.EvectionNeedSignIn = db.CheckingInParamsBools.Find(1).Value == true ? "true" : "false";
            ViewBag.OvertimeNeedSignIn = db.CheckingInParamsBools.Find(2).Value == true ? "true" : "false";
            ViewBag.OnDutyNeedSignIn = db.CheckingInParamsBools.Find(3).Value == true ? "true" : "false";
            return View(db.CheckingInParams.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HomeIndex(FormCollection collection)
        {
            List<CheckingInParams> list = db.CheckingInParams.OrderBy(p => p.Id).ToList();
            list[0].Value = int.Parse(collection["OvertimeAheadMinutes"]);
            list[1].Value = int.Parse(collection["OvertimeBackMinutes"]);
            list[2].Value = int.Parse(collection["EvectionAheadMinutes"]);
            list[3].Value = int.Parse(collection["EvectionBackMinutes"]);
            list[4].Value = int.Parse(collection["LateMinutes"]);
            list[5].Value = int.Parse(collection["LateToHour"]);
            list[6].Value = int.Parse(collection["EarlyMinutes"]);
            list[7].Value = int.Parse(collection["EarlyToHour"]);
            list[10].Value = int.Parse(collection["OnDutyAheadMinutes"]);
            list[11].Value = int.Parse(collection["OnDutyBackMinutes"]);

            for (int i = 0; i < list.Count; i++)
            {
                db.Entry(list[i]).State = EntityState.Modified;
                db.SaveChanges();
            }    

            List<CheckingInParamsBool> boolList = db.CheckingInParamsBools.OrderBy(p=>p.id).ToList();
            boolList[0].Value = collection["EvectionNeedSignIn"] == null ? false : true;
            boolList[1].Value = collection["OvertimeNeedSignIn"] == null ? false : true;
            boolList[2].Value = collection["OnDutyNeedSignIn"] == null ? false : true;
            for (int i = 0; i < boolList.Count; i++)
            {
                db.Entry(boolList[i]).State = EntityState.Modified;
                db.SaveChanges();
            }

                return RedirectToAction("HomeIndex");
        }
        // GET: CheckingInParams
        public ActionResult Index()
        {
            return View(db.CheckingInParams.ToList());
        }

        // GET: CheckingInParams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingInParams checkingInParams = db.CheckingInParams.Find(id);
            if (checkingInParams == null)
            {
                return HttpNotFound();
            }
            return View(checkingInParams);
        }

        // GET: CheckingInParams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckingInParams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value")] CheckingInParams checkingInParams)
        {
            if (ModelState.IsValid)
            {
                db.CheckingInParams.Add(checkingInParams);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checkingInParams);
        }

        // GET: CheckingInParams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingInParams checkingInParams = db.CheckingInParams.Find(id);
            if (checkingInParams == null)
            {
                return HttpNotFound();
            }
            return View(checkingInParams);
        }

        // POST: CheckingInParams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value")] CheckingInParams checkingInParams)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkingInParams).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checkingInParams);
        }

        // GET: CheckingInParams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingInParams checkingInParams = db.CheckingInParams.Find(id);
            if (checkingInParams == null)
            {
                return HttpNotFound();
            }
            return View(checkingInParams);
        }

        // POST: CheckingInParams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckingInParams checkingInParams = db.CheckingInParams.Find(id);
            db.CheckingInParams.Remove(checkingInParams);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
