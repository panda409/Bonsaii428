using Bonsaii.Models;
using Bonsaii.Models.Checking_in;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class CalendarController : BaseController
    {
        [Authorize(Roles = "Admin,Calendar_Index")]
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Tmp()
        {
            List<Staff> Staffs = (from p in db.Staffs orderby p.BillNumber where p.ArchiveTag == false select p).ToList();
            //p.JobState != "离职" select p).ToList();
            foreach (Staff tmp in Staffs)
            {
                tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
            }




            return View(Staffs);//使用ToPagedList方法时，需要引入using PagedList系统集成的分页函数。
        }


        [Authorize(Roles = "Admin,Calendar_Index")]
        public JsonResult GetWorkDays()
        {
            int year = int.Parse(Request["year"]);
            int month = int.Parse(Request["month"]);

            string number = Request["number"];

            int DaysInMonth = DateTime.DaysInMonth(year, month);
            DateTime FirstDay = new DateTime(year, month, 1);
            DateTime LastDay = new DateTime(year, month, DaysInMonth);
            List<string> monthDays = new List<string>();
            for (int i = FirstDay.Day; i <= LastDay.Day; i++)
                monthDays.Add("未排班");
            //获取一个月内的班次名称
            List<DateTime> WorkDays = (from x in db.WorkManages
                                     join y in db.Works on x.WorksId equals y.Id
                                     where x.Date <= LastDay && x.Date >= FirstDay && x.StaffNumber.Equals(number)
                                     orderby x.Date
                                      select x.Date).ToList();
            foreach (DateTime tmpDate in WorkDays)
                monthDays[tmpDate.Day - 1] = "工作";
            var holidays = (from x in db.HolidayTables
                            where x.Date <= LastDay && x.Date >= FirstDay && x.StaffNumber.Equals(number)
                            select new
                            {
                                x.Type,
                                x.StartHour,
                                x.EndHour,
                                x.Date
                            });

            //找出这个员工在这个月内的请假的情况 
            List<VacateApplies> vacates = (from x in db.VacateApplies
                           where x.StaffNumber.Equals(number) && ((x.EndDateTime <= LastDay && x.EndDateTime >= FirstDay) || (x.StartDateTime <= LastDay && x.StartDateTime >= FirstDay))
                           select x).ToList();
            foreach (VacateApplies tmpVacate in vacates)
            {
                DateTime fDay = FirstDay <= tmpVacate.StartDateTime.Date ? tmpVacate.StartDateTime.Date : FirstDay;
                DateTime lDay = LastDay <= tmpVacate.EndDateTime.Date ? LastDay : tmpVacate.EndDateTime.Date;
                int beginDay = fDay.Day;
                int endDay = lDay.Day;
                for (int i=beginDay;i<=endDay;i++)
                    monthDays[i - 1] = "请假";
            }
            //var daysOff = (from x in db.DaysOffApplies
            //               where x.Date <= LastDay && x.Date >= FirstDay && x.StaffNumber.Equals(number)
            //               select x
            //                   ).ToList();
            //注意数组是从0开始
            foreach (var tmp in holidays)
                monthDays[tmp.Date.Day - 1] = "休息";
            //foreach (var tmp in daysOff)
            //    WorkDays[tmp.Date.Day - 1] = "调休";

            return Json(new
            {
                Days = monthDays,
                LastDay = DaysInMonth
            });
        }
        [Authorize(Roles = "Admin,Calendar_Index")]
        /// <summary>
        /// 获取某一天具体的工作时间情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWorkTime()
        {
            DateTime TmpDate = Convert.ToDateTime(Request["date"]);
            string TmpNumber = Request["number"];
            var WorkTime = (from x in db.WorkManages
                            join y in db.WorkTimes on x.WorksId equals y.WorksId
                            where x.StaffNumber.Equals(TmpNumber) && x.Date == TmpDate
                            orderby y.StartTime
                            select new
                            {
                                StartTime = y.StartTime,
                                EndTime = y.EndTime,
                            }).ToList();
            //放假的情况
            var holiday = (from x in db.HolidayTables
                           where x.StaffNumber.Equals(TmpNumber) && x.Date == TmpDate
                           select new
                           {
                               StartHour = x.StartHour,
                               EndHour = x.EndHour
                           }).ToList();
            //调休的情况
            var daysOff = (from x in db.DaysOffApplies
                           where x.StaffNumber.Equals(TmpNumber) && x.Date == TmpDate
                           select new
                           {
                               StartHour = x.StartDateTime.Hour,
                               EndHour = x.EndDateTime.Hour
                           }).ToList();
            List<string> result = new List<string>();
            //处理半天放假的情况
            if (holiday.Count != 0)
            {
                foreach (var tmp in WorkTime)
                    if (tmp.StartTime.Hours >= holiday[0].EndHour || tmp.EndTime.Hours <= holiday[0].StartHour)
                        result.Add(tmp.StartTime.ToString() + "-" + tmp.EndTime.ToString());
            }
            else if (daysOff.Count != 0)
            {
                foreach (var tmp in WorkTime)
                    if (tmp.StartTime.Hours >= daysOff[0].EndHour || tmp.EndTime.Hours <= daysOff[0].StartHour)
                        result.Add(tmp.StartTime.ToString() + "-" + tmp.EndTime.ToString());
            }
            else
                foreach (var tmp in WorkTime)
                    result.Add(tmp.StartTime.ToString() + "-" + tmp.EndTime.ToString());

                
            //今天是工作日
            if (result.Count != 0)
                return Json(new
                {
                    flag = 1,
                    data = result
                });
            //没有这个人的排班情况和放假情况
            if (result.Count == 0 && holiday.Count == 0)
                return Json(new
                {
                    flag = 2
                });
            //今天是休息日
            return Json(new
            {
                flag = 0,
            });
        }

    }
}