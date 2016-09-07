using Bonsaii.Models;
using Bonsaii.Models.Audit;
using Bonsaii.Models.Checking_in;
using BonsaiiModels;
using BonsaiiModels.App;
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Bonsaii.Controllers
{
    public class AppAudit
    {
        static SystemDbContext sdb = new SystemDbContext();
        static JPushClient client = new JPushClient(CompanyPushAPI.app_key, CompanyPushAPI.master_secret);
        static string info = "你的";
        static string info1 = "申请提交成功！进入审批流";
        static string info2 = "申请提交成功！审核完成";
        static string info3 = "申请提交成功！进入待审核状态";
        //推送给自己
        public static void PushMe(string companyId,string userName,int status,string applyName)
        {
            PushPayload payload = new PushPayload();
            //对审批流状态进行判断，区分申请的审核类型
            if (status == 0)//进入审批流
            {
                payload = CompanyPushAPI.PushObject_auto_alias(companyId, userName, "heh", info + applyName + info1, "2");//选择一种方式
            }
            else if (status == 3)//进入自动审核完成，提交就审核完成
            {
                payload = CompanyPushAPI.PushObject_auto_alias(companyId, userName, "heh", info + applyName + info2, "2");//选择一种方式
            }
            else if (status == 6)//进入手动审核，需要自己审核完成
            {
                payload = CompanyPushAPI.PushObject_auto_alias(companyId, userName, "heh", info + applyName + info3, "2");//选择一种方式
            }
            //else//进入没有对应的审批流模板，此申请无效
            //{
            //    payload = CompanyPushAPI.PushObject_auto_alias(user.UserName, "heh", "你的请假申请提交成功！此申请无效", "2");//选择一种方式
            //}
            // PushPayload 
            try
            {
                var result1 = client.SendPush(payload);//推送


            }
            catch (APIRequestException e)//处理请求异常
            {
                var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode + "Error Message: " + e.ErrorCode;
                //return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (APIConnectionException e)//处理连接异常
            {
                var message = e.Message;
               // return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        //推送到审核人
        public static void Audit(string temp,string companyId)
        {
            string[] auditors = temp.Split(',');
            string[] audit = new string[auditors.Length];
            for (int i = 0; i < auditors.Length; i++)
            {
                string temp1 = auditors[i];
                audit[i] = temp1.Substring(0, 11);
            }
            PushPayload payload = CompanyPushAPI.PushObject_auto_alias(companyId,audit, "heh", "你需要审核的申请！", "2");//选择一种方式
            try
            {
                var result1 = client.SendPush(payload);//推送


            }
            catch (APIRequestException e)//处理请求异常
            {
                var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode + "Error Message: " + e.ErrorCode;

            }
            catch (APIConnectionException e)//处理连接异常
            {
                var message = e.Message;

            }
        }
        //推送给申请人
        public static void Push(string staffNumber, string companyId, byte status)
        {
            UserModels user1 = (from u in sdb.Users where u.StaffNumber == staffNumber && u.CompanyId == companyId select u).FirstOrDefault();
            if (user1 != null)
            {
                string info = null;
                if (status == 3)
                {
                    info = "你的申请审核通过！";
                }
                else if (status == 1)
                {
                    info = "你的申请处于在审核！";
                }
                else if (status ==5)
                {
                    info = "你的申请无效";
                }
                else
                    info = "你的申请审核拒绝";
                PushPayload payload = CompanyPushAPI.PushObject_auto_alias(user1.CompanyId,user1.UserName, "heh", info, "2");//选择一种方式
                try
                {
                    var result1 = client.SendPush(payload);//推送


                }
                catch (APIRequestException e)//处理请求异常
                {
                    var message = "Error response from JPush server. Should review and fix it." + "HTTP Status:" + e.Status + "Error Code: " + e.ErrorCode + "Error Message: " + e.ErrorCode;

                }
                catch (APIConnectionException e)//处理连接异常
                {
                    var message = e.Message;

                }
            }
        }
        public static void ReturnStatus(int auditProcessAId, byte status, string userName)
        {
            UserModels user = (from u in sdb.Users where u.UserName == userName select u).FirstOrDefault();
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            AuditApplication application = db.AuditApplications.Find(auditProcessAId);//修改Application的状态
            application.State = status;
            string btype = application.BType.Substring(0, 2);
            switch (btype)
            {
                case "21":
                    {//员工
                        Staff someone = (from p in db.Staffs where (p.BillNumber == application.BNumber) && (p.BillTypeNumber == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;//已审 
                            SystemDbContext sysdb = new SystemDbContext();
                            /*BindCodes*/
                            var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == user.CompanyId && p.StaffNumber == someone.StaffNumber) select p).ToList();
                            if (Ucount.Count == 0)
                            {//木有该员工
                                BindCode user1 = new BindCode();
                                string CompanyDbName = "Bonsaii" + user1.CompanyId;
                                user1.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                                user1.CompanyId = user1.CompanyId;
                                user1.StaffNumber = someone.StaffNumber;
                                user1.RealName = someone.Name;
                                user1.BindingCode = someone.BindingCode;
                                user1.Phone = someone.IndividualTelNumber;
                                user1.BindTag = false;
                                user1.LastTime = DateTime.Now;
                                user1.IsAvail = true;
                                sysdb.BindCodes.Add(user1);
                                sysdb.SaveChanges();
                            }
                            db.SaveChanges();
                            Push(someone.StaffNumber, user.CompanyId, status);
                        }
                        
                    } break;
                case "22":
                    { //变更
                        StaffChange someone = (from p in db.StaffChanges where (p.BillNumber == application.BNumber) && (p.BillTypeNumber == application.BType) select p).ToList().SingleOrDefault();
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
                                staff.AuditPerson = userName;
                                staff.Head = someone.Head;
                                staff.LogicCardNumber = someone.LogicCardNumber;
                                staff.HeadType = someone.HeadType;
                                staff.IDCardNumber = someone.IDCardNumber;
                                staff.DeadlineDate = someone.DeadlineDate;
                            }
                            db.SaveChanges();
                            Push(someone.StaffNumber, user.CompanyId, status);
                        }
                       
                    }
                    break;
                case "23":
                    { //离职
                        StaffApplication someone = (from p in db.StaffApplications where (p.BillNumber == application.BNumber) && (p.BillTypeNumber == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                            if (someone.AuditStatus == 3)
                            { //审核通过就在Staff里面把标志置为1
                                var staffId = (from p in db.Staffs where p.StaffNumber == someone.StaffNumber && p.ArchiveTag != true select p.Number).ToList().FirstOrDefault();
                                Staff staff = db.Staffs.Find(staffId);
                                staff.ArchiveTag = true;//true 代表离职
                                staff.BindingCode = null;
                                //要把信息写到离职档案中
                                StaffArchive staffArchive = new StaffArchive();
                                staffArchive.BillTypeNumber = someone.BillTypeNumber;
                                staffArchive.BillTypeName = someone.BillTypeName;
                                staffArchive.BillNumber = someone.BillNumber;
                                staffArchive.StaffNumber = staff.StaffNumber;
                                staffArchive.StaffName = staff.Name;
                                staffArchive.LeaveDate = someone.HopeLeaveDate;
                                staffArchive.Department = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).ToList().FirstOrDefault();
                                staffArchive.IdenticationNumber = staff.IdentificationNumber;
                                staffArchive.RecordPerson = staff.RecordPerson;
                                staffArchive.RecordTime = staff.RecordTime;
                                staffArchive.BlackList = false;
                                staffArchive.WorkPlus = false;
                                staff.ArchiveTag = true;//true 代表离职
                                db.StaffArchives.Add(staffArchive); //Users表中离职；
                                //修改系统表
                                SystemDbContext sysdb = new SystemDbContext();
                                var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == user.CompanyId && p.StaffNumber == someone.StaffNumber) select p).SingleOrDefault();
                                if (Ucount != null)
                                {
                                    sysdb.BindCodes.Remove(Ucount);
                                    sysdb.SaveChanges();
                                }
                                db.SaveChanges();
                                Push(someone.StaffNumber, user.CompanyId, status);
                            }
                        }
                    }
                    break;
                case "24":
                    {//技能
                        StaffSkill someone = (from p in db.StaffSkills where (p.BillNumber == application.BNumber) && (p.BillTypeNumber == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "25":
                    {//招聘
                        Recruitments someone = (from p in db.Recruitments where (p.BillCode == application.BNumber) && (p.BillType == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                       
                    }
                    break;
                case "26":
                    { //培训
                        TrainStart someone = (from p in db.TrainStarts where (p.BillNumber == application.BNumber) && (p.BillTypeNumber == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                       
                    }
                    break;
                case "27":
                    {//合同
                        Contract someone = (from p in db.Contracts where (p.BillNumber == application.BNumber) && (p.BillTypeNumber == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;//已审
                        }
                        
                    } break;
                case "31":
                    { //出差
                        EvectionApplies someone = (from p in db.EvectionApplies where (p.BillNumber == application.BNumber) && (p.BillType == application.BType) select p).SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        } 
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "32":
                    { //请假
                        VacateApplies someone = (from p in db.VacateApplies where (p.BillNumber == application.BNumber) && (p.BillType == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        } Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "33":
                    { //加班
                        OvertimeApplies someone = (from p in db.OvertimeApplies where (p.BillNumber == application.BNumber) && (p.BillType == application.BType) select p).ToList().SingleOrDefault();
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
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "34":
                    { //签卡
                        ChargeCardApplies someone = (from p in db.ChargeCardApplies where (p.BillNumber == application.BNumber) && (p.BillType == p.BillType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "35":
                    { //调休
                        DaysOffApplies someone = (from p in db.DaysOffApplies where (p.BillNumber == application.BNumber) && (p.BillType == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "36":
                    { //异地
                        VacateApplies someone = (from p in db.VacateApplies where (p.BillNumber == application.BNumber) && (p.BillType == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "37":
                    { //值班
                        OnDutyApplies someone = (from p in db.OnDutyApplies where (p.BillNumber == application.BNumber) && (p.BillType == application.BType) select p).ToList().SingleOrDefault();
                        if (someone != null)
                        {
                            someone.AuditStatus = status;
                        }
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                default:
                    break;
            }
            switch (btype)
            {
                case "27":
                    {//合同
                        Contract someone = (from p in db.Contracts where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;//已审
                    } break;
                case "21":
                    {//员工
                        Staff someone = (from p in db.Staffs where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;//已审
                    } break;
                case "24":
                    {//技能
                        StaffSkill someone = (from p in db.StaffSkills where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;
                    }
                    break;
                case "26":
                    { //培训
                        TrainStart someone = (from p in db.TrainStarts where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;
                    }
                    break;
                case "23":
                    { //离职
                        StaffApplication someone = (from p in db.StaffApplications where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;
                        if (someone.AuditStatus == 3)
                        { //审核通过就在Staff里面把标志置为1
                            var staffId = (from p in db.Staffs where p.StaffNumber == someone.StaffNumber && p.ArchiveTag != true select p.Number).ToList().FirstOrDefault();
                            Staff staff = db.Staffs.Find(staffId);
                            staff.ArchiveTag = true;//true 代表离职
                            //要把信息写到离职档案中
                            StaffArchive staffArchive = new StaffArchive();
                            staffArchive.BillTypeNumber = someone.BillTypeNumber;
                            staffArchive.BillTypeName = someone.BillTypeName;
                            staffArchive.StaffNumber = staff.StaffNumber;
                            staffArchive.StaffName = staff.Name;
                            staffArchive.Department = staff.Department;
                            staffArchive.IdenticationNumber = staff.IdentificationNumber;
                            staffArchive.RecordPerson = staff.RecordPerson;
                            staffArchive.RecordTime = staff.RecordTime;
                            staffArchive.BillNumber = Generate.GenerateBillNumber(staffArchive.BillTypeNumber, user.ConnectionString);
                            db.StaffArchives.Add(staffArchive);
                            db.SaveChanges();
                        }
                    }
                    break;
                case "22":
                    { //变更
                        StaffChange someone = (from p in db.StaffChanges where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
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
                            staff.AuditPerson = user.Name;
                            staff.Head = someone.Head;
                            staff.LogicCardNumber = someone.LogicCardNumber;
                            staff.HeadType = someone.HeadType;
                            staff.IDCardNumber = someone.IDCardNumber;
                            staff.DeadlineDate = someone.DeadlineDate;
                        }
                        db.SaveChanges();
                    }
                    break;
                case "33":
                    { //加班
                        OvertimeApplies someone = (from p in db.OvertimeApplies where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;

                        Push(someone.StaffNumber, user.CompanyId,status);

                    }
                    break;
                case "32":
                    { //请假
                        VacateApplies someone = (from p in db.VacateApplies where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "34":
                    { //请假
                        ChargeCardApplies someone = (from p in db.ChargeCardApplies where p.BillNumber == application.BNumber select p).ToList().SingleOrDefault();
                        someone.AuditStatus = status;
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "31":
                    { //出差
                        EvectionApplies someone = (from p in db.EvectionApplies where p.BillNumber == application.BNumber select p).SingleOrDefault();
                        someone.AuditStatus = status;
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "35":
                    { //出差
                        DaysOffApplies someone = (from p in db.DaysOffApplies where p.BillNumber == application.BNumber select p).SingleOrDefault();
                        someone.AuditStatus = status;
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "36":
                    { //出差
                        OffSiteApplies someone = (from p in db.OffSiteApplies where p.BillNumber == application.BNumber select p).SingleOrDefault();
                        someone.AuditStatus = status;
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;
                case "37":
                    { //出差
                        OnDutyApplies someone = (from p in db.OnDutyApplies where p.BillNumber == application.BNumber select p).SingleOrDefault();
                        someone.AuditStatus = status;
                        Push(someone.StaffNumber, user.CompanyId, status);
                    }
                    break;

                default:
                    break;
            }
            db.SaveChanges();
        }
        //如果审核通过 执行这段代码
        public static void PassStep(int id, string userName)
        {
            UserModels user = (from u in sdb.Users where u.UserName == userName select u).FirstOrDefault();
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            AuditProcess auditProcess = db.AuditProcesses.Find(id);

            var item = (from p in db.AuditSteps where p.SId == auditProcess.SId select p).FirstOrDefault();
            //表明item是最后一个节点。返回一个结果：审核成功
            if (item.ApprovedToSId == -1)
            {
                //AuditApplication auditApplication = db.AuditApplications.Find(auditProcess.AId);
                //auditApplication.State = 3;//已审
                ReturnStatus(auditProcess.AId, 3, userName);

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
                Audit(nextstep.Approver,user.CompanyId);
               
            }
            // db.SaveChanges();
        }
        //如果审核不通过就执行这个函数
        public static void NotPassStep(int id, string userName)
        {
            UserModels user = (from u in sdb.Users where u.UserName == userName select u).FirstOrDefault();
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            // auditProcess.Result = 4;//直接返回结果就是不通过
            var item = (from p in db.AuditSteps where p.SId == auditProcess.SId select p).FirstOrDefault();
            if (item.NotApprovedToSId == -1)//如果不再需要执行下一步了，就直接返回审核失败
            {
                ReturnStatus(auditProcess.AId, 4, userName);
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
                Audit(laststep.Approver,user.CompanyId);
            }

        }
    }
}