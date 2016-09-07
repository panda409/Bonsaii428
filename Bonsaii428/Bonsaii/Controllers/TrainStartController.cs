using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.IO;
using OfficeOpenXml;
using NPOI.HSSF.UserModel;
using Bonsaii.Models.Audit;
using BonsaiiModels.Staffs;

namespace Bonsaii.Controllers
{

    public class TrainStartController : BaseController
    {
        public byte AuditApplicationStaffSkill(TrainStart staffSkill)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == staffSkill.BillTypeNumber select p).ToList().FirstOrDefault();

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
                                    (staffSkill.BillTypeNumber == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    auditApplication.BNumber = staffSkill.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.CreatorName = this.Name;
                    auditApplication.State = 0;//代表等待审
                    auditApplication.Info =
                 "单据名称：" + staffSkill.BillTypeNumber + ";" +
                 "培训类型：" + staffSkill.TrainType + ";" +
                 "培训主题：" + staffSkill.TrainTheme + ";" +
                 "培训讲师：" + staffSkill.TrainPerson + ";" +
                 "培训地址：" + staffSkill.TrainPlace + ";" +
                 "开始时间：" + staffSkill.StartDate + ";" +
                 "结束时间：" + staffSkill.EndDate + ";" +
                 "培训费用：" + staffSkill.TrainCost +"元"+";" +
                 "联系电话：" + staffSkill.TellNumber + ";" +
                 "参加人员：" + staffSkill.JoinPerson + ";" +
                 "列席人员：" + staffSkill.TrainManage + ";" +
                 "培训内容：" + staffSkill.TrainContent + ";" +
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
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
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
                    trainStart.AuditStatus = 3;
                    TrainStartPassAudit(trainStart);
                }
                else
                {
                    //不通过审批
                    trainStart.AuditStatus = 4;

                }
                trainStart.AuditPerson = this.UserName;
                trainStart.AuditTime = DateTime.Now;
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
            TrainStart staffSkill = db.TrainStarts.Find(id);
            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationStaffSkill(staffSkill);
            //需要对原表做出的修改
            staffSkill.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TrainStart
         [Authorize(Roles = "Admin,TrainStart_Index")]
        public ActionResult Index()
        {

            var train = (from ts in db.TrainStarts
                        join bp in db.BillProperties on ts.BillTypeNumber equals bp.Type
                        select new TrainStartViewModel
                        {
                            Id = ts.Id,
                            BillTypeNumber = ts.BillTypeNumber,
                            BillTypeName = bp.TypeName,
                            BillNumber = ts.BillNumber,
                            TrainType = ts.TrainType,
                            StartDate = ts.StartDate,
                            EndDate = ts.EndDate,
                            TrainCost = ts.TrainCost,
                            TellNumber = ts.TellNumber,
                            AuditStatus = ts.AuditStatus,
                            Remark = ts.Remark,
                            TrainTheme = ts.TrainTheme,
                            TrainPlace = ts.TrainPlace,
                            TrainPerson = ts.TrainPerson,
                            TrainManage = ts.TrainManage,
                            JoinPerson = ts.JoinPerson,
                            ChoosePerson = ts.ChoosePerson,
                            TrainContent = ts.TrainContent
                        }).ToList();
            /*查找预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                                 where q.TableName == "TrainStarts" select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找预留字段(value)*/
            var fieldValueList = (from tsr in db.TrainStartReserves
                                  join rf in db.ReserveFields on tsr.FieldId equals rf.Id
                                  select new TrainStartViewModel { Id = tsr.Number, Description = rf.Description, Value = tsr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            foreach (var tmp in train)
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
            return View(train.OrderByDescending(c=>c.Id));
        }

        // GET: TrainStart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
            {
                return HttpNotFound();
            }
            BillPropertyModels bill=db.BillProperties.Where(b=>b.Type.Equals(trainStart.BillTypeNumber)).SingleOrDefault();
            trainStart.BillTypeNumber=trainStart.BillTypeNumber+" "+bill.TypeName;
            trainStart.JoinPerson = trainStart.JoinPerson.Replace("multiselect-all,", "");
            trainStart.ChoosePerson = trainStart.ChoosePerson.Replace("multiselect-all,", "");
            /*查找预留字段(value)*/
            var fieldValueList = (from tsr in db.TrainStartReserves
                                  join rf in db.ReserveFields on tsr.FieldId equals rf.Id
                                  where tsr.Number == trainStart.Id
                                  select new TrainStartViewModel { Id = tsr.Number, Description = rf.Description, Value = tsr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(trainStart);
        }

        // GET: TrainStart/Create
         [Authorize(Roles = "Admin,TrainStart_Create")]
        public ActionResult Create()
        {
            var temp = (from p in db.StaffParamTypes where p.Name == "培训类型" select p.Id).FirstOrDefault();
            List<SelectListItem> item = (from p in db.StaffParams where p.StaffParamTypeId == temp select p).ToList().Select(c => new SelectListItem
            {
                Value = c.Value,//保存的值
                Text = c.Value//显示的值
            }).ToList();
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = "-请选择-";
            i.Selected = true;
            item.Add(i);

            ViewBag.List = item;
            //List<SelectListItem> staff = (from s in db.Staffs
            //                              join d in db.Departments on s.Department equals d.DepartmentId
            //                              select new
            //                              {
            //                                  StaffNumber = s.StaffNumber,
            //                                  StaffName = s.Name,
            //                                  StaffDepartment = d.Name,
            //                                  StaffPosition = s.Position
            //                              }).ToList().Select(s => new SelectListItem
            //{
            //    Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition,
            //    Value = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition
            //}).ToList();
            //ViewBag.staff = staff;
            /*查找预留字段(name)*/
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                                                              on p.TableNameId equals q.Id 

                             where q.TableName == "TrainStarts" select p).ToList();
            ViewBag.fieldList = fieldList;

            var staff = (from s in db.Staffs
                         where s.AuditStatus == 3 && s.ArchiveTag == false
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
                            // value = s.StaffNumber + "-" + s.Name + "-" + s.StaffDepartment + "-" + s.StaffPosition
                         }).ToList();
            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;

            return View();
        }


        // POST: TrainStart/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,TrainStart_Create")]
        public ActionResult Create(TrainStart trainStart)
        {
            var staff = (from s in db.Staffs
                         where s.AuditStatus == 3 && s.ArchiveTag == false
                         select new StaffModel
                         {
                             department = s.Department,
                             text = s.StaffNumber + "-" + s.Name,
                             value = s.StaffNumber + "-" + s.Name + "<" + s.IndividualTelNumber + "-" + s.Email + ">"
                         }).ToList();
            var group = (from d in db.Departments
                         select new StaffModel { department = d.DepartmentId, name = d.Name }).ToList();

            Dictionary<string, int> sum = new Dictionary<string, int>();
            foreach (var g in group)
            {
                int count = 0;
                foreach (var s in staff)
                {
                    if (s.department == g.department)
                        count++;
                }
                sum.Add(g.department, count);
            }

            ViewBag.Count = sum;
            ViewBag.Receiver = staff;
            ViewBag.Group = group;



            var temp = (from p in db.StaffParamTypes where p.Name == "培训类型" select p.Id).FirstOrDefault();
            List<SelectListItem> item = (from p in db.StaffParams where p.StaffParamTypeId == temp select p).ToList().Select(c => new SelectListItem
            {
                Value = c.Value,//保存的值
                Text = c.Value//显示的值
            }).ToList();
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = "请选择";
            i.Selected = true;
            item.Add(i);

            ViewBag.List = item;
            //List<SelectListItem> staff = (from s in db.Staffs
            //                              join d in db.Departments on s.Department equals d.DepartmentId
            //                              select new
            //                              {
            //                                  StaffNumber = s.StaffNumber,
            //                                  StaffName = s.Name,
            //                                  StaffDepartment = d.Name,
            //                                  StaffPosition = s.Position
            //                              }).ToList().Select(s => new SelectListItem
            //                              {
            //                                  Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition,
            //                                  Value = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition
            //                              }).ToList();
            //ViewBag.staff = staff;
            /*查找预留字段(name)*/
            var fieldList1 = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                              where q.TableName == "TrainStarts" select p).ToList();
            ViewBag.fieldList = fieldList1;
            if (ModelState.IsValid)
            {
                trainStart.JoinPerson = Request["JoinPerson"];
                trainStart.ChoosePerson = Request["ChoosePerson"];
                trainStart.RecordPerson = this.Name;
                trainStart.RecordTime = DateTime.Now;
                db.TrainStarts.Add(trainStart);
             
                //提交审批
                byte status = AuditApplicationStaffSkill(trainStart);
                //需要对原表做出的修改
                trainStart.AuditStatus = status;
                /*查找预留字段(name)*/
                var fieldList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                                 where q.TableName == "TrainStarts" select p).ToList();
                ViewBag.fieldList = fieldList;
                /*遍历，保存变化的字段*/
                foreach (var temp1 in fieldList)
                {
                    TrainStartReserve rr = new TrainStartReserve();
                    rr.Number = trainStart.Id;
                    rr.FieldId = temp1.Id;
                    rr.Value = Request[temp1.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (rr.Value == null) rr.Value = " ";
                    db.TrainStartReserves.Add(rr);
                    db.SaveChanges();
                }
                db.SaveChanges();
                if (status == 7) {
                    ViewBag.alertMessage = true;
                    return View(trainStart);
                }
                else
                    if (status == 3) {
                        TrainStartPassAudit(trainStart);
                    }
              
                return RedirectToAction("Index");
            }
            return View(trainStart);
        }

