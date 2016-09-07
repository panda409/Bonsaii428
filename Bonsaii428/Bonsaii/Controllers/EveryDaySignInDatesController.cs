using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels.GlobalStaticVaribles;
using Bonsaii.Models.Checking_in;

namespace Bonsaii.Controllers
{
    public class EveryDaySignInDatesController : BaseController
    {
        // GET: EveryDaySignInDates
        public ActionResult Index()
        {
            List<EveryDaySignInDateViewModel> list = (from x in db.EveryDaySignInDates
                                                      join y in db.Staffs on x.StaffNumber equals y.StaffNumber
                                                      join z in db.Departments on y.Department equals z.DepartmentId
                                                      orderby x.Date descending
                                                      select new EveryDaySignInDateViewModel()
                                                      {
                                                          Id = x.Id,
                                                          Date = x.Date,
                                                          StaffNumber = x.StaffNumber,
                                                          StaffName = y.Name,
                                                          DepartmentName = z.Name,
                                                          WorkHours = x.WorkHours,


                                                          WorkDays = x.WorkDays,
                                                          NormalWorkOvertimeHours = x.NormalWorkOvertimeHours,
                                                          WeekendWorkOvertimeHours = x.WeekendWorkOvertimeHours,
                                                          VacateType = x.VacateType,
                                                          VacateHours = x.VacateHours,
                                                          HolidayHours = x.HolidayHours,
                                                          TotalWorkOvertimeHours = x.TotalWorkOvertimeHours,
                                                          TotalComeLateMinutes = x.TotalComeLateMinutes,
                                                          TotalLeaveEarlyMinutes = x.TotalLeaveEarlyMinutes,
                                                          AbsenteeismHours = x.AbsenteeismHours,
                                                          AuditStatus = x.AuditStatus,
                                                          StaffConfirm = x.StaffConfirm,
                                                          IsNightWork = x.IsNightWork,
                                                          WorkOvertimeHours = x.WorkOvertimeHours,
                                                          OriginalSignInData = x.OriginalSignInData,
                                                          Remark = x.Remark,
                                                          IsOnEvection = x.IsOnEvection,
                                                      }).ToList();
            return View(list);
        }

        // GET: EveryDaySignInDates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EveryDaySignInDateViewModel result = (from x in db.EveryDaySignInDates
                                                              where x.Id == id
                                                              join y in db.Staffs on x.StaffNumber equals y.StaffNumber
                                                              join z in db.Departments on y.Department equals z.DepartmentId
                                                              join p in db.Works on x.WorksId equals p.Id
                                                              select new EveryDaySignInDateViewModel()
                                                              {
                                                                  Week = x.Week,
                                                                  Date = x.Date,
                                                                  StaffNumber = x.StaffNumber,
                                                                  StaffName = y.Name,
                                                                  CompanyName = CompanyFullName,
                                                                  DepartmentName = z.Name,
                                                                  Position = y.Position,
                                                                  WorksId = x.WorksId,
                                                                  WorkName = p.Name,
                                                                  WorkHours = x.WorkHours,
                                                                  WorkDays = x.WorkDays,
                                                                  NormalWorkOvertimeHours = x.NormalWorkOvertimeHours,
                                                                  WeekendWorkOvertimeHours = x.WeekendWorkOvertimeHours,
                                                                  VacateType = x.VacateType,
                                                                  VacateHours = x.VacateHours,
                                                                  HolidayHours = x.HolidayHours,
                                                                  TotalWorkOvertimeHours = x.TotalWorkOvertimeHours,
                                                                  TotalComeLateMinutes = x.TotalComeLateMinutes,
                                                                  TotalLeaveEarlyMinutes = x.TotalLeaveEarlyMinutes,
                                                                  AbsenteeismHours = x.AbsenteeismHours,
                                                                  AuditStatus = x.AuditStatus,
                                                                  StaffConfirm = x.StaffConfirm,
                                                                  IsNightWork = x.IsNightWork,
                                                                  WorkOvertimeHours = x.WorkOvertimeHours,
                                                                  OriginalSignInData = x.OriginalSignInData,
                                                                  Remark = x.Remark,
                                                                  IsOnEvection = x.IsOnEvection,
                                                              }).Single();

            result.SignInCard = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(result.StaffNumber) && p.WorkDate == result.Date).OrderBy(x => x.NeedWorkTime).ToList();

            result.SignInCard = db.SignInCardStatus.Where(p => p.WorkDate == result.Date && p.StaffNumber.Equals(result.StaffNumber)).OrderBy(p=>p.NeedWorkTime).ToList();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
        // GET: EveryDaySignInDates/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: EveryDaySignInDates/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Week,Date,StaffNumber,WorksId,WorkHours,WorkDays,NormalWorkOvertimeHours,WeekendWorkOvertimeHours,VacateType,VacateHours,HolidayHours,TotalWorkOvertimeHours,TotalComeLateMinutes,TotalLeaveEarlyMinutes,AbsenteeismHours,AuditStatus,StaffConfirm,IsNightWork,WorkOvertimeHours,OriginalSignInData,Remark,IsOnEvection")] EveryDaySignInDate everyDaySignInDate)
        {
            if (ModelState.IsValid)
            {
                db.EveryDaySignInDates.Add(everyDaySignInDate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(everyDaySignInDate);
        }

        // GET: EveryDaySignInDates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EveryDaySignInDate everyDaySignInDate = db.EveryDaySignInDates.Find(id);
            if (everyDaySignInDate == null)
            {
                return HttpNotFound();
            }
            return View(everyDaySignInDate);
        }

        // POST: EveryDaySignInDates/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Week,Date,StaffNumber,WorksId,WorkHours,WorkDays,NormalWorkOvertimeHours,WeekendWorkOvertimeHours,VacateType,VacateHours,HolidayHours,TotalWorkOvertimeHours,TotalComeLateMinutes,TotalLeaveEarlyMinutes,AbsenteeismHours,AuditStatus,StaffConfirm,IsNightWork,WorkOvertimeHours,OriginalSignInData,Remark,IsOnEvection")] EveryDaySignInDate everyDaySignInDate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(everyDaySignInDate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(everyDaySignInDate);
        }

        // GET: EveryDaySignInDates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EveryDaySignInDate everyDaySignInDate = db.EveryDaySignInDates.Find(id);
            if (everyDaySignInDate == null)
            {
                return HttpNotFound();
            }
            return View(everyDaySignInDate);
        }

        // POST: EveryDaySignInDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EveryDaySignInDate everyDaySignInDate = db.EveryDaySignInDates.Find(id);
            db.EveryDaySignInDates.Remove(everyDaySignInDate);
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
