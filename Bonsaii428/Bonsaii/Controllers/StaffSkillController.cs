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

namespace Bonsaii.Controllers
{
    public class StaffSkillController : BaseController
    {
       
        public byte AuditApplicationStaffSkill(StaffSkill staffSkill)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == staffSkill.BillTypeNumber select p).ToList().FirstOrDefault();
         
            if (item.IsAutoAudit == 1)
            { //如果为0 代表不能自动审核 如果为1  代表可以自动审核
                return 3;//代表自动审核
            }

            if (item.IsAutoAudit == 2)
            {
                //手动审核,也写到db.AditApplications这个表中但是不走process？
                return 6;//手动审核

            }

            if (item.IsAutoAudit == 3)
            { //如果不自动审核，就要走人工审核流程。即，把信息写入db.AditApplications这个表中
                 AuditApplication auditApplication = new AuditApplication();
                 auditApplication.BType= item.Type;  
                 auditApplication.TypeName = item.TypeName;
                 auditApplication.CreateDate = DateTime.Now;

                 var template = (from p in db.AuditTemplates where (
                              (staffSkill.BillTypeNumber == p.BType)&&(DateTime.Now>p.StartTime)&&(DateTime.Now<p.ExpireTime)
                              ) 
                          select p).ToList().FirstOrDefault();
                 if (template != null)
                 {
                    // Staff staff = db.Staffs.Where(c => c.StaffNumber.Equals(staffSkill.StaffNumber)).ToList().Single();
                     Staff staff = (from p in db.Staffs where p.StaffNumber == staffSkill.StaffNumber && p.AuditStatus==3&&p.ArchiveTag==false select p).ToList().Single();
                     var tmpDepartment = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).ToList().Single();
                     var tmpBillType = (from p in db.BillProperties where p.Type == staffSkill.BillTypeNumber select p.TypeFullName).ToList().Single();
                     auditApplication.Info =
                    "单据名称：" + tmpBillType + ";" +
                    "工       号：" + staffSkill.StaffNumber + ";" +
                    "员工名称：" + staff.Name + ";" +
                    "所在部门：" + tmpDepartment+ ";" +
                    "性       别：" + staff.Gender + ";" +
                    "职       位：" + staff.Position + ";" +
                    "用工性质：" + staff.WorkProperty + ";";
                     //"单别：" + staffSkill.BillTypeNumber + ";" +
                     //"单号:" + staffSkill.BillNumber + ";" +
                     //"员工:" + staffSkill.StaffNumber + ";" +
                     //"技能编号:" + staffSkill.SkillNumber + ";" +
                     //"备注:" + staffSkill.SkillRemark + ";" +
                     //"单据类别编号:" + staffSkill.BillTypeNumber + ";" +
                     //"生效日期:" + staffSkill.ValidDate + ";" +
                     //"创建日期:" + staffSkill.RecordTime + ";" +
                     //"录入人员:" + staffSkill.RecordPerson + ";";

                     auditApplication.BNumber = staffSkill.BillNumber;
                     auditApplication.TId = template.Id;
                     auditApplication.Creator = this.UserName;
                     auditApplication.CreatorName = this.Name;
                     auditApplication.State = 0;//代表等待审核
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
                         //   "提交日期：" + auditApplication.CreateDate + ";";
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

          [Authorize(Roles = "Admin,StaffSkill_Index")]
        // GET: StaffSkill
        public ActionResult Index()
        {
            /*员工技能表固定信息*/
            var q = (from ss in db.StaffSkills
                     join p in db.States on ss.AuditStatus equals p.Id
                     join s in db.Staffs on ss.StaffNumber equals s.StaffNumber
                     join sp in db.StaffParams on ss.SkillNumber equals sp.Id.ToString()
                     join bp in db.BillProperties on ss.BillTypeNumber equals bp.Type
                     join d in db.Departments on s.Department equals d.DepartmentId
                     into gc    /*左联：显示所有员工表的字段*/
                     from d in gc.DefaultIfEmpty()
                     select new StaffSkillViewModel { Department = d.Name, ValidDate = ss.ValidDate, Id = ss.Id, 
                         BillTypeName = bp.TypeName, BillTypeNumber = ss.BillTypeNumber, BillNumber = ss.BillNumber, 
                         StaffNumber = ss.StaffNumber, StaffName = s.Name, SkillNumber = ss.SkillNumber, SkillName = sp.Value, 
                       //  SkillRemark = ss.SkillRemark,AuditStatus = ss.AuditStatus}).ToList();
                        SkillRemark = ss.SkillRemark,
                        AuditStatus=ss.AuditStatus,
                        //审核人员及日期
                        AuditPerson = ss.AuditPerson,
                        AuditTime = ss.AuditTime,
                        AuditStatusName = p.Description,
                       
                     }).ToList();
            /*查找员工技能预留字段(name)*/
            var fieldNameList = (
                                  from p in db.ReserveFields 
                                  join qq in db.TableNameContrasts on p.TableNameId equals qq.Id
                                 where qq.TableName == "StaffSkills" && p.Status == true
                                 select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  where rf.Status == true && rf.Status == true
                                  select new StaffSkillViewModel { Id = ssr.Number, Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(q.OrderByDescending(c=>c.Id));
        }
       
        public JsonResult Index1()
        {
            List<Object> obj = new List<Object>();
            var Staffs = db.Staffs.ToList();
            foreach (var temp in Staffs)
            {
                obj.Add(new { number = temp.StaffNumber, name = temp.Name });
                // return Json(obj);
            }
            return Json(obj);
        }

        public ActionResult SkillDetails(string staffNumber)
        { 
          
         var name = (from p in db.Staffs where p.StaffNumber == staffNumber select p.Name).ToList().Single();

         var staffSkills = 
                    (from ss in db.StaffSkills where ss.StaffNumber==staffNumber&&ss.AuditStatus==3
                     join p in db.States on ss.AuditStatus equals p.Id
                     join s in db.Staffs on ss.StaffNumber equals s.StaffNumber
                     join sp in db.StaffParams on ss.SkillNumber equals sp.Id.ToString()
                     join bp in db.BillProperties on ss.BillTypeNumber equals bp.Type
                     join d in db.Departments on s.Department equals d.DepartmentId
                     into gc    /*左联：显示所有员工表的字段*/
                     from d in gc.DefaultIfEmpty()
                     select new StaffSkillViewModel { 
                         Department = d.Name, ValidDate = ss.ValidDate, Id = ss.Id, 
                         BillTypeName = bp.TypeName, BillTypeNumber = ss.BillTypeNumber, BillNumber = ss.BillNumber, 
                         StaffNumber = ss.StaffNumber, StaffName = s.Name, SkillNumber = ss.SkillNumber, SkillName = sp.Value, 
                         SkillRemark = ss.SkillRemark,AuditStatus=ss.AuditStatus,
                        AuditPerson = ss.AuditPerson,
                        AuditTime = ss.AuditTime,
                         AuditStatusName = p.Description,
                     }).ToList();
        ViewBag.name=name;
        ViewBag.skill = staffSkills;
        return View();
        }
        //GET: StaffSkill/ManualAudit/5
        public ActionResult ManualAudit(int?id,int flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            if (staffSkill == null)
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
                    staffSkill.AuditStatus = 3;
                }
                else
                {
                    //不通过审批
                    staffSkill.AuditStatus = 4;
                   
                }
                    staffSkill.AuditPerson = this.UserName;
                    staffSkill.AuditTime = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            catch {
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
            StaffSkill staffSkill = db.StaffSkills.Find(id);
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

        // GET: StaffSkill/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            //较少冗余
            StaffSkillViewModel staffSkillViewModel = new StaffSkillViewModel();
            staffSkillViewModel.Id = staffSkill.Id;
            staffSkillViewModel.BillNumber = staffSkill.BillNumber;
            staffSkillViewModel.BillTypeNumber = staffSkill.BillTypeNumber;
            staffSkillViewModel.StaffNumber = staffSkill.StaffNumber;
            staffSkillViewModel.SkillNumber = staffSkill.SkillNumber;
            staffSkillViewModel.SkillRemark = staffSkill.SkillRemark;
            staffSkillViewModel.ValidDate = staffSkill.ValidDate;
           
            //审核相关的三个字段
            staffSkillViewModel.AuditStatus = staffSkill.AuditStatus;
            staffSkillViewModel.AuditStatusName = db.States.Find(staffSkill.AuditStatus).Description;
            staffSkillViewModel.AuditPerson = staffSkill.AuditPerson;
            staffSkillViewModel.AuditTime = staffSkill.AuditTime;

            var stafflParam = db.StaffParams.Where(sp => sp.Id.ToString().Equals(staffSkill.SkillNumber));
            var staffs = (from s in db.Staffs
                          join d in db.Departments on s.Department equals d.DepartmentId
                          where s.StaffNumber == staffSkill.StaffNumber
                          select new { Name = s.Name, Department = d.Name }).ToList();
            var billTypeNumber = db.BillProperties.Where(bp => bp.Type.Equals(staffSkill.BillTypeNumber));
            foreach (var billTypeNumber1 in billTypeNumber)
            {
                staffSkillViewModel.BillTypeName = billTypeNumber1.TypeName;
            }
            foreach (var temp in stafflParam)
            {
                staffSkillViewModel.SkillName = temp.Value;
            }
            foreach (var temp in staffs)
            {
                staffSkillViewModel.StaffName = temp.Name;
                staffSkillViewModel.Department = temp.Department;
            }
            /*查找员工技能预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts on p.TableNameId equals q.Id
                                 where q.TableName == "StaffSkills" && p.Status == true
                                 select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  where ssr.Number == staffSkill.Id && rf.Status == true
                                  select new StaffSkillViewModel { Id = ssr.Number, Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(staffSkillViewModel);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public void CreateInite() 
        {
            ViewBag.date = DateTime.Now.ToString("yyyy/MM/dd");
            List<SelectListItem> billNumber = db.BillProperties.Where(b => b.BillSort.Equals("24")).ToList().Select(c => new SelectListItem
            {
                Value = c.Type,//保存的值
                Text = c.Type + " " + c.TypeName,//显示的值
            }).ToList();
            //增加一个null选项
            SelectListItem bill = new SelectListItem();
            bill.Value = " ";
            bill.Text = "-请选择-";
            bill.Selected = true;
            billNumber.Add(bill);
            ViewBag.billNumberList = billNumber;
            //4.员工技能
            List<SelectListItem> staffSkill = new List<SelectListItem>();
            ///linq多表查询
            var staffSkill1 = from spt in db.StaffParamTypes
                              join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                              where spt.Name == "员工技能"
                              select new { id = sp.Id, value = sp.Value };

            foreach (var tt in staffSkill1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.id.ToString();
                i.Text = tt.value;
                staffSkill.Add(i);

            }
            ViewBag.staffSkill = staffSkill;
            /*查找员工技能预留字段(name)*/
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts on p.TableNameId equals q.Id
                             where q.TableName == "StaffSkills" && p.Status == true
                             select p).ToList();
            ViewBag.fieldList = fieldList;
        }

         [Authorize(Roles = "Admin,StaffSkill_Create")]
        // GET: StaffSkill/Create
        public ActionResult Create()
        {
            CreateInite();
            return View();
        }

           [Authorize(Roles = "Admin,StaffSkill_Create")]
        // POST: StaffSkill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffSkill staffSkill3)
        {
            if (ModelState.IsValid)
            {
             
                staffSkill3.RecordTime = DateTime.Now;
                
                if (staffSkill3.ValidDate < DateTime.Today)
                {
                    ModelState.AddModelError("", "生效日期错误！请重新选择");
                    CreateInite();
                    StaffSkillViewModel staffSkill = new StaffSkillViewModel();
                    staffSkill.Id = staffSkill3.Id;
                    staffSkill.BillNumber = staffSkill3.BillNumber;
                    staffSkill.StaffNumber = staffSkill3.StaffNumber;
                    staffSkill.SkillNumber = staffSkill3.SkillNumber;
                    staffSkill.SkillRemark = staffSkill3.SkillRemark;
                    staffSkill.ValidDate = staffSkill3.ValidDate;
                 
                    return View(staffSkill);
                }

                staffSkill3.BillNumber = GenerateBillNumber(staffSkill3.BillTypeNumber);
                staffSkill3.RecordPerson = base.Name;
                /*先保存员工技能固定的字段（为了生成主键Id）*/
                db.StaffSkills.Add(staffSkill3);
                db.SaveChanges();
                //提交审核
                byte status = AuditApplicationStaffSkill(staffSkill3);
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    CreateInite();
                    StaffSkillViewModel staffSkill = new StaffSkillViewModel();
                    staffSkill.Id = staffSkill3.Id;
                    // staffSkill.BillTypeNumber = GenerateBillNumber();
                    staffSkill.BillNumber = staffSkill3.BillNumber;
                    staffSkill.StaffNumber = staffSkill3.StaffNumber;
                    staffSkill.SkillNumber = staffSkill3.SkillNumber;
                    staffSkill.SkillRemark = staffSkill3.SkillRemark;
                    staffSkill.ValidDate = staffSkill3.ValidDate;
                    return View(staffSkill3);
                }
                else {
                    //需要对原表做出的修改
                    staffSkill3.AuditStatus = status;
                    db.SaveChanges();
                }
              
                /*查找员工技能预留字段(name)*/
                var fieldList = (from p in db.ReserveFields join q in db.TableNameContrasts on p.TableNameId equals q.Id where q.TableName == "StaffSkills" select p).ToList();
                ViewBag.fieldList = fieldList;
                /*遍历，保存员工技能变化的字段*/
                foreach (var temp in fieldList)
                {
                    StaffSkillReserve ssr = new StaffSkillReserve();
                    ssr.Number = staffSkill3.Id;
                    ssr.FieldId = temp.Id;
                    ssr.Value = Request[temp.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (ssr.Value == null) ssr.Value = " ";
                    db.StaffSkillReserves.Add(ssr);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                CreateInite();
                StaffSkillViewModel staffSkill = new StaffSkillViewModel();
                staffSkill.Id = staffSkill3.Id;
               // staffSkill.BillTypeNumber = GenerateBillNumber();
                staffSkill.BillNumber = staffSkill3.BillNumber;
                staffSkill.StaffNumber = staffSkill3.StaffNumber;
                staffSkill.SkillNumber = staffSkill3.SkillNumber;
                staffSkill.SkillRemark = staffSkill3.SkillRemark;
                staffSkill.ValidDate = staffSkill3.ValidDate;

                return View(staffSkill);
            }
        }

           [Authorize(Roles = "Admin,StaffSkill_Edit")]
        // GET: StaffSkill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StaffSkill staffSkill = db.StaffSkills.Find(id);

            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            StaffSkillViewModel staffSkillViewModel = new StaffSkillViewModel();
            staffSkillViewModel.Id = staffSkill.Id;
            staffSkillViewModel.BillNumber = staffSkill.BillNumber;
            staffSkillViewModel.BillTypeNumber = staffSkill.BillTypeNumber;
            staffSkillViewModel.StaffNumber = staffSkill.StaffNumber;
            staffSkillViewModel.SkillNumber = staffSkill.SkillNumber;
            staffSkillViewModel.SkillRemark = staffSkill.SkillRemark;
            staffSkillViewModel.ValidDate = staffSkill.ValidDate;
            var stafflParam = db.StaffParams.Where(sp => sp.Id.ToString().Equals(staffSkill.SkillNumber));
            var staffs = (from s in db.Staffs
                          join d in db.Departments on s.Department equals d.DepartmentId
                          into gc    /*左联：显示所有员工表的字段*/
                          from d in gc.DefaultIfEmpty()
                          where s.StaffNumber == staffSkill.StaffNumber
                          select new { Name = s.Name, Department = d.Name }).ToList();
            var billTypeNumber = db.BillProperties.Where(bp => bp.Type.Equals(staffSkill.BillTypeNumber));
            foreach (var billTypeNumber1 in billTypeNumber)
            {
                staffSkillViewModel.BillTypeName = billTypeNumber1.TypeName;
            }
            foreach (var temp in stafflParam)
            {
                staffSkillViewModel.SkillName = temp.Value;
            }
            foreach (var temp in staffs)
            {
                staffSkillViewModel.StaffName = temp.Name;
                staffSkillViewModel.Department = temp.Department;
            }
            //4.员工技能
            List<SelectListItem> staffSkill2 = new List<SelectListItem>();
            ///linq多表查询
            var staffSkill1 = from spt in db.StaffParamTypes
                              join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                              where spt.Name == "员工技能" 
                              select new { id = sp.Id, value = sp.Value };

            foreach (var tt in staffSkill1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.id.ToString();
                i.Text = tt.value;
                staffSkill2.Add(i);

            }
            ViewBag.staffSkill = staffSkill2;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  //where ssr.Number == staffSkill.Id
                                  where ssr.Number == id && rf.Status == true
                                  select new StaffSkillViewModel { Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            return View(staffSkillViewModel);
        }

        // POST: StaffSkill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,StaffSkill_Edit")]
        public ActionResult Edit(StaffSkillViewModel staffSkill)
        {
            if (ModelState.IsValid)
            {
                StaffSkill ss = db.StaffSkills.Find(staffSkill.Id);
                /*查找员工技能预留字段(value)*/
                var fieldValueList = (from ssr in db.StaffSkillReserves
                                      join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                      where ssr.Number == staffSkill.Id && rf.Status == true
                                      select new StaffSkillViewModel { Id = ssr.Id, Description = rf.Description, Value = ssr.Value }).ToList();
                //ViewBag.fieldValueList = fieldValueList;
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    StaffSkillReserve ssr = db.StaffSkillReserves.Find(temp.Id);
                    ssr.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                ss.ValidDate = staffSkill.ValidDate;
                ss.StaffNumber = staffSkill.StaffNumber;
                ss.SkillRemark = staffSkill.SkillRemark;
                ss.SkillNumber = staffSkill.SkillNumber;
                ss.BillTypeNumber = staffSkill.BillTypeNumber;
                ss.BillNumber = staffSkill.BillNumber;
                ss.ChangePerson = base.Name;
                ss.ChangeTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffSkill);
        }

        [Authorize(Roles = "Admin,StaffSkill_Delete")]
        // GET: StaffSkill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            if (staffSkill == null)
            {
                return HttpNotFound();
            }
            StaffSkillViewModel staffSkillViewModel = new StaffSkillViewModel();
            staffSkillViewModel.Id = staffSkill.Id;
            staffSkillViewModel.BillNumber = staffSkill.BillNumber;
            staffSkillViewModel.BillTypeNumber = staffSkill.BillTypeNumber;
            staffSkillViewModel.StaffNumber = staffSkill.StaffNumber;
            staffSkillViewModel.SkillNumber = staffSkill.SkillNumber;
            staffSkillViewModel.SkillRemark = staffSkill.SkillRemark;
            staffSkillViewModel.ValidDate = staffSkill.ValidDate;
            staffSkillViewModel.AuditStatusName = db.States.Find(staffSkill.AuditStatus).Description;
            var stafflParam = db.StaffParams.Where(sp => sp.Id.ToString().Equals(staffSkill.SkillNumber));
            var staffs = (from s in db.Staffs
                          join d in db.Departments on s.Department equals d.DepartmentId
                          where s.StaffNumber == staffSkill.StaffNumber
                          select new { Name = s.Name, Department = d.Name }).ToList();
            var billTypeNumber = db.BillProperties.Where(bp => bp.Type.Equals(staffSkill.BillTypeNumber));
            foreach (var billTypeNumber1 in billTypeNumber)
            {
                staffSkillViewModel.BillTypeName = billTypeNumber1.TypeName;
            }
            foreach (var temp in stafflParam)
            {
                staffSkillViewModel.SkillName = temp.Value;
            }
            foreach (var temp in staffs)
            {
                staffSkillViewModel.StaffName = temp.Name;
                staffSkillViewModel.Department = temp.Department;
            }
            /*显示预留字段以及预留字段的值*/
            /*查找员工技能预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                on p.TableNameId equals q.Id 
                                 where q.TableName == "StaffSkills"
                                  && p.Status == true
                                 select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找员工技能预留字段(value)*/
            var fieldValueList = (from ssr in db.StaffSkillReserves
                                  join rf in db.ReserveFields on ssr.FieldId equals rf.Id
                                  where ssr.Number == staffSkill.Id && rf.Status == true
                                  select new StaffSkillViewModel { Id = ssr.Number, Description = rf.Description, Value = ssr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(staffSkillViewModel);
        }

           [Authorize(Roles = "Admin,StaffSkill_Delete")]
        // POST: StaffSkill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from ssr in db.StaffSkillReserves
                        where ssr.Number == id
                        select new StaffSkillViewModel { Id = ssr.Id }).ToList();
            foreach (var temp in item)
            {
                StaffSkillReserve ssr = db.StaffSkillReserves.Find(temp.Id);
                db.StaffSkillReserves.Remove(ssr);
            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            StaffSkill staffSkill = db.StaffSkills.Find(id);
            db.StaffSkills.Remove(staffSkill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch(string name)
        {
            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == name
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

            string temp = GenerateBillNumber(number);

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
         [HttpPost]
        public JsonResult StaffSearch()
        {

            try
            {
                var items = (from s in db.Staffs
                             where s.AuditStatus == 3 && s.ArchiveTag == false
                             join d in db.Departments on s.Department equals d.DepartmentId
                             into gc
                             from d in gc.DefaultIfEmpty()
                             select new
                                   {
                                       text = s.StaffNumber+" "+s.Name,
                                       id = s.StaffNumber
                                   }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        [HttpPost]
        public JsonResult SkillNumberSearch()
        {
            try
            {
                var items = (from sp in db.StaffParams
                             join spt in db.StaffParamTypes on sp.StaffParamTypeId equals spt.Id
                             where spt.Name == "员工技能"
                             select new
                             {
                                 text = sp.Value,
                                 id = sp.Id,
                                 order=sp.StaffParamOrder,
                                 isDefault=sp.IsDefault

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
                var items = (from s in db.Staffs where s.AuditStatus==3&&s.ArchiveTag==false
                             join d in db.Departments on s.Department equals d.DepartmentId
                             where s.StaffNumber==number

                             select new
                             {
                                 DepartmentName=d.Name
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
