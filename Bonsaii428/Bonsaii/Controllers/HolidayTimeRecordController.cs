using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.Works;

namespace Bonsaii.Controllers
{
    public class HolidayTimeRecordController : BaseController
    {
        // GET: HolidayTimeRecord
        public ActionResult Index()
        {
            //var holidayRecord = (from htr in db.HolidayTimeRecords
            //                     join s in db.Staffs on htr.Number equals s.StaffNumber
            //                     join htn in db.HolidayTimeNames on htr.Tag equals htn.Id.ToString()
            //                     select new HolidayTimeRecordViewModel{Id=htr.Id, Number = htr.Number, Name = s.Name, RecordTime = htr.RecordTime, Tag = htn.Name }).ToList();
            return View();
        }
        public ActionResult IndexPerson()
        {
            var staff = (from s in db.Staffs
                         join d in db.Departments on s.Department equals d.DepartmentId
                         into gc
                         from d in gc.DefaultIfEmpty()
                         select new StaffViewModel { StaffNumber = s.StaffNumber, Name = s.Name, Department = d.Name }).ToList();
            return View(staff);
            //var holidayRecord = (from htr in db.HolidayTimeRecords
            //                     join s in db.Staffs on htr.Number equals s.StaffNumber
            //                     join htn in db.HolidayTimeNames on htr.Tag equals htn.Id.ToString()
            //                     select new HolidayTimeRecordViewModel { Id = htr.Id, Number = htr.Number, Name = s.Name, RecordTime = htr.RecordTime, Tag = htn.Name }).ToList();
            //return View(holidayRecord);
        }
        public ActionResult IndexCompany()
        {

            return View(db.Departments.ToList());
        }

