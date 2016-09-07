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
    public class StaffArchiveController : BaseController
    {
        //public ActionResult ArchiveIndex(){
        //    List<Staff> Staffs = (from p in db.Staffs orderby p.BillNumber where p.ArchiveTag == true select p).ToList();//选择的是离职的员工
        //    foreach (Staff tmp in Staffs)
        //    {
        //        tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
        //        tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
        //    }
        //    StaffArchiveIndexViewModel staffArchiveModel = new StaffArchiveIndexViewModel();
        //    staffArchiveModel.AuditPerson
        //    {
            
            
            
        //    }
        //    return View(Staffs);
        //}
        // GET: StaffArchive
        [Authorize(Roles = "Admin,StaffArchive_Index")]
        public ActionResult Index()
        {

            var fieldNameList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id
                                 where q.TableName == "StaffArchives" && p.Status == true
                                 select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找预留字段(value)*/
            var fieldValueList = (from sa in db.StaffArchiveReserves
                                  join rf in db.ReserveFields on sa.FieldId equals rf.Id where rf.Status == true
                                  select new StaffArchiveViewModel { Id = sa.Number, Description = rf.Description, Value = sa.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(db.StaffArchives.ToList().OrderByDescending(c=>c.Id));
        }

        // GET: StaffArchive/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            if (staffArchive == null)
            {
                return HttpNotFound();
            }
            /*表预留字段*/
            var fieldValueList = (from sar in db.StaffArchiveReserves
                                  join rf in db.ReserveFields on sar.FieldId equals rf.Id
                                  where sar.Number == id  &&  rf.Status == true 
                                  select new StaffArchiveViewModel { Id = sar.Number, Description = rf.Description, Value = sar.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(staffArchive);
        }



        // GET: StaffArchive/Create
        [Authorize(Roles = "Admin,StaffArchive_Create")]
        public ActionResult Create()
        {
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                             where q.TableName == "StaffArchives" && p.Status==true select p).ToList();
            ViewBag.fieldList = fieldList;
            ViewBag.date = DateTime.Now;
            return View();
        }

        // POST: StaffArchive/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffArchive_Create")]
        public ActionResult Create(StaffArchive staffArchive)
        {
            if (ModelState.IsValid)
            {
                staffArchive.RecordPerson = this.Name;
                staffArchive.RecordTime = DateTime.Now;
                var billTypeName = (from p in db.BillProperties where p.Type == staffArchive.BillTypeNumber select p.TypeName).ToList().FirstOrDefault();
                staffArchive.BillTypeName = billTypeName;
                staffArchive.BillNumber = GenerateBillNumber(staffArchive.BillTypeNumber);
                db.StaffArchives.Add(staffArchive);
                db.SaveChanges();
                Staff staff = db.Staffs.Where(s => s.StaffNumber.Equals(staffArchive.StaffNumber)).SingleOrDefault();
                staff.ArchiveTag = true;
                db.SaveChanges();
                var fieldList = (from p in db.ReserveFields 
                                     join q in db.TableNameContrasts
                                   on p.TableNameId equals q.Id
                                 where q.TableName == "StaffArchives"&& p.Status==true  select p).ToList();
                ViewBag.fieldList = fieldList;

                /*遍历，保存员工基本信息预留字段*/
                foreach (var temp in fieldList)
                {
                    StaffArchiveReserve sr = new StaffArchiveReserve();
                    sr.Number = staffArchive.Id;
                    sr.FieldId = temp.Id;
                    sr.Value = Request[temp.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (sr.Value == null) sr.Value = " ";
                    db.StaffArchiveReserves.Add(sr);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                var fieldList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                                 where q.TableName == "StaffArchives" && p.Status==true select p).ToList();
                ViewBag.fieldList = fieldList;
                return View(staffArchive);
            }

        }

        // GET: StaffArchive/Edit/5
        [Authorize(Roles = "Admin,StaffArchive_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            if (staffArchive == null)
            {
                return HttpNotFound();
            }
            /*查找表预留字段*/
            var fieldValueList = (from sar in db.StaffArchiveReserves
                             join rf in db.ReserveFields on sar.FieldId equals rf.Id
                                  where sar.Number == id && rf.Status == true
                             select new StaffArchiveViewModel { Description = rf.Description, Value = sar.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(staffArchive);
        }

        // POST: StaffArchive/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffArchive_Edit")]
        public ActionResult Edit(StaffArchive staffArchive)
        {
            if (ModelState.IsValid)
            {
                /*查找预留字段(value)*/
                var fieldValueList = (from sar in db.StaffArchiveReserves
                                      join rf in db.ReserveFields on sar.FieldId equals rf.Id
                                      where sar.Number == staffArchive.Id && rf.Status==true
                                      select new StaffArchiveViewModel { Id = sar.Id, Description = rf.Description, Value = sar.Value }).ToList();
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    StaffArchiveReserve sar = db.StaffArchiveReserves.Find(temp.Id);
                    sar.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                StaffArchive archive = db.StaffArchives.Find(staffArchive.Id);
                archive.ReApplyDate = staffArchive.ReApplyDate;
                archive.BlackList = staffArchive.BlackList;
                archive.WorkPlus = staffArchive.WorkPlus;
                archive.Remark = staffArchive.Remark;
                archive.Tag = staffArchive.Tag;
                archive.ChangePerson = this.Name;
                archive.ChangeTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffArchive);
        }

        // GET: StaffArchive/Delete/5
        [Authorize(Roles = "Admin,StaffArchive_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            if (staffArchive == null)
            {
                return HttpNotFound();
            }
            /*查表预留字段(value)*/
            var fieldValueList = (from sar in db.StaffArchiveReserves
                                  join rf in db.ReserveFields on sar.FieldId equals rf.Id
                                  where sar.Number == id && rf.Status == true
                                  select new StaffArchiveViewModel { Id = sar.Number, Description = rf.Description, Value = sar.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(staffArchive);
        }

        // POST: StaffArchive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffArchive_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from sar in db.StaffArchiveReserves
                        where sar.Number == id
                        select new StaffArchiveViewModel { Id = sar.Id }).ToList();
            foreach (var temp in item)
            {
                StaffArchiveReserve sar = db.StaffArchiveReserves.Find(temp.Id);
                db.StaffArchiveReserves.Remove(sar);

            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            StaffArchive staffArchive = db.StaffArchives.Find(id);
            db.StaffArchives.Remove(staffArchive);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch()
        {

            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == "23"
                             select new
                             {
                                 text = b.Type + " " + b.TypeName,
                                 id = b.Type
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        //可以提取成公共方法
        [HttpPost]
        public JsonResult BillTypeNumber(string number)
        {

            string temp = Generate.GenerateBillNumber(number, this.ConnectionString);
            try
            {
                var items = (from p in db.BillProperties
                             where p.Type.Contains(number) || p.TypeName.Contains(number)
                             select new
                             {
                                 billNumber = temp,
                                 billTypeName=p.TypeName
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        [HttpPost]
        public JsonResult StaffSearch()
        {

            try
            {
                var items = (from p in db.StaffApplications
                             where(p.AuditStatus == 3)
                             select
                                 new { 
                                 text=p.StaffNumber+""+p.StaffName,
                                 id=p.StaffNumber
                                 }
                    ).ToList();

                //var items = (from s in db.Staffs
                //             join d in db.Departments on s.Department equals d.DepartmentId
                //             into gc
                //             from d in gc.DefaultIfEmpty()
                //             where s.ArchiveTag==true //离职状态为true
                //             select new
                //             {
                //                 text = s.StaffNumber + " " + s.Name,
                //                 id = s.StaffNumber
                //             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }

        [HttpPost]
        public JsonResult StaffNumber(string number)
        {
            try
            {
                var items = (from s in db.Staffs
                             join d in db.Departments on s.Department equals d.DepartmentId
                             where s.StaffNumber == number
                             from p in db.StaffApplications where p.StaffNumber == number

                             select new
                             {
                                 name = s.Name,
                                 departmentName = d.Name,
                                 identicationNumber = s.IdentificationNumber,
                                 staffNumber=s.StaffNumber,
                                 billTypeNumber =p.BillTypeNumber,
                                 billTypeName = p.BillTypeName
                                
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

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
