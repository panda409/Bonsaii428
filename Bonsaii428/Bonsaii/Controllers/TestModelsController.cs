using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.GlobalStaticVaribles;

namespace Bonsaii.Controllers
{
    public class TestModelsController : BaseController
    {
        // GET: TestModels
        public ActionResult Index()
        {
            
            return View(db.TestModels.ToList());
        }

        // GET: TestModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestModels testModels = db.TestModels.Find(id);
            if (testModels == null)
            {
                return HttpNotFound();
            }
            return View(testModels);
        }

        // GET: TestModels/Create
        public ActionResult Create()
        {
            ViewBag.List = CirculateMethod.GetCirculateMethod();
            return View();
        }

        // POST: TestModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Method,IsChecked,FilePath")] TestModels testModels)
        {
            if (ModelState.IsValid)
            {
                db.TestModels.Add(testModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testModels);
        }

        // GET: TestModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestModels testModels = db.TestModels.Find(id);
            if (testModels == null)
            {
                return HttpNotFound();
            }
            return View(testModels);
        }

        // POST: TestModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Method,IsChecked,FilePath")] TestModels testModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testModels);
        }

        // GET: TestModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestModels testModels = db.TestModels.Find(id);
            if (testModels == null)
            {
                return HttpNotFound();
            }
            return View(testModels);
        }

        // POST: TestModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestModels testModels = db.TestModels.Find(id);
            db.TestModels.Remove(testModels);
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
