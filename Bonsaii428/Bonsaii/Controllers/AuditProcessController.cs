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
using BonsaiiModels;
using System.Configuration;
using System.Collections;

namespace Bonsaii.Controllers
{
    public class AuditProcessController : BaseController
    {
        // GET: AuditProcess
        public ActionResult Index()
        {
            List<AuditProcess> list = (from p in db.AuditProcesses select p
                    ).ToList();
            foreach (AuditProcess tmp in list)
            {
                tmp.TemplateName = db.AuditTemplates.Find(tmp.TId).Name;
                tmp.ResultDescription = db.States.Find(tmp.Result).Description;
                //tmp.DepartmentName = 
            }
            return View(list.OrderByDescending(c=>c.Id));
        }

        // GET: AuditProcess/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            if (auditProcess == null)
            {
                return HttpNotFound();
            }
            string[] sArray = auditProcess.Info.Split(new char[] { ';' });
            ViewBag.sInfoArray = sArray;
            auditProcess.ResultDescription = db.States.Find(auditProcess.Result).Description;
            return View(auditProcess);
        }
        /// <summary>
        /// info内容已经转换成为string[]
        /// 现在需要把string[]转换成一段html代码
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        public static string ConvertDataTableToHtml(string[] arrays)
        {
          // "<table>";
            string html = null;
            foreach (string array in arrays)
            {
                html += "<tr>";
                string[] arrayColumns = array.Split(new char[] { '：' });
                foreach (var arrayColumn in arrayColumns)
                {
                    html += "<td>" + arrayColumn + "</td>";
                }
                html += "</tr>";
            }
            return html;
        }
        /// <summary>
        /// 审批方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StepCheck(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            ///根据逗号来分割字符串
            string[] sArray = auditProcess.Info.Split(new char[] { ';' });
            ///string转html有必要吗
            int len = sArray.Length;
            ArrayList newList = new ArrayList(sArray);
            newList.RemoveAt(len-1);
            sArray=(string[])newList.ToArray(typeof(string));
            ViewBag.sInfoArray = ConvertDataTableToHtml(sArray);
            auditProcess.ResultDescription = db.States.Find(auditProcess.Result).Description;
            /*之前的审核步骤*/
            var PList = (from p in db.AuditProcesses
                         where p.AId == auditProcess.AId && p.Id != auditProcess.Id
                         select p).ToList();
            ViewBag.PList = PList;

