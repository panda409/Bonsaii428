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
    public class DimensionController : Controller
    {
        private SystemDbContext db = new SystemDbContext();
        // GET: Dimension
        public ActionResult Index()
        {
            return View(db.Dimensions.ToList());
        }

        // GET: Dimension/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dimension dimension = db.Dimensions.Find(id);
            if (dimension == null)
            {
                return HttpNotFound();
            }
            return View(dimension);
        }

        // GET: Dimension/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dimension/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DimensionName")] Dimension dimension)
        {
            if (ModelState.IsValid)
            {
                db.Dimensions.Add(dimension);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dimension);
        }

        // GET: Dimension/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dimension dimension = db.Dimensions.Find(id);
            if (dimension == null)
            {
                return HttpNotFound();
            }
            return View(dimension);
        }

        // POST: Dimension/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DimensionName")] Dimension dimension)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dimension).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dimension);
        }

        // GET: Dimension/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dimension dimension = db.Dimensions.Find(id);
            if (dimension == null)
            {
                return HttpNotFound();
            }
            return View(dimension);
        }

        // POST: Dimension/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dimension dimension = db.Dimensions.Find(id);
            db.Dimensions.Remove(dimension);
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
