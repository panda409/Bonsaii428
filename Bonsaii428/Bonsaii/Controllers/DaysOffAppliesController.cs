using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.Audit;
using Bonsaii.Models.Checking_in;

namespace Bonsaii.Controllers
{
    public class DaysOffAppliesController : BaseController
    {
        public byte AuditApplicationEvection(DaysOffApplies daysOffApplies)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == daysOffApplies.BillType select p).ToList().FirstOrDefault();

            if (item.IsAutoAudit == 1)//自动审核是1
            { //如果为0 代表不能自动审核 如果为1  代表可以自动审核
                return 3;//代表自动审核
            }

            if (item.IsAutoAudit == 2)//手动审核是2
            {
                //手动审核,也写到db.AditApplications这个表中但是不走process？
                return 6;//手动审核

            }

            if (item.IsAutoAudit == 3)
            { //如果不自动审核，就要走人工审核流程。即，把信息写入db.AditApplications这个表中
                AuditApplication auditApplication = new AuditApplication();
                auditApplication.BType = item.Type;
                auditApplication.TypeName = item.TypeName;
                auditApplication.CreateDate = DateTime.Now;

                var template = (from p in db.AuditTemplates
                                where (
                                    (daysOffApplies.BillType == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    Staff tmpStaff = db.Staffs.Where(p => p.StaffNumber.Equals(daysOffApplies.StaffNumber)).ToList().First();
                    auditApplication.BNumber = daysOffApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.CreatorName = this.Name;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单据名称：" + db.BillProperties.Where(p => p.Type.Equals(daysOffApplies.BillType)).ToList().First().TypeName + ";" +
                 "工      号：" + daysOffApplies.StaffNumber + ";" +
                 "姓      名：" + tmpStaff.Name + ";" +
                 "所在部门：" + db.Departments.Where(p => p.DepartmentId.Equals(tmpStaff.Department)).ToList().First().Name + ";" +
                 "性       别：" + tmpStaff.Gender + ";" +
                 "职      位：" + tmpStaff.Position + ";" +
                 "用工性质：" + tmpStaff.WorkProperty + ";" +
                 "开始时间：" + daysOffApplies.StartDateTime + ";" +
                 "结束时间：" + daysOffApplies.EndDateTime + ";" +
                 "调休时数：" + daysOffApplies.Hours + ";" +
                 "调休原因：" + daysOffApplies.Reason + ";" +
                 "备注：" + daysOffApplies.Remark + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 7;
                    }
                    else
                    {
                        AuditProcess auditProcess = new AuditProcess();
                        auditProcess.AId = auditApplication.Id;
                        auditProcess.SId = step.SId;
                        auditProcess.TId = step.TId;
                        auditProcess.BType = auditApplication.BType;
                        auditProcess.BNumber = auditApplication.BNumber;
                        auditProcess.TypeName = auditApplication.TypeName;
                        auditProcess.Info = auditApplication.Info + "提交人员：" + auditApplication.CreatorName + "-" + auditApplication.Creator + ";" +
                            "提交日期：" + auditApplication.CreateDate + ";";
                        auditProcess.AuditDate = DateTime.Now;
                        auditProcess.CreateDate = auditApplication.CreateDate;
                        auditProcess.Result = 0;//待审
                        auditProcess.DeadlineDate = DateTime.Now.AddDays(step.Days);//记录一下该节点最晚的审核时间；
                        auditProcess.Approver = step.Approver;
                        db.AuditProcesses.Add(auditProcess);
                        db.SaveChanges();

                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 7;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        // GET: EvectionApplies
        [Authorize(Roles = "Admin,DaysOffApplies_Index")]
        public ActionResult Index()
        {
            List<DaysOffAppliesViewModel> daysOffApplies = (from p in db.DaysOffApplies
                                                            join q in db.BillProperties on p.BillType equals q.Type
                                                            join y in db.Staffs on p.StaffNumber equals y.StaffNumber
                                                            join z in db.Departments on y.Department equals z.DepartmentId
                                                            join n in db.States on p.AuditStatus equals n.Id
                                                            select new DaysOffAppliesViewModel
                                                             {
                                                                 Id = p.Id,
                                                                 BillTypeName = q.TypeName,
                                                                 StaffNumber = p.StaffNumber,
                                                                 StaffName = y.Name,
                                                                 DepartmentName = z.Name,
                                                                 StartDateTime = p.StartDateTime,
                                                                 Hours = p.Hours,
                                                                 AuditStatusName = n.Description
                                                             }).ToList();
            return View(daysOffApplies);
        }
        [Authorize(Roles = "Admin,DaysOffApplies_Details")]
        // GET: EvectionApplies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaysOffApplies daysOffApplies = db.DaysOffApplies.Find(id);
            if (daysOffApplies == null)
            {
                return HttpNotFound();
            }
            daysOffApplies.AuditStatusName = db.States.Find(daysOffApplies.AuditStatus).Description;
            return View(daysOffApplies);
        }
        public ActionResult ManualAudit(int? id, int flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaysOffApplies daysOffApplies = db.DaysOffApplies.Find(id);
            if (daysOffApplies == null)
            {
                return HttpNotFound();
            }
            //手动审批；这部分是自己给自己审批
            //需要对原表做出的修改
            try
            {
                if (flag == 1)
                {
                    //通过审批
                    daysOffApplies.AuditStatus = 3;
                }
                else
                {
                    //不通过审批
                    daysOffApplies.AuditStatus = 4;

                }
                daysOffApplies.AuditPerson = this.UserName;
                daysOffApplies.AuditTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaysOffApplies daysOffApplies = db.DaysOffApplies.Find(id);
            if (daysOffApplies == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationEvection(daysOffApplies);
            //需要对原表做出的修改
            daysOffApplies.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        [Authorize(Roles = "Admin,DaysOffApplies_Create")]
        // GET: DaysOffApplies/Create
        public ActionResult Create()
        {
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("35")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            return View();
        }
        [HttpPost]
        public JsonResult GetAvailableHours(string StaffNumber)
        {
            OnDutyHours hours = db.OnDutyHours.Find(StaffNumber);
            if (hours == null)
                return Json(new { hours = 0 });
            else
                return Json(new { hours = hours.AvailableHours });
        }
        // POST: DaysOffApplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,DaysOffApplies_Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BillType,StaffNumber,Hours,StartDateTime,EndDateTime,Reason,AvailableHours,Remark")] DaysOffApplies daysOffApplies)
        {
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("35")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            if (ModelState.IsValid)
            {
                OnDutyHours onDutyHours = db.OnDutyHours.Find(daysOffApplies.StaffNumber);
                if (onDutyHours == null)
                {
                    ModelState.AddModelError("", "该员工没有可用的值班时数！");
                    return View(daysOffApplies);
                }
                daysOffApplies.BillNumber = GenerateBillNumber(daysOffApplies.BillType);
                daysOffApplies.Date = daysOffApplies.StartDateTime.Date;
                //     daysOffApplies.Hours = (daysOffApplies.EndDateTime - daysOffApplies.StartDateTime).Hours;
                //更新这个员工的可用小时数

                if (daysOffApplies.Hours > onDutyHours.AvailableHours)
                {

                    ModelState.AddModelError("", "申请的调休数超过可用值班数！");
                    return View(daysOffApplies);
                }
                onDutyHours.AvailableHours -= daysOffApplies.Hours;
                db.Entry(onDutyHours).State = EntityState.Modified;


                db.DaysOffApplies.Add(daysOffApplies);
                byte status = AuditApplicationEvection(daysOffApplies);
                //没有找到该单据的审批模板
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    return View(daysOffApplies);
                }
                //需要对原表做出的修改
                daysOffApplies.AuditStatus = status;
                try
                {
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    var tmp = e;
                }
                return RedirectToAction("Index");
            }

            return View(daysOffApplies);
        }
        [Authorize(Roles = "Admin,DaysOffApplies_Edit")]
        // GET: DaysOffApplies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaysOffApplies daysOffApplies = db.DaysOffApplies.Find(id);
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("35")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            if (daysOffApplies == null)
            {
                return HttpNotFound();
            }

            return View(daysOffApplies);
        }

        // POST: DaysOffApplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,DaysOffApplies_Edit")]
        public ActionResult Edit([Bind(Include = "StaffNumber,StartDateTime,EndDateTime,Hours,Reason,Remark")] DaysOffApplies daysOffApplies)
        {
            if (ModelState.IsValid)
            {
                OnDutyHours onDutyHours = db.OnDutyHours.Find(daysOffApplies.StaffNumber);



                daysOffApplies.Date = daysOffApplies.StartDateTime.Date;
                //          daysOffApplies.Hours = (daysOffApplies.EndDateTime - daysOffApplies.StartDateTime).Hours;

                //更新这个员工的可用小时数

                if (daysOffApplies.Hours > onDutyHours.AvailableHours)
                {

                    ModelState.AddModelError("", "申请的调休数超过可用值班数！");
                    return View(daysOffApplies);
                }
                db.Entry(onDutyHours).State = EntityState.Modified;
                db.Entry(daysOffApplies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("35")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            return View(daysOffApplies);
        }

        [Authorize(Roles = "Admin,DaysOffApplies_Delete")]
        // GET: DaysOffApplies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaysOffApplies daysOffApplies = db.DaysOffApplies.Find(id);
            if (daysOffApplies == null)
            {
                return HttpNotFound();
            }
            return View(daysOffApplies);
        }
        // POST: DaysOffApplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DaysOffApplies daysOffApplies = db.DaysOffApplies.Find(id);
            OnDutyHours onDutyHours = db.OnDutyHours.Find(daysOffApplies.StaffNumber);
            onDutyHours.AvailableHours += daysOffApplies.Hours;
            db.Entry(onDutyHours).State = EntityState.Modified;
            db.DaysOffApplies.Remove(daysOffApplies);
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
