using Bonsaii.Models;
using Bonsaii.Models.Audit;
using Bonsaii.Models.Checking_in;
using Bonsaii.Models.Works;
using BonsaiiModels;
using BonsaiiModels.App;
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using JpushApiExample;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii;

namespace Bonsaii.Controllers
{
    public class AppController : AppBaseController
    {
        int missScore = 2;
        int lateScore = 1;
        int earlyScore = 1;
        JPushClient client = new JPushClient(CompanyPushAPI.app_key, CompanyPushAPI.master_secret);
        //public static string url_url =//http://192.168.0.19:8888图片的访问地址。

        SystemDbContext sdb = new SystemDbContext();
        public JsonResult TestApp()
        {

            return this.packageJson(1, "恭喜你，你已经是登录用户了！可以为所欲为！", null);
        }
        #region 推送
        //企业推送信息列表
        public JsonResult CompanyPush(string companyId, string userName)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            List<object> list = new List<object>();
            DateTime datebefore = DateTime.Now.AddMonths(-1);//最近一个月的信息
            DateTime datenow = DateTime.Now;
            var compush = (from push in db.CompanyPushes orderby push.RecordTime descending where push.RecordTime >= datebefore && push.RecordTime <= datenow select push).ToList();
            if (compush.Count != 0)
            {
                foreach (var temp in compush)
                {
                    if (temp.Target.Equals("所有用户"))
                    {
                        list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                    }
                    else if (temp.Target.Equals("选择用户(只要符合其中一种条件的推送)"))
                    {
                        string[] selects = temp.Tag.Split(',');
                        var staff = (from s in db.Staffs
                                     join d in db.Departments on s.Department equals d.DepartmentId
                                     where s.StaffNumber == user.StaffNumber
                                     select new { department = d.Name }).FirstOrDefault();
                        for (int i = 0; i < selects.Length; i++)
                        {

                            if (selects[i] == user.Gender || selects[i] == user.Strict || selects[i] == staff.department)//符合其中任何一条的条件即可
                            {
                                list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                                break;
                            }

                        }

                    }
                    else
                    {
                        string[] selects = temp.Tag.Split(',');
                        var staff = (from s in db.Staffs
                                     join d in db.Departments on s.Department equals d.DepartmentId
                                     where s.StaffNumber == user.StaffNumber
                                     select new { department = d.Name }).FirstOrDefault();
                        int i = 0;
                        for (; i < selects.Length; i++)
                        {

                            if (selects[i] == user.Gender || selects[i] == user.Strict || selects[i] == staff.department)//符合所有条件
                            {
                                continue;
                            }
                            else//不符合条件就中断
                                break;

                        }
                        if (i == selects.Length)
                        {
                            list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                        }
                    }

                }
                return Json(new
                {
                    result = true,
                    data = list
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = false,
                msg = "没有推送信息"
            }, JsonRequestBehavior.AllowGet);

        }
        // GET: /CompanyPush/Details/5
        public ActionResult Details(int? id, string connString)
        {
            BonsaiiDbContext bonsaii = new BonsaiiDbContext(connString);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyPush companypush = bonsaii.CompanyPushes.Find(id);
            if (companypush == null)
            {
                return HttpNotFound();
            }
            return View(companypush);
        }
        //企业推送信息具体详情
        public JsonResult CompanyPushDetail(string companyId, string userName, int id)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            CompanyPush comPush = db.CompanyPushes.Find(id);
            if (comPush == null)
            {
                return Json(new
                {
                    result = false,
                    msg = "没有推送信息详情"
                }, JsonRequestBehavior.AllowGet);
            }
            string url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/PlatformPush/Details/?id=" + id + "&connString=" + user.ConnectionString + "&tag=" + Math.Log10(100021321);
            return Json(new
            {
                result = true,
                url = url,
                title = comPush.TagTitle,
                content = comPush.TagContent,
                pushTime = comPush.RecordTime.ToShortDateString(),
                pushPerson = comPush.PersonName
            }, JsonRequestBehavior.AllowGet);

        }
        //平台推送信息列表
        public JsonResult PlatformPush(string companyId, string userName)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            var push = (from p in sdb.Pushes orderby p.RecordTime descending select p).ToList();
            if (push.Count != 0)
            {
                List<object> list = new List<object>();
                string tt;
                if (user.BindTag == true)
                {
                    tt = "在职";
                }
                else
                {
                    tt = "不在职";
                }
                foreach (var temp in push)
                {
                    if (temp.Target.Equals("所有用户"))
                    {
                        list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                    }
                    else if (temp.Target.Equals("选择用户(只要符合其中一种条件的推送)"))
                    {
                        string[] selects = temp.Tag.Split(',');
                        for (int i = 0; i < selects.Length; i++)
                        {

                            if (selects[i] == user.Gender || selects[i] == user.Strict || selects[i] == tt)//符合其中任何一条的条件即可
                            {
                                list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                                break;
                            }
                        }
                    }
                    else
                    {

                        string[] selects = temp.Tag.Split(',');
                        int i = 0;
                        for (; i < selects.Length; i++)
                        {

                            if (selects[i] == user.Gender || selects[i] == user.Strict || selects[i] == tt)
                            {
                                continue;
                            }
                            else
                                break;
                        }
                        if (i == selects.Length)
                        {
                            list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                        }
                    }
                    list.Add(new { id = temp.Id, title = temp.TagTitle, picture = temp.Url });
                }
                return Json(new
                {
                    result = true,
                    data = list
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = false,
                msg = "没有推送信息"
            }, JsonRequestBehavior.AllowGet);
        }
        //平台推送信息具体详情
        public JsonResult PushInfoDetail(string companyId, string userName, int id)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            Push p = sdb.Pushes.Find(id);
            if (p != null)
            {
                string url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/PlatformPush/Look/" + id;
                return Json(new
                {
                    result = true,
                    url = url,

                    title = p.TagTitle,
                    content = p.TagContent,
                    pushTime = p.RecordTime.ToShortDateString(),
                    pushPerson = p.PersonName
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = false,
                msg = "没有推送信息详情"
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 申请单
        //给请假申请加审批流  （加了推送）
        public byte AuditApplicationVacate(VacateApplies vacateApplies, string connString, string userName, string companyId)//(string BillTypeNumber,int id)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
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
                    auditApplication.BNumber = vacateApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = userName;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                auditApplication.Info =
                    "单号:" + vacateApplies.BillNumber + ";" +
                    "员工:" + vacateApplies.StaffNumber + ";" +
                    "请假开始日期:" + vacateApplies.StartDateTime.ToString() + ";" +
                    "请假结束日期:" + vacateApplies.EndDateTime.ToString() + ";" +
                    "请假时数:" + vacateApplies.Hours + ";" +
                    "请假理由:" + vacateApplies.Reason + ";" +
                    "备注:" + vacateApplies.Remark + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();
                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 6;
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
                        AppAudit.Audit(step.Approver, companyId);//推送给审核人

                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 6;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        //给出差申请加审批流 （加了推送）
        public byte AuditApplicationEvection(EvectionApplies evectionApplies, string connString, string userName, string companyId)//(string BillTypeNumber,int id)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == evectionApplies.BillType select p).ToList().FirstOrDefault();

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
                                    (evectionApplies.BillType == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    auditApplication.BNumber = evectionApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = userName;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单别：" + evectionApplies.BillType + ";" +
                 "单号:" + evectionApplies.BillNumber + ";" +
                 "员工:" + evectionApplies.StaffNumber + ";" +
                 "开始时间:" + evectionApplies.StartDateTime.ToString() + ";" +
                 "结束时间:" + evectionApplies.EndDateTime.ToString() + ";" +
                 "出差天数:" + evectionApplies.Days + ";" +
                 "理由:" + evectionApplies.Reason + ";" +
                 "备注:" + evectionApplies.Remark + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 6;
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
                        AppAudit.Audit(step.Approver, companyId);//推送给审核人
                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 6;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        //给加班申请加审批流 （加了推送）
        public byte AuditApplicationOvertime(OvertimeApplies overtimeApplies, string connString, string userName, string companyId)//(string BillTypeNumber,int id)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
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
                    auditApplication.BNumber = overtimeApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = userName;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单别：" + overtimeApplies.BillType + ";" +
                 "单号:" + overtimeApplies.BillNumber + ";" +
                 "员工:" + overtimeApplies.StaffNumber + ";" +
                 "开始时间:" + overtimeApplies.StartDateTime.ToString() + ";" +
                 "结束时间:" + overtimeApplies.EndDateTime.ToString() + ";" +
                 "加班小时数:" + overtimeApplies.Hours + ";" +
                 "理由:" + overtimeApplies.Reason + ";" +
                 "备注:" + overtimeApplies.Remark + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 6;
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
                        AppAudit.Audit(step.Approver, companyId);//推送给审核人
                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 6;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        //给补卡签卡加审批流 （加了推送）
        public byte AuditApplicationChargeCard(ChargeCardApplies chargeCardApplies, string connString, string userName, string companyId)//(string BillTypeNumber,int id)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == chargeCardApplies.BillType select p).ToList().FirstOrDefault();

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
                                    (chargeCardApplies.BillType == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    auditApplication.BNumber = chargeCardApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = userName;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单别：" + chargeCardApplies.BillType + ";" +
                 "单号:" + chargeCardApplies.BillNumber + ";" +
                 "员工:" + chargeCardApplies.StaffNumber + ";" +
                 "签卡日期:" + chargeCardApplies.DateTime.ToString() + ";" +
                 "备注:" + chargeCardApplies.Remark + ";" +
                 "签卡理由:" + chargeCardApplies.Reason + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 6;
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
                        AppAudit.Audit(step.Approver, companyId);//推送给审核人
                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 6;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        //给调休申请加审批流（加了推送）
        public byte AuditApplicationDaysOff(DaysOffApplies daysOffApplies, string connString, string userName, string companyId)//(string BillTypeNumber,int id)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
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
                    auditApplication.BNumber = daysOffApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单别：" + daysOffApplies.BillType + ";" +
                 "单号:" + daysOffApplies.BillNumber + ";" +
                 "员工:" + daysOffApplies.StaffNumber + ";" +
                 "开始时间:" + daysOffApplies.StartDateTime.ToString() + ";" +
                 "结束时间:" + daysOffApplies.EndDateTime.ToString() + ";" +
                 "调休时数:" + daysOffApplies.Hours + ";" +
                 "理由:" + daysOffApplies.Reason + ";" +
                 "备注:" + daysOffApplies.Remark + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 6;
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
                        AppAudit.Audit(step.Approver, companyId);//推送给审核人

                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 6;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        //给调异地考勤加审批流（加了推送）
        public byte AuditApplicationOffSite(OffSiteApplies offSiteApplies, string connString, string userName, string companyId)//(string BillTypeNumber,int id)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == offSiteApplies.Type select p).ToList().FirstOrDefault();

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
                                    (offSiteApplies.Type == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    auditApplication.BNumber = offSiteApplies.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                 "单别：" + offSiteApplies.Type + ";" +
                 "单号:" + offSiteApplies.BillNumber + ";" +
                 "员工:" + offSiteApplies.StaffNumber + ";" +
                 "时间:" + offSiteApplies.Date.ToString() + ";" +
                        //"结束时间:" + offSiteApplies.EndDateTime + ";" +
                        //"调休时数:" + offSiteApplies.Hours + ";" +
                 "理由:" + offSiteApplies.Reason + ";" +
                 "地址:" + offSiteApplies.Address + ";";
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    if (step == null)
                    {
                        return 6;
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
                        AppAudit.Audit(step.Approver, companyId);//推送给审核人

                    }
                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 6;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        /// <summary>
        /// 封装接口调用要返回的Json对象
        /// </summary>
        /// <param name="result">结果值,0代表请求失败，1是成功，-1表示APP用户还没有登录</param>
        /// <param name="msg">执行的结果信息</param>
        /// <param name="data">执行的结果数据</param>
        /// <returns></returns>
        public JsonResult packageJson(int result, string msg, object data)
        {
            return Json(new
            {
                Result = result,
                Msg = msg,
                Data = data
            }, JsonRequestBehavior.AllowGet);
        }




        //异地考勤          
        public JsonResult YiDiKaoQin(string staffNumber, string companyId, string type, string latitude, string longitude, string address, string reason, string photoCount)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            OffSiteApplies kaoqin = new OffSiteApplies();
            kaoqin.StaffNumber = staffNumber;
            kaoqin.Type = type;
            kaoqin.BillNumber = Generate.GenerateBillNumber(type, user.ConnectionString);
            kaoqin.Latitude = latitude;
            kaoqin.Longitude = longitude;
            kaoqin.Address = address;
            kaoqin.Reason = reason;
            kaoqin.IsRead = false;
            kaoqin.Date = DateTime.Now;//Convert.ToDateTime(date);
            byte status = AuditApplicationOffSite(kaoqin, user.ConnectionString, user.UserName, user.CompanyId);
            //需要对原表做出的修改
            kaoqin.AuditStatus = status;

            db1.OffSiteApplies.Add(kaoqin);
            db1.SaveChanges();

            int photocount = Convert.ToInt16(photoCount);
            for (int i = 1; i <= photocount; i++)
            {
                HttpPostedFileBase photo = Request.Files["photo" + Convert.ToString(i)];
                VacatePhoto yidi = new VacatePhoto();
                yidi.StaffNumber = staffNumber;
                yidi.Date = DateTime.Now;
                yidi.BillSort = type;
                yidi.PhotoType = photo.ContentType;//获取图片类型
                yidi.Photo = new byte[photo.ContentLength];//新建一个长度等于图片大小的二进制地址
                photo.InputStream.Read(yidi.Photo, 0, photo.ContentLength);//将image读取到Photo中
                db1.VacatePhotos.Add(yidi);
                db1.SaveChanges();
            }
            AppAudit.PushMe(companyId, user.UserName, status, "异地考勤");

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            // return Json();

        }
        //单据性质(加审批流程)
        public JsonResult BillProperty(string companyId, string billSort, string type)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            List<object> list = new List<object>();
            List<object> listtype = new List<object>();
            var billproperty = (from bp in db1.BillProperties where bp.BillSort == billSort select bp).ToList();
            int tt = Convert.ToInt16(type);
            var listtype1 = (from sp in db1.StaffParams where sp.StaffParamTypeId == tt select sp).ToList();

            foreach (var temp in billproperty)
            {
                var template = (from t in db1.AuditTemplates where t.BType == temp.Type select t).ToList();
                DateTime datetime = DateTime.Now;
                List<object> list1 = new List<object>();
                int flag = 1;
                if (template.Count() != 0)//模板存在添加模板
                {
                    foreach (var temp1 in template)
                    {
                        if (temp1.StartTime <= datetime && temp1.ExpireTime >= datetime && flag == 1)
                        {
                            var step = (from s in db1.AuditSteps where s.TId == temp1.Id select s).ToList();//模板对应的审批流程
                            if (step.Count() != 0)
                            {
                                foreach (var steps in step)
                                {
                                    list1.Add(new { name = steps.Name, phone = steps.Approver });
                                }
                                flag = 0;
                                break;
                            }


                        }
                    }

                }
                list.Add(new { type = temp.Type, typename = temp.TypeName, auditprocess = list1 });

            }
            foreach (var temp in listtype1)
            {
                listtype.Add(new { type = temp.Id, typename = temp.Value });
            }

            if (list.Count() != 0 && listtype.Count() != 0)
            {
                return Json(new { result = true, data = list, data1 = listtype }, JsonRequestBehavior.AllowGet);
            }
            else if (list.Count() != 0)
            {
                return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
            }
            else if (list.Count() == 0 && listtype.Count() != 0)
            {
                return Json(new { result = true, data = list, data1 = listtype }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, message = "此类单据不存在" }, JsonRequestBehavior.AllowGet);


        }
        //App异地考勤审核结果返回

        //
        //出差申请
        public JsonResult ChuChai(string staffNumber, string companyId, string billType, string chuChaiType, string addressCount, DateTime startDate, DateTime endDate, string remark)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            if (user == null)
            {
                return Json(new { result = false, message = "公司ID不存在" }, JsonRequestBehavior.AllowGet);
            }

            var staff = db1.Staffs.Where(s => s.StaffNumber.Equals(staffNumber)).ToList();
            if (staff.Count == 0)
            {
                return Json(new { result = false, message = "员工号码不存在" }, JsonRequestBehavior.AllowGet);
            }


            EvectionApplies chuchai = new EvectionApplies();
            chuchai.BillNumber = Generate.GenerateBillNumber(billType, user.ConnectionString);
            chuchai.BillType = billType;
            chuchai.StaffNumber = staffNumber;
            int addresscount = Convert.ToInt16(addressCount);
            int tt = Convert.ToInt16(chuChaiType);
            StaffParam sp = db1.StaffParams.Find(tt);
            chuchai.StartDateTime = startDate;
            chuchai.EndDateTime = endDate;
            chuchai.Reason = sp.Value;//出差原因
            chuchai.Remark = remark;
            chuchai.IsRead = false;
            TimeSpan time = chuchai.EndDateTime - chuchai.StartDateTime;
            chuchai.Days = (int)time.TotalDays;


            byte status = AuditApplicationEvection(chuchai, user.ConnectionString, user.UserName, user.CompanyId);
            //需要对原表做出的修改
            chuchai.AuditStatus = status;
            for (int i = 1; i <= addresscount; i++)
            {
                chuchai.Location += Request["address" + i] + "-";
            }

            db1.EvectionApplies.Add(chuchai);
            db1.SaveChanges();
            AppAudit.PushMe(companyId, user.UserName, status, "出差");

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        //请假申请
        public JsonResult QingJia(string staffNumber, string companyId, string billType, string qingJiaType, string photoCount, DateTime startDate, DateTime endDate, string remark)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            VacateApplies qingjia = new VacateApplies();
            int tt = Convert.ToInt16(qingJiaType);
            StaffParam sp = db1.StaffParams.Find(tt);
            qingjia.BillNumber = Generate.GenerateBillNumber(billType, user.ConnectionString);
            qingjia.BillType = billType;
            qingjia.StaffNumber = staffNumber;
            qingjia.StartDateTime = startDate;
            qingjia.EndDateTime = endDate;
            qingjia.Reason = sp.Value;
            qingjia.Remark = remark;
            qingjia.IsRead = false;
            TimeSpan time = qingjia.EndDateTime - qingjia.StartDateTime;
            qingjia.Hours = (int)time.TotalHours;


            byte status = AuditApplicationVacate(qingjia, user.ConnectionString, user.UserName, user.CompanyId);
            //需要对原表做出的修改
            qingjia.AuditStatus = status;

            db1.VacateApplies.Add(qingjia);
            db1.SaveChanges();
            int photocount = Convert.ToInt16(photoCount);
            for (int i = 1; i <= photocount; i++)
            {
                HttpPostedFileBase photo = Request.Files["photo" + Convert.ToString(i)];
                VacatePhoto vp = new VacatePhoto();
                vp.BillNumber = qingjia.BillNumber;
                vp.StaffNumber = staffNumber;
                vp.Date = DateTime.Now;
                vp.BillSort = billType;
                vp.PhotoType = photo.ContentType;//获取图片类型
                vp.Photo = new byte[photo.ContentLength];//新建一个长度等于图片大小的二进制地址
                photo.InputStream.Read(vp.Photo, 0, photo.ContentLength);//将image读取到Photo中
                db1.VacatePhotos.Add(vp);
                db1.SaveChanges();
            }
            AppAudit.PushMe(companyId, user.UserName, status, "请假");
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        //加班申请
        public JsonResult JiaBan(string staffNumber, string companyId, string billType, string jiaBanType, string photoCount, DateTime startDate, DateTime endDate, string remark)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            OvertimeApplies jiaban = new OvertimeApplies();
            int tt = Convert.ToInt16(jiaBanType);
            StaffParam sp = db1.StaffParams.Find(tt);
            jiaban.BillNumber = Generate.GenerateBillNumber(billType, user.ConnectionString);
            jiaban.BillType = billType;
            jiaban.StaffNumber = staffNumber;
            jiaban.StartDateTime = startDate;
            jiaban.EndDateTime = endDate;
            jiaban.Reason = sp.Value;
            jiaban.Hours = 12;
            jiaban.Remark = remark;
            jiaban.IsRead = false;
            TimeSpan time = jiaban.EndDateTime - jiaban.StartDateTime;
            jiaban.Hours = (int)time.TotalHours;
            byte status = AuditApplicationOvertime(jiaban, user.ConnectionString, user.UserName, user.CompanyId);
            //需要对原表做出的修改
            jiaban.AuditStatus = status;

            db1.OvertimeApplies.Add(jiaban);
            db1.SaveChanges();

            int photocount = Convert.ToInt16(photoCount);
            for (int i = 1; i <= photocount; i++)
            {
                HttpPostedFileBase photo = Request.Files["photo" + Convert.ToString(i)];
                VacatePhoto vp = new VacatePhoto();
                vp.BillNumber = jiaban.BillNumber;
                vp.StaffNumber = staffNumber;
                vp.Date = DateTime.Now;
                vp.BillSort = billType;
                vp.PhotoType = photo.ContentType;//获取图片类型
                vp.Photo = new byte[photo.ContentLength];//新建一个长度等于图片大小的二进制地址
                photo.InputStream.Read(vp.Photo, 0, photo.ContentLength);//将image读取到Photo中
                db1.VacatePhotos.Add(vp);
                db1.SaveChanges();
            }
            AppAudit.PushMe(companyId, user.UserName, status, "加班");

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);


        }

        //调休申请
        public JsonResult TiaoXiu(string staffNumber, string companyId, string billType, string tiaoXiuType, string photoCount, DateTime startDate, DateTime endDate, string remark)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            int tt = Convert.ToInt16(tiaoXiuType);
            StaffParam sp = db1.StaffParams.Find(tt);
            DaysOffApplies tiaoxiu = new DaysOffApplies();
            tiaoxiu.BillNumber = Generate.GenerateBillNumber(billType, user.ConnectionString);
            tiaoxiu.BillType = billType;
            tiaoxiu.StaffNumber = staffNumber;
            tiaoxiu.StartDateTime = startDate;
            tiaoxiu.EndDateTime = endDate;
            tiaoxiu.Reason = sp.Value;
            tiaoxiu.Remark = remark;
            tiaoxiu.IsRead = false;
            byte status = AuditApplicationDaysOff(tiaoxiu, user.ConnectionString, user.UserName, user.CompanyId);
            //需要对原表做出的修改
            tiaoxiu.AuditStatus = status;
            db1.DaysOffApplies.Add(tiaoxiu);
            db1.SaveChanges();
            int photocount = Convert.ToInt16(photoCount);
            for (int i = 1; i <= photocount; i++)
            {
                HttpPostedFileBase photo = Request.Files["photo" + Convert.ToString(i)];
                VacatePhoto vp = new VacatePhoto();
                vp.BillNumber = tiaoxiu.BillNumber;
                vp.StaffNumber = staffNumber;
                vp.Date = DateTime.Now;
                vp.BillSort = billType;
                vp.PhotoType = photo.ContentType;//获取图片类型
                vp.Photo = new byte[photo.ContentLength];//新建一个长度等于图片大小的二进制地址
                photo.InputStream.Read(vp.Photo, 0, photo.ContentLength);//将image读取到Photo中
                db1.VacatePhotos.Add(vp);
                db1.SaveChanges();
            }
            AppAudit.PushMe(companyId, user.UserName, status, "调休");
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        //补签申请
        public JsonResult BuQian(string staffNumber, string companyId, string billType, string photoCount, DateTime date, string remark)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            ChargeCardApplies buka = new ChargeCardApplies();
            buka.BillNumber = Generate.GenerateBillNumber(billType, user.ConnectionString);
            buka.BillType = billType;
            buka.StaffNumber = staffNumber;
            buka.DateTime = date;
            buka.Remark = remark;
            buka.IsRead = false;

            byte status = AuditApplicationChargeCard(buka, user.ConnectionString, user.UserName, user.CompanyId);
            //需要对原表做出的修改
            buka.AuditStatus = status;
            db1.ChargeCardApplies.Add(buka);
            db1.SaveChanges();
            int photocount = Convert.ToInt16(photoCount);
            for (int i = 1; i <= photocount; i++)
            {
                HttpPostedFileBase photo = Request.Files["photo" + Convert.ToString(i)];
                VacatePhoto vp = new VacatePhoto();
                vp.BillNumber = buka.BillNumber;
                vp.StaffNumber = staffNumber;
                vp.Date = DateTime.Now;
                vp.BillSort = billType;
                vp.PhotoType = photo.ContentType;//获取图片类型
                vp.Photo = new byte[photo.ContentLength];//新建一个长度等于图片大小的二进制地址
                photo.InputStream.Read(vp.Photo, 0, photo.ContentLength);//将image读取到Photo中
                db1.VacatePhotos.Add(vp);
                db1.SaveChanges();
            }
            AppAudit.PushMe(companyId, user.UserName, status, "补签");
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region 考勤信息
        //个人月考勤
        public JsonResult YueKaoQin(string staffNumber, string companyId, string year, string month)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            int Year = Convert.ToInt16(year);
            int Month = Convert.ToInt16(month);
            List<object> list1 = new List<object>();
            List<object> list2 = new List<object>();
            List<object> list3 = new List<object>();
            List<object> list4 = new List<object>();
            List<object> list5 = new List<object>();
            List<object> list6 = new List<object>();
            List<object> list7 = new List<object>();
            //早退次数
            var leaveearly = (from sics in db1.SignInCardStatus
                              where sics.StaffNumber == staffNumber && sics.WorkDate.Year == Year && sics.WorkDate.Month == Month && sics.LeaveEarlyMinutes != 0
                              select sics).ToList();
            foreach (var temp in leaveearly)
            {
                list1.Add(new
                {
                    workdate = temp.WorkDate.ToShortDateString(),
                    needworktime = temp.NeedWorkTime.ToString().Substring(0, 5),
                    needstarttime = temp.NeedStartTime.ToString().Substring(0, 5),
                    needendtime = temp.NeedEndTime.ToString().Substring(0, 5),
                    signintime = temp.SignInTime.ToString(),
                    type = temp.Type,
                    status = "早退" + temp.LeaveEarlyMinutes + "分钟"
                });
            }
            //迟到次数
            var comelate = (from sics in db1.SignInCardStatus
                            where sics.StaffNumber == staffNumber && sics.WorkDate.Year == Year && sics.WorkDate.Month == Month && sics.ComeLateMinutes != 0
                            select sics).ToList();
            foreach (var temp in comelate)
            {
                list2.Add(new
                {
                    workdate = temp.WorkDate.ToShortDateString(),
                    needworktime = temp.NeedWorkTime.ToString().Substring(0, 5),
                    needstarttime = temp.NeedStartTime.ToString().Substring(0, 5),
                    needendtime = temp.NeedEndTime.ToString().Substring(0, 5),
                    signintime = temp.SignInTime.ToString(),
                    type = temp.Type,
                    status = "迟到" + temp.ComeLateMinutes + "分钟"
                });
            }
            //旷工
            var kuanggong = (from sics in db1.SignInCardStatus
                             where sics.StaffNumber == staffNumber && sics.WorkDate.Year == Year && sics.WorkDate.Month == Month && sics.SignInTime == null && sics.Type == "上班"
                             select sics).ToList();
            foreach (var temp in kuanggong)
            {
                list3.Add(new
                {
                    workdate = temp.WorkDate.ToShortDateString(),
                    needworktime = temp.NeedWorkTime.ToString().Substring(0, 5),
                    needstarttime = temp.NeedStartTime.ToString().Substring(0, 5),
                    needendtime = temp.NeedEndTime.ToString().Substring(0, 5),
                    signintime = temp.SignInTime.ToString(),
                    type = temp.Type,
                    status = "没有打卡"
                });
            }
            var qingjia = (from v in db1.VacateApplies where v.StaffNumber == staffNumber && v.StartDateTime.Year == Year && v.StartDateTime.Month == Month select v).ToList();//请假
            foreach (var temp in qingjia)
            {
                list4.Add(new { startTime = temp.StartDateTime.ToShortTimeString(), endTime = temp.EndDateTime.ToShortTimeString() });
            }
            var jiaban = (from o in db1.OvertimeApplies where o.StaffNumber == staffNumber && o.StartDateTime.Year == Year && o.StartDateTime.Month == Month select o).ToList();//加班
            foreach (var temp in jiaban)
            {
                list5.Add(new { startTime = temp.StartDateTime.ToShortTimeString(), endTime = temp.EndDateTime.ToShortTimeString() });
            }
            var chuchai = (from e in db1.EvectionApplies where e.StaffNumber == staffNumber && e.StartDateTime.Year == Year && e.StartDateTime.Month == Month select e).ToList();//出差
            foreach (var temp in chuchai)
            {
                list6.Add(new { startTime = temp.StartDateTime.ToShortTimeString(), endTime = temp.EndDateTime.ToShortTimeString() });
            }
            //签卡
            var qianka = (from v in db1.ChargeCardApplies where v.StaffNumber == staffNumber && v.DateTime.Year == Year && v.DateTime.Month == Month select v).ToList();
            foreach (var temp in qianka)
            {
                list7.Add(new { startTime = temp.DateTime.ToShortTimeString() });
            }



            //应出天数

            DateTime date = new DateTime(Year, Month, 1);
            DateTime date1 = new DateTime(Year, Month + 1, 1);
            TimeSpan time = date1 - date;
            int day = time.Days;
            var holiday = (from h in db1.HolidayTables where h.StaffNumber == staffNumber && h.Date.Year == Year && h.Date.Month == Month select h).ToList();
            int yingchu = day - holiday.Count;
            return Json(new
            {
                result = true,
                leaveearly = leaveearly.Count(),
                comelate = comelate.Count(),
                jiaban = jiaban.Count,
                qingjia = qingjia.Count,
                kuanggong = kuanggong.Count,
                qianka = qianka.Count,
                yingchu = yingchu,
                chuchai = chuchai.Count,
                leaveearlydetail = list1,
                comelatedetail = list2,
                kuanggongdetail = list3,
                qingjiadetail = list4,
                jiabandetail = list5,
                chuchaidetail = list6,
                qiankadetail = list7

            }, JsonRequestBehavior.AllowGet);


        }

        //个人日考勤
        public JsonResult KaoQinInfo(string staffNumber, string companyId, DateTime date)
        {
            try
            {
                DateTime date1 = DateTime.Now;
                if(date==DateTime.Now.Date)
                {
                    CheckingInManage check = new CheckingInManage();
                    CheckingInManage.CalculateDay(date);
                    check.CalculateStaffDay(staffNumber,date);
                }
                //CheckingInManage.Equals("");
                UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);

                Staff staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).SingleOrDefault();
                int classOrder = Convert.ToInt16(staff.ClassOrder);
                Works work = db1.Works.Find(classOrder);
                var workTimes = (from wt in db1.WorkTimes orderby wt.StartTime ascending where wt.WorksId == work.Id select wt).ToList();
                int hours = 0;
                double oldHours = 0;
                
                foreach (var temp in workTimes)
                {
                    hours += temp.WorkHours;
                    if (date1.TimeOfDay > temp.StartTime && date1.TimeOfDay < temp.EndTime)
                    {
                        oldHours += (date1.TimeOfDay - temp.StartTime).TotalHours;
                    }
                    else if (date1.TimeOfDay > temp.EndTime)
                    {
                        oldHours += temp.WorkHours;
                    }

                }
                string orderId = staff.ClassOrder;
                var worktime = (from wt in db1.WorkTimes orderby wt.StartTime ascending where wt.WorksId.ToString() == staff.ClassOrder select wt).ToList();
                string worktimes = null;
                foreach (var temp in worktime)
                {
                    worktimes += temp.StartTime.ToString().Substring(0, 5) + "-" + temp.EndTime.ToString().Substring(0, 5) + " ";
                }
                
                List<object> list = new List<object>();
                // DateTime time = Convert.ToDateTime(date);
                var signRecord = (from s in db1.SignInCardStatus where s.WorkDate == date && s.StaffNumber == staffNumber orderby s.NeedWorkTime ascending select s).ToList();
                string status = null;
                foreach (var temp in signRecord)
                {
                    if (DateTime.Now.Date == temp.WorkDate)
                    {
                        if (temp.ComeLateMinutes != 0 && temp.LeaveEarlyMinutes == 0)
                        {

                            status = "迟到" + temp.ComeLateMinutes + "分钟";

                        }
                        else if (temp.ComeLateMinutes == 0 && temp.LeaveEarlyMinutes != 0)
                        {

                            status = "早退" + temp.LeaveEarlyMinutes + "分钟";

                        }
                        else if (temp.ComeLateMinutes == 0 && temp.LeaveEarlyMinutes == 0)
                        {
                            if (temp.SignInTime == null)
                            {
                                if (DateTime.Now.TimeOfDay > temp.NeedEndTime)
                                {
                                    status = "没有打卡，异常";
                                }
                                else
                                {
                                    status = "没有打卡，正常";
                                }

                            }
                            else
                            {
                                if (temp.SignInTime > temp.NeedEndTime)
                                {
                                    status = "打卡异常";
                                }
                                else
                                {
                                    status = "打卡正常";
                                }


                            }
                        }
                        list.Add(new
                        {
                            workdate = temp.WorkDate.ToShortDateString(),
                            needworktime = temp.NeedWorkTime.ToString().Substring(0, 5),
                            needstarttime = temp.NeedStartTime.ToString().Substring(0, 5),
                            needendtime = temp.NeedEndTime.ToString().Substring(0, 5),
                            signintime = temp.SignInTime.ToString(),
                            work = worktimes,
                            type = temp.Type,
                            status = status,
                            banCi = work.Name,
                            workTime = hours,
                            oldHours = oldHours
                        });
                    }
                    else
                    {
                        if (temp.ComeLateMinutes != 0 && temp.LeaveEarlyMinutes == 0)
                        {

                            status = "迟到" + temp.ComeLateMinutes + "分钟";

                        }
                        else if (temp.ComeLateMinutes == 0 && temp.LeaveEarlyMinutes != 0)
                        {

                            status = "早退" + temp.LeaveEarlyMinutes + "分钟";

                        }
                        else if (temp.ComeLateMinutes == 0 && temp.LeaveEarlyMinutes == 0)
                        {
                            if (temp.SignInTime == null)
                            {
                                status = "没有打卡，异常";
                            }
                            else
                            {
                                if (temp.SignInTime > temp.NeedEndTime)
                                {
                                    status = "打卡异常";
                                }
                                else
                                {
                                    status = "打卡正常";
                                }


                            }
                        }
                        list.Add(new
                        {
                            workdate = temp.WorkDate.ToShortDateString(),
                            needworktime = temp.NeedWorkTime.ToString().Substring(0, 5),
                            needstarttime = temp.NeedStartTime.ToString().Substring(0, 5),
                            needendtime = temp.NeedEndTime.ToString().Substring(0, 5),
                            signintime = temp.SignInTime.ToString(),
                            work = worktimes,
                            type = temp.Type,
                            status = status,
                            banCi = work.Name,
                            workTime = hours,
                            oldHours = oldHours
                        });
                    }
                }
                return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return Json(new { result = false, message = ee.Message }, JsonRequestBehavior.AllowGet);

            }



        }
        //管理报表
        public JsonResult ManageReport(string userName, string companyId, DateTime date)//date:当日的时间
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var department = (from d in db1.Departments select d).ToList();
            // DateTime today = Convert.ToDateTime(date);

            List<object> list = new List<object>();
            if (department.Count != 0)//图形数据
            {
                foreach (var temp in department)//查询每个部门的员工
                {
                    int late = 0;//迟到
                    int early = 0;//早退
                    int miss = 0;//缺卡
                    var staffs = (from s in db1.Staffs where s.Department == temp.DepartmentId select s).ToList();
                    if (staffs.Count != 0)//查询有员工的部门
                    {
                        foreach (var staff in staffs)
                        {
                            var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staff.StaffNumber && si.WorkDate == date select si).ToList();
                            foreach (var sign in signin)
                            {
                                if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                                {
                                    miss++;
                                }
                                else if (sign.ComeLateMinutes != 0)//迟到
                                {
                                    late++;

                                }
                                else if (sign.LeaveEarlyMinutes != 0)//早退
                                {
                                    early++;
                                }
                            }
                        }
                        list.Add(new { department = temp.Name, late = late, early = early, miss = miss });
                    }
                }

            }
            else
            {
                return Json(new { result = false, message = "公司目前还没有部门" }, JsonRequestBehavior.AllowGet);
            }
            var signall = (from sii in db1.SignInCardStatus where sii.WorkDate == date select sii).ToList();//全公司的员工今天的打卡情况:当前迟到、早退、缺卡情况。
            var chuchai = (from e in db1.EvectionApplies where e.StartDateTime <= DateTime.Now && DateTime.Now <= e.EndDateTime select e).ToList();//出差
            var qingjia = (from v in db1.VacateApplies where v.StartDateTime <= DateTime.Now && DateTime.Now <= v.EndDateTime select v).ToList();//请假
            var jiaban = (from o in db1.OvertimeApplies where o.StartDateTime <= DateTime.Now && DateTime.Now <= o.EndDateTime select o).ToList();//加班
            var qianka = (from c in db1.ChargeCardApplies where c.DateTime.Year == date.Year && c.DateTime.Month == date.Month && c.DateTime.Day == date.Day select c).ToList();//签卡
            var tiaoxiu = (from r in db1.DaysOffApplies where r.StartDateTime <= DateTime.Now && DateTime.Now <= r.EndDateTime select r).ToList();//调休
            var yidi = (from y in db1.OffSiteApplies where y.Date.Year == date.Year && y.Date.Month == date.Month && y.Date.Day == date.Day select y).ToList();//异地
            int lateall = 0;//迟到
            int earlyall = 0;//早退
            int missall = 0;//缺卡
            int noall = 0;//没有打卡
            foreach (var sign in signall)
            {
                if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                {
                    missall++;
                }
                else if (sign.ComeLateMinutes != 0)//迟到
                {
                    lateall++;

                }
                else if (sign.LeaveEarlyMinutes != 0)//早退
                {
                    earlyall++;
                }
                else if (sign.SignInTime == null)
                {
                    noall++;
                }
            }
            return Json(new
            {
                result = true,
                data = list,
                miss = missall,
                late = lateall,
                early = earlyall,
                nodaka = noall,
                chuchai = chuchai.Count(),
                qingjia = qingjia.Count(),
                jiaban = jiaban.Count(),
                qianka = qianka.Count(),
                tiaoxiu = tiaoxiu.Count(),
                yidi = yidi.Count()
            }, JsonRequestBehavior.AllowGet);

        }
        //缺勤记录
        public JsonResult NoSignIn(string companyId, string userName, DateTime date)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                List<object> list = new List<object>();
                //DateTime datetime = Convert.ToDateTime(date);
                var department = (from d in db1.Departments select d).ToList();
                foreach (var depart in department)//查询每个部门的员工
                {
                    List<object> list1 = new List<object>();
                    var staffs = (from s in db1.Staffs where s.Department == depart.DepartmentId select s).ToList();
                    foreach (var staff in staffs)//员工的缺勤统计
                    {
                        var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staff.StaffNumber && si.SignInTime == null && si.WorkDate == date && si.Type == "上班" select si).ToList();
                        if (signin.Count != 0)
                        {
                            if (staff.Head != null)
                            {
                                list1.Add(new { name = staff.Name, count = signin.Count, headUrl = Picture(staff.Head, user.CompanyId + staff.StaffNumber) });
                            }
                            else
                            {
                                list1.Add(new { name = staff.Name, count = signin.Count, headUrl = "null" });
                            }
                        }
                    }
                    if (list1.Count != 0)
                    {
                        list.Add(new { department = depart.Name, staffs = list1 });
                    }
                }
                if (list.Count != 0)
                {
                    return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = false, message = "没有缺勤人员" });
            }
            return Json(new { result = false, message = "传入参数错误" });
        }
        //请假/调休（人员）
        public JsonResult QingJiaAndTiaoXiu(string companyId, string userName, DateTime date)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                List<object> list = new List<object>();//请假
                List<object> list1 = new List<object>();//调休
                List<object> listall = new List<object>();
                // DateTime datetime = Convert.ToDateTime(date);
                var qingjia = (from v in db1.VacateApplies where v.StartDateTime <= date && v.EndDateTime >= date select v).ToList();
                var tiaoxiu = (from r in db1.DaysOffApplies where r.StartDateTime <= date && r.EndDateTime >= date select r).ToList();
                if (qingjia.Count != 0)
                {
                    foreach (var temp in qingjia)
                    {
                        var department = (from s in db1.Staffs
                                          join d in db1.Departments on s.Department equals d.DepartmentId
                                          where s.StaffNumber == temp.StaffNumber
                                          select new { department = d.Name, name = s.Name }).FirstOrDefault();
                        UserModels user1 = (from u in sdb.Users where u.StaffNumber == temp.StaffNumber && u.UserName == userName select u).FirstOrDefault();
                        string headUrl = null;
                        if (user1 != null)
                        {
                            headUrl = Picture(user1.Head, user1.CompanyId + user1.UserName);
                        }
                        if (department != null)
                        {
                            list.Add(new { department = department.department, name = department.name, headUrl = headUrl });
                        }

                    }
                    listall.Add(new { qingJia = "请假人员", list = list });
                }
                else
                    listall.Add(new { qingJia = "请假人员", list = list });
                if (tiaoxiu.Count != 0)
                {
                    foreach (var temp in tiaoxiu)
                    {
                        var department = (from s in db1.Staffs
                                          join d in db1.Departments on s.Department equals d.DepartmentId
                                          where s.StaffNumber == temp.StaffNumber
                                          select new { department = d.Name, name = s.Name }).FirstOrDefault();
                        UserModels user1 = (from u in sdb.Users where u.StaffNumber == temp.StaffNumber && u.UserName == userName select u).FirstOrDefault();
                        string headUrl = null;
                        if (user1 != null)
                        {
                            headUrl = Picture(user1.Head, user1.CompanyId + user1.UserName);
                        }
                        if (department != null)
                        {
                            list1.Add(new { department = department.department, name = department.name, headUrl = headUrl });
                        }

                    }
                    listall.Add(new { tiaoXiu = "调休人员", list = list1 });
                }
                else
                    listall.Add(new { tiaoXiu = "调休人员", list = list1 });
                if (qingjia.Count == 0 && tiaoxiu.Count == 0)
                {
                    return Json(new { result = false, data = listall, message = "没有请假和调休人员" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = true, data = listall });
            }
            return Json(new { result = false, message = "传入参数错误" });
        }
        //值班/加班（人员）
        public JsonResult ZhiBanAndJiaBan(string companyId, string userName, DateTime date)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                List<object> list = new List<object>();//请假
                List<object> list1 = new List<object>();//调休
                List<object> listall = new List<object>();
                //DateTime datetime = Convert.ToDateTime(date);
                var jiaban = (from v in db1.OvertimeApplies where v.StartDateTime <= date && v.EndDateTime >= date select v).ToList();
                var zhiban = (from r in db1.OnDutyApplies where r.StartDateTime <= date && r.EndDateTime >= date select r).ToList();
                if (jiaban.Count != 0)
                {
                    foreach (var temp in jiaban)
                    {
                        var department = (from s in db1.Staffs
                                          join d in db1.Departments on s.Department equals d.DepartmentId
                                          where s.StaffNumber == temp.StaffNumber
                                          select new { department = d.Name, name = s.Name }).FirstOrDefault();
                        UserModels user1 = (from u in sdb.Users where u.StaffNumber == temp.StaffNumber && u.UserName == userName select u).FirstOrDefault();
                        string headUrl = null;
                        if (user1 != null)
                        {
                            headUrl = Picture(user1.Head, user1.CompanyId + user1.UserName);
                        }
                        if (department != null)
                        {
                            list.Add(new { department = department.department, name = department.name, headUrl = headUrl });
                        }

                    }
                    listall.Add(new { jiaBan = "加班人员", list = list });
                }
                else
                    listall.Add(new { jiaBan = "加班人员", list = list });
                if (zhiban.Count != 0)
                {
                    foreach (var temp in zhiban)
                    {
                        var department = (from s in db1.Staffs
                                          join d in db1.Departments on s.Department equals d.DepartmentId
                                          where s.StaffNumber == temp.StaffNumber
                                          select new { department = d.Name, name = s.Name }).FirstOrDefault();
                        UserModels user1 = (from u in sdb.Users where u.StaffNumber == temp.StaffNumber && u.UserName == userName select u).FirstOrDefault();
                        string headUrl = null;
                        if (user1 != null)
                        {
                            headUrl = Picture(user1.Head, user1.CompanyId + user1.UserName);
                        }
                        if (department != null)
                        {
                            list1.Add(new { department = department.department, name = department.name, headUrl = headUrl });
                        }

                    }
                    listall.Add(new { zhiBan = "值班人员", list = list1 });
                }
                else
                    listall.Add(new { zhiBan = "值班人员", list = list1 });
                if (jiaban.Count == 0 && zhiban.Count == 0)
                {
                    return Json(new { result = false, data = listall, message = "没有加班和值班人员" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = true, data = listall });
            }
            return Json(new { result = false, message = "传入参数错误" });
        }

        //公司荣誉墙（公司个人月考勤排名）
        public JsonResult HonorWallCompany(string userName, string companyId, int year, int month)
        {

            // int Year = Convert.ToInt16(year);
            // int Month = Convert.ToInt16(month);
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            List<HonerWallOrderViewModel> list = new List<HonerWallOrderViewModel>();
            var staffs = (from s in db1.Staffs select s).ToList();
            if (staffs.Count != 0)//查询有员工的部门
            {
                foreach (var staff in staffs)
                {
                    int late = 0;//迟到
                    int early = 0;//早退
                    int miss = 0;//缺卡
                    var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staff.StaffNumber && si.WorkDate.Year == year && si.WorkDate.Month == month select si).ToList();
                    foreach (var sign in signin)//每个员工的本月的上班记录
                    {
                        if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                        {
                            miss++;
                        }
                        else if (sign.ComeLateMinutes != 0)//迟到
                        {
                            late++;

                        }
                        else if (sign.LeaveEarlyMinutes != 0)//早退
                        {
                            early++;
                        }
                    }
                    var department = (from d in db1.Departments where d.DepartmentId == staff.Department select d).ToList();
                    //miss：5，late：2，early：2                     
                    HonerWallOrderViewModel honor = new HonerWallOrderViewModel();
                    honor.Department = department.FirstOrDefault().Name;
                    honor.Name = staff.Name;
                    honor.StaffNumber = staff.StaffNumber;
                    if (staff.Head != null)
                    {
                        honor.HeadUrl = Picture(staff.Head, user.CompanyId + staff.StaffNumber);
                    }
                    else
                    {
                        honor.HeadUrl = null;
                    }
                    honor.Score = 100 - (miss * missScore + late * lateScore + early * earlyScore);
                    list.Add(honor);
                    // list.Add(new { late = late, early = early, miss = miss });
                }

            }
            List<HonerWallOrderViewModel> order = new List<HonerWallOrderViewModel>();
            order = (from li in list orderby li.Score descending select li).ToList();
            return Json(new { result = true, data = order.Take(20) }, JsonRequestBehavior.AllowGet);


        }

        //部门荣誉墙(部门个人月考勤排名)
        public JsonResult HonorWallDepartment(string companyId, string userName, int year, int month)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            Staff s1 = db1.Staffs.Where(s => s.StaffNumber.Equals(user.StaffNumber)).FirstOrDefault();
            Department d1 = (from d in db1.Departments where d.DepartmentId == s1.Department select d).FirstOrDefault();
            if (s1 == null)
            {
                return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);
            }
            if (d1 == null)
            {
                return Json(new { result = false, message = "员工没有部门" }, JsonRequestBehavior.AllowGet);
            }
            List<HonerWallOrderViewModel> list = new List<HonerWallOrderViewModel>();
            int late = 0;//迟到
            int early = 0;//早退
            int miss = 0;//缺卡


            var staffs = (from s in db1.Staffs where s.Department == s1.Department select s).ToList();
            if (staffs.Count != 0)
            {

                foreach (var staff in staffs)
                {
                    late = 0; early = 0; miss = 0;
                    var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staff.StaffNumber && si.WorkDate.Year == year && si.WorkDate.Month == month select si).ToList();
                    foreach (var sign in signin)//每个员工的本月的上班记录
                    {
                        if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                        {
                            miss++;
                        }
                        else if (sign.ComeLateMinutes != 0)//迟到
                        {
                            late++;

                        }
                        else if (sign.LeaveEarlyMinutes != 0)//早退
                        {
                            early++;
                        }
                    }
                    HonerWallOrderViewModel honor = new HonerWallOrderViewModel();
                    honor.Name = staff.Name;
                    honor.StaffNumber = staff.StaffNumber;
                    if (staff.Head != null)
                    {
                        honor.HeadUrl = Picture(staff.Head, user.CompanyId + staff.StaffNumber);
                    }
                    else
                    {
                        honor.HeadUrl = null;
                    }
                    honor.Score = 100 - (miss * missScore + late * lateScore + early * earlyScore);
                    honor.Department = d1.Name;
                    list.Add(honor);
                }

            }

            List<HonerWallOrderViewModel> order = new List<HonerWallOrderViewModel>();
            order = (from li in list orderby li.Score descending select li).ToList();
            return Json(new { result = true, data = order.Take(20), department = d1.Name }, JsonRequestBehavior.AllowGet);


        }
        //今日考勤((ok)2016-3-9)
        public JsonResult PersonalDay(string staffNumber, string companyId, double money)//date：时间精确到分钟数
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            Staff staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).FirstOrDefault();
            if (staff != null)
            {
                int temp = Convert.ToInt16(staff.ClassOrder);
                int time = 0;
                var worktime = (from wt in db1.WorkTimes orderby wt.StartTime ascending where wt.WorksId == temp select wt).ToList();
                TimeSpan timespan = DateTime.Now.TimeOfDay;
                TimeSpan tp = new TimeSpan(0, 0, 0, 0);
                foreach (var work in worktime)//一天工作多长时间
                {
                    time += work.WorkHours;

                    if (timespan >= work.StartTime && timespan <= work.EndTime)
                    {
                        tp += timespan - work.StartTime;
                    }
                    else if (timespan > work.EndTime)
                    {
                        tp += work.EndTime - work.StartTime;
                    }
                }

                DateTime dt1 = (DateTime)staff.Entrydate;//入职时间
                DateTime dt2 = DateTime.Now;
                TimeSpan ts = dt2 - dt1;
                int total = time * ts.Days;
                double moneyday = tp.TotalHours * money;
                return Json(new { result = true, entryDays = ts.Days, entryHour = total, money = moneyday, message = "正确数据" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        //月考勤（个人月考勤（当月体检））
        public JsonResult PersonalMonth(string staffNumber, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).ToList();
            if (staff.Count != 0)
            {
                DateTime today = DateTime.Now;
                int late = 0;//迟到
                int early = 0;//早退
                int miss = 0;//缺卡
                var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staffNumber && si.WorkDate.Year == today.Year && si.WorkDate.Month == today.Month select si).ToList();
                foreach (var sign in signin)//每个员工的本月的上班记录
                {
                    if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                    {
                        miss++;
                    }
                    else if (sign.ComeLateMinutes != 0)//迟到
                    {
                        late++;

                    }
                    else if (sign.LeaveEarlyMinutes != 0)//早退
                    {
                        early++;
                    }
                }
                return Json(new { result = true, late = late, early = early, miss = miss, score = miss * 5 + late * 2 + early * 2 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        //月考勤（个人月考勤（当月体检明细））
        public JsonResult PersonalMonthDetail(string staffNumber, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).ToList();
            List<object> list = new List<object>();
            if (staff.Count != 0)
            {
                DateTime today = DateTime.Now;

                var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staffNumber && si.WorkDate.Year == today.Year && si.WorkDate.Month == today.Month && si.WorkDate <= today select si).ToList();
                foreach (var sign in signin)//每个员工的本月的上班记录
                {
                    if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                    {
                        list.Add(new { date = sign.WorkDate, worktime = sign.NeedWorkTime.ToString(), signin = sign.SignInTime.ToString(), message = "缺卡" });
                    }

                }
                foreach (var sign in signin)//每个员工的本月的上班记录
                {
                    if (sign.ComeLateMinutes != 0)//迟到
                    {
                        list.Add(new { date = sign.WorkDate, worktime = sign.NeedWorkTime.ToString(), signin = sign.SignInTime.ToString(), message = "迟到" + sign.ComeLateMinutes });
                    }

                }
                foreach (var sign in signin)//每个员工的本月的上班记录
                {
                    if (sign.LeaveEarlyMinutes != 0)//早退
                    {
                        list.Add(new { date = sign.WorkDate, worktime = sign.NeedWorkTime.ToString(), signin = sign.SignInTime.ToString(), message = "早退" + sign.LeaveEarlyMinutes });
                    }

                }
                return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        //部门当日考勤(与详情合并)
        public JsonResult DepartmentDay(string staffNumber, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).ToList();
            if (staff.Count != 0)
            {
                DateTime date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                string temptemp = staff.FirstOrDefault().Department;
                var staffnum = (from ss in db1.Staffs where ss.Department.Equals(temptemp) select ss).ToList();//当前部门人数
                var man = (from ss in db1.Staffs where ss.Department.Equals(temptemp) && ss.Gender.Equals("男") select ss).ToList();//统计男性的个数
                List<object> qingjialist = new List<object>();
                var qingjia = (from v in db1.VacateApplies where v.StartDateTime <= DateTime.Now && DateTime.Now <= v.EndDateTime select v).ToList();//请假
                foreach (var qj in qingjia)//请假具体人员
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == qj.StaffNumber select ss).FirstOrDefault();
                    qingjialist.Add(new { name = st.Name, position = st.Position });

                }
                var jiaban = (from o in db1.OvertimeApplies where o.StartDateTime <= DateTime.Now && DateTime.Now <= o.EndDateTime select o).ToList();//加班
                List<object> jiabanlist = new List<object>();
                foreach (var jb in jiaban)//加班具体人员
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == jb.StaffNumber select ss).FirstOrDefault();
                    jiabanlist.Add(new { name = st.Name, position = st.Position });

                }
                var chuchai = (from e in db1.EvectionApplies where e.StartDateTime <= DateTime.Now && DateTime.Now <= e.EndDateTime select e).ToList();//出差
                List<object> chuchailist = new List<object>();
                foreach (var cc in chuchai)//出差具体人员
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == cc.StaffNumber select ss).FirstOrDefault();
                    chuchailist.Add(new { name = st.Name, position = st.Position });

                }
                List<object> queqinlist = new List<object>();
                List<object> chidaolist = new List<object>();
                List<object> zhengchanglist = new List<object>();
                int normaldaka = 0;//正常打卡次数
                int late = 0;//迟到
                int count1 = 0;
                foreach (var temp in staffnum)
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == temp.StaffNumber select ss).FirstOrDefault();
                    var lateall = (from s in db1.SignInCardStatus where s.StaffNumber == temp.StaffNumber && s.WorkDate == date && s.SignInTime != null && s.ComeLateMinutes != 0 select s).ToList();//迟到
                    late += lateall.Count();
                    if (lateall.Count != 0)//迟到具体人员
                    {
                        chidaolist.Add(new { name = st.Name, position = st.Position, chiDaoNumber = lateall.Count });
                    }
                    var lateall1 = (from s in db1.SignInCardStatus where s.StaffNumber == temp.StaffNumber && s.WorkDate == date && s.SignInTime != null select s).ToList();//出勤人数
                    if (lateall1.Count != 0)
                    {
                        count1++;
                    }
                    else//缺勤具体人员
                    {
                        queqinlist.Add(new { name = st.Name, position = st.Position });
                    }
                    var sign = (from s in db1.SignInCardStatus where s.StaffNumber == temp.StaffNumber && s.WorkDate == date && s.SignInTime != null select s).ToList();
                    foreach (var ss in sign)
                    {
                        int zhengchang = 0;
                        if (ss.Type.Equals("上班") && ss.NeedStartTime <= ss.SignInTime && ss.NeedWorkTime >= ss.SignInTime)
                        {
                            normaldaka++; zhengchang++;
                        }
                        if (ss.Type.Equals("下班") && ss.NeedEndTime >= ss.SignInTime && ss.NeedWorkTime <= ss.SignInTime)
                        {
                            normaldaka++; zhengchang++;
                        }
                        zhengchanglist.Add(new { name = st.Name, position = st.Position, zhengChangNum = zhengchang });//正常具体人员
                    }
                }
                //出勤率  
                float chuqin = (float)count1 / (float)staffnum.Count;
                return Json(new
                {
                    result = true,
                    chuqinlu = chuqin,
                    late = late,
                    normaldaka = normaldaka,
                    manNumber = man.Count(),
                    womenNumber = staffnum.Count() - man.Count(),
                    qingjia = qingjia.Count(),
                    jiaban = jiaban.Count(),
                    chuchai = chuchai.Count(),
                    qingjialist = qingjialist,
                    jiabanlist = jiabanlist,
                    chuchailist = chuchailist,
                    queqinlist = queqinlist,
                    chidaolist = chidaolist,
                    zhengchanglist = zhengchanglist
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        //部门当日考勤(明细)
        public JsonResult DepartmentDayDetail(string staffNumber, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).ToList();
            if (staff.Count != 0)
            {
                DateTime date = DateTime.Now;
                string temptemp = staff.FirstOrDefault().Department;
                var staffnum = (from ss in db1.Staffs where ss.Department.Equals(temptemp) select ss).ToList();
                var man = (from ss in db1.Staffs where ss.Department.Equals(temptemp) && ss.Gender.Equals("男") select ss).ToList();//统计男性的个数
                List<object> qingjialist = new List<object>();
                var qingjia = (from v in db1.VacateApplies where v.StartDateTime <= DateTime.Now && DateTime.Now <= v.EndDateTime select v).ToList();//请假
                foreach (var qj in qingjia)//请假具体人员
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == qj.StaffNumber select ss).FirstOrDefault();
                    qingjialist.Add(new { name = st.Name, position = st.Position });

                }
                var jiaban = (from o in db1.OvertimeApplies where o.StartDateTime <= DateTime.Now && DateTime.Now <= o.EndDateTime select o).ToList();//加班
                List<object> jiabanlist = new List<object>();
                foreach (var jb in jiaban)//加班具体人员
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == jb.StaffNumber select ss).FirstOrDefault();
                    jiabanlist.Add(new { name = st.Name, position = st.Position });

                }
                var chuchai = (from e in db1.EvectionApplies where e.StartDateTime <= DateTime.Now && DateTime.Now <= e.EndDateTime select e).ToList();//出差
                List<object> chuchailist = new List<object>();
                foreach (var cc in chuchai)//出差具体人员
                {
                    Staff st = (from ss in db1.Staffs where ss.StaffNumber == cc.StaffNumber select ss).FirstOrDefault();
                    chuchailist.Add(new { name = st.Name, position = st.Position });

                }

                int normaldaka = 0;//正常打卡次数
                int late = 0;//迟到
                foreach (var temp in staffnum)
                {
                    var lateall = (from s in db1.SignInCardStatus where s.StaffNumber == temp.StaffNumber && s.WorkDate == date && s.SignInTime != null && s.ComeLateMinutes != 0 select s).ToList();//迟到
                    late += lateall.Count();
                    var sign = (from s in db1.SignInCardStatus where s.StaffNumber == temp.StaffNumber && s.WorkDate == date && s.SignInTime != null select s).ToList();
                    foreach (var ss in sign)
                    {
                        if (ss.Type.Equals("上班") && ss.NeedStartTime <= ss.SignInTime && ss.NeedWorkTime >= ss.SignInTime)
                        {
                            normaldaka++;
                        }
                        if (ss.Type.Equals("下班") && ss.NeedEndTime >= ss.SignInTime && ss.NeedWorkTime <= ss.SignInTime)
                        {
                            normaldaka++;
                        }
                    }
                }
                return Json(new { result = true, late = late, normaldaka = normaldaka, manNumber = man.Count(), womenNumber = staffnum.Count() - man.Count(), qingjia = qingjia.Count(), jiaban = jiaban.Count(), chuchai = chuchai.Count() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        //个人日迟到排行榜
        public JsonResult PersonLateDay(string companyId, string userName, DateTime date)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                List<HonerWallOrderViewModel> list = new List<HonerWallOrderViewModel>();
                //DateTime day = Convert.ToDateTime(date);
                var staffs = (from s in db1.Staffs select s).ToList();
                if (staffs.Count != 0)//查询有员工的部门
                {
                    foreach (var staff in staffs)
                    {
                        var late = (from s in db1.SignInCardStatus where s.WorkDate == date && staff.StaffNumber == s.StaffNumber && s.ComeLateMinutes != 0 select s).ToList();
                        if (late.Count != 0)
                        {
                            string head = null;
                            if (staff.Head != null)
                            {
                                head = Picture(staff.Head, user.CompanyId + staff.StaffNumber);
                            }
                            var department = (from d in db1.Departments where d.DepartmentId == staff.Department select d).ToList();
                            HonerWallOrderViewModel honor = new HonerWallOrderViewModel();
                            honor.Department = department.FirstOrDefault().Name;
                            honor.Name = staff.Name;
                            honor.StaffNumber = staff.StaffNumber;
                            honor.Score = late.Count;//今日迟到次数
                            honor.LateMintues = 0;
                            honor.HeadUrl = head;
                            foreach (var ll in late)
                            {
                                honor.LateMintues += ll.ComeLateMinutes;
                            }
                            list.Add(honor);
                        }
                    }
                    if (list.Count != 0)
                    {
                        List<HonerWallOrderViewModel> order = new List<HonerWallOrderViewModel>();
                        order = (from li in list orderby li.Score descending select li).ToList();
                        return Json(new { result = true, order = order, shuoming = "日迟到：按照迟到分钟数多到少进行了排序" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { result = false, message = "没有员工迟到" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "没有员工" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "传入的参数错误" }, JsonRequestBehavior.AllowGet);
        }
        //个人月迟到排行榜
        public JsonResult PersonLateMonth(string companyId, string userName, int year, int month)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                List<HonerWallOrderViewModel> list = new List<HonerWallOrderViewModel>();
                //int month1 = Convert.ToInt16(month);
                //int year1 = Convert.ToInt16(year);
                var staffs = (from s in db1.Staffs select s).ToList();
                if (staffs.Count != 0)//查询有员工的部门
                {
                    foreach (var staff in staffs)
                    {
                        var late = (from s in db1.SignInCardStatus where s.WorkDate.Month == month && s.WorkDate.Year == year && staff.StaffNumber == s.StaffNumber && s.ComeLateMinutes != 0 select s).ToList();
                        if (late.Count != 0)
                        {
                            string head = null;
                            if (staff.Head != null)
                            {
                                head = Picture(staff.Head, user.CompanyId + staff.StaffNumber);
                            }
                            var department = (from d in db1.Departments where d.DepartmentId == staff.Department select d).ToList();
                            HonerWallOrderViewModel honor = new HonerWallOrderViewModel();
                            honor.Department = department.FirstOrDefault().Name;
                            honor.Name = staff.Name;
                            honor.StaffNumber = staff.StaffNumber;
                            honor.Score = late.Count();//当月迟到次数
                            honor.LateMintues = 0;
                            honor.HeadUrl = head;
                            foreach (var ll in late)
                            {
                                honor.LateMintues += ll.ComeLateMinutes;
                            }
                            list.Add(honor);
                        }
                    }
                    if (list.Count != 0)
                    {
                        List<HonerWallOrderViewModel> order = new List<HonerWallOrderViewModel>();
                        order = (from li in list orderby li.Score descending select li).ToList();
                        return Json(new { result = true, order = order, shuoming = "月迟到：按照迟到次数多到少进行了排序" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { result = false, message = "月迟到：没有员工迟到" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "没有员工" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "传入的参数错误" }, JsonRequestBehavior.AllowGet);
        }
        //迟到饼图（针对部门）
        public JsonResult DepartmentMonth(string userName, string companyId, int year, int month)
        {
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" }, JsonRequestBehavior.AllowGet);
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var departments = (from d in db1.Departments select d).ToList();
            List<object> list = new List<object>();
            foreach (var department in departments)
            {
                int count = 0;
                var staffs = (from s in db1.Staffs where s.Department == department.DepartmentId select s).ToList();
                foreach (var staff in staffs)
                {
                    var sign = (from si in db1.SignInCardStatus where si.StaffNumber == staff.StaffNumber && si.WorkDate.Year == year && si.WorkDate.Month == month && si.ComeLateMinutes != 0 select si).ToList();
                    count += sign.Count;
                }
                list.Add(new { department = department.Name, lateNumber = count });
            }
            return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
        }
        //入职申请(没有做)
        public JsonResult EntryApplication()
        {
            return Json(new { });
        }
        //个人综合排名

        public JsonResult PersonalAll(string staffNumber, string companyId, string year, string month)
        {

            int Year = Convert.ToInt16(year);
            int Month = Convert.ToInt16(month);
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            //var department = (from d in db1.Departments select d).ToList();
            // DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            List<HonerWallOrderViewModel> list = new List<HonerWallOrderViewModel>();


            var staffs = (from s in db1.Staffs select s).ToList();
            if (staffs.Count != 0)//查询有员工的部门
            {
                foreach (var staff in staffs)
                {
                    int late = 0;//迟到
                    int early = 0;//早退
                    int miss = 0;//缺卡
                    var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staff.StaffNumber && si.WorkDate.Year == Year && si.WorkDate.Month == Month select si).ToList();
                    foreach (var sign in signin)//每个员工的本月的上班记录
                    {
                        if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                        {
                            miss++;
                        }
                        else if (sign.ComeLateMinutes != 0)//迟到
                        {
                            late++;

                        }
                        else if (sign.LeaveEarlyMinutes != 0)//早退
                        {
                            early++;
                        }
                    }
                    var department = (from d in db1.Departments where d.DepartmentId == staff.Department select d).ToList();
                    //miss：5，late：2，early：2                     
                    HonerWallOrderViewModel honor = new HonerWallOrderViewModel();
                    honor.Department = department.FirstOrDefault().Name;
                    honor.StaffNumber = staff.StaffNumber;
                    honor.Name = staff.Name;
                    honor.Score = miss * 5 + late * 2 + early * 2;
                    list.Add(honor);
                    // list.Add(new { late = late, early = early, miss = miss });
                }

            }
            List<HonerWallOrderViewModel> order = new List<HonerWallOrderViewModel>();
            order = (from li in list orderby li.Score ascending select li).ToList();
            int rank = 0;
            for (int i = 0; i < order.Count; i++)
            {
                if (order[i].StaffNumber == staffNumber)
                {
                    rank = i + 1;
                }

            }
            return Json(new { result = true, rank = rank, allPerson = order.Count }, JsonRequestBehavior.AllowGet);


        }
        //个人当月上班情况各项统计明细(没有做)
        public JsonResult PersonalWorkMonth(string staffNumber, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).ToList();
            var man = (from m in db1.Staffs where m.Department == staff.FirstOrDefault().Department && m.Gender == "男" select m).ToList();//group by m.Gender
            var woman = (from m in db1.Staffs where m.Department == staff.FirstOrDefault().Department && m.Gender == "女" select m).ToList();
            if (staff.Count != 0)
            {
                DateTime today = DateTime.Now;
                int late = 0;//迟到
                int early = 0;//早退
                int miss = 0;//缺卡
                var signin = (from si in db1.SignInCardStatus where si.StaffNumber == staffNumber && si.WorkDate.Year == today.Year && si.WorkDate.Month == today.Month select si).ToList();
                foreach (var sign in signin)//每个员工的本月的上班记录
                {
                    if (sign.SignInTime < sign.NeedStartTime || sign.SignInTime > sign.NeedEndTime)//缺卡
                    {
                        miss++;
                    }
                    else if (sign.ComeLateMinutes != 0)//迟到
                    {
                        late++;

                    }
                    else if (sign.LeaveEarlyMinutes != 0)//早退
                    {
                        early++;
                    }
                }
                return Json(new { result = true, late = late, early = early, miss = miss, score = miss * 5 + late * 2 + early * 2, man = man.Count(), woman = woman.Count() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        //申请明细
        public JsonResult ApplicationDetail(string staffNumber, string companyId, string applicationType)
        {


            int type = Convert.ToInt32(applicationType);
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" }, JsonRequestBehavior.AllowGet);
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            if (type == 0)
            {
                int status = 6;
                int status1 = 1;
                var chuchai1 = (from ea in db1.EvectionApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && (ea.AuditStatus == type || ea.AuditStatus == status || ea.AuditStatus == status1) select ea).ToList();//&& ea.AuditStatus == type 
                List<ApplicationDetailViewModel> list11 = new List<ApplicationDetailViewModel>();
                foreach (var temp in chuchai1)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list11.Add(application);
                    //temp.StartDateTime.ToShortTimeString();

                }
                var jiaban1 = (from ea in db1.OvertimeApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && (ea.AuditStatus == type || ea.AuditStatus == status || ea.AuditStatus == status1) select ea).ToList();//&& ea.AuditStatus == type

                foreach (var temp in jiaban1)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list11.Add(application);

                }
                var tiaoxiu1 = (from ea in db1.DaysOffApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && (ea.AuditStatus == type || ea.AuditStatus == status || ea.AuditStatus == status1) select ea).ToList();//&& ea.AuditStatus == type

                foreach (var temp in tiaoxiu1)
                {
                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list11.Add(application);
                }
                var buka1 = (from ea in db1.ChargeCardApplies orderby ea.DateTime descending where ea.StaffNumber == staffNumber && (ea.AuditStatus == type || ea.AuditStatus == status || ea.AuditStatus == status1) select ea).ToList();//&& ea.AuditStatus == type

                foreach (var temp in buka1)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.DateTime.ToShortDateString();
                    application.Time = temp.DateTime.Day.ToString() + "日" + temp.DateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list11.Add(application);


                }
                var qingjia1 = (from ea in db1.VacateApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && (ea.AuditStatus == type || ea.AuditStatus == status || ea.AuditStatus == status1) select ea).ToList();//&& ea.AuditStatus == type

                foreach (var temp in qingjia1)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();


                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list11.Add(application);

                }
                var qidi1 = (from ea in db1.OffSiteApplies orderby ea.Date descending where ea.StaffNumber == staffNumber && (ea.AuditStatus == type || ea.AuditStatus == status || ea.AuditStatus == status1) select ea).ToList();//&& ea.AuditStatus == type

                foreach (var temp in qidi1)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.Type)).FirstOrDefault();
                    //var sp = (from sps in db1.StaffParams where sps.StaffParamTypeId == id select sps).ToList();
                    //if (sp.Count != 0)
                    //{
                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.Type;
                    application.Application = bp.TypeName;
                    application.Date = temp.Date.ToShortDateString();
                    application.Time = temp.Date.Day.ToString() + "日" + temp.Date.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list11.Add(application);
                    // }
                }
                if (list11.Count() == 0)
                {
                    return Json(new { result = true, flag = 0, data = "没用申请单" }, JsonRequestBehavior.AllowGet);//没用申请单

                }
                else
                {
                    List<ApplicationDetailViewModel> list = new List<ApplicationDetailViewModel>();
                    //List<ApplicationDetail> date = new List<ApplicationDetail>();

                    list = (from l in list11 orderby l.Date descending select l).ToList();
                    //for (int i = 0; i < list.Count(); i++)
                    //{
                    //    ApplicationDetail application1 = new ApplicationDetail();
                    //    application1.Id = list[i].Id;
                    //    application1.BillSort = list[i].BillSort;
                    //    application1.Application = list[i].Application;
                    //    application1.Date = list[i].Date.ToShortDateString();
                    //    application1.Time = list[i].Time;
                    //    date.Add(application1);

                    //}
                    return Json(new { result = true, flag = 1, data = list }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var chuchai = (from ea in db1.EvectionApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && ea.AuditStatus == type select ea).ToList();//&& ea.AuditStatus == type 
                List<ApplicationDetailViewModel> list1 = new List<ApplicationDetailViewModel>();
                foreach (var temp in chuchai)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list1.Add(application);

                }
                var jiaban = (from ea in db1.OvertimeApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && ea.AuditStatus == type select ea).ToList();//&& ea.AuditStatus == type
                List<object> list2 = new List<object>();
                foreach (var temp in jiaban)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list1.Add(application);

                }
                var tiaoxiu = (from ea in db1.DaysOffApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && ea.AuditStatus == type select ea).ToList();//&& ea.AuditStatus == type
                List<object> list3 = new List<object>();
                foreach (var temp in tiaoxiu)
                {
                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list1.Add(application);
                }
                var buka = (from ea in db1.ChargeCardApplies orderby ea.DateTime descending where ea.StaffNumber == staffNumber && ea.AuditStatus == type select ea).ToList();//&& ea.AuditStatus == type
                List<object> list4 = new List<object>();
                foreach (var temp in buka)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();

                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.DateTime.ToShortDateString();
                    application.Time = temp.DateTime.Day.ToString() + "日" + temp.DateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list1.Add(application);


                }
                var qingjia = (from ea in db1.VacateApplies orderby ea.StartDateTime descending where ea.StaffNumber == staffNumber && ea.AuditStatus == type select ea).ToList();//&& ea.AuditStatus == type
                List<object> list5 = new List<object>();
                foreach (var temp in qingjia)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.BillType)).FirstOrDefault();


                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.BillType;
                    application.Application = bp.TypeName;
                    application.Date = temp.StartDateTime.ToShortDateString();
                    application.Time = temp.StartDateTime.Day.ToString() + "日" + temp.StartDateTime.ToShortTimeString()
                        + "至" + temp.EndDateTime.Day.ToString() + "日" + temp.EndDateTime.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list1.Add(application);

                }
                var qidi = (from ea in db1.OffSiteApplies orderby ea.Date descending where ea.StaffNumber == staffNumber && ea.AuditStatus == type select ea).ToList();//&& ea.AuditStatus == type
                List<object> list6 = new List<object>();
                foreach (var temp in qidi)
                {

                    BillPropertyModels bp = db1.BillProperties.Where(p => p.Type.Equals(temp.Type)).FirstOrDefault();
                    //var sp = (from sps in db1.StaffParams where sps.StaffParamTypeId == id select sps).ToList();
                    //if (sp.Count != 0)
                    //{
                    ApplicationDetailViewModel application = new ApplicationDetailViewModel();
                    application.Id = temp.Id;
                    application.BillSort = temp.Type;
                    application.Application = bp.TypeName;
                    application.Date = temp.Date.ToShortDateString();
                    application.Time = temp.Date.Day.ToString() + "日" + temp.Date.ToShortTimeString();
                    application.BillNumber = temp.BillNumber;
                    list1.Add(application);
                    // }
                }
                if (list1.Count() == 0)
                {
                    return Json(new { result = true, flag = 0, data = "没用申请单" }, JsonRequestBehavior.AllowGet);//没用申请单

                }
                else
                {
                    List<ApplicationDetailViewModel> list = new List<ApplicationDetailViewModel>();
                    //List<ApplicationDetail> date = new List<ApplicationDetail>();

                    list = (from l in list1 orderby l.Date descending select l).ToList();
                    //for (int i = 0; i < list.Count(); i++)
                    //{
                    //    ApplicationDetail application1 = new ApplicationDetail();
                    //    application1.Id = list[i].Id;
                    //    application1.BillSort = list[i].BillSort;
                    //    application1.Application = list[i].Application;
                    //    application1.Date = list[i].Date.ToShortDateString();
                    //    application1.Time = list[i].Time;
                    //    date.Add(application1);

                    //}
                    return Json(new { result = true, flag = 1, data = list }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //具体明细-审批流程
        public JsonResult AuditProcedure(string staffNumber, string companyId, string billSort, DateTime date, string billNumber)//billSort：单据的主键编号。flag：区分那种单据。date：是否在审核的时间范围之内
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" }, JsonRequestBehavior.AllowGet);
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            //DateTime datetime = Convert.ToDateTime(date);
            var staff = (from s in db1.Staffs where s.StaffNumber == staffNumber select s).ToList();
            if (staff.Count != 0)
            {

                var template = (from t in db1.AuditTemplates where t.BType == billSort select t).ToList();
                List<object> list = new List<object>();
                int flag = 1;
                if (template.Count() == 0)
                {
                    return Json(new { result = false, message = "不存在审批" }, JsonRequestBehavior.AllowGet);
                }
                foreach (var temp in template)
                {
                    if (temp.StartTime <= date && temp.ExpireTime >= date && flag == 1)//判断审批模板的有效性
                    {
                        var step = (from s in db1.AuditSteps where s.TId == temp.Id select s).ToList();
                        if (step.Count() == 0)
                        {
                            return Json(new { result = false, message = "没有审批流程" }, JsonRequestBehavior.AllowGet);
                        }
                        var process = (from p in db1.AuditProcesses where p.BType == billSort && p.BNumber == billNumber select p).ToList();//单据审批到第几步
                        int countProcess = 1;
                        foreach (var steps in step)
                        {
                            if (process.Count >= countProcess)
                            {
                                list.Add(new { name = steps.Name, phone = steps.Approver, auditTag = true });
                                countProcess++;

                            }
                            else
                            {
                                list.Add(new { name = steps.Name, phone = steps.Approver, auditTag = false });
                            }

                        }
                        flag = 0;
                        break;
                    }
                }
                return Json(new { result = true, auditstep = list }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, message = "员工不存在" }, JsonRequestBehavior.AllowGet);

        }
        //具体明细-信息详情(申请人)
        public JsonResult SpecificDetail(string staffNumber, string companyId, string billSort, int id, string billNumber)//billSort：单据的主键编号。flag：区分那种单据。
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.StaffNumber == staffNumber select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数不正确" }, JsonRequestBehavior.AllowGet);
            }
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            // DateTime datetime = Convert.ToDateTime(date);
            string tempbill = billSort.Substring(0, 2);
            List<object> list2 = new List<object>();
            if (tempbill == "31")//出差
            {
                var eve = (from ea in db1.EvectionApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    //var param = (from sp in db1.StaffParams where sp.Value == temp.Reason select sp).ToList();
                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = temp.Reason, day = temp.Days, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }

            }
            else if (tempbill == "32")//请假
            {
                var eve = (from ea in db1.VacateApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {
                    var photo = (from p in db1.VacatePhotos where p.BillNumber == billNumber && p.BillSort == billSort select p).ToList();
                    List<string> photos = new List<string>();
                    foreach (var pp in photo)
                    {
                        photos.Add(Picture(pp.Photo, staffNumber + billNumber));
                    }

                    //var param = (from sp in db1.StaffParams where sp.Value == temp.Reason select sp).ToList();
                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = photos, billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = temp.Reason, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                    //return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "33")//加班
            {
                var eve = (from ea in db1.OvertimeApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {
                    var photo = (from p in db1.VacatePhotos where p.BillNumber == billNumber && p.BillSort == billSort select p).ToList();
                    List<string> photos = new List<string>();
                    foreach (var pp in photo)
                    {
                        photos.Add(Picture(pp.Photo, staffNumber + billNumber));
                    }

                    //var param = (from sp in db1.StaffParams where sp.Value == temp.Reason select sp).ToList();
                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = photos, billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = temp.Reason, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                    //return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "34")//签卡
            {
                var eve = (from ea in db1.ChargeCardApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {
                    var photo = (from p in db1.VacatePhotos where p.BillNumber == billNumber && p.BillSort == billSort select p).ToList();
                    List<string> photos = new List<string>();
                    foreach (var pp in photo)
                    {
                        photos.Add(Picture(pp.Photo, staffNumber + billNumber));
                    }
                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = photos, billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.DateTime.ToString(), reason = temp.Remark });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "35")//调休
            {
                var eve = (from ea in db1.DaysOffApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {
                    var photo = (from p in db1.VacatePhotos where p.BillNumber == billNumber && p.BillSort == billSort select p).ToList();
                    List<string> photos = new List<string>();
                    foreach (var pp in photo)
                    {
                        photos.Add(Picture(pp.Photo, staffNumber + billNumber));
                    }
                    //StaffParam param = (from sp in db1.StaffParams where sp.Value == temp.Reason select sp).FirstOrDefault();
                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();

                    list2.Add(new { photo = photos, billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = temp.Reason, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });


                }
            }
            else if (tempbill == "36")//异地
            {
                var eve = (from ea in db1.OffSiteApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "37")//值班
            {
                var eve = (from ea in db1.OnDutyApplies where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Reason });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "21")//入职
            {
                var eve = (from ea in db1.Staffs where ea.Number == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Entrydate.ToString(), resaon = " " });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "22")//人事
            {
                var eve = (from ea in db1.StaffChanges where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.EffectiveDate.ToString(), resaon = " " });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "23")//离职
            {
                var eve = (from ea in db1.StaffArchives where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.ReApplyDate.ToString(), reason = " " });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "24")//技能
            {
                var eve = (from ea in db1.StaffSkills where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.ValidDate.ToString(), reason = " " });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "25")//招聘
            {
                var eve = (from ea in db1.Recruitments where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillCode, billType = bill.TypeName, date = temp.RecordTime.ToString(), reason = "" });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "26")//培训
            {
                var eve = (from ea in db1.TrainStarts where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.EndDate.ToString(), startTime = temp.StartDate.ToString(), endTime = temp.EndDate.ToString(), reason = " " });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }
            else if (tempbill == "27")//合同
            {
                var eve = (from ea in db1.Contracts where ea.Id == id select ea).ToList();
                foreach (var temp in eve)
                {

                    var bill = (from bp in db1.BillProperties where bp.Type == billSort select bp).FirstOrDefault();
                    list2.Add(new { photo = "", billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.SignDate.ToString(), startTime = temp.SignDate.ToString(), endTime = temp.DueDate.ToString(), reason = " " });
                    // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                }
            }

            return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

        }
        //具体明细-信息详情(审核人)



        //需要我审核的申请的详情（目前不需要）
        public JsonResult AuditApplicationDetail(string userName, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).First();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                var applicationlist = (from aa in db1.AuditApplications where aa.State != 3 select aa).ToList();//查询没有审核的单据
                //已审核的申请
                if (applicationlist.Count != 0)
                {
                    List<object> list = new List<object>();
                    foreach (var temp in applicationlist)//依次取出每个单据
                    {
                        var processlist = (from a in db1.AuditProcesses where a.Result != 3 && a.AId == temp.Id select a).ToList();//查询每个单据没有审核的步骤                           
                        foreach (var temp1 in processlist)//依次取出每个步骤
                        {
                            if (temp1.Approver.Contains(userName) && temp1.AuditPerson != userName)
                            {
                                Staff staff = (from s in db1.Staffs where s.BillNumber == temp1.BNumber select s).FirstOrDefault();
                                string tempbill = temp1.BType.Substring(0, 2);
                                if (tempbill == "31")//出差
                                {
                                    EvectionApplies eve = (from ea in db1.EvectionApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.Hour.ToString() + ":" + eve.StartDateTime.Minute.ToString()
                    + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.Hour.ToString() + ":" + eve.EndDateTime.Minute.ToString();
                                        list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name, date = date });//需要审核的单据的具体某一步的所有信息
                                        // list.Add(new { billSort = eve.billSort, billNumber = eve.BillNumber, billType = eve.TypeName, type = param.FirstOrDefault().Value, day = temp.Days, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }

                                }
                                else if (tempbill == "32")//请假
                                {
                                    VacateApplies eve = (from ea in db1.VacateApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.Hour.ToString() + ":" + eve.StartDateTime.Minute.ToString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.Hour.ToString() + ":" + eve.EndDateTime.Minute.ToString();
                                        list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name, date = date });//需要审核的单据的具体某一步的所有信息
                                        //list.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = param.FirstOrDefault().Value, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        //return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "33")//加班
                                {
                                    OvertimeApplies eve = (from ea in db1.OvertimeApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.Hour.ToString() + ":" + eve.StartDateTime.Minute.ToString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.Hour.ToString() + ":" + eve.EndDateTime.Minute.ToString();
                                        list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name, date = date });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = param.FirstOrDefault().Value, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        //return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "34")//签卡
                                {
                                    ChargeCardApplies eve = (from ea in db1.ChargeCardApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        string date = eve.DateTime.Day.ToString() + "日" + eve.DateTime.Hour.ToString() + ":" + eve.DateTime.Minute.ToString();
                                        list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name, date = date });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.DateTime.ToString(), reason = temp.Remark });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "35")//调休
                                {
                                    DaysOffApplies eve = (from ea in db1.DaysOffApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.Hour.ToString() + ":" + eve.StartDateTime.Minute.ToString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.Hour.ToString() + ":" + eve.EndDateTime.Minute.ToString();
                                        list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name, date = date });//需要审核的单据的具体某一步的所有信息
                                        //list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = param.FirstOrDefault().Value, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else //异地
                                {
                                    OffSiteApplies eve = (from ea in db1.OffSiteApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        string date = eve.Date.Day.ToString() + "日" + eve.Date.Hour.ToString() + ":" + eve.Date.Minute.ToString();
                                        list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name, date = date });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                //list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name });//需要审核的单据的具体某一步的所有信息
                            }
                        }
                    }
                    if (list.Count != 0)
                    {
                        return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "没有申请单！" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { result = false, message = "没有申请单！" }, JsonRequestBehavior.AllowGet);
                }


            }
            else
                return Json(new { result = false, message = "传入参数错误" }, JsonRequestBehavior.AllowGet);

        }
        //需要我审核的申请
        public JsonResult AuditApplication(string userName, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                var applicationlist = (from aa in db1.AuditApplications where aa.State != 3 select aa).ToList();//查询没有审核的单据
                //已审核的申请
                if (applicationlist.Count != 0)
                {
                    List<object> list = new List<object>();
                    foreach (var temp in applicationlist)//依次取出每个单据
                    {
                        var processlist = (from a in db1.AuditProcesses where a.Result != 3 && a.AId == temp.Id select a).ToList();//查询每个单据没有审核的步骤                           
                        foreach (var temp1 in processlist)//依次取出每个步骤
                        {
                            if (temp1.Approver.Contains(userName) && temp1.AuditPerson != userName)
                            {

                                string tempbill = temp1.BType.Substring(0, 2);
                                if (tempbill == "31")//出差
                                {
                                    EvectionApplies eve = (from ea in db1.EvectionApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();

                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.ToShortTimeString()
                    + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.StartDateTime.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list.Add(new { billSort = eve.billSort, billNumber = eve.BillNumber, billType = eve.TypeName, type = param.FirstOrDefault().Value, day = temp.Days, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }

                                }
                                else if (tempbill == "32")//请假
                                {
                                    VacateApplies eve = (from ea in db1.VacateApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.ToShortTimeString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.StartDateTime.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        //list.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = param.FirstOrDefault().Value, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        //return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "33")//加班
                                {
                                    OvertimeApplies eve = (from ea in db1.OvertimeApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.ToShortTimeString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.StartDateTime.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = param.FirstOrDefault().Value, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        //return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "34")//签卡
                                {
                                    ChargeCardApplies eve = (from ea in db1.ChargeCardApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.DateTime.Day.ToString() + "日" + eve.DateTime.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.DateTime.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.DateTime.ToString(), reason = temp.Remark });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "35")//调休
                                {
                                    DaysOffApplies eve = (from ea in db1.DaysOffApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.ToShortTimeString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.StartDateTime.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        //list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, type = param.FirstOrDefault().Value, day = temp.Hours, startTime = temp.StartDateTime.ToString(), endTime = temp.EndDateTime.ToString(), reason = temp.Remark });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "36")//异地
                                {
                                    OffSiteApplies eve = (from ea in db1.OffSiteApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.Date.Day.ToString() + "日" + eve.Date.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.Date.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "37")//值班
                                {
                                    OnDutyApplies eve = (from ea in db1.OnDutyApplies where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.StartDateTime.Day.ToString() + "日" + eve.StartDateTime.ToShortTimeString()
                  + "至" + eve.EndDateTime.Day.ToString() + "日" + eve.EndDateTime.ToShortTimeString();
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.StartDateTime.ToShortDateString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "21")//入职
                                {
                                    Staff eve = (from ea in db1.Staffs where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.Entrydate.ToString() + "日";//.Entrydate()
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Number,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.Entrydate.ToString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "22")//人事
                                {
                                    StaffChange eve = (from ea in db1.StaffChanges where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.EffectiveDate.ToString() + "日";//.Entrydate()
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.EffectiveDate.ToString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "23")//离职
                                {
                                    StaffArchive eve = (from ea in db1.StaffArchives where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.ReApplyDate.ToString() + "日";//.Entrydate()
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.ReApplyDate.ToString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "24")//技能
                                {
                                    StaffSkill eve = (from ea in db1.StaffSkills where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.ValidDate.ToString() + "日";//.Entrydate()
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.ValidDate.ToString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "25")//招聘
                                {
                                    Recruitments eve = (from ea in db1.Recruitments where ea.BillCode == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        //Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        //string date = eve.StartDate.ToString() + "日至" + eve.EndDate.ToString() + "日";//.Entrydate()
                                        list.Add(new
                                        {
                                            billNumber = eve.BillCode,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = eve.DepartmentName,
                                           // time = date,
                                           time=eve.Position,
                                            staffNumber = "",
                                            date = eve.RecordTime.ToString(),
                                            info = temp1.Info,
                                            headUrl = "",
                                            
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "26")//培训
                                {
                                    TrainStart eve = (from ea in db1.TrainStarts where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        //Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.StaffNumber select s).FirstOrDefault();
                                        string date = eve.StartDate.ToString() + "日至" + eve.EndDate.ToString() + "日";//.Entrydate()
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = eve.TrainType,
                                            time = date,
                                            staffNumber = "",
                                            date = eve.StartDate.ToString(),
                                            info = temp1.Info,
                                            headUrl = ""
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else if (tempbill == "27")//合同
                                {
                                    Contract eve = (from ea in db1.Contracts where ea.BillNumber == temp1.BNumber select ea).FirstOrDefault();
                                    if (eve != null)
                                    {
                                        Staff staff = (from s in db1.Staffs where s.StaffNumber == eve.SecondParty select s).FirstOrDefault();
                                        string head = HeadURL(staff.Head, user.CompanyId + staff.Name);
                                        string date = eve.SignDate.ToString() + "日至" + eve.DueDate.ToString() + "日";
                                        list.Add(new
                                        {
                                            billNumber = eve.BillNumber,
                                            auditId = temp1.Id,
                                            detailId = eve.Id,
                                            billSort = temp1.BType,
                                            billName = temp1.TypeName,
                                            name = staff.Name,
                                            time = date,
                                            staffNumber = staff.StaffNumber,
                                            date = eve.SignDate.ToString(),
                                            info = temp1.Info,
                                            headUrl = head
                                        });//需要审核的单据的具体某一步的所有信息
                                        // list2.Add(new { billSort = billSort, billNumber = temp.BillNumber, billType = bill.TypeName, date = temp.Date.ToString(), reason = temp.Reason });
                                        // return Json(new { result = true, data = list2 }, JsonRequestBehavior.AllowGet);

                                    }
                                }

                                //list.Add(new { id = temp1.Id, billSort = temp1.BType, billName = temp1.TypeName, name = staff.Name });//需要审核的单据的具体某一步的所有信息
                            }
                        }
                    }
                    if (list.Count != 0)
                    {
                        return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = "没有申请单！" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { result = false, message = "没有申请单！" }, JsonRequestBehavior.AllowGet);
                }


            }
            else
                return Json(new { result = false, message = "传入参数错误" }, JsonRequestBehavior.AllowGet);

        }
        //审核申请
        public JsonResult Audit(int id, string remark, int flag, string userName, string companyId)//flag的值：1:代表已经审核,0:为未通过。
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId && u.UserName == userName select u).FirstOrDefault();
            if (user == null)
            {
                return Json(new { result = false, message = "传入参数错误" });
            }
            BonsaiiDbContext db = new BonsaiiDbContext(user.ConnectionString);
            //AuditProcess auditprocess = db1.AuditProcesses.Find(id);
            //auditprocess.AuditDate = DateTime.Now;//实际的审核时间
            //auditprocess.AuditPerson = userName;
            //auditprocess.Result = (byte)flag;
            //auditprocess.Comment = remark;
            //db1.SaveChanges();

            AuditProcess auditProcess = db.AuditProcesses.Find(id);
            auditProcess.AuditDate = DateTime.Now;//实际的审核时间
            if (auditProcess == null)
            {
                return Json(new { result = false, message = "没有对应审批流程" });
            }

            ////只要AuditProcess进入这一步。AuditApplication状态就改为在审核。
            //AuditApplication application = db.AuditApplications.Find(auditProcess.AId);//修改Application的状态
            //application.State = 1;
            ////调用一个函数；用来改变Staff的审核状态
            //ReturnStatus(application,1);

            AppAudit.ReturnStatus(auditProcess.AId, 1, userName);

            /*超过审核时间，不再审核*/
            //if (auditProcess.AuditDate < DateTime.Now)
            if (auditProcess.AuditDate > auditProcess.DeadlineDate)//如果实际的审核时间大于截止日期
            {
                auditProcess.Result = 5;//过期，打回//
                //AuditApplication auditApplication = db.AuditApplications.Find(auditProcess.AId);//修改Application的状态
                //auditApplication.State = 5;//待审核 5(过期未处理)
                AppAudit.ReturnStatus(auditProcess.AId, 5, userName);
            }
            else
            {
                if (flag == 1)
                {
                    // this.PassStep(id);
                    auditProcess.Result = 3;//直接返回结果就是不通过
                    auditProcess.Comment = Request["Comment"];
                    auditProcess.AuditPerson = userName + user.Name;
                    db.SaveChanges();
                    AppAudit.PassStep(id, userName);

                }
                else if (flag == 0)//审核不通过
                {
                    auditProcess.Result = 4;//已审
                    auditProcess.Comment = Request["Comment"];
                    auditProcess.AuditPerson = userName + user.Name;
                    db.SaveChanges();
                    // this.NotPassStep(id);
                    AppAudit.NotPassStep(id, userName);
                }
            }
            db.SaveChanges();
            return Json(new { result = true, message = "审核完成" }, JsonRequestBehavior.AllowGet);


        }

        //我已审核的申请
        public JsonResult AuditFinish(string userName, string companyId)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).First();
            if (user != null)
            {
                BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
                var auditlist = (from a in db1.AuditProcesses where a.AuditPerson.Contains(userName) select a).ToList();
                //已审核的申请
                if (auditlist.Count != 0)
                {
                    List<object> list = new List<object>();
                    foreach (var temp in auditlist)
                    {
                        list.Add(new { billName = temp.TypeName, auditDate = temp.AuditDate.ToString(), info = temp.Info, comment = temp.Comment });
                    }
                    return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { result = false, message = "没有申请单！" }, JsonRequestBehavior.AllowGet);
                }


            }
            else
                return Json(new { result = false, message = "公司号不正确！" }, JsonRequestBehavior.AllowGet);


        }

        //个人工作日历
        public JsonResult PersonalWork(string staffNumber, string companyId, string year, string month)
        {

            int Year = Convert.ToInt16(year);
            int Month = Convert.ToInt16(month);
            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            var workmanages = (from wm in db1.WorkManages orderby wm.Date ascending where wm.StaffNumber == staffNumber && wm.Date.Year == Year && wm.Date.Month == Month select wm).ToList();//个人排班具体月份。
            List<object> list = new List<object>();
            if (workmanages.Count != 0)
            {
                foreach (var temp in workmanages)
                {
                    list.Add(new { workdate = temp.Date.ToShortDateString() });
                }
                int workeid = workmanages.FirstOrDefault().WorksId;//班次ID
                var holiday = (from h in db1.HolidayTables orderby h.Date ascending where h.StaffNumber == staffNumber && h.Date.Year == Year && h.Date.Month == Month select new { date = h.Date.ToString(), type = h.Type, starthour = h.StartHour.ToString(), endhour = h.EndHour.ToString() }).ToList();//db1.HolidayTables.Where(h => h.StaffNumber.Equals(staffNumber)).ToList();//个人假日
                var work = db1.Works.Where(w => w.Id.Equals(workeid));//个人班次
                var worktimes = (from wt in db1.WorkTimes orderby wt.StartTime ascending where wt.WorksId == workeid select new { starttime = wt.StartTime.ToString().Substring(0, 5), endtime = wt.EndTime.ToString().Substring(0, 5) }).ToList();//班次时间升序
                if (worktimes.Count() != 0)
                {

                    return Json(new { result = true, work = list, holiday = holiday, worktimes = worktimes }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = true, work = list, holiday = holiday, message = "没有排班时间" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, message = "该员工本月现在还没有排班" }, JsonRequestBehavior.AllowGet);


        }
        //个人工作日历(这个为准)
        public JsonResult PersonalWorkCalender(string staffNumber, string companyId, string year, string month)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            // int year = int.Parse(Request["year"]);
            //int month = int.Parse(Request["month"]);

            //string number = Request["number"];
            int Year = Convert.ToInt16(year);
            int Month = Convert.ToInt16(month);
            int DaysInMonth = DateTime.DaysInMonth(Year, Month);
            DateTime FirstDay = new DateTime(Year, Month, 1);
            DateTime LastDay = new DateTime(Year, Month, DaysInMonth);

            //获取一个月内的班次名称
            List<string> WorkDays = (from x in db1.WorkManages
                                     join y in db1.Works on x.WorksId equals y.Id
                                     where x.Date <= LastDay && x.Date >= FirstDay && x.StaffNumber.Equals(staffNumber)
                                     orderby x.Date
                                     select y.Name).ToList();//问题：当workdays为空时，holidays有值。就会越界。


            var holidays = (from x in db1.HolidayTables
                            where x.Date <= LastDay && x.Date >= FirstDay && x.StaffNumber.Equals(staffNumber)
                            select new
                            {
                                x.Type,
                                x.StartHour,
                                x.EndHour,
                                x.Date
                            });

            //注意数组是从0开始
            foreach (var tmp in holidays)
                WorkDays[tmp.Date.Day - 1] = "休息";

            return Json(new
            {
                result = true,
                Days = WorkDays,
                LastDay = DaysInMonth
            }, JsonRequestBehavior.AllowGet);




        }
        //每天的工作时间
        public JsonResult PersonalWorkTime(string staffNumber, string companyId, DateTime date)
        {

            UserModels user = (from u in sdb.Users where u.CompanyId == companyId select u).FirstOrDefault();
            BonsaiiDbContext db1 = new BonsaiiDbContext(user.ConnectionString);
            //DateTime TmpDate = Convert.ToDateTime(date);

            var WorkTime = (from x in db1.WorkManages
                            join y in db1.WorkTimes on x.WorksId equals y.WorksId
                            where x.StaffNumber.Equals(staffNumber) && x.Date == date
                            orderby y.StartTime
                            select new
                            {
                                StartTime = y.StartTime,
                                EndTime = y.EndTime,
                            }).ToList();

            var holiday = (from x in db1.HolidayTables
                           where x.StaffNumber.Equals(staffNumber) && x.Date == date
                           select new
                           {
                               StartHour = x.StartHour,
                               EndHour = x.EndHour
                           }).ToList();
            List<string> result = new List<string>();
            if (holiday.Count != 0)
            {
                foreach (var tmp in WorkTime)
                    if (tmp.StartTime.Hours >= holiday[0].EndHour || tmp.EndTime.Hours <= holiday[0].StartHour)
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
                    data = result,
                    specific = "今天是工作日"
                }, JsonRequestBehavior.AllowGet);
            //没有这个人的排班情况和放假情况
            if (result.Count == 0 && holiday.Count == 0)
                return Json(new
                {
                    flag = 2,
                    specific = "没有这个人的排班情况和放假情况"
                }, JsonRequestBehavior.AllowGet);
            //今天是休息日
            return Json(new
            {
                flag = 0,
                specific = "今天是休息日"
            }, JsonRequestBehavior.AllowGet);

        }


        //图像是否为空
        public string HeadURL(byte[] head, string name)
        {
            string headUrl = null;
            if (head != null)
            {
                headUrl = Picture(head, name);
            }
            return headUrl;
        }
        //图片转换
        public string Picture(byte[] photo, string name)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "files\\";//System.Environment.CurrentDirectory 
            System.IO.MemoryStream ms = new System.IO.MemoryStream(photo);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            string url = path + name + ".jpg";
            img.Save(url);
            url = url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/files/" + name + ".jpg";
            return url;

        }

        public JsonResult TestTime(DateTime date)
        {
            DateTime datetime = date;
            DateTime date1 = DateTime.Now;
            return Json(new { datetime = datetime.ToString(), result = datetime.ToShortTimeString(), date1 = date1.ToShortTimeString(), date2 = date1.ToShortDateString(), date3 = date1.ToString(), dd = date1 });
        }
    }
}