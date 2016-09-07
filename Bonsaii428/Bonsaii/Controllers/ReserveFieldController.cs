
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
    public class ReserveFieldController : BaseController
    {
        // GET: TableNameContrast
        public ActionResult IndexTableName()
        {
            return View(db.TableNameContrasts.ToList());
        }

        // GET: ReserveRecord
        public ActionResult Index()
        {
            ViewBag.TableNameId = new SelectList(db.ReserveFields,"Id","Name");
            var reserveFields = db.ReserveFields.Include(s => s.TableName).OrderBy(p => p.TableNameId);

            return View(reserveFields.ToList());
        }
        //public ActionResult IndexInfo(string table)
        //{
        //    return View(db.ReserveFields.Where(rf=>rf.TableName.Equals(table)).ToList());
        //}
        // GET: ReserveRecord/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReserveField reserveField = db.ReserveFields.Find(id);
            if (reserveField == null)
            {
                return HttpNotFound();
            }
            return View(reserveField);
        }

        // GET: ReserveRecord/Create
        public ActionResult Create()
        {
            ViewBag.TableNameId = new SelectList(db.TableNameContrasts, "Id", "TableDescription");
            return View();
        }

        // POST: ReserveRecord/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReserveField reserveField)
        {
            if (ModelState.IsValid)
            {
              //  var reservef = db.ReserveFields.Where(rf => rf.TableName.Equals(reserveField.TableName)).ToList();

               // var reservef = db.TableNameContrasts.Where(rf=>rf.Id.E)
                //var reservef = (from p in db.ReserveFields
                //                join q in db.TableNameContrasts on p.TableNameId equals q.Id
                //                select p).ToList();
                var reservef = (from p in db.ReserveFields where p.Id == reserveField.TableNameId select p).ToList();         

                foreach(var temp in reservef)
                {
                    if(temp.FieldName==reserveField.FieldName)
                    {
                        ModelState.AddModelError("", "抱歉，该字段已经被注册！");
                        return View(reserveField);
                    }
                    if (temp.Description == reserveField.Description)
                    {
                        ModelState.AddModelError("", "抱歉，该字段的描述已经被注册！");
                        return View(reserveField);
                    }
                }
                reserveField.RecordPerson = this.Name;
                reserveField.RecordTime = DateTime.Now;
                db.ReserveFields.Add(reserveField);
            
                db.SaveChanges();
                return RedirectToAction("Index", new { table = reserveField.TableName });
            }

            return View(reserveField);
        }

        // GET: ReserveRecord/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReserveField reserveField = db.ReserveFields.Find(id);
            if (reserveField == null)
            {
                return HttpNotFound();
            }
            ViewBag.TableNameId = new SelectList(db.TableNameContrasts, "Id", "TableDescription");
            return View(reserveField);
        }

        // POST: ReserveRecord/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReserveField reserveField)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserveField).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { table = reserveField .TableName});
            }
            return View(reserveField);
        }

        // GET: ReserveRecord/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReserveField reserveField = db.ReserveFields.Find(id);
            if (reserveField == null)
            {
                return HttpNotFound();
            }
            ViewBag.TableNameId = new SelectList(db.TableNameContrasts, "Id", "TableDescription");
            return View(reserveField);
        }

        // POST: ReserveRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //确保固定字段表、变化字段表和变化字段存储表的数据一致性。
            ReserveField reserveField = db.ReserveFields.Find(id);

            var item = (from p in db.TableNameContrasts where p.Id == reserveField.TableNameId 
                        select p.TableName).ToList().FirstOrDefault();

            //var item = (from p in db.ReserveFields
            //                               join q in db.TableNameContrasts on p.TableNameId equals reserveField.Id
            //                                      select q.TableName).ToList().FirstOrDefault();

            if (item == "StaffArchives")
            {              
                foreach (StaffArchiveReserve t in db.StaffArchiveReserves.Where(sarq => sarq.FieldId.Equals(id)).ToList())
                {
                    db.StaffArchiveReserves.Remove(t);
                    db.SaveChanges();
                }
            }
            if (item == "StaffChanges")
            {                
                foreach (StaffChangeReserve t in db.StaffChangeReserves.Where(scr=> scr.FieldId.Equals(id)).ToList())
                {
                    db.StaffChangeReserves.Remove(t);
                    db.SaveChanges();
                }
            }
            if (item == "Staffs")
            {
                var staffReserves = (from p in db.StaffReserves where p.FieldId == id select p).ToList();
                foreach (var staffReserve in staffReserves)
                {
                    db.StaffReserves.Remove(staffReserve);
                   
                }
                db.SaveChanges();
                //foreach (StaffReserve t in db.StaffReserves.Where(sr => sr.FieldId.Equals(id)).ToList())
                //{
                //    db.StaffReserves.Remove(t);
                //    db.SaveChanges();
                //}
            }
            if (item == "StaffSkills")
            {
                foreach (StaffSkillReserve t in db.StaffSkillReserves.Where(skr => skr.FieldId.Equals(id)).ToList())
                {
                    db.StaffSkillReserves.Remove(t);
                    db.SaveChanges();
                }
            }
            if (item == "Departments")
            {
                foreach (DepartmentReserve t in db.DepartmentReserves.Where(dr => dr.FieldId.Equals(id)).ToList())
                {
                    db.DepartmentReserves.Remove(t);
                    db.SaveChanges();
                }
            }
            if (item == "StaffApplications")
            {
                foreach (StaffApplicationReserve t in db.StaffApplicationReserves.Where(sar => sar.FieldId.Equals(id)).ToList())
                {
                    db.StaffApplicationReserves.Remove(t);
                    db.SaveChanges();
                }
            }
            db.ReserveFields.Remove(reserveField);
            db.SaveChanges();
            return RedirectToAction("Index", new { table = reserveField.TableName });
        }

        //显示的是TableNameContrasts中的TableNameId
        ////[HttpPost]
        ////public JsonResult TableNameSearch(string name)
        ////{
        ////    try
        ////    {
        ////        var items = (from p in db.ReserveFields
        ////                     join q in db.TableNameContrasts on p.TableNameId equals q.Id
        ////                     where q.TableName == name
        ////                     select new
        ////                     {
        ////                         text = q.TableDescription,
        ////                         //  id = sp.Id
        ////                         id = p.TableNameId
        ////                     }).ToList();

        ////        return Json(items);
        ////    }
        ////    catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        ////}


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