        // GET: TrainStart/Edit/5
        [Authorize(Roles = "Admin,TrainStart_Edit")]
        public ActionResult Edit(int? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
            {
                return HttpNotFound();
            }

            var temp = (from p in db.StaffParamTypes where p.Name == "培训类型" select p.Id).FirstOrDefault();
            List<SelectListItem> item = (from p in db.StaffParams where p.StaffParamTypeId == temp select p).ToList().Select(c => new SelectListItem
            {
                Value = c.Value,//保存的值
                Text = c.Value//显示的值
            }).ToList();
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = "-请选择-";
            i.Selected = true;
            item.Add(i);

            ViewBag.List = item;
            List<SelectListItem> staff = (from s in db.Staffs
                                          join d in db.Departments on s.Department equals d.DepartmentId
                                          select new
                                          {
                                              StaffNumber = s.StaffNumber,
                                              StaffName = s.Name,
                                              StaffDepartment = d.Name,
                                              StaffPosition = s.Position
                                          }).ToList().Select(s => new SelectListItem
                                          {
                                               Text = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition,
                                               Value = s.StaffNumber + "-" + s.StaffName + "-" + s.StaffDepartment + "-" + s.StaffPosition
                                          }).ToList();
            ViewBag.staff = staff;
            List<string> staff1 = (from s in db.Staffs 
                                   join d in db.Departments on s.Department equals d.DepartmentId
                                   select s.StaffNumber + "-" + s.Name + "-" + d.Name + "-" + s.Position).ToList();
             ViewBag.join = new MultiSelectList( staff1,this.SelectedListByName(trainStart.JoinPerson));
            ViewBag.choose = new MultiSelectList(staff1,this.SelectedListByName(trainStart.ChoosePerson));
            ViewBag.BillTypeNumber = trainStart.BillTypeNumber+" "+trainStart.BillTypeName;
            ViewBag.BillNumber = trainStart.BillNumber;
            ViewBag.TrainType = trainStart.TrainType;
            ViewBag.StartDate = trainStart.StartDate.ToString();
            ViewBag.EndDate = trainStart.EndDate.ToString();
            ViewBag.TrainCost = trainStart.TrainCost;
            ViewBag.TellNumber = trainStart.TellNumber;
            ViewBag.Remark = trainStart.Remark;
            ViewBag.TrainTheme = trainStart.TrainTheme;
            ViewBag.TrainPlace = trainStart.TrainPlace;
            ViewBag.TrainPerson = trainStart.TrainPerson;         
            ViewBag.TrainManage = trainStart.TrainManage;        
            ViewBag.TrainContent = trainStart.TrainContent;
            BillPropertyModels bill = db.BillProperties.Where(b => b.Type.Equals(trainStart.BillTypeNumber)).SingleOrDefault();
            trainStart.BillTypeNumber = trainStart.BillTypeNumber + " " + bill.TypeName;
            /*查找预留字段(value)*/
            var fieldValueList = (from tsr in db.TrainStartReserves
                                  join rf in db.ReserveFields on tsr.FieldId equals rf.Id
                                  where tsr.Number == id
                                  select new TrainStartViewModel { Description = rf.Description, Value = tsr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View();
        }
        /// <summary>
        /// 字符串根据逗号分隔
        /// </summary>
        /// <param name="id">招聘单的Id和要获取的属性的名称</param>
        /// <returns></returns>
        public string[] SelectedListByName(string Name)
        {
            return Name.Split(new char[] { ',' });
        }
        // POST: TrainStart/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,TrainStart_Edit")]
        public ActionResult Edit(TrainStart trainStart)
        {
            if (ModelState.IsValid)
            {
                TrainStart train = db.TrainStarts.Find(trainStart.Id);
                train.BillTypeNumber = trainStart.BillTypeNumber.Substring(0, 4);
                train.TrainType = trainStart.TrainType;
                train.StartDate = trainStart.StartDate;
                train.EndDate = trainStart.EndDate;
                train.TrainCost = trainStart.TrainCost;
                train.TellNumber = trainStart.TellNumber;
                train.Remark = trainStart.Remark;
                train.TrainTheme = trainStart.TrainTheme;
                train.TrainPlace = trainStart.TrainPlace;
                train.TrainPerson = trainStart.TrainPerson;
                train.ChoosePerson = Request["ChoosePerson"];
                train.TrainManage = trainStart.TrainManage;
                train.JoinPerson = Request["JoinPerson"];
                train.TrainContent = trainStart.TrainContent;
                train.ChangePerson = this.Name;
                train.ChangeTime = DateTime.Now;
              
                db.SaveChanges();
                /*查找预留字段(value)*/
                var fieldValueList = (from tsr in db.TrainStartReserves
                                      join rf in db.ReserveFields on tsr.FieldId equals rf.Id
                                      where tsr.Number == trainStart.Id
                                      select new TrainStartViewModel { Id = tsr.Id, Description = rf.Description, Value = tsr.Value }).ToList();

                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    TrainStartReserve rr = db.TrainStartReserves.Find(temp.Id);
                    rr.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(trainStart);
        }

        // GET: TrainStart/Delete/5
        [Authorize(Roles = "Admin,TrainStart_Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainStart trainStart = db.TrainStarts.Find(id);
            if (trainStart == null)
            {
                return HttpNotFound();
            }
            BillPropertyModels bill = db.BillProperties.Where(b => b.Type.Equals(trainStart.BillTypeNumber)).SingleOrDefault();
            trainStart.BillTypeNumber = trainStart.BillTypeNumber + " " + bill.TypeName;
            trainStart.JoinPerson = trainStart.JoinPerson.Replace("multiselect-all,", "");
            trainStart.ChoosePerson = trainStart.ChoosePerson.Replace("multiselect-all,", "");
            /*查找预留字段(value)*/
            var fieldValueList = (from tsr in db.TrainStartReserves
                                  join rf in db.ReserveFields on tsr.FieldId equals rf.Id
                                  where tsr.Number == trainStart.Id
                                  select new TrainStartViewModel { Id = tsr.Number, Description = rf.Description, Value = tsr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(trainStart);
        }

        // POST: TrainStart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,TrainStart_Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from tsr in db.TrainStartReserves
                        where tsr.Number == id
                        select new TrainStartViewModel { Id = tsr.Id }).ToList();
            foreach (var temp in item)
            {
                TrainStartReserve tsr = db.TrainStartReserves.Find(temp.Id);
                db.TrainStartReserves.Remove(tsr);
            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            TrainStart trainStart = db.TrainStarts.Find(id);
            db.TrainStarts.Remove(trainStart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult BillTypeNumberSearch()
        {

            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == "26"
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
                                 billNumber = temp
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
