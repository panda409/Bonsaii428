using Bonsaii.Models.Checking_in;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class MonthSignInDatesController : BaseController
    {
        public ActionResult Index()
        {
            List<MonthSignInViewModel> list = (from x in db.MonthSignIns
                                                      join y in db.Staffs on x.StaffNumber equals y.StaffNumber
                                                      join z in db.Departments on y.Department equals z.DepartmentId
                                                      orderby x.Date descending
                                               select new MonthSignInViewModel()
                                                      {
                                                          Id = x.Id,
                                                          Date = x.Date,
                                                          StaffNumber = x.StaffNumber,
                                                          StaffName = y.Name,
                                                          DepartmentName = z.Name,
                                                          NormalWorkHours = x.NormalWorkHours,


                                                          NormalWorkDays = x.NormalWorkDays,
                                                          OvertimeApplyHours = x.OvertimeApplyHours,
                                                          NormalOvertimeHours = x.NormalOvertimeHours,
                                                          HolidayOvertimeHours = x.HolidayOvertimeHours,
                                                          OtherOvertimeHours = x.OtherOvertimeHours,
                                                          TotalOvertimeHours = x.TotalOvertimeHours,
                                                          ComeLateMinutes = x.ComeLateMinutes,
                                                          ComeLateTimes = x.ComeLateTimes,
                                                          LeaveEarlyMinutes = x.LeaveEarlyMinutes,
                                                          LeaveEarlyTimes = x.LeaveEarlyTimes,
                                                          AbsentHours = x.AbsentHours,
                                                          AbsentTimes = x.AbsentTimes,
                                                          HolidayDays = x.HolidayDays,
                                                          VacateHours = x.VacateHours,
                                                          VacateDays = x.VacateDays,
                                                          VacateTimes = x.VacateTimes,
                                                      }).ToList();
            return View(list);
        }

    }
}