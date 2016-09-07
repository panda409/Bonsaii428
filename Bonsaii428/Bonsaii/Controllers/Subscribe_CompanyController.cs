using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels.Subscribe;

namespace Bonsaii.Controllers
{
    public class Subscribe_CompanyController : Controller
    {
        private SystemDbContext db = new SystemDbContext();

        // GET: Subscribe_Company
        public ActionResult Index()
        {
            return View(db.Subscribe_Companies.ToList());
        }

        // GET: Subscribe_Company/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribe_Company subscribe_Company = db.Subscribe_Companies.Find(id);
            if (subscribe_Company == null)
            {
                return HttpNotFound();
            }
            return View(subscribe_Company);
        }

        // GET: Subscribe_Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscribe_Company/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubScribeId,CompanyId")] Subscribe_Company subscribe_Company)
        {
            if (ModelState.IsValid)
            {
                db.Subscribe_Companies.Add(subscribe_Company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscribe_Company);
        }

        // GET: Subscribe_Company/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribe_Company subscribe_Company = db.Subscribe_Companies.Find(id);
            if (subscribe_Company == null)
            {
                return HttpNotFound();
            }
            return View(subscribe_Company);
        }

        // POST: Subscribe_Company/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubScribeId,CompanyId")] Subscribe_Company subscribe_Company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscribe_Company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscribe_Company);
        }

        // GET: Subscribe_Company/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribe_Company subscribe_Company = db.Subscribe_Companies.Find(id);
            if (subscribe_Company == null)
            {
                return HttpNotFound();
            }
            return View(subscribe_Company);
        }

        // POST: Subscribe_Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscribe_Company subscribe_Company = db.Subscribe_Companies.Find(id);
            db.Subscribe_Companies.Remove(subscribe_Company);
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
