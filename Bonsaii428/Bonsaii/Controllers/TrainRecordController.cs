using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels;

namespace Bonsaii.Controllers
{
    public class TrainRecordController : BaseController
    {

        // GET: TrainRecord
        [Authorize(Roles = "Admin,TrainRecord_Index")]
        public ActionResult Index()
        {
            DateTime time=Convert.ToDateTime(DateTime.Now.ToShortDateString());
            //var train = (from ts in db.TrainStarts where ts.StartDate >= time select ts).ToList().OrderByDescending(c => c.Id);
            var train = (from ts in db.TrainStarts  select ts).ToList().OrderByDescending(c => c.Id);
            ViewBag.train = train;
            return View();
        }
        public ActionResult TrainRecordDetails(string staffNumber)
        {
            var name = (from p in db.Staffs where p.StaffNumber == staffNumber&&p.ArchiveTag!=true select p.Name).ToList().Single();
            var trainPerson = (from tr in db.TrainRecords where tr.StaffNumber==staffNumber
                              // join b in db.BillProperties on tr.BillTypeNumber equals b.Type
                               //join s in db.Staffs on tr.StaffNumber equals s.StaffNumber
                              // join d in db.Departments on s.Department equals d.DepartmentId
                               join r in db.TrainStarts on tr.TrainId equals r.Id 
                               into gc
                               from d in gc.DefaultIfEmpty()
                              // where tr.BillNumber == trainNumber && tr.Time == temp
                               select new TrainRecordViewModel
                               {
                                   Id = tr.Id,
                                   BillNumber = tr.BillNumber,
                                   BillTypeNumber = tr.BillTypeNumber,
                                  // BillTypeName = b.TypeName,
                                   StaffNumber = tr.StaffNumber,
                                  // StaffName = s.Name,
                                  // DepartmentName = d.Name,
                                   Tag = tr.Tag,
                                   Time = tr.Time,
                                   //Position = s.Position
                                   TrainTheme = d.TrainTheme,
                                   TrainPlace=d.TrainPlace,
                                   TrainType=d.TrainType
                               }).ToList();
            ViewBag.name = name;
            return View(trainPerson);
        }
        public ActionResult IndexInfo(string trainNumber)
        {
            string temp = DateTime.Now.ToShortDateString();

            var trainPerson = (from tr in db.TrainRecords
                               join b in db.BillProperties on tr.BillTypeNumber equals b.Type
                               join s in db.Staffs on tr.StaffNumber equals s.StaffNumber
                               join d in db.Departments on s.Department equals d.DepartmentId
                           into gc
                               from d in gc.DefaultIfEmpty()
                               where tr.BillNumber == trainNumber&&tr.Time==temp
                               select new TrainRecordViewModel
                               {
                                   Id = tr.Id,
                                   BillNumber = tr.BillNumber,
                                   BillTypeNumber = tr.BillTypeNumber,
                                   BillTypeName = b.TypeName,
                                   StaffNumber = tr.StaffNumber,
                                   StaffName = s.Name,
                                   DepartmentName = d.Name,
                                   Tag = tr.Tag,
                                   Time = tr.Time,
                                   Position = s.Position
                               }).ToList();
            // ViewBag.trainPerson = trainPerson;
            return View(trainPerson);
        }
        // GET: TrainRecord/Details/5 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainRecord trainRecord = db.TrainRecords.Find(id);
            if (trainRecord == null)
            {
                return HttpNotFound();
            }
            return View(trainRecord);
        }

        // GET: TrainRecord/Create
         [Authorize(Roles = "Admin,TrainRecord_Create")]
        public ActionResult Create()
        
        {
            return View();
        }

        // POST: TrainRecord/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,TrainRecord_Create")]
        public ActionResult Create([Bind(Include = "Id,BillNumber,StaffNumber,StaffName,Tag,RecordTime,BillTypeNumber,Time,RecordPerson")] TrainRecord trainRecord)
        {
            if (ModelState.IsValid)
            {
                db.TrainRecords.Add(trainRecord);
                db.SaveChanges();
                return RedirectToAction("IndexInfo");
            }

            return View(trainRecord);
        }

        // GET: TrainRecord/Edit/5
         [Authorize(Roles = "Admin,TrainRecord_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainRecord trainRecord = db.TrainRecords.Find(id);
            if (trainRecord == null)
            {
                return HttpNotFound();
            }
            return View(trainRecord);
        }

        // POST: TrainRecord/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,TrainRecord_Edit")]
        public ActionResult Edit([Bind(Include = "Id,BillNumber,StaffNumber,StaffName,Tag,RecordTime")] TrainRecord trainRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexInfo");
            }
            return View(trainRecord);
        }

        // GET: TrainRecord/Delete/5
         [Authorize(Roles = "Admin,TrainRecord_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainRecord trainRecord = db.TrainRecords.Find(id);
            if (trainRecord == null)
            {
                return HttpNotFound();
            }
            return View(trainRecord);
        }

        // POST: TrainRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,TrainRecord_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainRecord trainRecord = db.TrainRecords.Find(id);
            db.TrainRecords.Remove(trainRecord);
            db.SaveChanges();
            return RedirectToAction("IndexInfo");
        }

        public JsonResult Record(int number)
        {
            TrainRecord train = db.TrainRecords.Find(number);
            if (train.Tag == true)
            {
                List<object> obj = new List<object>();
                train.Tag = false;
                train.RecordTime = null;
                train.RecordPerson = null;
                db.SaveChanges();
                obj.Add(new { temp = "未签到" });
                return Json(obj);

            }
            else
            {
                List<object> obj = new List<object>();
                train.Tag = true;
                train.RecordTime = DateTime.Now;
                train.RecordPerson = this.Name;
                db.SaveChanges();
                obj.Add(new { temp = "已签到" });
                return Json(obj);
            }


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
