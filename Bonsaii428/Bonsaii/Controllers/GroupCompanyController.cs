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
    public class GroupCompanyController : BaseController
    {
        //private BonsaiiDbContext db;

        //public GroupCompanyController()
        //{
        //    db = new BonsaiiDbContext(base.UserData.ConnectionString);
        //}
        private SystemDbContext db = new SystemDbContext(); 

        public GroupCompanyController()
        {
        }
        // GET: GroupCompany
        public ActionResult Index()
        {
            return View(db.GroupCompanies.ToList());
        }

        // GET: GroupCompany/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupCompany groupCompany = db.GroupCompanies.Find(id);
            if (groupCompany == null)
            {
                return HttpNotFound();
            }
            return View(groupCompany);
        }

        // GET: GroupCompany/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GroupCompany/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupCompany groupCompany)        {

            if (ModelState.IsValid)
            {
                db.GroupCompanies.Add(groupCompany);
                groupCompany.CompanyNumber = (new Random().Next(1111, 9999)).ToString();
               
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groupCompany);
        }

        // GET: GroupCompany/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupCompany groupCompany = db.GroupCompanies.Find(id);
            if (groupCompany == null)
            {
                return HttpNotFound();
            }
            return View(groupCompany);
        }

        // POST: GroupCompany/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNumber,FullName,TelNumber,ShortName,EnglishName,LegalRepresentative,EstablishDate,Email,Address,Url,Logo,Remark")] GroupCompany groupCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupCompany);
        }

        // GET: GroupCompany/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupCompany groupCompany = db.GroupCompanies.Find(id);
            if (groupCompany == null)
            {
                return HttpNotFound();
            }
            return View(groupCompany);
        }

        // POST: GroupCompany/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GroupCompany groupCompany = db.GroupCompanies.Find(id);
            db.GroupCompanies.Remove(groupCompany);
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
