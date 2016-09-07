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
    public class TableNameContrastController : BaseController
    {
        // GET: TableNameContrast
        public ActionResult Index()
        {
            return View(db.TableNameContrasts.ToList());
        }

        // GET: TableNameContrast/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableNameContrast tableNameContrast = db.TableNameContrasts.Find(id);
            if (tableNameContrast == null)
            {
                return HttpNotFound();
            }
            return View(tableNameContrast);
        }

        // GET: TableNameContrast/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableNameContrast/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TableName,TableDescription")] TableNameContrast tableNameContrast)
        {
            if (ModelState.IsValid)
            {
                db.TableNameContrasts.Add(tableNameContrast);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableNameContrast);
        }

        // GET: TableNameContrast/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableNameContrast tableNameContrast = db.TableNameContrasts.Find(id);
            if (tableNameContrast == null)
            {
                return HttpNotFound();
            }
            return View(tableNameContrast);
        }

        // POST: TableNameContrast/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TableName,TableDescription")] TableNameContrast tableNameContrast)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableNameContrast).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableNameContrast);
        }

        // GET: TableNameContrast/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableNameContrast tableNameContrast = db.TableNameContrasts.Find(id);
            if (tableNameContrast == null)
            {
                return HttpNotFound();
            }
            return View(tableNameContrast);
        }

        // POST: TableNameContrast/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TableNameContrast tableNameContrast = db.TableNameContrasts.Find(id);
            db.TableNameContrasts.Remove(tableNameContrast);
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
