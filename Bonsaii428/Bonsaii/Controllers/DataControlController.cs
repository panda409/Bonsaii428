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
    public class DataControlController : BaseController
    {
        // GET: DataControl
        public ActionResult Index()
        {
            ViewBag.TableNameId = new SelectList(db.DataControls, "Id", "Name");
            var dataControls = db.DataControls.Include(s => s.TableName).OrderBy(p => p.TableNameId);
            return View(db.DataControls.ToList());
        }

        // GET: DataControl/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataControl dataControl = db.DataControls.Find(id);
            if (dataControl == null)
            {
                return HttpNotFound();
            }
            return View(dataControl);
        }

        // GET: DataControl/Create
        public ActionResult Create()
        {
            ViewBag.TableNameId = new SelectList(db.TableNameContrasts, "Id", "TableDescription");
          //  List<String> datas = new List<String>();
          //  datas.Add("StaffNumber");

            return View();
        }

        // POST: DataControl/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TableNameId,TableColumn,Description")] DataControl dataControl)
        {
            if (ModelState.IsValid)
            {
                db.DataControls.Add(dataControl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dataControl);
        }

        // GET: DataControl/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataControl dataControl = db.DataControls.Find(id);
            if (dataControl == null)
            {
                return HttpNotFound();
            }
            ViewBag.TableNameId = new SelectList(db.TableNameContrasts, "Id", "TableDescription");
            return View(dataControl);
        }

        // POST: DataControl/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TableNameId,TableColumn,Description")] DataControl dataControl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataControl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dataControl);
        }

        // GET: DataControl/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataControl dataControl = db.DataControls.Find(id);
            if (dataControl == null)
            {
                return HttpNotFound();
            }
            return View(dataControl);
        }

        // POST: DataControl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataControl dataControl = db.DataControls.Find(id);
            db.DataControls.Remove(dataControl);
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
