using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using Bonsaii.Models.Checking_in;
using Bonsaii.Models.Audit;
namespace Bonsaii.Controllers
{
    public class VacateAppliesController : BaseController
    {
        public byte AuditApplicationVacate(VacateApplies vacateApplies)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == vacateApplies.BillType select p).ToList().FirstOrDefault();

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
                                    (vacateApplies.BillType == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    Staff tmpStaff = db.Staffs.Where(p => p.StaffNumber.Equals(vacateApplies.StaffNumber)).ToList().First();
                    auditApplication.BNumber = vacateApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.CreatorName = this.Name;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                auditApplication.Info =
                 "单据名称：" + db.BillProperties.Where(p => p.Type.Equals(vacateApplies.BillType)).ToList().First().TypeName + ";" +
                 "工      号：" + vacateApplies.StaffNumber + ";" +
                 "姓      名：" + tmpStaff.Name + ";" + 
                 "所在部门：" + db.Departments.Where(p => p.DepartmentId.Equals(tmpStaff.Department)).ToList().First().Name + ";" + 
                 "性       别：" + tmpStaff.Gender + ";" + 
                 "职      位：" + tmpStaff.Position + ";" + 
                 "用工性质：" + tmpStaff.WorkProperty + ";" +
                 "请假时间：" + vacateApplies.StartDateTime + ";" +
                 "结束时间：" + vacateApplies.EndDateTime + ";" +
                 "请假时数：" + vacateApplies.Hours + ";" +
                 "请假原因：" + vacateApplies.Reason + ";" +
                    "备注：" + vacateApplies.Remark + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();
                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 7;
                    }
                    else {
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
            VacateApplies vacateApplies = db.VacateApplies.Find(id);
            if (vacateApplies == null)
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
                    vacateApplies.AuditStatus = 3;
                }
                else
                {
                    //不通过审批
                    vacateApplies.AuditStatus = 4;

                }
                vacateApplies.AuditPerson = this.UserName;
                vacateApplies.AuditTime = DateTime.Now;
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
            VacateApplies vacateApplies = db.VacateApplies.Find(id);
            if (vacateApplies == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationVacate(vacateApplies);
            //需要对原表做出的修改
            vacateApplies.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: VacateApplies
        [Authorize(Roles = "Admin,VacateApplies_Index")]
        public ActionResult Index()
        {
            List<VacateAppliesViewModel> vacateApplies = (from p in db.VacateApplies
                                                              join q in db.BillProperties on p.BillType equals q.Type
                                                              join y in db.Staffs on p.StaffNumber equals y.StaffNumber
                                                              join z in db.Departments on y.Department equals z.DepartmentId
                                                              join n in db.States on p.AuditStatus equals n.Id
                                                          select new VacateAppliesViewModel
                                                              {
                                                                  Id = p.Id,
                                                                  BillTypeName = q.TypeName,
                                                                  StaffNumber = p.StaffNumber,
                                                                  StaffName = y.Name,
                                                                  StartDateTime = p.StartDateTime,
                                                                  Hours = p.Hours,
                                                                  DepartmentName = z.Name,
                                                                  AuditStatusName = n.Description
                                                              }).ToList();
            return View(vacateApplies);
        }
        // GET: VacateApplies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacateApplies vacateApplies = db.VacateApplies.Find(id);
            if (vacateApplies == null)
            {
                return HttpNotFound();
            }
            vacateApplies.AuditStatusName = db.States.Find(vacateApplies.AuditStatus).Description;
            return View(vacateApplies);
        }

        // GET: VacateApplies/Create
         [Authorize(Roles = "Admin,VacateApplies_Create")]
        public ActionResult Create()
        {
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("32")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
             return View();
        }
        // POST: VacateApplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,VacateApplies_Create")]
        public ActionResult Create([Bind(Include = "Id,BillType,BillNumber,StaffNumber,StartDateTime,EndDateTime,Reason,Picture,Remark")] VacateApplies vacateApplies)
        {
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("32")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            if (ModelState.IsValid)
            {
               vacateApplies.BillNumber = GenerateBillNumber(vacateApplies.BillType);
                //手动计算出加班小时数，不需要用户手动填写
               vacateApplies.Hours = (vacateApplies.EndDateTime - vacateApplies.StartDateTime).Days*24 + (vacateApplies.EndDateTime - vacateApplies.StartDateTime).Hours;
               vacateApplies.IsRead = false;
                db.VacateApplies.Add(vacateApplies);
                byte status = AuditApplicationVacate(vacateApplies);

                //没有找到该单据的审批模板
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    return View(vacateApplies);
                }

                //需要对原表做出的修改
                vacateApplies.AuditStatus = status;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vacateApplies);
        }

        // GET: VacateApplies/Edit/5
         [Authorize(Roles = "Admin,VacateApplies_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacateApplies vacateApplies = db.VacateApplies.Find(id);
            if (vacateApplies == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("32")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            return View(vacateApplies);
        }

        // POST: VacateApplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,VacateApplies_Edit")]
        public ActionResult Edit([Bind(Include = "Id,BillType,BillNumber,StaffNumber,StartDateTime,EndDateTime,Reason,Picture,Remark")] VacateApplies vacateApplies)
        {
            if (ModelState.IsValid)
            {
                vacateApplies.Hours = (vacateApplies.EndDateTime - vacateApplies.StartDateTime).Hours;
               
                db.Entry(vacateApplies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> item = (from s in db.BillProperties
                                         where s.BillSort.Equals("32")
                                         select new SelectListItem()
                                         {
                                             Value = s.Type,
                                             Text = s.Type + s.TypeName,
                                         }).ToList();
            ViewBag.List = item;
            return View(vacateApplies);
        }

        // GET: VacateApplies/Delete/5
         [Authorize(Roles = "Admin,VacateApplies_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacateApplies vacateApplies = db.VacateApplies.Find(id);
            if (vacateApplies == null)
            {
                return HttpNotFound();
            }
            vacateApplies.AuditStatusName = db.States.Find(vacateApplies.AuditStatus).Description;
            return View(vacateApplies);
        }

        // POST: VacateApplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VacateApplies vacateApplies = db.VacateApplies.Find(id);
            db.VacateApplies.Remove(vacateApplies);
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

        public string Text { get; set; }
    }
}
