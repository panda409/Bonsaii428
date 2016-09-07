using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii;
using Bonsaii.Models;
using Bonsaii.Models.Audit;
using Bonsaii.Models.Checking_in;

namespace Bonsaii.Controllers
{
    public class OnDutyAppliesController : BaseController
    {
        public byte AuditApplicationOvertime(OnDutyApplies overtimeApplies)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == overtimeApplies.BillType select p).ToList().FirstOrDefault();

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
                                    (overtimeApplies.BillType == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    Staff tmpStaff = db.Staffs.Where(p => p.StaffNumber.Equals(overtimeApplies.StaffNumber)).ToList().First();
                    auditApplication.BNumber = overtimeApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.CreatorName = this.Name;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单据名称：" + db.BillProperties.Where(p => p.Type.Equals(overtimeApplies.BillType)).ToList().First().TypeName + ";" +
                 "工      号：" + overtimeApplies.StaffNumber + ";" +
                 "姓      名：" + tmpStaff.Name + ";" + 
                 "所在部门：" + db.Departments.Where(p => p.DepartmentId.Equals(tmpStaff.Department)).ToList().First().Name + ";" + 
                 "性       别：" + tmpStaff.Gender + ";" + 
                 "职      位：" + tmpStaff.Position + ";" + 
                 "用工性质：" + tmpStaff.WorkProperty + ";" +
                 "开始时间：" + overtimeApplies.StartDateTime + ";" +
                 "结束时间：" + overtimeApplies.EndDateTime + ";" +
                 "值班时数：" + overtimeApplies.Hours + ";" +
                 "值班原因：" + overtimeApplies.Reason + ";" +
                 "备注：" + overtimeApplies.Remark + ";";
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
        public ActionResult ManualAudit(int? id, int flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnDutyApplies overtimeApplies = db.OnDutyApplies.Find(id);
            if (overtimeApplies == null)
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
                    overtimeApplies.AuditStatus = 3;
                }
                else
                {
                    //不通过审批
                    overtimeApplies.AuditStatus = 4;

                }
                overtimeApplies.AuditPerson = this.UserName;
                overtimeApplies.AuditTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //GET: StaffSkill/Submit/5
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnDutyApplies overtimeApplies = db.OnDutyApplies.Find(id);
            if (overtimeApplies == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationOvertime(overtimeApplies);
            //需要对原表做出的修改
            overtimeApplies.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

         [Authorize(Roles = "Admin,OnDutyApplies_Index")]
        // GET: OnDutyApplies
        public ActionResult Index()
        {
            List<OnDutyAppliesViewModel> onDutyAppliesList = (from p in db.OnDutyApplies
                                                              join q in db.BillProperties on p.BillType equals q.Type
                                                              join y in db.Staffs on p.StaffNumber equals y.StaffNumber
                                                              join z in db.Departments on y.Department equals z.DepartmentId
                                                              join n in db.States on p.AuditStatus equals n.Id
                                                              select new OnDutyAppliesViewModel
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
            return View(onDutyAppliesList);
        }
         [Authorize(Roles = "Admin,OnDutyApplies_Details")]
        // GET: OnDutyApplies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnDutyApplies onDutyApplies = db.OnDutyApplies.Find(id);
            if (onDutyApplies == null)
            {
                return HttpNotFound();
            }
            return View(onDutyApplies);
        }
         [Authorize(Roles = "Admin,OnDutyApplies_Create")]
        // GET: OnDutyApplies/Create
        public ActionResult Create()
        {
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("37")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            return View();
        }

        // POST: OnDutyApplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Roles = "Admin,OnDutyApplies_Create")]
         public ActionResult Create([Bind(Include = "Id,BillType,Hours,BillNumber,StaffNumber,StartDateTime,EndDateTime,Reason,Remark")] OnDutyApplies onDutyApplies)
        {
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("37")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            if (ModelState.IsValid)
            {
                onDutyApplies.BillNumber = GenerateBillNumber(onDutyApplies.BillType);
       //         onDutyApplies.Hours = (onDutyApplies.EndDateTime - onDutyApplies.StartDateTime).Days*24 + (onDutyApplies.EndDateTime - onDutyApplies.StartDateTime).Hours;
                onDutyApplies.Date = onDutyApplies.StartDateTime.Date;
                db.OnDutyApplies.Add(onDutyApplies);
                byte status = AuditApplicationOvertime(onDutyApplies);

                //没有找到该单据的审批模板
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    return View(onDutyApplies);
                }
                //需要对原表做出的修改
                onDutyApplies.AuditStatus = status;
                try { db.SaveChanges(); }
                catch (Exception e)
                {
                    var tmp = e;
                }
               

                return RedirectToAction("Index");
            }
            return View(onDutyApplies);
        }
         [Authorize(Roles = "Admin,OnDutyApplies_Edit")]
        // GET: OnDutyApplies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnDutyApplies onDutyApplies = db.OnDutyApplies.Find(id);
            if (onDutyApplies == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("37")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            return View(onDutyApplies);
        }

        // POST: OnDutyApplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,OnDutyApplies_Edit")]
        public ActionResult Edit([Bind(Include = "Id,BillType,Hours,BillNumber,StaffNumber,StartDateTime,EndDateTime,Reason,Remark")] OnDutyApplies onDutyApplies)
        {
            if (ModelState.IsValid)
            {
        //        onDutyApplies.Hours = (onDutyApplies.EndDateTime - onDutyApplies.StartDateTime).Days * 24 + (onDutyApplies.EndDateTime - onDutyApplies.StartDateTime).Hours;
                onDutyApplies.Date = onDutyApplies.StartDateTime.Date;
                db.Entry(onDutyApplies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("37")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            return View(onDutyApplies);
        }
         [Authorize(Roles = "Admin,OnDutyApplies_Delete")]
        // GET: OnDutyApplies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnDutyApplies onDutyApplies = db.OnDutyApplies.Find(id);
            if (onDutyApplies == null)
            {
                return HttpNotFound();
            }

            onDutyApplies.AuditStatusName = db.States.Find(onDutyApplies.AuditStatus).Description;
            
            return View(onDutyApplies);
        }

        // POST: OnDutyApplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,OnDutyApplies_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OnDutyApplies onDutyApplies = db.OnDutyApplies.Find(id);
            db.OnDutyApplies.Remove(onDutyApplies);
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
