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
    public class WorkManagesController : BaseController
    {
        // GET: WorkManages
         [Authorize(Roles = "Admin,WorkManages_Index")]
        public ActionResult Index()
        {
            List<WorkManages> workmanages = db.WorkManages.ToList();
            foreach (WorkManages tmp in workmanages)
            {
                tmp.DepartmentName = (from x in db.Departments where x.DepartmentId == tmp.DepartmentId select x.Name).Single();
                tmp.WorksName = db.Works.Find(tmp.WorksId).Name;
            }
            return View(workmanages);
        }

        // GET: WorkManages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkManages workManages = db.WorkManages.Find(id);
            if (workManages == null)
            {
                return HttpNotFound();
            }
            return View(workManages);
        }

        // GET: WorkManages/Create
        [Authorize(Roles = "Admin,WorkManages_Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkManages_Create")]
        public ActionResult Create([Bind(Include = "Id,StartDate,EndDate,WorksId,AuditStatus,StaffNumber,Remark")] WorkManages workManages)
        {
            if (ModelState.IsValid)
            {
                db.WorkManages.Add(workManages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workManages);
        }

        // GET: WorkManages/Edit/5
          [Authorize(Roles = "Admin,WorkManages_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkManages workManages = db.WorkManages.Find(id);
            if (workManages == null)
            {
                return HttpNotFound();
            }
            return View(workManages);
        }

        // POST: WorkManages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkManages_Edit")]
        public ActionResult Edit([Bind(Include = "Id,StartDate,EndDate,WorksId,AuditStatus,StaffNumber,Remark")] WorkManages workManages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workManages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workManages);
        }

        // GET: WorkManages/Delete/5
          [Authorize(Roles = "Admin,WorkManages_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkManages workManages = db.WorkManages.Find(id);
            if (workManages == null)
            {
                return HttpNotFound();
            }
            return View(workManages);
        }

        // POST: WorkManages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkManages_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkManages workManages = db.WorkManages.Find(id);
            db.WorkManages.Remove(workManages);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,WorkManages_DepartmentIndex")]
        public ActionResult DepartmentIndex()
        {
            List<WorkArrangeViewModel> list = db.WorkManages.Where(p=>p.Flag==true).Select(p => new WorkArrangeViewModel()
            {
                Date = p.Date,
                WorksId = p.WorksId,
                DepartmentId = p.DepartmentId,
            }).Distinct().OrderBy(x => x.DepartmentId).ThenBy(x => x.Date).ToList();

            foreach (WorkArrangeViewModel tmp in list)
            {
                tmp.WorksName = db.Works.Find(tmp.WorksId).Name;
                tmp.DepartmentName = db.Departments.Where(p => p.DepartmentId.Equals(tmp.DepartmentId)).Single().Name;
            }
            List<WorkArrangeViewModel> result = this.GetDepartmentWorks(list);
            return View(result);
        }

        //针对部门创建排班
          [Authorize(Roles = "Admin,WorkManages_DepartmentCreate")]
        public ActionResult DepartmentCreate()
        {
            ViewBag.DepartmentsList = Generate.GetDepartments(base.ConnectionString);
            ViewBag.WorksList = Generate.GetWorks(base.ConnectionString);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkManages_DepartmentCreate")]
        public ActionResult DepartmentCreate([Bind(Include = "Id,StartDate,EndDate,WorksId,AuditStatus,DepartmentId,Remark")] WorkManages workManages)
        {
            if (ModelState.IsValid)
            {
                List<string> StaffNumbers = (from x in db.Staffs where x.Department.Equals(workManages.DepartmentId) select x.StaffNumber).ToList();
                WorkManages wm = db.WorkManages.Find(13);

                foreach (string tmpStaffNumber in StaffNumbers)
                {
                    List<WorkManages> tmpWorkManages = (from x in db.WorkManages
                                                        where x.StaffNumber.Equals(tmpStaffNumber) && x.Date <= workManages.EndDate && x.Date >= workManages.StartDate
                                                        select x).ToList();
                    //删除原来的在相同时间段内的排班情况
                    db.WorkManages.RemoveRange(tmpWorkManages);
                    //插入新的排班情况
                    int days = workManages.EndDate.DayOfYear - workManages.StartDate.DayOfYear;
                    for (int i = 0; i <= days; i++)
                    {
                        db.WorkManages.Add(new WorkManages()
                        {
                            WorksId = workManages.WorksId,
                            AuditStatus = 1,
                            Flag = true,        //标示是针对部门的排班
                            StaffNumber = tmpStaffNumber,
                            DepartmentId = workManages.DepartmentId,
                            Remark = workManages.Remark,
                            Date = workManages.StartDate.AddDays(i),
                        });
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("DepartmentIndex");
            }
            ViewBag.DepartmentsList = Generate.GetDepartments(base.ConnectionString);
            ViewBag.WorksList = Generate.GetWorks(base.ConnectionString);
            return View(workManages);
        }


        /// <summary>
        /// 根据从数据库当中排序好的所有部门的排班信息(这个排序是先针对部门编号，然后针对班次的日期），整理出各个班次的开始时间和结束时间
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<WorkArrangeViewModel> GetDepartmentWorks(List<WorkArrangeViewModel> list)
        {
            List<WorkArrangeViewModel> result = new List<WorkArrangeViewModel>();
            Dictionary<string, WorkArrangeViewModel> dic = new Dictionary<string, WorkArrangeViewModel>();
            string preKey = null;       //保存上一个班次的信息
            foreach (WorkArrangeViewModel tmp in list)
            {
                string tmpKey = tmp.DepartmentId + tmp.WorksId.ToString();      //用部门编号+班次编号作为Hash中的Key
                if (dic.ContainsKey(tmpKey))
                    dic[tmpKey].EndDate = tmp.Date.ToString("yyyy-MM-dd");
                else
                {
                    if (preKey != null)
                        result.Add(dic[preKey]);
                    dic.Clear();
                    tmp.StartDate = tmp.Date.ToString("yyyy-MM-dd");
                    //这里初始化的原始是：可能这个班次只有一天，这样的话，如果不在这里初始化，就不会有EndDate的值了
                    tmp.EndDate = tmp.Date.ToString("yyyy-MM-dd");      
                    dic.Add(tmpKey, tmp);
                    preKey = tmpKey;
                }
            }
            if (dic.Count != 0)
                result.Add(dic[preKey]);
            return result;
        }
        // POST: WorkManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DepartmentCreate([Bind(Include = "Id,StartDate,EndDate,WorksId,AuditStatus,DepartmentId,Remark")] WorkManages workManages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        List<string> StaffNumbers = (from x in db.Staffs where x.Department.Equals(workManages.DepartmentId) select x.StaffNumber).ToList();

        //        List<WorkManages> tmpWorkManages = db.WorkManages.Where(p => p.DepartmentId.Equals(workManages.DepartmentId)).ToList();
        //        foreach (WorkManages tmp in tmpWorkManages)
        //        {
        //            /**
        //             * 对于每一个员工，他的排班在一入职的时候，一定会分配一个默认排班的，所以在进行部门排班或者个人排班的时候，
        //             * 这个员工一定已经有一条记录在WorkManages表当中，所以当进行部门排班或者个人排班的时候，只需要对这个员工的
        //             * 排班信息进行更新就行了
        //             * */
        //            tmp.StartDate = workManages.StartDate;
        //            tmp.EndDate = workManages.EndDate;
        //            tmp.WorksId = workManages.WorksId;
        //            tmp.Remark = workManages.Remark;
        //            tmp.Flag = true;           //1表示是部门排班
        //            db.Entry(tmp).State = EntityState.Modified;
        //            db.SaveChanges();           
        //        }

        //        return RedirectToAction("DepartmentIndex");
        //    }
        //    return View(workManages);
        //}

        /// <summary>
        /// 整合列表当中的日期为一个范围
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<WorkArrangeViewModel> GetPersonalWorks(List<WorkArrangeViewModel> list)
        {
            List<WorkArrangeViewModel> result = new List<WorkArrangeViewModel>();
            Dictionary<string, WorkArrangeViewModel> dic = new Dictionary<string, WorkArrangeViewModel>();
            string preKey = null;
            foreach (WorkArrangeViewModel tmp in list)
            {
                string tmpKey = tmp.StaffNumber + tmp.WorksId.ToString();
                if (dic.ContainsKey(tmpKey))
                    dic[tmpKey].EndDate = tmp.Date.ToString("yyyy-MM-dd");
                else
                {
                    if (preKey != null)
                        result.Add(dic[preKey]);
                    dic.Clear();
                    tmp.StartDate = tmp.Date.ToString("yyyy-MM-dd");
                    tmp.EndDate = tmp.Date.ToString("yyyy-MM-dd");
                    dic.Add(tmpKey, tmp);
                    preKey = tmpKey;
                }
            }
            if (dic.Count != 0)
                result.Add(dic[preKey]);
            return result;
        }
          [Authorize(Roles = "Admin,WorkManages_PersonalIndex")]
        public ActionResult PersonalIndex()
        {
        
                List<WorkArrangeViewModel> list = db.WorkManages.Where(p=>p.Flag==false).Select(p => new WorkArrangeViewModel(){
                Date = p.Date,
                WorksId = p.WorksId,
                StaffNumber = p.StaffNumber,
            }).Distinct().OrderBy(x=>x.StaffNumber).ThenBy(x=>x.Date).ToList();

            foreach (WorkArrangeViewModel tmp in list)
            {
                tmp.WorksName = db.Works.Find(tmp.WorksId).Name;
                tmp.StaffName = db.Staffs.Where(p => p.StaffNumber.Equals(tmp.StaffNumber)).Single().Name;
            }

            return View(this.GetPersonalWorks(list));
        }



        //public ActionResult PersonalIndex()
        //{
        //    List<WorkManageViewModel> workmanages = db.WorkManages.Where(p => p.Flag == false).Select(x=> new WorkManageViewModel()
        //    {
        //        Date = x.StartDate,
        //        EndDate = x.EndDate,
        //        WorksId = x.WorksId,
        //        Remark = x.Remark,
        //        DepartmentId = x.DepartmentId,
        //        StaffNumber = x.StaffNumber,
        //    }).ToList();
        //    foreach (WorkManageViewModel tmp in workmanages)
        //    {
        //        tmp.WorksName = db.Works.Find(tmp.WorksId).Name;
        //        tmp.StaffName = db.Staffs.Where(p => p.StaffNumber.Equals(tmp.StaffNumber)).Single().Name;
        //    }
        //    return View(workmanages);
        //}


        //针对个人创建排班
          [Authorize(Roles = "Admin,WorkManages_PersonalCreate")]
        public ActionResult PersonalCreate()
        {
            ViewBag.DepartmentsList = Generate.GetDepartments(base.ConnectionString);
            ViewBag.WorksList = Generate.GetWorks(base.ConnectionString);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,WorkManages_Create")]
        public ActionResult PersonalCreate([Bind(Include = "Id,StartDate,EndDate,WorksId,AuditStatus,StaffNumber,Remark")] WorkManages workManages)
        {
            if (ModelState.IsValid)
            {
                string staffNumber = workManages.StaffNumber.Split(new char[] { '-' })[0];
                /**
                 * 对于每一个员工，他的排班在一入职的时候，一定会分配一个默认排班的，所以在进行部门排班或者个人排版的时候，
                 * 这个员工一定已经有一条记录在WorkManages表当中，所以当进行部门排班或者个人排班的时候，只需要对这个员工的
                 * 排班信息进行更新就行了
                 * */
                string tmpDepartmentId = (from x in db.Staffs where x.StaffNumber.Equals(staffNumber) select x.Department).Single();
                List<WorkManages> tmpWorkManages = (from x in db.WorkManages
                                                    where x.StaffNumber.Equals(staffNumber) && x.Date <= workManages.EndDate && x.Date >= workManages.StartDate
                                                    select x).ToList();
                //删除原来的在相同时间段内的排班情况
                db.WorkManages.RemoveRange(tmpWorkManages);
                //插入新的排班情况
                int days = workManages.EndDate.DayOfYear - workManages.StartDate.DayOfYear;
                for (int i = 0; i <= days; i++)
                {
                    db.WorkManages.Add(new WorkManages()
                    {
                        WorksId = workManages.WorksId,
                        AuditStatus = 1,
                        Flag = false,        //标示是针对个人的排班
                        StaffNumber = staffNumber,
                        DepartmentId = tmpDepartmentId,
                        Remark = workManages.Remark,
                        Date = workManages.StartDate.AddDays(i),
                    });
                }
                db.SaveChanges();
                return RedirectToAction("PersonalIndex");
            }
            ViewBag.DepartmentsList = Generate.GetDepartments(base.ConnectionString);
            ViewBag.WorksList = Generate.GetWorks(base.ConnectionString);
            return View(workManages);
        }
        // POST: WorkManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PersonalCreate([Bind(Include = "Id,StartDate,EndDate,WorksId,AuditStatus,StaffNumber,Remark")] WorkManages workManages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string  staffNumber = workManages.StaffNumber.Split(new char []{ '-' })[0];
        //            /**
        //             * 对于每一个员工，他的排班在一入职的时候，一定会分配一个默认排班的，所以在进行部门排班或者个人排版的时候，
        //             * 这个员工一定已经有一条记录在WorkManages表当中，所以当进行部门排班或者个人排班的时候，只需要对这个员工的
        //             * 排班信息进行更新就行了
        //             * */

        //        WorkManages tmpWorkManages = db.WorkManages.Where(p => p.StaffNumber.Equals(staffNumber)).Single();
        //        tmpWorkManages.StartDate = workManages.StartDate;
        //        tmpWorkManages.EndDate = workManages.EndDate;
        //        tmpWorkManages.WorksId = workManages.WorksId;
        //        tmpWorkManages.Remark = workManages.Remark;
        //        tmpWorkManages.Flag = false;
        //        db.Entry(tmpWorkManages).State = EntityState.Modified;
        //        db.SaveChanges();


        //        return RedirectToAction("Index");
        //    }
        //    return View(workManages);
        //}


        /*实现员工工号搜索：显示姓名和工号*/
        [HttpPost]
        public JsonResult StaffnumberSearch(string number)
        {
            try
            {
                // var item = db.Staffs.Where(w => (w.StaffNumber).Contains(number)).ToList().Select(w => new { id=w.StaffNumber,name=w.StaffNumber});
                var items = (from p in db.Staffs where p.StaffNumber.Contains(number) || p.Name.Contains(number) select p.StaffNumber + "-" + p.Name).ToList();//.ToList().Select p;

                return Json(new
                {
                    success = true,
                    data = items
                });
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }
        public JsonResult GetWorkTimesByWorkId(int WorksId)
        {
            List<WorkTimes> worktimes = db.WorkTimes.Where(p => p.WorksId == WorksId).OrderBy(p=>p.StartTime).ToList();
            List<object> obj = new List<object>();
            foreach (WorkTimes tmp in worktimes)
            {
                obj.Add(new
                {
                    StartTime = tmp.StartTime.ToString(),
                    EndTime = tmp.EndTime.ToString(),
                    WorkHours = tmp.WorkHours,
                    OvettimeHours = tmp.OvettimeHours,
                    AheadMinutes = tmp.AheadMinutes,
                    BackMinutes = tmp.BackMinutes,
                    LateMinutes = tmp.LateMinutes,
                    LeaveEarlyMinutes = tmp.LeaveEarlyMinutes,
                });
            }
            return Json(obj);
        }


        public JsonResult GetStaffInfoByDepartmentId(string DepartmentId)
        {
            var staffs = (from x in db.Staffs where x.Department.Equals(DepartmentId) select new { x.Number, x.Name, x.Position }).ToList();
            List<object> obj = new List<object>();
            foreach (var tmp in staffs)
            {
                obj.Add(new
                {
                    Number = tmp.Number,
                    Name = tmp.Name,
                    Position = tmp.Position,
                });
            }
            return Json(obj);
        }

        public void Test()
        {
            List<WorkDayModel> list = Generate.GetWorkDaysByStaffNumber("cz000007", base.ConnectionString);
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
