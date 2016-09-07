using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels;


namespace Bonsaii.Controllers
{
    public class SubscribeListController : Controller
    {
        private SystemDbContext db = new SystemDbContext();

        // GET: SubscribeList
        public ActionResult Index()
        {
            return View(db.SubscribeLists.ToList());
        }

        // GET: SubscribeList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            if (subscribeList == null)
            {
                return HttpNotFound();
            }
            return View(subscribeList);
        }

        // GET: SubscribeList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubscribeList/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubscribeName,SQL,IsSQLLegal,IsAvailable,CreateDate")] SubscribeList subscribeList)
        {
            if (ModelState.IsValid)
            {
                db.SubscribeLists.Add(subscribeList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscribeList);
        }

        // GET: SubscribeList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            if (subscribeList == null)
            {
                return HttpNotFound();
            }
            return View(subscribeList);
        }

        // POST: SubscribeList/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubscribeName,SQL,IsSQLLegal,IsAvailable,CreateDate")] SubscribeList subscribeList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscribeList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscribeList);
        }

        // GET: SubscribeList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            if (subscribeList == null)
            {
                return HttpNotFound();
            }
            return View(subscribeList);
        }

        // POST: SubscribeList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscribeList subscribeList = db.SubscribeLists.Find(id);
            db.SubscribeLists.Remove(subscribeList);
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
