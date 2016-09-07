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
    public class BackgroundController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();

        // GET: Background
        public ActionResult Index()
        {
            return View(db.Backgrounds.ToList());
        }

        // GET: Background/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Background background = db.Backgrounds.Find(id);
            if (background == null)
            {
                return HttpNotFound();
            }
            return View(background);
        }

        // GET: Background/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Background/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,XueLi")] Background background)
        {
            if (ModelState.IsValid)
            {
                db.Backgrounds.Add(background);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(background);
        }

        // GET: Background/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Background background = db.Backgrounds.Find(id);
            if (background == null)
            {
                return HttpNotFound();
            }
            return View(background);
        }

        // POST: Background/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,XueLi")] Background background)
        {
            if (ModelState.IsValid)
            {
                db.Entry(background).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(background);
        }

        // GET: Background/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Background background = db.Backgrounds.Find(id);
            if (background == null)
            {
                return HttpNotFound();
            }
            return View(background);
        }

        // POST: Background/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Background background = db.Backgrounds.Find(id);
            db.Backgrounds.Remove(background);
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
