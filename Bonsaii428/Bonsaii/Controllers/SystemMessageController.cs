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
    public class SystemMessageController : BaseController
    {
        private SystemDbContext db = new SystemDbContext();

        // GET: SystemMessage
        public ActionResult Index()
        {

            List<SystemMessage> mess = db.SystemMessages.Where(c => c.UserName == this.UserName && c.CompanyId == this.CompanyId).ToList();
            //foreach (var item in mess) {

            //    if (item.MessTime != null)
            //    {
            //        DateTime tmp = (DateTime)item.MessTime;
            //        //string tmp1 = tmp.Date.ToString("yyyy/MM/dd");
            //        //item.MessDateDisplay = tmp.Date.ToString("yyyy/MM/dd");
            //    }
            //}
             return View(mess.OrderByDescending(c=>c.Id));
        }

        // GET: SystemMessage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            if (systemMessage == null)
            {
                return HttpNotFound();
            }
            systemMessage.IsRead = true;
            db.SaveChanges();
            return View(systemMessage);
        }

        // GET: SystemMessage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemMessage/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MessTitle,MessBody,MessTime,IsRead")] SystemMessage systemMessage)
        {
            if (ModelState.IsValid)
            {
                systemMessage.MessTime = DateTime.Now;
                db.SystemMessages.Add(systemMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(systemMessage);
        }

        // GET: SystemMessage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            if (systemMessage == null)
            {
                return HttpNotFound();
            }
            return View(systemMessage);
        }

        // POST: SystemMessage/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MessTitle,MessBody,MessTime,IsRead")] SystemMessage systemMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(systemMessage);
        }

        // GET: SystemMessage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            if (systemMessage == null)
            {
                return HttpNotFound();
            }
            return View(systemMessage);
        }

        // POST: SystemMessage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            db.SystemMessages.Remove(systemMessage);
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
