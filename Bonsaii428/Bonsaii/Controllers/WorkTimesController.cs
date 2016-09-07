using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.Works;

namespace Bonsaii.Controllers
{
    public class WorkTimesController : BaseController
    {

        // GET: WorkTimes
         [Authorize(Roles = "Admin,WorkTimes_Index")]
        public ActionResult Index()
        {
            var workTimes = db.WorkTimes.Include(w => w.Works);
            return View(workTimes.ToList());
        }

        // GET: WorkTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimes workTimes = db.WorkTimes.Find(id);
            if (workTimes == null)
            {
                return HttpNotFound();
            }
            return View(workTimes);
        }

        // GET: WorkTimes/Create
         [Authorize(Roles = "Admin,WorkTimes_Create")]
        public ActionResult Create()
        {
            ViewBag.WorksId = new SelectList(db.Works, "Id", "Name");
            return View();
        }

        // POST: WorkTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkTimes_Create")]
        public ActionResult Create([Bind(Include = "Id,WorksId,StartTime,EndTime,WorkHours,OvettimeHours,AheadMinutes,BackMinutes,LateMinutes,LeaveEarlyMinutes,IsAheadToOvertime,IsBackToOvertime")] WorkTimes workTimes)
        {
            if (ModelState.IsValid)
            {
                db.WorkTimes.Add(workTimes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorksId = new SelectList(db.Works, "Id", "Name", workTimes.WorksId);
            return View(workTimes);
        }

        // GET: WorkTimes/Edit/5
        [Authorize(Roles = "Admin,WorkTimes_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimes workTimes = db.WorkTimes.Find(id);
            ViewBag.WorkId = workTimes.WorksId;
            if (workTimes == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorksId = new SelectList(db.Works, "Id", "Name", workTimes.WorksId);
            return View(workTimes);
        }

        // POST: WorkTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkTimes_Edit")]
        public ActionResult Edit([Bind(Include = "Id,WorksId,StartTime,EndTime,WorkHours,OvettimeHours,AheadMinutes,BackMinutes,LateMinutes,LeaveEarlyMinutes,IsAheadToOvertime,IsBackToOvertime")] WorkTimes workTimes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workTimes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workTimes);
        }

        // GET: WorkTimes/Delete/5
        [Authorize(Roles = "Admin,WorkTimes_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimes workTimes = db.WorkTimes.Find(id);
            if (workTimes == null)
            {
                return HttpNotFound();
            }
            return View(workTimes);
        }

        // POST: WorkTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkTimes_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkTimes workTimes = db.WorkTimes.Find(id);
            db.WorkTimes.Remove(workTimes);
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
