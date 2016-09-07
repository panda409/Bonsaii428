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
using BonsaiiModels.Staffs;

namespace Bonsaii.Controllers
{
    public class HolidayTablesController : BaseController
    {
        // GET: HolidayTables
        public ActionResult Index()
        {

            return View(db.HolidayTables.ToList());
        }

        // GET: HolidayTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTables holidayTables = db.HolidayTables.Find(id);
            if (holidayTables == null)
            {
                return HttpNotFound();
            }
            return View(holidayTables);
        }

        // GET: HolidayTables/Create
        public ActionResult Create()
        {
            ViewBag.list = GetHolidayTypes();
            return View();
        }

        // POST: HolidayTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StaffNumber,Date,Type,StartHour,EndHour,Remark")] HolidayTables holidayTables)
        {
            if (ModelState.IsValid)
            {
                db.HolidayTables.Add(holidayTables);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holidayTables);
        }

        // GET: HolidayTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTables holidayTables = db.HolidayTables.Find(id);
            if (holidayTables == null)
            {
                return HttpNotFound();
            }
            return View(holidayTables);
        }

        // POST: HolidayTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,Date,Type,StartHour,EndHour,Remark")] HolidayTables holidayTables)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holidayTables).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holidayTables);
        }

        // GET: HolidayTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTables holidayTables = db.HolidayTables.Find(id);
            if (holidayTables == null)
            {
                return HttpNotFound();
            }
            return View(holidayTables);
        }

        // POST: HolidayTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolidayTables holidayTables = db.HolidayTables.Find(id);
            db.HolidayTables.Remove(holidayTables);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonalIndex()
        {
            List<HolidayTableViewModel> list = (from x in db.HolidayTables
                                                where x.Flag == false
                                                join y in db.Staffs on x.StaffNumber equals y.StaffNumber
                                                select new HolidayTableViewModel()
                                                {
                                                    StaffNumber = x.StaffNumber,
                                                    Date = x.Date,
                                                    StartHour = x.StartHour,
                                                    EndHour = x.EndHour,
                                                    Remark = x.Remark,
                                                    Flag = x.Flag,
                                                    StaffName = y.Name
                                                }).ToList();
            //List<HolidayTableViewModel> list = db.HolidayTables.Where(p => p.Flag == false).Select(p => new HolidayTableViewModel()
            //{
            //    StaffNumber = p.StaffNumber,
            //    Date = p.Date,
            //    StartHour = p.StartHour,
            //    EndHour = p.EndHour,
            //    Remark = p.Remark,
            //    Flag = p.Flag,
            //}).ToList();

            return View(list);
        }
        public ActionResult PersonalCreate()
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber// + "-" + s.Name         //显示框中显示的选中值直接就是员工号，不带姓名的
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            ViewBag.list = GetHolidayTypes();
            return View();
        }
        [HttpPost]
        public ActionResult PersonalCreate([Bind(Include = "Id,Staffs,Date,StartHour,EndHour,Remark,Flag,Type")] HolidayTables holidayTables)
        {
            var staff = (from s in db.Staffs
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber// + "-" + s.Name         //显示框中显示的选中值直接就是员工号，不带姓名的
                         }).ToList();

            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;
            ViewBag.list = GetHolidayTypes();
            if (ModelState.IsValid)
            {
                List<string> numbers = holidayTables.Staffs.Split(',').ToList();
                List<HolidayTables> tmpHoliday = new List<HolidayTables>();
                foreach (string tmpStaffNumber in numbers)
                {
                    db.HolidayTables.RemoveRange(db.HolidayTables.Where(p => p.StaffNumber.Equals(tmpStaffNumber) && p.Date == holidayTables.Date).ToList());
                    db.SaveChanges();
                    tmpHoliday.Add(new HolidayTables()
                    {
                        Flag = false,        //标示是针对个人的排班
                        StaffNumber = tmpStaffNumber,
                        DepartmentId = db.Staffs.Where(p => p.StaffNumber.Equals(tmpStaffNumber)).Single().Department,
                        Date = holidayTables.Date,
                        StartHour = holidayTables.StartHour,
                        EndHour = holidayTables.EndHour,
                        Type = holidayTables.Type,
                        Remark = holidayTables.Remark,
                    });

                }
                db.HolidayTables.AddRange(tmpHoliday);
                db.SaveChanges();
                return RedirectToAction("PersonalIndex");
            }
            ViewBag.list = GetHolidayTypes();
            return View(holidayTables);
        }
        public ActionResult DepartmentIndex()
        {
            //       var list = db.HolidayTables.Where(p => p.Flag == true).Select(p => new { p.DepartmentId,p.Date,p.StartHour,p.EndHour}).GroupBy(p => p.DepartmentId).ToList();
            List<DepartmentHolidayViewModel> list = (from x in db.HolidayTables
                                                     join y in db.Departments on x.DepartmentId equals y.DepartmentId
                                                     select new DepartmentHolidayViewModel
                                                    {
                                                        DepartmentId = x.DepartmentId,
                                                        DepartmentName = y.Name,
                                                        Date = x.Date,
                                                        StartHour = x.StartHour,
                                                        EndHour = x.EndHour,
                                                        Type = x.Type,
                                                        Remark = x.Remark
                                                    }).Distinct().ToList();
            return View(list);
        }
        public ActionResult DepartmentCreate()
        {
            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name,text = d.Name,value = d.DepartmentId }).ToList();

            ViewBag.Group = group;
            ViewBag.DepartmentsList = Generate.GetDepartments(base.ConnectionString);
            ViewBag.list = GetHolidayTypes();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkManages_DepartmentCreate")]
        public ActionResult DepartmentCreate([Bind(Include = "Id,DptIds,Date,StartHour,EndHour,Remark,Type")] HolidayTables holidayTable)
        {
            if (ModelState.IsValid)
            {
                List<string> dptIds = holidayTable.DptIds.Split(',').ToList();

                foreach (string tmp in dptIds)
                {
                    //删除原来所有的这个部门在这一天的放假情况
                    List<HolidayTables> tmpList = db.HolidayTables.Where(p => p.DepartmentId.Equals(tmp) && p.Date == holidayTable.Date).ToList();
                    db.HolidayTables.RemoveRange(tmpList);
                    db.SaveChanges();

                    List<string> StaffNubmers = db.Staffs.Where(p => p.Department.Equals(tmp)).Select(p => p.StaffNumber).ToList();
                    tmpList.Clear();
                    foreach (string tmpStaffNumber in StaffNubmers)
                    {

                        tmpList.Add(new HolidayTables()
                        {
                            Flag = true,        //标示是针对部门的假日表
                            DepartmentId = tmp,
                            StaffNumber = tmpStaffNumber,
                            StartHour = holidayTable.StartHour,
                            EndHour = holidayTable.EndHour,
                            Remark = holidayTable.Remark,
                            Date = holidayTable.Date,
                            Type = holidayTable.Type,
                        });
                    }
                    db.HolidayTables.AddRange(tmpList);
                    db.SaveChanges();
                }
                return RedirectToAction("DepartmentIndex");
            }
            ViewBag.list = GetHolidayTypes();
            ViewBag.DepartmentsList = Generate.GetDepartments(base.ConnectionString);
            return View(holidayTable);
        }
        public static SelectList GetHolidayTypes()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "普通双休", Text = "普通双休" },
                new SelectListItem() { Value = "法定假日", Text = "法定假日" },
            };
            return new SelectList(list, "Value", "Text");
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