            //根据AuditApplication的Id寻找AuditProcess,然后寻找结果列表
            var resultList = (from p in db.AuditProcesses where p.AId == auditProcess.AId&&p.Id!=auditProcess.Id
                              orderby p.SId select p).ToList();
            ViewBag.resultlist = resultList;//存放结果列表
            foreach (var resultListItem in resultList)
            {
                resultListItem.ResultDescription = db.States.Find(resultListItem.Result).Description;
            }
            return View(auditProcess);
        }

        public void ReturnStatus(int auditProcessAId, byte status)
        {
             AuditApplication application = db.AuditApplications.Find(auditProcessAId);//修改Application的状态
             application.State = status;
             string btype = application.BType.Substring(0,2);       
             switch(btype){
                 case "21":{//员工
                        Staff someone = (from p in db.Staffs where (p.BillNumber == application.BNumber)&&(p.BillTypeNumber==application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            AddDefaultWork(someone.StaffNumber, Int32.Parse(someone.ClassOrder), someone.Department);
                            someone.AuditStatus = status;//已审 
                            SystemDbContext sysdb = new SystemDbContext();
                            /*BindCodes*/
                            var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == this.CompanyId && p.StaffNumber == someone.StaffNumber) select p).ToList();
                            if (Ucount.Count == 0)
                            {//木有该员工
                                BindCode user = new BindCode();
                                string CompanyDbName = "Bonsaii" + this.CompanyId;
                                user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                                user.CompanyId = this.CompanyId;
                                user.StaffNumber = someone.StaffNumber;
                                user.RealName = someone.Name;
                                user.BindingCode = someone.BindingCode;
                                user.Phone = someone.IndividualTelNumber;
                                user.BindTag = false;
                                user.LastTime = DateTime.Now;
                                user.IsAvail = true; 
                                sysdb.BindCodes.Add(user);
                                sysdb.SaveChanges();
                            }
                            db.SaveChanges();
                        }
                 } break;
                 case "22":
                     { //变更
                         StaffChange someone = (from p in db.StaffChanges where (p.BillNumber == application.BNumber)&&(p.BillTypeNumber==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                             if (someone.AuditStatus == 3)
                             {
                                 var staffId = (from p in db.Staffs where p.StaffNumber == someone.StaffNumber && p.ArchiveTag != true select p.Number).ToList().FirstOrDefault();
                                 Staff staff = db.Staffs.Find(staffId);
                                 staff.Name = someone.Name;
                                 staff.Gender = someone.Gender;
                                 staff.Department = someone.Department;
                                 staff.WorkType = someone.WorkType;
                                 staff.Position = someone.Position;
                                 staff.IdentificationNumber = someone.IdentificationNumber;
                                 staff.Nationality = someone.Nationality;
                                 staff.IdentificationNumber = someone.IdentificationNumber;
                                 staff.Entrydate = someone.Entrydate;
                                 staff.ClassOrder = someone.ClassOrder;
                                 staff.ApplyOvertimeSwitch = staff.ApplyOvertimeSwitch;
                                 staff.JobState = someone.JobState;
                                 staff.AbnormalChange = someone.AbnormalChange;
                                 staff.FreeCard = someone.FreeCard;
                                 staff.WorkProperty = someone.WorkProperty;
                                 staff.WorkType = someone.WorkType;
                                 staff.Source = someone.Source;
                                 staff.QualifyingPeriodFull = someone.QualifyingPeriodFull;
                                 staff.MaritalStatus = someone.MaritalStatus;
                                 staff.BirthDate = someone.BirthDate;
                                 staff.NativePlace = someone.NativePlace;
                                 staff.HealthCondition = someone.HealthCondition;
                                 staff.Nation = someone.Nation;
                                 staff.Address = someone.Address;
                                 staff.VisaOffice = someone.VisaOffice;
                                 staff.HomeTelNumber = someone.HomeTelNumber;
                                 staff.EducationBackground = someone.EducationBackground;
                                 staff.GraduationSchool = someone.GraduationSchool;
                                 staff.SchoolMajor = someone.SchoolMajor;
                                 staff.Degree = someone.SchoolMajor;
                                 staff.Introducer = someone.Introducer;
                                 staff.IndividualTelNumber = someone.IndividualTelNumber;
                                 staff.BankCardNumber = someone.BankCardNumber;
                                 staff.UrgencyContactMan = someone.UrgencyContactMan;
                                 staff.UrgencyContactAddress = someone.UrgencyContactAddress;
                                 staff.UrgencyContactPhoneNumber = someone.UrgencyContactPhoneNumber;
                                 staff.PhysicalCardNumber = someone.PhysicalCardNumber;
                                 staff.LeaveDate = someone.LeaveDate;
                                 staff.LeaveType = someone.LeaveType;
                                 staff.LeaveReason = someone.LeaveReason;
                                 staff.AuditStatus = someone.AuditStatus;
                                 staff.HealthCondition = someone.HealthCondition;
                                 staff.ChangeTime = someone.RecordTime;
                                 staff.ChangePerson = someone.RecordPerson;
                                 staff.AuditTime = DateTime.Now;
                                 staff.AuditPerson = this.UserName;
                                 staff.Head = someone.Head;
                                 staff.LogicCardNumber = someone.LogicCardNumber;
                                 staff.HeadType = someone.HeadType;
                                 staff.IDCardNumber = someone.IDCardNumber;
                                 staff.DeadlineDate = someone.DeadlineDate;
                             }
                             db.SaveChanges();
                         }
                     }
                     break;
                 case "23": { //离职
                     StaffApplication someone = (from p in db.StaffApplications where (p.BillNumber == application.BNumber)&&(p.BillTypeNumber==application.BType) select p).ToList().SingleOrDefault();
                     if (someone != null)
                     {
                         someone.AuditStatus = status;
                         if (someone.AuditStatus == 3)
                         {
                             StaffApplicationPassAudit(someone);
                         }
                     }
                 }
                     break;
                 case "24":
                     {//技能
                         StaffSkill someone = (from p in db.StaffSkills where (p.BillNumber == application.BNumber)&&(p.BillTypeNumber==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "25":
                     {//招聘
                         Recruitments someone = (from p in db.Recruitments where (p.BillCode == application.BNumber)&&(p.BillType==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "26":
                     { //培训
                         TrainStart someone = (from p in db.TrainStarts where (p.BillNumber == application.BNumber)&&(p.BillTypeNumber==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                             TrainStartPassAudit(someone);
                         }
                     }
                     break;
                 case "27":
                     {//合同
                         Contract someone = (from p in db.Contracts where (p.BillNumber == application.BNumber)&&(p.BillTypeNumber==application.BType)select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;//已审
                         }
                     } break;
                 case "31":
                     { //出差
                         EvectionApplies someone = (from p in db.EvectionApplies where (p.BillNumber == application.BNumber)&&(p.BillType==application.BType) select p).SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "32":
                     { //请假
                         VacateApplies someone = (from p in db.VacateApplies where (p.BillNumber == application.BNumber)&&(p.BillType==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "33":
                     { //加班
                         OvertimeApplies someone = (from p in db.OvertimeApplies where (p.BillNumber == application.BNumber)&&(p.BillType==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             OvertimeApplies correct = db.OvertimeApplies.Find(someone.Id);
                             correct.AuditPerson = someone.AuditPerson;
                             correct.AuditStatus = status;
                             correct.AuditStatusName = someone.AuditStatusName;
                             correct.AuditTime = someone.AuditTime;
                             correct.BillNumber = someone.BillNumber;
                             correct.BillType = someone.BillType;
                             correct.Date = someone.Date;
                             correct.EndDateTime = someone.EndDateTime;
                             correct.Hours = someone.Hours;
                             correct.IsRead = someone.IsRead;
                             correct.Reason = someone.Reason;
                             correct.Remark = someone.Remark;
                             correct.StaffNumber = someone.StaffNumber;
                             correct.StartDateTime = someone.StartDateTime;
                             db.SaveChanges();
                         }
                     }
                     break;
                 case "34":
                     { //签卡
                         ChargeCardApplies someone = (from p in db.ChargeCardApplies where (p.BillNumber == application.BNumber)&&(p.BillType==p.BillType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "35":
                     { //调休
                         DaysOffApplies someone = (from p in db.DaysOffApplies where (p.BillNumber == application.BNumber)&&(p.BillType==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "36":
                     { //异地
                         VacateApplies someone = (from p in db.VacateApplies where (p.BillNumber == application.BNumber)&&(p.BillType==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 case "37":
                     { //值班
                         OnDutyApplies someone = (from p in db.OnDutyApplies where (p.BillNumber == application.BNumber)&&(p.BillType==application.BType) select p).ToList().SingleOrDefault();
                         if (someone != null)
                         {
                             someone.AuditStatus = status;
                         }
                     }
                     break;
                 default:
                     break;
             }
             db.SaveChanges();
         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, int? flag)
        {
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            auditProcess.AuditDate = DateTime.Now;//实际的审核时间
            auditProcess.AuditPerson = this.UserName+"-"+this.Name;
            if (auditProcess == null)
            {
                return HttpNotFound();
            }

            ////只要AuditProcess进入这一步。AuditApplication状态就改为在审核。
            //AuditApplication application = db.AuditApplications.Find(auditProcess.AId);//修改Application的状态
            //application.State = 1;
            ////调用一个函数；用来改变Staff的审核状态
            //ReturnStatus(application,1);

            ReturnStatus(auditProcess.AId,1);

            /*超过审核时间，不再审核*/
            //if (auditProcess.AuditDate < DateTime.Now)
            if (auditProcess.AuditDate > auditProcess.DeadlineDate)//如果实际的审核时间大于截止日期
            {
                auditProcess.Result = 5;//过期，打回//
                //AuditApplication auditApplication = db.AuditApplications.Find(auditProcess.AId);//修改Application的状态
                //auditApplication.State = 5;//待审核 5(过期未处理)
                ReturnStatus(auditProcess.AId,5);
            }
            else
            {
                if (flag == 1){
                    // this.PassStep(id);
                    auditProcess.Result = 3;//通过
                    auditProcess.Comment = Request["Comment"];
                    auditProcess.AuditPerson = this.Name + "-"+this.UserName;
                    db.SaveChanges();
                    PassStep(id);
                 
                }
                else if (flag == 0)//审核不通过
                {
                    auditProcess.Result = 4;//不通过
                    auditProcess.Comment = Request["Comment"]; 
                    auditProcess.AuditPerson = this.Name + "-" + this.UserName;
                    db.SaveChanges();
                    // this.NotPassStep(id);
                    NotPassStep(id);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //如果审核通过 执行这段代码
        public void PassStep(int id)
        {
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
       
            var item = (from p in db.AuditSteps where p.SId == auditProcess.SId select p).FirstOrDefault();
            //表明item是最后一个节点。返回一个结果：审核成功
            if (item.ApprovedToSId == -1)
            {
                //AuditApplication auditApplication = db.AuditApplications.Find(auditProcess.AId);
                //auditApplication.State = 3;//已审
                ReturnStatus(auditProcess.AId,3);
            }
            else//如果不是最后一个节点，那么寻找下一个节点，插入到AuditProcess表中
            {
                int temp = item.ApprovedToSId;
                var nextstep = (from p in db.AuditSteps where p.SId == temp select p).FirstOrDefault();//下一个节点

                AuditProcess nextauditProcess = new AuditProcess();

                nextauditProcess.AId = auditProcess.AId;//还是auditProcess的AId
                nextauditProcess.SId = nextstep.SId;
                nextauditProcess.TId = nextstep.TId;
                nextauditProcess.BType = auditProcess.BType;
                nextauditProcess.BNumber = auditProcess.BNumber;
                nextauditProcess.TypeName = auditProcess.TypeName;
                nextauditProcess.Info = auditProcess.Info;
                nextauditProcess.CreateDate = auditProcess.CreateDate;//还是记录auditProcess的申请时间；
                nextauditProcess.Result = 0;//待审核
                nextauditProcess.Comment = null;
                nextauditProcess.DeadlineDate = DateTime.Now.AddDays(item.Days);//记录一下该节点最晚的审核时间；
                nextauditProcess.AuditDate = DateTime.Now;//实际的审核时间
                nextauditProcess.Approver = nextstep.Approver;
                
                db.AuditProcesses.Add(nextauditProcess);
                db.SaveChanges();
            }
           // db.SaveChanges();
        }
        //如果审核不通过就执行这个函数
        public void NotPassStep(int id)
        { 
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
           // auditProcess.Result = 4;//直接返回结果就是不通过
            var item = (from p in db.AuditSteps where p.SId == auditProcess.SId select p).FirstOrDefault();
            if (item.NotApprovedToSId == -1)//如果不再需要执行下一步了，就直接返回审核失败
            {
                ReturnStatus(auditProcess.AId,4);
            }
            else
            {
                int temp = item.NotApprovedToSId;
                var laststep = (from p in db.AuditSteps where p.SId == temp select p).FirstOrDefault();//返回上一个节点

                AuditProcess lastauditProcess = new AuditProcess();

                lastauditProcess.AId = auditProcess.AId;//还是这个AId
                lastauditProcess.SId = laststep.SId;
                lastauditProcess.TId = laststep.TId;
                lastauditProcess.BType = auditProcess.BType;
                lastauditProcess.BNumber = auditProcess.BNumber;
                lastauditProcess.TypeName = auditProcess.TypeName;
                lastauditProcess.Info = auditProcess.Info;
                lastauditProcess.CreateDate = auditProcess.CreateDate;//还是记录auditProcess的申请时间；
                lastauditProcess.Result = 0;
                lastauditProcess.Comment = null;
                lastauditProcess.DeadlineDate = DateTime.Now.AddDays(item.Days);//记录一下该节点最晚的审核时间；
                lastauditProcess.AuditDate = DateTime.Now; //AuditDate就记录为当前的时间；
                lastauditProcess.Approver = laststep.Approver;

                db.AuditProcesses.Add(lastauditProcess);
                db.SaveChanges();
            }

        }
        // POST: AuditProcess/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(AuditProcess auditProcess)
        //{
        //    if (ModelState.IsValid)
        //    {


        //        db.Entry(auditProcess).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(auditProcess);
        //}

        // GET: AuditProcess/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            if (auditProcess == null)
            {
                return HttpNotFound();
            }
            string[] sArray = auditProcess.Info.Split(new char[] { ';' });
            ViewBag.sInfoArray = sArray;
            auditProcess.ResultDescription = db.States.Find(auditProcess.Result).Description;
            return View(auditProcess);
        }

        // POST: AuditProcess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            db.AuditProcesses.Remove(auditProcess);
            //如果删除了一个process。如果是第一条或者最后一条，那么该员工应该是审核失效/过期；或者干脆不允许其删除

            //

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
