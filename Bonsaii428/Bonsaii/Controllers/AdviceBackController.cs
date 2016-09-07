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
    public class AdviceBackController : BaseController
    {
        private SystemDbContext db = new SystemDbContext();

        [Authorize(Roles = "Admin,AdviceBack_Index")]
        // GET: AdviceBack
        public ActionResult Index()
        {
            return View(db.AdviceBacks.Where(ab=>ab.CompanyId.Equals(this.CompanyId)).ToList());
        }
       [Authorize(Roles = "Admin,AdviceBack_Details")]
        // GET: AdviceBack/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdviceBack adviceBack = db.AdviceBacks.Find(id);
            if (adviceBack == null)
            {
                return HttpNotFound();
            }
            return View(adviceBack);
        }
        [Authorize(Roles = "Admin,AdviceBack_Create")]
        // GET: AdviceBack/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin,AdviceBack_Create")]
        // POST: AdviceBack/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdviceBack adviceBack)
        {
            if (ModelState.IsValid)
            {
                adviceBack.CompanyId = this.CompanyId;
                adviceBack.RecordTime = DateTime.Now;
                adviceBack.RecordPerson = this.Name;
                db.AdviceBacks.Add(adviceBack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adviceBack);
        }
        [Authorize(Roles = "Admin,AdviceBack_Edit")]
        // GET: AdviceBack/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdviceBack adviceBack = db.AdviceBacks.Find(id);
            if (adviceBack == null)
            {
                return HttpNotFound();
            }
            return View(adviceBack);
        }
         [Authorize(Roles = "Admin,AdviceBack_Edit")]
        // POST: AdviceBack/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdviceBack adviceBack)
        {
            if (ModelState.IsValid)
            {
                AdviceBack advice = db.AdviceBacks.Find(adviceBack.Id);
                advice.ChangeTime = DateTime.Now;
                advice.ChangePerson = this.Name;
                advice.Content = adviceBack.Content;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adviceBack);
        }
         [Authorize(Roles = "Admin,AdviceBack_Delete")]
        // GET: AdviceBack/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdviceBack adviceBack = db.AdviceBacks.Find(id);
            if (adviceBack == null)
            {
                return HttpNotFound();
            }
            return View(adviceBack);
        }
           [Authorize(Roles = "Admin,AdviceBack_Delete")]
        // POST: AdviceBack/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdviceBack adviceBack = db.AdviceBacks.Find(id);
            db.AdviceBacks.Remove(adviceBack);
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
