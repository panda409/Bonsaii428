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
    public class WorksController : BaseController
    {

        // GET: Works
             [Authorize(Roles = "Admin,Works_Index")]
        public ActionResult Index()
        {
            return View(db.Works.ToList());
        }

        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Works works = db.Works.Find(id);
            if (works == null)
            {
                return HttpNotFound();
            }

            return View(works);
        }

        // GET: Works/Create
            [Authorize(Roles = "Admin,Works_Create")]
        public ActionResult Create()
        {
            ViewBag.Id = Guid.NewGuid();
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Works_Create")]
        public ActionResult Create( Works works)
        {
            if (ModelState.IsValid)
            {
                works.TotalWorkHours = 0;
                works.TotalOvertimeHours = 0;
                db.Works.Add(works);
                db.SaveChanges();
                int id = db.Works.OrderBy(p => p.Id).ToList().Last().Id;
                return RedirectToAction("WorkTimesIndex", new { id = id });
            }
            return View(works);
        }

       [Authorize(Roles = "Admin,Works_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Works works = db.Works.Find(id);
            if (works == null)
            {
                return HttpNotFound();
            }
            return View(works);
        }
        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Works_Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name,IsAutoWork,AutoWorkHours,AutoWorkExtraToOvertime,IsOverDays,TotalWorkHours,TotalOvertimeHours,LatePunishment,Remark")] Works works)
        {
            if (ModelState.IsValid)
            {
                db.Entry(works).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(works);
        }
        // GET: Works/Delete/5
         [Authorize(Roles = "Admin,Works_Delete")]
        public ActionResult Delete(int? id)
        {
            bool Flag = true;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Works works = db.Works.Find(id);
            if (works == null)
            {
                return HttpNotFound();
            }
             var worktimes = db.WorkTimes.Where(p => p.WorksId == id).FirstOrDefault();
             var workmanager = db.WorkManages.Where(p => p.WorksId == id).FirstOrDefault();
            if (worktimes == null && workmanager == null)
            {
                Flag = true;
            } 
            else { 
                Flag = false;
            }
            ViewBag.a = Flag;
            return View(works);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Works_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Works works = db.Works.Find(id);
            db.Works.Remove(works);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         [Authorize(Roles = "Admin,Works_Index")]
        public ActionResult WorkTimesIndex(int id)
        {
            List<WorkTimes> workTimes = db.WorkTimes.Where(p => p.WorksId == id).ToList();
            ViewBag.WorkId = id;
            return View(workTimes);
        }

        public ActionResult WorkTimesCreate(int id)
        {
            ViewBag.WorkId = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Works_Create")]
        public ActionResult WorkTimesCreate(WorkTimes workTimes)
        {
            if (ModelState.IsValid)
            {
                workTimes.WorksId = int.Parse(Request["WorkId"]);
                db.WorkTimes.Add(workTimes);
                db.SaveChanges();
                Works tmp = db.Works.Find(workTimes.WorksId);

                tmp.TotalWorkHours += workTimes.WorkHours;
                tmp.TotalOvertimeHours += workTimes.OvettimeHours;

                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("WorkTimesIndex", new { id = workTimes.WorksId });
            }
            return View(workTimes);
        }


        // GET: WorkTimes/Edit/5
         [Authorize(Roles = "Admin,Works_Edit")]
        public ActionResult WorkTimesEdit(int? id)
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
            ViewBag.WorkId = workTimes.WorksId;
            return View(workTimes);
        }

        // POST: WorkTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Works_Edit")]
        public ActionResult WorkTimesEdit([Bind(Include = "Id,WorksId,StartTime,EndTime,WorkHours,OvettimeHours,AheadMinutes,BackMinutes,LateMinutes,LeaveEarlyMinutes,IsAheadToOvertime,IsBackToOvertime")] WorkTimes workTimes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workTimes).State = EntityState.Modified;
                db.SaveChanges();
                Works tmp = db.Works.Find(workTimes.WorksId);
                List<WorkTimes> workHours = db.WorkTimes.Where(p=>p.WorksId == workTimes.WorksId).ToList();

                    tmp.TotalWorkHours = 0;
                tmp.TotalOvertimeHours = 0;
                    foreach (WorkTimes tmpInt in workHours)
                    {
                        tmp.TotalWorkHours += tmpInt.WorkHours;
                        tmp.TotalOvertimeHours += tmpInt.OvettimeHours;
                    }
                
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("WorkTimesIndex", new { id = workTimes.WorksId });
            }
            return View(workTimes);
        }


        // GET: WorkTimes/Delete/5
         [Authorize(Roles = "Admin,Works_Delete")]
        public ActionResult WorkTimesDelete(int? id)
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
            ViewBag.Id = id;
            ViewBag.WorkId = workTimes.WorksId;
            return View(workTimes);
        }

        // POST: WorkTimes/Delete/5
        [HttpPost, ActionName("WorkTimesDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Works_Delete")]
        public ActionResult WorkTimesDeleteConfirmed(FormCollection collection)
        {
            WorkTimes workTimes = db.WorkTimes.Find(int.Parse(collection["Id"]));
            int WorkId = workTimes.WorksId;
            db.WorkTimes.Remove(workTimes);
            db.SaveChanges();
            Works tmp = db.Works.Find(workTimes.WorksId);
            List<WorkTimes> workHours = db.WorkTimes.Where(p => p.WorksId == workTimes.WorksId).ToList();

            tmp.TotalWorkHours = 0;
            tmp.TotalOvertimeHours = 0;
            foreach (WorkTimes tmpInt in workHours)
            {
                tmp.TotalWorkHours += tmpInt.WorkHours;
                tmp.TotalOvertimeHours += tmpInt.OvettimeHours;
            }
            db.Entry(tmp).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WorkTimesIndex", new { id = WorkId });
        }


        public ActionResult Test()
        {
            ViewBag.Id = Guid.NewGuid();
            return View();                                                 
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
