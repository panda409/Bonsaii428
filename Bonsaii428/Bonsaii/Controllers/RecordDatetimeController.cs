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
    public class RecordDatetimeController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();

        // GET: RecordDatetime
        public ActionResult Index()
        {
            return View(db.RecordDatetimes.ToList());
        }

        // GET: RecordDatetime/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecordDatetime recordDatetime = db.RecordDatetimes.Find(id);
            if (recordDatetime == null)
            {
                return HttpNotFound();
            }
            return View(recordDatetime);
        }

        // GET: RecordDatetime/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecordDatetime/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Recordtime,Tag,Month,Day")] RecordDatetime recordDatetime)
        {
            if (ModelState.IsValid)
            {
                db.RecordDatetimes.Add(recordDatetime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recordDatetime);
        }

        // GET: RecordDatetime/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecordDatetime recordDatetime = db.RecordDatetimes.Find(id);
            if (recordDatetime == null)
            {
                return HttpNotFound();
            } 
            
            return View(recordDatetime);
        }

        // POST: RecordDatetime/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Recordtime,Tag,Month,Day")] RecordDatetime recordDatetime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recordDatetime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recordDatetime);
        }

        // GET: RecordDatetime/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecordDatetime recordDatetime = db.RecordDatetimes.Find(id);
            if (recordDatetime == null)
            {
                return HttpNotFound();
            }
            return View(recordDatetime);
        }

        // POST: RecordDatetime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecordDatetime recordDatetime = db.RecordDatetimes.Find(id);
            db.RecordDatetimes.Remove(recordDatetime);
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