        // GET: HolidayTimeRecord/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTimeRecord holidayTimeRecord = db.HolidayTimeRecords.Find(id);
            if (holidayTimeRecord == null)
            {
                return HttpNotFound();
            }
            var htr1 = db.HolidayTimeNames.Where(htr => htr.Id.ToString().Equals(holidayTimeRecord.Tag)).SingleOrDefault();
            holidayTimeRecord.Tag = htr1.Name;
            return View(holidayTimeRecord);
        }
        public ActionResult CreateCompany()
        {
            List<SelectListItem> htr = db.HolidayTimeNames.ToList().Select(h => new SelectListItem
            {

                Value = h.Id.ToString(),
                Text = h.Name
            }).ToList();

            ViewBag.holiday = htr;
            List<SelectListItem> department = db.Departments.ToList().Select(d => new SelectListItem
            {

                Value = d.DepartmentId.ToString(),
                Text = d.Name
            }).ToList();
            ViewBag.department = department;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCompany([Bind(Include = "Nian,Range,Week1,Week2,Week3,week4,Week5,Week6,Week7")] WeekTag weektag)
        {

            string date = weektag.Nian;
            DateTime begindate = Convert.ToDateTime(date + "-01-01");
            //DateTime begindate = Convert.ToDateTime(date+"01-01");
            //DateTime enddate = Convert.ToDateTime(date+"12-31");
            //System.TimeSpan tsdiffer = enddate.Date - begindate.Date;
            // int intdiffer = tsdiffer.Days + 1;
            int intdiffer;
            if (DateTime.IsLeapYear(int.Parse(weektag.Nian)))
            {
                intdiffer = 366;
            }
            else
                intdiffer = 365;

            List<DateTime> list = new List<DateTime>();
            if (weektag.Range == "1")
            {
                for (int i = 0; i < intdiffer; i++)
                {
                    DateTime dttemp = begindate.Date.AddDays(i);

                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "1" == weektag.Week1) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "1" == weektag.Week2) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "1" == weektag.Week3) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "1" == weektag.Week4) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Friday && "1" == weektag.Week5) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "1" == weektag.Week6) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "1" == weektag.Week7))
                    {

                        var department = db.Departments.ToList();
                        var staff = db.Staffs.ToList();
                        foreach (var temp in department)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp.DepartmentId;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "1";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();
                        }
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp1.StaffNumber;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "1";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();

                        }
                    }
                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "2" == weektag.Week1) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "2" == weektag.Week2) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "2" == weektag.Week3) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "2" == weektag.Week4) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Friday && "2" == weektag.Week5) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "2" == weektag.Week6) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "2" == weektag.Week7))
                    {
                        var department = db.Departments.ToList();
                        var staff = db.Staffs.ToList();
                        foreach (var temp in department)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp.DepartmentId;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "2";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();
                        }
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp1.StaffNumber;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "2";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();

                        }
                    }
                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "3" == weektag.Week1) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "3" == weektag.Week2) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "3" == weektag.Week3) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "3" == weektag.Week4) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Friday && "3" == weektag.Week5) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "3" == weektag.Week6) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "3" == weektag.Week7))
                    {
                        var department = db.Departments.ToList();
                        var staff = db.Staffs.ToList();
                        foreach (var temp in department)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp.DepartmentId;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "3";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();
                        }
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp1.StaffNumber;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "3";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();

                        }
                    }
                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "4" == weektag.Week1) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "4" == weektag.Week2) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "4" == weektag.Week3) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "4" == weektag.Week4) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Friday && "4" == weektag.Week5) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "4" == weektag.Week6) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "4" == weektag.Week7))
                    {
                        var department = db.Departments.ToList();
                        var staff = db.Staffs.ToList();
                        foreach (var temp in department)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp.DepartmentId;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "4";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();
                        }
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                            holidayTime.Number = temp1.StaffNumber;
                            holidayTime.RecordTimeHoliday = dttemp;
                            holidayTime.Tag = "4";
                            db.HolidayTimeRecords.Add(holidayTime);
                            db.SaveChanges();

                        }
                    }

                }
            }
            else
            {
                var htrdepartment = db.HolidayTimeRecords.Where(d => d.Number.Equals(weektag.Range)).ToList();
                foreach (var temp in htrdepartment)
                {
                    HolidayTimeRecord depart = db.HolidayTimeRecords.Find(temp.Id);
                    db.HolidayTimeRecords.Remove(depart);
                }
                db.SaveChanges();
                var staff1 = db.Staffs.Where(s => s.Department.Equals(weektag.Range)).ToList();

                foreach (var temp1 in staff1)
                {
                    var htrdepartment1 = db.HolidayTimeRecords.Where(d => d.Number.Equals(temp1.StaffNumber));
                    foreach (var temp2 in htrdepartment1)
                    {
                        HolidayTimeRecord depart = db.HolidayTimeRecords.Find(temp2.Id);
                        db.HolidayTimeRecords.Remove(depart);
                    }

                }
                db.SaveChanges();
                for (int i = 0; i < intdiffer; i++)
                {
                    DateTime dttemp = begindate.Date.AddDays(i);

                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "1" == weektag.Week1) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "1" == weektag.Week2) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "1" == weektag.Week3) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "1" == weektag.Week4) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Friday && "1" == weektag.Week5) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "1" == weektag.Week6) ||
                        (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "1" == weektag.Week7))
                    {
                        HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                        holidayTime.Number = weektag.Range;
                        holidayTime.RecordTimeHoliday = dttemp;
                        holidayTime.Tag = "1";
                        db.HolidayTimeRecords.Add(holidayTime);
                        db.SaveChanges();
                        var staff = db.Staffs.Where(s => s.Department.Equals(weektag.Range)).ToList();
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime1 = new HolidayTimeRecord();
                            holidayTime1.Number = temp1.StaffNumber;
                            holidayTime1.RecordTimeHoliday = dttemp;
                            holidayTime1.Tag = "1";
                            db.HolidayTimeRecords.Add(holidayTime1);
                            db.SaveChanges();

                        }

                    }
                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "2" == weektag.Week1) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "2" == weektag.Week2) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "2" == weektag.Week3) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "2" == weektag.Week4) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Friday && "2" == weektag.Week5) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "2" == weektag.Week6) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "2" == weektag.Week7))
                    {
                        HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                        holidayTime.Number = weektag.Range;
                        holidayTime.RecordTimeHoliday = dttemp;
                        holidayTime.Tag = "2";
                        db.HolidayTimeRecords.Add(holidayTime);
                        db.SaveChanges();
                        var staff = db.Staffs.Where(s => s.Department.Equals(weektag.Range)).ToList();
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime1 = new HolidayTimeRecord();
                            holidayTime1.Number = temp1.StaffNumber;
                            holidayTime1.RecordTimeHoliday = dttemp;
                            holidayTime1.Tag = "2";
                            db.HolidayTimeRecords.Add(holidayTime1);
                            db.SaveChanges();

                        }
                    }
                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "3" == weektag.Week1) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "3" == weektag.Week2) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "3" == weektag.Week3) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "3" == weektag.Week4) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Friday && "3" == weektag.Week5) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "3" == weektag.Week6) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "3" == weektag.Week7))
                    {
                        HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                        holidayTime.Number = weektag.Range;
                        holidayTime.RecordTimeHoliday = dttemp;
                        holidayTime.Tag = "3";
                        db.HolidayTimeRecords.Add(holidayTime);
                        db.SaveChanges();
                        var staff = db.Staffs.Where(s => s.Department.Equals(weektag.Range)).ToList();
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime1 = new HolidayTimeRecord();
                            holidayTime1.Number = temp1.StaffNumber;
                            holidayTime1.RecordTimeHoliday = dttemp;
                            holidayTime1.Tag = "3";
                            db.HolidayTimeRecords.Add(holidayTime1);
                            db.SaveChanges();

                        }
                    }
                    if ((dttemp.DayOfWeek == System.DayOfWeek.Monday && "4" == weektag.Week1) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Tuesday && "4" == weektag.Week2) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Wednesday && "4" == weektag.Week3) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Thursday && "4" == weektag.Week4) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Friday && "4" == weektag.Week5) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Saturday && "4" == weektag.Week6) ||
                       (dttemp.DayOfWeek == System.DayOfWeek.Sunday && "4" == weektag.Week7))
                    {
                        HolidayTimeRecord holidayTime = new HolidayTimeRecord();
                        holidayTime.Number = weektag.Range;
                        holidayTime.RecordTimeHoliday = dttemp;
                        holidayTime.Tag = "4";
                        db.HolidayTimeRecords.Add(holidayTime);
                        db.SaveChanges();
                        var staff = db.Staffs.Where(s => s.Department.Equals(weektag.Range)).ToList();
                        foreach (var temp1 in staff)
                        {
                            HolidayTimeRecord holidayTime1 = new HolidayTimeRecord();
                            holidayTime1.Number = temp1.StaffNumber;
                            holidayTime1.RecordTimeHoliday = dttemp;
                            holidayTime1.Tag = "4";
                            db.HolidayTimeRecords.Add(holidayTime1);
                            db.SaveChanges();

                        }
                    }

                }
            }
            db.WeekTags.Add(weektag);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult CreateCompanyDay()
        {
            List<SelectListItem> name = db.HolidayTimeNames.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Name,//显示的值
            }).ToList();
            //增加一个null选
            ViewBag.name = name;
          
            return View();
        }

        // POST: HolidayTimeRecord/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]//修改某个部门的某一天的工作情况
        public ActionResult CreateCompanyDay([Bind(Include = "Id,Number,RecordTime,SortNumber,Tag")] HolidayTimeRecord holidayTimeRecord)
        {
            if (ModelState.IsValid)
            {
                HolidayTimeRecord htr = (from htr1 in db.HolidayTimeRecords
                                         where htr1.Number == holidayTimeRecord.Number && htr1.RecordTimeHoliday == holidayTimeRecord.RecordTimeHoliday
                                         select htr1).Single();
                if (htr != null)
                {
                    db.HolidayTimeRecords.Remove(htr);
                    db.SaveChanges();
                }
                if (htr.Tag != "5")
                {
                    holidayTimeRecord.RecordPerson = this.Name;
                    holidayTimeRecord.RecordTime = DateTime.Now;
                    holidayTimeRecord.ChangePerson = this.Name;
                    holidayTimeRecord.ChangeTime = DateTime.Now;
                    db.HolidayTimeRecords.Add(holidayTimeRecord);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(holidayTimeRecord);
        }

        // GET: HolidayTimeRecord/Create
        public ActionResult Create()
        {
            List<SelectListItem> name = db.HolidayTimeNames.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Name,//显示的值
            }).ToList();
            //增加一个null选
            ViewBag.name = name;
            return View();
        }

        // POST: HolidayTimeRecord/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]//修改某个员工的某一天的工作情况
        public ActionResult Create([Bind(Include = "Id,Number,RecordTime,SortNumber,Tag")] HolidayTimeRecord holidayTimeRecord)
        {
            if (ModelState.IsValid)
            {
                var htrtemp = (from htr1 in db.HolidayTimeRecords
                               where htr1.Number == holidayTimeRecord.Number && htr1.RecordTimeHoliday == holidayTimeRecord.RecordTimeHoliday
                               select htr1).ToList();
                if (htrtemp.Count != 0)
                {
                    HolidayTimeRecord htr = (from htr1 in db.HolidayTimeRecords
                                             where htr1.Number == holidayTimeRecord.Number && htr1.RecordTimeHoliday == holidayTimeRecord.RecordTimeHoliday
                                             select htr1).Single();
                    if (htr != null)
                    {
                        db.HolidayTimeRecords.Remove(htr);
                        db.SaveChanges();
                    }
                    if (htr.Tag != "5")
                    {
                        holidayTimeRecord.RecordPerson = this.Name;
                        holidayTimeRecord.RecordTime = DateTime.Now;
                        holidayTimeRecord.ChangePerson = this.Name;
                        holidayTimeRecord.ChangeTime = DateTime.Now;
                        db.HolidayTimeRecords.Add(holidayTimeRecord);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (holidayTimeRecord.Tag != "5")
                    {
                        holidayTimeRecord.RecordPerson = this.Name;
                        holidayTimeRecord.RecordTime = DateTime.Now;
                        holidayTimeRecord.ChangePerson = this.Name;
                        holidayTimeRecord.ChangeTime = DateTime.Now;
                        db.HolidayTimeRecords.Add(holidayTimeRecord);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(holidayTimeRecord);
        }

        // GET: HolidayTimeRecord/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTimeRecord holidayTimeRecord = db.HolidayTimeRecords.Find(id);
            List<SelectListItem> name = db.HolidayTimeNames.ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),//保存的值
                Text = c.Name,//显示的值
            }).ToList();
            //增加一个null选
            ViewBag.name = name;
            if (holidayTimeRecord == null)
            {
                return HttpNotFound();
            }
            return View(holidayTimeRecord);
        }

        // POST: HolidayTimeRecord/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,RecordTime,SortNumber,Tag")] HolidayTimeRecord holidayTimeRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holidayTimeRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holidayTimeRecord);
        }

        // GET: HolidayTimeRecord/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTimeRecord holidayTimeRecord = db.HolidayTimeRecords.Find(id);
            if (holidayTimeRecord == null)
            {
                return HttpNotFound();
            }
            var htr1 = db.HolidayTimeNames.Where(htr => htr.Id.ToString().Equals(holidayTimeRecord.Tag)).SingleOrDefault();
            holidayTimeRecord.Tag = htr1.Name;
            return View(holidayTimeRecord);
        }

        // POST: HolidayTimeRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolidayTimeRecord holidayTimeRecord = db.HolidayTimeRecords.Find(id);
            db.HolidayTimeRecords.Remove(holidayTimeRecord);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult ResultHoliday(string month, string year, string staffNumber)
        {
            List<Object> obj = new List<Object>();
            var number = (from htr in db.HolidayTimeRecords
                          join htn in db.HolidayTimeNames on htr.Tag equals htn.Id.ToString()
                          where htr.RecordTimeHoliday.Year.ToString() == year && htr.RecordTimeHoliday.Month.ToString() == month && htr.Number == staffNumber
                          select new { Tag = htr.Tag, Day = htr.RecordTimeHoliday.Day.ToString(), TagName = htn.Name }).ToList();

            var number1 = (from htr in db.HolidayTimeRecords
                           join htn in db.HolidayTimeNames on htr.Tag equals htn.Id.ToString()
                           join h in db.Holidays on htr.Number equals ("H" + h.Id.ToString())
                           where htr.RecordTimeHoliday.Year.ToString() == year && htr.RecordTimeHoliday.Month.ToString() == month
                           select new { Tag = htr.Tag, Day = htr.RecordTimeHoliday.Day.ToString(), TagName = h.JieJiaName }).ToList();
            List<WorkDayModel> workDayModel = Generate.GetWorkDaysByStaffNumber(staffNumber, this.ConnectionString);
            
            foreach(var temp in workDayModel)
            {
                if(temp.Date.Month.ToString()==month&&temp.Date.Year.ToString()==year)
                { 
                obj.Add(new { Tag = "1", Day = temp.Date.Day.ToString(), TagName = temp.WorkTime });
                }
            }
           

            foreach (var temp in number)
            {
                obj.Add(new { Tag = temp.Tag, Day = temp.Day, TagName = temp.TagName });
            }
            foreach (var temp in number1)
            {
                obj.Add(new { Tag = temp.Tag, Day = temp.Day, TagName = temp.TagName });
            }
            return Json(obj);
        }
       public JsonResult DepartmentSearch(string number)
        {
            try
            {
                var items = (from d in db.Departments
                             select new
                             {
                                 text = d.Name,
                                 id = d.DepartmentId
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
