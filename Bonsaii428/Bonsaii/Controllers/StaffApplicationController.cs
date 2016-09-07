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
using BonsaiiModels;

namespace Bonsaii.Controllers
{
    public class StaffApplicationController : BaseController
    {
        [Authorize(Roles = "Admin,StaffApplication_Index")]
        public ActionResult Index()
        {
            var recordList = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                                  on p.TableNameId equals q.Id
                              select p).ToList();
            ViewBag.recordList = recordList;
            var pp = (from saf in db.StaffApplicationReserves
                      join rf in db.ReserveFields on saf.FieldId equals rf.Id
                      select new StaffApplicationViewModel
                      {
                          Id = saf.Number,
                          Description = rf.Description,
                          Value = saf.Value
                      }).ToList();

            ViewBag.List = pp;

            List<StaffApplication> list = db.StaffApplications.ToList();

            foreach (var tmp in list){
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
                tmp.Department = (from p in db.Staffs where p.StaffNumber == tmp.StaffNumber 
                                  join q  in db.Departments on p.Department equals q.DepartmentId 
                                  select q.Name).ToList().SingleOrDefault();
            }
            return View(list.OrderByDescending(c=>c.Id));
        }

        // GET: StaffApplication/Details/5
        public ActionResult Details(int? id,int? flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplication staffApplications = db.StaffApplications.Find(id);
            if (staffApplications == null)
            {
                return HttpNotFound();
            }
            else if (flag == 1)
            {
                    staffApplications.AuditStatus = 0;
                    
                    staffApplications.ApplyDate = DateTime.Today;
                   
            }
            staffApplications.AuditStatusName = db.States.Find(staffApplications.AuditStatus).Description;
             db.SaveChanges();
             var fieldValueList = (from sar in db.StaffApplicationReserves
                                   join rf in db.ReserveFields on sar.FieldId equals rf.Id
                                   where sar.Number == id 
                                   select new StaffApplicationViewModel { Id = sar.Number, Description = rf.Description, Value = sar.Value }).ToList();
             ViewBag.fieldValueList = fieldValueList;
            return View(staffApplications);
        }


