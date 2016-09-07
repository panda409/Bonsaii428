using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;

namespace Bonsaii.Controllers
{
    public class HolidayController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();

        // GET: Holiday
        public ActionResult Index()
        {
            return View(db.Holidays.ToList());
        }

        // GET: Holiday/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            return View(holiday);
        }

        // GET: Holiday/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Holiday/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JieJiaName,BeginTime,EndTime")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                holiday.RecordPerson = this.Name;
                holiday.RecordTime = DateTime.Now;
                db.Holidays.Add(holiday);
                db.SaveChanges();
                string number = "H";
                number+=holiday.Id.ToString();
                for (var i = holiday.BeginTime; i <= holiday.EndTime; i = i.AddDays(1))
                {
                    HolidayTimeRecord htr = new HolidayTimeRecord();
                    htr.Number = number;
                    htr.RecordTimeHoliday = i;
                    htr.Tag = "2";
                    db.HolidayTimeRecords.Add(htr);
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }

            return View(holiday);
        }

        // GET: Holiday/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            
            return View(holiday);
        }

        // POST: Holiday/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JieJiaName,BeginTime,EndTime")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                //for (var i = holiday.BeginTime; i <= holiday.EndTime; i = i.AddDays(1))
                //{
                //    var temp = db.RecordDatetimes.Where(rd => rd.Recordtime.Equals(i));
                //    foreach (var tt in temp)
                //    {
                //        tt.Tag = "0";
                //        db.Entry(tt).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }

                //} 
                string number = "H";
                number += holiday.Id.ToString();
                var htrtemp = (from htr in db.HolidayTimeRecords 
                               where htr.Number==number
                               select htr).ToList();
               if(htrtemp.Count!=0)
               { 
                foreach (var temp in htrtemp)
                {
                    HolidayTimeRecord htr = db.HolidayTimeRecords.Find(temp.Id);
                    db.HolidayTimeRecords.Remove(htr);
                }
                db.SaveChanges();
               }
                for (var i = holiday.BeginTime; i <= holiday.EndTime; i = i.AddDays(1))
                {
                    HolidayTimeRecord htr1 = new HolidayTimeRecord();
                    htr1.Number = number;
                    htr1.RecordTimeHoliday = i;
                    htr1.Tag = "2";
                    db.HolidayTimeRecords.Add(htr1);
                    db.SaveChanges();

                }
                Holiday holi = db.Holidays.Find(holiday.Id);
                holi.ChangePerson = this.Name;
                holi.ChangeTime = DateTime.Now;
                holi.JieJiaName = holiday.JieJiaName;
                holi.BeginTime = holiday.BeginTime;
                holi.EndTime = holiday.EndTime;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holiday);
        }

        // GET: Holiday/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            return View(holiday);
        }

        // POST: Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Holiday holiday = db.Holidays.Find(id);
            //for (var i = holiday.BeginTime; i <= holiday.EndTime; i = i.AddDays(1))
            //{
            //    var temp = db.RecordDatetimes.Where(rd => rd.Recordtime.Equals(i));
            //    foreach (var tt in temp)
            //    {
            //        tt.Tag = "0";
            //        db.Entry(tt).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }

            //}
            db.Holidays.Remove(holiday);
            db.SaveChanges();
            string number = "H";
            number += holiday.Id.ToString();
            var htrtemp = (from htr in db.HolidayTimeRecords
                           where htr.Number == number
                           select htr).ToList();
            if (htrtemp.Count != 0)
            {
                foreach (var temp in htrtemp)
                {
                    HolidayTimeRecord htr = db.HolidayTimeRecords.Find(temp.Id);
                    db.HolidayTimeRecords.Remove(htr);
                }
                db.SaveChanges();
            }
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
