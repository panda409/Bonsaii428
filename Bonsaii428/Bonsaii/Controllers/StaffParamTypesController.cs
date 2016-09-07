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
    public class StaffParamTypesController : BaseController
    {

        // GET: StaffParamTypes
        public ActionResult Index()
        {
            return View(db.StaffParamTypes.ToList());
        }

        // GET: StaffParamTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffParamType staffParamType = db.StaffParamTypes.Find(id);
            if (staffParamType == null)
            {
                return HttpNotFound();
            }
            return View(staffParamType);
        }

        // GET: StaffParamTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffParamTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] StaffParamType staffParamType)
        {
            if (ModelState.IsValid)
            {
                staffParamType.RecordPerson = this.Name;
                staffParamType.RecordTime = DateTime.Now;
                db.StaffParamTypes.Add(staffParamType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffParamType);
        }

        // GET: StaffParamTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffParamType staffParamType = db.StaffParamTypes.Find(id);
            if (staffParamType == null)
            {
                return HttpNotFound();
            }
            return View(staffParamType);
        }

        // POST: StaffParamTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] StaffParamType staffParamType)
        {
            if (ModelState.IsValid)
            {
                StaffParamType staffParam = db.StaffParamTypes.Find(staffParamType.Id);
                staffParam.ChangePerson = this.Name;
                staffParam.ChangeTime = DateTime.Now;
                staffParam.Name = staffParamType.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffParamType);
        }

        // GET: StaffParamTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffParamType staffParamType = db.StaffParamTypes.Find(id);
            if (staffParamType == null)
            {
                return HttpNotFound();
            }
            return View(staffParamType);
        }

        // POST: StaffParamTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffParamType staffParamType = db.StaffParamTypes.Find(id);
            db.StaffParamTypes.Remove(staffParamType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ParamsIndex(int id)
        {
            var staffParams = db.StaffParams.Include(s => s.StaffParamType).Where(p => p.StaffParamTypeId == id);
            ViewBag.TypeId = id;
            return View(staffParams.OrderBy(p=>p.StaffParamOrder).ToList());
        }

        public ActionResult ParamsCreate(int TypeId)
        {
            ViewBag.TypeId = TypeId;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ParamsCreate(StaffParam staffParam)
        {
            if (ModelState.IsValid)
            {
                if (staffParam.IsDefault == true)
                {
                    //找到旧的默认值，改为false（如果有的话）
                    var oldStaffParam = db.StaffParams.Where(p => p.StaffParamTypeId == staffParam.StaffParamTypeId && p.IsDefault == true).ToList();
                    if (oldStaffParam.Count == 1)
                    {
                        oldStaffParam[0].IsDefault = false;
                        db.Entry(oldStaffParam[0]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                staffParam.Extra = "0";
                staffParam.RecordPerson = this.Name;
                staffParam.RecordTime = DateTime.Now;
                staffParam.StaffParamOrder = 99;
                
                db.StaffParams.Add(staffParam);
                db.SaveChanges();
                return RedirectToAction("ParamsIndex", new { id = staffParam.StaffParamTypeId });
            }
            ViewBag.TypeId = staffParam.StaffParamTypeId;
            return View(staffParam);
        }

        // GET: StaffParams/Edit/5
        public ActionResult ParamsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffParam staffParam = db.StaffParams.Find(id);
            if (staffParam == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffParamTypeId = new SelectList(db.StaffParamTypes, "Id", "Name", staffParam.StaffParamTypeId);
            return View(staffParam);
        }

        // POST: StaffParams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ParamsEdit([Bind(Include = "Id,Name,Value,StaffParamOrder,IsDefault,StaffParamTypeId")] StaffParam staffParam)
        {
            if (ModelState.IsValid)
            {
                if (staffParam.IsDefault == true)
                {
                    //找到旧的默认值，改为false（如果有的话）
                    var oldStaffParam = db.StaffParams.Where(p => p.StaffParamTypeId == staffParam.StaffParamTypeId && p.IsDefault == true).ToList();
                    if (oldStaffParam.Count == 1)
                    {
                        oldStaffParam[0].IsDefault = false;
                        db.Entry(oldStaffParam[0]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                StaffParam param = db.StaffParams.Find(staffParam.Id);
                param.ChangePerson = this.Name;
                param.ChangeTime = DateTime.Now;
                param.Value = staffParam.Value;
                param.StaffParamTypeId = staffParam.StaffParamTypeId;
                param.StaffParamOrder = staffParam.StaffParamOrder;
                param.IsDefault = staffParam.IsDefault;
                db.Entry(param).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ParamsIndex", new { id = staffParam.StaffParamTypeId });
            }
            ViewBag.StaffParamTypeId = new SelectList(db.StaffParamTypes, "Id", "Name", staffParam.StaffParamTypeId);
            return View(staffParam);
        }

        // GET: StaffParams/Delete/5
        public ActionResult ParamsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffParam staffParam = db.StaffParams.Find(id);
            if (staffParam == null)
            {
                return HttpNotFound();
            }
            return View(staffParam);
        }

        // POST: StaffParams/Delete/5
        [HttpPost, ActionName("ParamsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ParamsDeleteConfirmed(int id)
        {
            StaffParam staffParam = db.StaffParams.Find(id);

            db.StaffParams.Remove(staffParam);
            db.SaveChanges();
            return RedirectToAction("ParamsIndex", new { id=staffParam.StaffParamTypeId});
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