        public byte AuditApplication(StaffApplication staffApplication)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == staffApplication.BillTypeNumber select p).ToList().FirstOrDefault();

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
                                    (staffApplication.BillTypeNumber == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    Staff staff = db.Staffs.Where(c => c.StaffNumber.Equals(staffApplication.StaffNumber)).ToList().Single();
                    string departmentName = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).SingleOrDefault();
                    DateTime entryDate_dateTime = (DateTime)staff.Entrydate;
                    DateTime leaveDate_dateTime = (DateTime)staffApplication.HopeLeaveDate;

                    auditApplication.BNumber = staffApplication.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.State = 0;//代表等待审
                    auditApplication.Info =
                   "单据名称：" + staffApplication.BillTypeName + ";" +
                   "工       号：" + staffApplication.StaffNumber + ";" +
                   "员工名称：" + staffApplication.StaffName + ";" +
                   "所在部门：" + departmentName + ";" +
                   "性       别：" + staff.Gender + ";" +
                   "职       位：" + staff.Position + ";" +
                   "用工性质：" + staff.WorkProperty + ";" +
                   "入职日期：" + entryDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                   "离职日期：" + leaveDate_dateTime.ToString("yyyy/MM/dd") + ";" +
                   "离职类型：" + staffApplication.LeaveType + ";" +
                   "离职原因：" + staffApplication.LeaveReason + ";" +
                   "备       注：" + staffApplication.Remark + ";" +
                 //"单别：" + staffApplication.BillTypeNumber + ";" +
                 //"单号:" + staffApplication.BillNumber + ";" +
                 //"员工工号:" + staffApplication.StaffNumber + ";" +
                 //"姓名:" + staffApplication.StaffName + ";" +
                 //"期望离职日期" + staffApplication.HopeLeaveDate + ";" +
                 //"离职类别" + staffApplication.LeaveType + ";" +
                 //"离职原因" + staffApplication.LeaveReason + ";" +
                 //"备注:" + staffApplication.Remark + ";" +
                 //"单据类别编号:" + staffApplication.BillTypeNumber + ";" +
                 //"创建日期:" + staffApplication.RecordTime + ";" +
                 //"录入人员:" + staffApplication.RecordPerson + ";";
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
                        auditProcess.Info = auditApplication.Info;
                        //auditProcess.Info = auditApplication.Info + "提交人员：" + auditApplication.CreatorName + "-" + auditApplication.Creator + ";" +
                        //    "提交日期：" + auditApplication.CreateDate + ";";
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
            StaffApplication staffApplication = db.StaffApplications.Find(id);
            if (staffApplication == null)
            {
                return HttpNotFound();
            }
            //手动审批；这部分是自己给自己审批
            //需要对原表做出的修改
            try
            {
                if (flag == 1)
                {
                    StaffApplicationPassAudit(staffApplication);
                }
                else
                {
                    //不通过审批
                    staffApplication.AuditStatus = 4;

                }
                staffApplication.AuditPerson = this.UserName;
                staffApplication.AuditTime = DateTime.Now;
              
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: StaffApplication/Create
        [Authorize(Roles = "Admin,StaffApplication_Create")]
        public ActionResult Create()
        {
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                               on p.TableNameId equals q.Id
                             where q.TableName == "StaffApplications" select p).ToList();
            ViewBag.fieldList = fieldList;
            ViewBag.date = DateTime.Now.ToString("yyyy/MM/dd");

            return View();
        }

        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplication staffSkill = db.StaffApplications.Find(id);
            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplication(staffSkill);
            //需要对原表做出的修改
            staffSkill.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
 
        public void AddReserves(StaffApplication staffApplication) {
            db.SaveChanges();
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                             on p.TableNameId equals q.Id
                             where q.TableName == "StaffApplications"
                             select p).ToList();
            ViewBag.fieldList = fieldList;

            /*遍历，保存员工基本信息预留字段*/
            foreach (var temp in fieldList)
            {
                StaffApplicationReserve sar = new StaffApplicationReserve();
                sar.Number = staffApplication.Id;
                sar.FieldId = temp.Id;
                sar.Value = Request[temp.FieldName];
                /*占位，为了在Index中显示整齐的格式*/
                if (sar.Value == null) sar.Value = " ";
                db.StaffApplicationReserves.Add(sar);
                db.SaveChanges();
            }
        }
        // POST: StaffApplication/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffApplication_Create")]
        public ActionResult Create(StaffApplication staffApplication)
        {
            if (ModelState.IsValid)
            {
                /*保存staffApplication的内容*/
                ViewBag.date = DateTime.Now.ToString("yyyy/MM/dd");
                staffApplication.RecordTime = DateTime.Now;
                staffApplication.RecordPerson = base.Name;
                ///查找单据类别名称
                var billTypeName = (from p in db.BillProperties where p.Type == staffApplication.BillTypeNumber select p.TypeName).ToList().FirstOrDefault();
                staffApplication.BillTypeName = billTypeName;
                staffApplication.BillNumber = GenerateBillNumber(staffApplication.BillTypeNumber);
                ///提交审核
                byte status = AuditApplication(staffApplication); 
                staffApplication.AuditStatus = status;
                db.StaffApplications.Add(staffApplication);
                //自定义字段
                AddReserves(staffApplication);

                /*根据staffApplication的审核状态来做相应的处理*/
                if (status == 7)///没有找到该单据的审批模板
                {
                    ViewBag.alertMessage = true;
                    var fieldList = (from p in db.ReserveFields
                                     join q in db.TableNameContrasts
                                    on p.TableNameId equals q.Id
                                     where q.TableName == "StaffApplications"
                                     select p).ToList();
                    ViewBag.fieldList = fieldList;
                    return View(staffApplication);
                }
                else {
                    if (status == 3)//审核通过
                    {
                        StaffApplicationPassAudit(staffApplication);
                    }
                    return RedirectToAction("Index");
                }
             
            }
            else
            {
                var fieldList = (from p in db.ReserveFields
                                  join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id
                                 where q.TableName == "StaffApplications" select p).ToList();
                ViewBag.fieldList = fieldList;
                ViewBag.date = DateTime.Now.ToString("yyyy/MM/dd");
                return View(staffApplication);
            }
           
        }

        // GET: StaffApplication/Edit/5
        [Authorize(Roles = "Admin,StaffApplication_Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplication staffApplications = db.StaffApplications.Find(id);
            if (staffApplications == null)
            {
                return HttpNotFound();
            }
            /*查找表预留字段*/
            var fieldList = (from sar in db.StaffApplicationReserves
                             join rf in db.ReserveFields on sar.FieldId equals rf.Id
                             where sar.Number == id
                             select new StaffApplicationViewModel { Description = rf.Description, Value = sar.Value }).ToList();
            ViewBag.fieldList = fieldList;
            return View(staffApplications);
        }

        // POST: StaffApplication/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffApplication_Edit")]
        public ActionResult Edit(StaffApplication staffApplication)
        {
            if (ModelState.IsValid)
            {
                /*查找预留字段(value)*/
                var fieldValueList = (from sar in db.StaffApplicationReserves
                                      join rf in db.ReserveFields on sar.FieldId equals rf.Id
                                      where sar.Number == staffApplication.Id
                                      select new StaffApplicationViewModel { Id = sar.Id, Description = rf.Description, Value = sar.Value }).ToList();
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    StaffApplicationReserve sar = db.StaffApplicationReserves.Find(temp.Id);
                    sar.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                StaffApplication staffApplications = db.StaffApplications.Find(staffApplication.Id);
                staffApplications.ChangeTime = DateTime.Now;
                staffApplications.ChangePerson = base.Name;
                staffApplications.ApplyDate = staffApplication.ApplyDate;
                staffApplications.HopeLeaveDate = staffApplication.HopeLeaveDate;
                staffApplications.LeaveType = staffApplication.LeaveType;
                staffApplications.LeaveReason = staffApplication.LeaveReason;
                staffApplications.Remark = staffApplication.Remark;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffApplication);
        }

        // GET: StaffApplication/Delete/5
        [Authorize(Roles = "Admin,StaffApplication_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffApplication staffApplications = db.StaffApplications.Find(id);
            if (staffApplications == null)
            {
                return HttpNotFound();
            }
            /*查找表预留字段*/
            var fieldValueList = (from sar in db.StaffApplicationReserves
                             join rf in db.ReserveFields on sar.FieldId equals rf.Id
                             where sar.Number == id
                                  select new StaffApplicationViewModel { Id = sar.Number, Description = rf.Description, Value = sar.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(staffApplications);
        }

        // POST: StaffApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffApplication_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from sar in db.StaffApplicationReserves
                        where sar.Number == id
                        select new StaffApplicationViewModel { Id = sar.Id }).ToList();
            foreach (var temp in item)
            {
                StaffApplicationReserve sar = db.StaffApplicationReserves.Find(temp.Id);
                db.StaffApplicationReserves.Remove(sar);

            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            StaffApplication staffApplications = db.StaffApplications.Find(id);
            db.StaffApplications.Remove(staffApplications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
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
                var items = (from s in db.Staffs
                             where (s.AuditStatus == 3&&s.ArchiveTag==false)
                             join d in db.Departments on s.Department equals d.DepartmentId
                             into gc
                             from d in gc.DefaultIfEmpty()
                             select new
                             {
                                 text = s.StaffNumber + " " + s.Name,
                                 id = s.StaffNumber
                             }).ToList();

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
                             where (s.AuditStatus == 3 && s.ArchiveTag == false)
                             join d in db.Departments on s.Department equals d.DepartmentId
                             where s.StaffNumber == number

                             select new
                             {
                                 DepartmentName = d.Name,
                                 staffName=s.Name
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
