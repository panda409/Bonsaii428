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
    public class StaffBasicParamsController : BaseController
    {

        // GET: StaffBasicParams
        public ActionResult Index()
        {
            return View(db.StaffBasicParams.ToList());
        }

        // GET: StaffBasicParams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffBasicParam staffBasicParam = db.StaffBasicParams.Find(id);
            if (staffBasicParam == null)
            {
                return HttpNotFound();
            }
            return View(staffBasicParam);
        }

        // GET: StaffBasicParams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffBasicParams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value")] StaffBasicParam staffBasicParam)
        {
            if (ModelState.IsValid)
            {
                db.StaffBasicParams.Add(staffBasicParam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffBasicParam);
        }

        // GET: StaffBasicParams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffBasicParam staffBasicParam = db.StaffBasicParams.Find(id);
            if (staffBasicParam == null)
            {
                return HttpNotFound();
            }
            return View(staffBasicParam);
        }

        // POST: StaffBasicParams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value")] StaffBasicParam staffBasicParam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffBasicParam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffBasicParam);
        }

        // GET: StaffBasicParams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffBasicParam staffBasicParam = db.StaffBasicParams.Find(id);
            if (staffBasicParam == null)
            {
                return HttpNotFound();
            }
            return View(staffBasicParam);
        }

        // POST: StaffBasicParams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffBasicParam staffBasicParam = db.StaffBasicParams.Find(id);
            db.StaffBasicParams.Remove(staffBasicParam);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Manage()
        {
            //获取工种对应的Id
            int WorkTypeId = db.StaffParamTypes.Where(p => p.Name == "工种").Single().Id;


            ViewBag.Period = db.StaffParams.Where(p => p.StaffParamTypeId == WorkTypeId).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Value
            }).ToList();


            //ViewBag.Perid = db.StaffBasicParams.Where(p => p.Id == 1).Single().Value;
            ViewBag.Profile = db.StaffBasicParams.Where(p => p.Id == 2).Single().Value;
            List<string> list = new List<string>() { "提醒", "提醒并通用", "提醒并拒绝" };
            List<SelectListItem> item = list.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            }).ToList();
            ViewBag.ProfileList = item;

            ViewBag.Len = db.StaffBasicParams.Where(p => p.Id == 3).Single().Value;

            List<string> list2 = new List<string>() { "自动生成逻辑卡号", "逻辑卡号和物理工号相等", "逻辑卡号和物理卡号相等" };
            List<SelectListItem> item2 = list2.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            }).ToList();
            ViewBag.LenList = item2;
            ViewBag.Photo = db.StaffBasicParams.Where(p => p.Id == 4).Single().Value;
            ViewBag.Leave = db.StaffBasicParams.Where(p => p.Id == 5).Single().Value;
            return View();
        }


        public JsonResult GetDays(int id)
        {
            string days = db.StaffParams.Where(p => p.Id == id).Single().Extra;
            int result = int.Parse(days);
            return Json(
                new
                {
                    days = result
                }
                );
        }


        [HttpPost]
        public ActionResult Manage(string Profile, string Len, string Photo, string Leave, int Period, string PeriodDays)
        {

            StaffParam tmpStaffParams = db.StaffParams.Where(p => p.Id == Period).Single();
            tmpStaffParams.Extra = PeriodDays;
            db.Entry(tmpStaffParams).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            
            StaffBasicParam[] list = db.StaffBasicParams.ToArray();
            list[1].Value = Profile;
            list[2].Value = Len;
            list[3].Value = Photo;
            list[4].Value = Leave;
            db.Entry(list[1]).State = System.Data.Entity.EntityState.Modified;
            db.Entry(list[2]).State = System.Data.Entity.EntityState.Modified;
            db.Entry(list[3]).State = System.Data.Entity.EntityState.Modified;
            db.Entry(list[4]).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Manage");
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
