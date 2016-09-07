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
    public class StaffParamsController : BaseController
    {

        // GET: StaffParams
        public ActionResult Index()
        {
            ViewBag.StaffParamTypeId = new SelectList(db.StaffParamTypes, "Id", "Name");

            var staffParams = db.StaffParams.Include(s => s.StaffParamType).OrderBy(p=>p.StaffParamTypeId);
            return View(staffParams.ToList());
        }

        // GET: StaffParams/Details/5
        public ActionResult Details(int? id)
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

        // GET: StaffParams/Create
        public ActionResult Create()
        {
            ViewBag.StaffParamTypeId = new SelectList(db.StaffParamTypes, "Id", "Name");
            return View();
        }

        // POST: StaffParams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value,StaffParamTypeId")] StaffParam staffParam)
        {
            if (ModelState.IsValid)
            {
                staffParam.Extra = "0";
                staffParam.RecordPerson = this.Name;
                staffParam.RecordTime = DateTime.Now;
                db.StaffParams.Add(staffParam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffParamTypeId = new SelectList(db.StaffParamTypes, "Id", "Name", staffParam.StaffParamTypeId);
            return View(staffParam);
        }

        // GET: StaffParams/Edit/5
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "Id,Name,Value,StaffParamTypeId")] StaffParam staffParam)
        {
            if (ModelState.IsValid)
            {
                StaffParam param = db.StaffParams.Find(staffParam.Id);
                param.ChangePerson = this.Name;
                param.ChangeTime = DateTime.Now;
                param.Value = staffParam.Value;
                param.StaffParamTypeId = staffParam.StaffParamTypeId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffParamTypeId = new SelectList(db.StaffParamTypes, "Id", "Name", staffParam.StaffParamTypeId);
            return View(staffParam);
        }

        // GET: StaffParams/Delete/5
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffParam staffParam = db.StaffParams.Find(id);

            db.StaffParams.Remove(staffParam);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 根据人事基本参数的名称，获取具体的参数内容（比如：参数为“婚姻状况”，返回值为“已婚”、“未婚”、”离异“）
        /// </summary>
        /// <param name="TypeName">人事参数的类型名称</param>
        /// <returns>返回具体的参数内容的列表</returns>
        public SelectList GetParamsByName(string TypeName)
        {
            var tmp = from x in db.StaffParams where (from d in db.StaffParamTypes where d.Name == TypeName select d.Id).Contains(x.StaffParamTypeId) select x.Value;
            return new SelectList(tmp);
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
