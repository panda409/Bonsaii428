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
using System.Data.Entity.Validation;

namespace Bonsaii.Controllers
{
    public class RecruitmentsController : BaseController
    {
        public byte AuditApplicationStaffSkill(Recruitments staffSkill)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == staffSkill.BillType select p).ToList().FirstOrDefault();

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
                auditApplication.BType = item.Type;
                auditApplication.TypeName = item.TypeName;
                auditApplication.CreateDate = DateTime.Now;

                var template = (from p in db.AuditTemplates
                                where (
                                    (staffSkill.BillType == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();
                if (template != null)
                {
                   // Staff staff = db.Staffs.Where(c => c.StaffNumber.Equals(staffSkill.)).ToList().Single();
                   // var tmpDepartment = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).ToList().Single();
                    var tmpBillType = (from p in db.BillProperties where p.Type == staffSkill.BillType select p.TypeFullName).ToList().Single();
                    auditApplication.Info =
                   "单据名称：" + tmpBillType + ";" +
                   "申请部门：" + staffSkill.DepartmentName + ";" +
                   "招聘职务：" + staffSkill.Position + ";" +
                   "招聘人数：" + staffSkill.RequiredNumber + ";" +
                   "性别：" + staffSkill.Gender + ";"+
                   "年龄：" + staffSkill.Age + ";"+
                   "婚姻状况：" + staffSkill.MaritalStatus + ";"+
                   "学历要求：" + staffSkill.EducationBackground + ";"+
                   "专业：" + staffSkill.Major + ";"+
                   "工作经验：" + staffSkill.WorkExperience + ";"+
                   "技能要求：" + staffSkill.Skill + ";"+
                   "其他条件：" + staffSkill.Others + ";"
                   ;
                    //"单别：" + staffSkill.BillTypeNumber + ";" +
                    //"单号:" + staffSkill.BillNumber + ";" +
                    //"员工:" + staffSkill.StaffNumber + ";" +
                    //"技能编号:" + staffSkill.SkillNumber + ";" +
                    //"备注:" + staffSkill.SkillRemark + ";" +
                    //"单据类别编号:" + staffSkill.BillTypeNumber + ";" +
                    //"生效日期:" + staffSkill.ValidDate + ";" +
                    //"创建日期:" + staffSkill.RecordTime + ";" +
                    //"录入人员:" + staffSkill.RecordPerson + ";";

                    auditApplication.BNumber = staffSkill.BillCode;
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
        //GET: StaffSkill/ManualAudit/5
        public ActionResult ManualAudit(int? id, int flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recruitments staffSkill = db.Recruitments.Find(id);
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
            Recruitments staffSkill = db.Recruitments.Find(id);
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


        // GET: Recruitments
        public ActionResult Index()
        {
            //var recruitment = (from r in db.Recruitments
            //                   join b in db.BillProperties on r.BillType equals b.Type
            //                   select new RecruitmentViewModel { }).ToList();
            /*查找预留字段(name)*/
            var fieldNameList = (from p in db.ReserveFields 
                                 join q in db.TableNameContrasts on p.TableNameId equals q.Id
                                 where q.TableName == "Recruitments"
                                 select p).ToList();
            ViewBag.fieldNameList = fieldNameList;
            /*查找预留字段(value)*/
            var fieldValueList = (from rr in db.RecruitmentReserves
                                  join rf in db.ReserveFields on rr.FieldId equals rf.Id
                                  select new RecruitmentViewModel { Id = rr.Number, Description = rf.Description, Value = rr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            var recruitments = db.Recruitments.OrderByDescending(c=>c.Id).ToList();
            foreach (var recruitment in recruitments)
            {
                recruitment.AuditStatusName = db.States.Find(recruitment.AuditStatus).Description;
            }

            return View(recruitments);
        }

        // GET: Recruitments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recruitments recruitments = db.Recruitments.Find(id);
            if (recruitments == null)
            {
                return HttpNotFound();
            }
            BillPropertyModels bill = db.BillProperties.Where(b => b.Type.Equals(recruitments.BillType)).SingleOrDefault();
            recruitments.BillType = recruitments.BillType + " " + bill.TypeName;          
            /*查找预留字段(value)*/
            var fieldValueList = (from rr in db.RecruitmentReserves
                                  join rf in db.ReserveFields on rr.FieldId equals rf.Id
                                  where rr.Number==recruitments.Id
                                  select new RecruitmentViewModel { Id = rr.Number, Description = rf.Description, Value = rr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(recruitments);
        }

        // GET: Recruitments/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentsList = new SelectList(db.Departments.ToList(), "Name", "Name");

            //       ViewBag.PositionsList = new SelectList(db.)
            //   var tmp = from x in db.StaffParams where (from d in db.StaffParamTypes where d.Name == "员工岗位" select d.Id).Contains(x.StaffParamTypeId) select x.Value;

            ViewBag.PositionsList = this.GetParamsByName("员工岗位");

            ViewBag.GendersList = this.GetParamsByName("性别");
            ViewBag.MaritalStatusList = this.GetParamsByName("婚姻状况");
            ViewBag.EduBackgroundsList = this.GetParamsByName("学历");
            ViewBag.MajorsList = this.GetParamsByName("专业");
            /*查找预留字段(name)*/
            var fieldList = (from p in db.ReserveFields
                                    join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id where
                                 q.TableName == "Recruitments" select p).ToList();
            ViewBag.fieldList = fieldList;
           
            return View();
        }

        // POST: Recruitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recruitments recruitments)
        {
           
            if (ModelState.IsValid)
            {
                Recruitments tmpRecruit = new Recruitments()
                {
                    BillType = Request["BillType"].Length == 4 ? Request["BillType"] : Request["BillType"].Substring(0, 4),
                    BillCode = Request["BillCode"],
                    DepartmentName = Request["DepartmentName"],
                    Position = Request["Position"],
                    RequiredNumber = int.Parse(Request["RequiredNumber"]),
                    Gender = Request["Gender"],
                    Age = Request["Age"],
                    MaritalStatus = Request["MaritalStatus"],
                    EducationBackground = Request["EducationBackground"],
                    Major = Request["Major"],
                    WorkExperience = Request["WorkExperience"],
                    Skill = Request["Skill"],
                    Others = Request["Others"],
                    Status = "等待招聘",
                    IsAudit = true,
                    IsPublished = false,
                    RecordPerson = this.Name,
                    RecordTime = DateTime.Now
                };
                tmpRecruit.BillCode = GenerateBillNumber(tmpRecruit.BillType);
                tmpRecruit.AuditStatus = recruitments.AuditStatus;
                tmpRecruit.AuditPerson = recruitments.AuditPerson;
                tmpRecruit.AuditTime = recruitments.AuditTime;
                db.Recruitments.Add(tmpRecruit);
              //  db.SaveChanges();

                //提交审核
                byte status = AuditApplicationStaffSkill(tmpRecruit);
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    return View(recruitments);
                }
                else
                {
                    //需要对原表做出的修改
                    tmpRecruit.AuditStatus = status;
                    try {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e) { throw e; }
                }


                /*查找预留字段(name)*/
                var fieldList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id 
                                 where q.TableName == "Recruitments" select p).ToList();
                ViewBag.fieldList = fieldList;
                /*遍历，保存变化的字段*/
                foreach (var temp in fieldList)
                {
                    RecruitmentReserve rr = new RecruitmentReserve();
                    rr.Number = tmpRecruit.Id;
                    rr.FieldId = temp.Id;
                    rr.Value = Request[temp.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (rr.Value == null) rr.Value = " ";
                    db.RecruitmentReserves.Add(rr);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentsList = new SelectList(db.Departments.ToList(), "Name", "Name");
            ViewBag.PositionsList = this.GetParamsByName("员工岗位");
            ViewBag.GendersList = this.GetParamsByName("性别");
            ViewBag.MaritalStatusList = this.GetParamsByName("婚姻状况");
            ViewBag.EduBackgroundsList = this.GetParamsByName("学历");
            ViewBag.MajorsList = this.GetParamsByName("专业");
            /*查找员工技能预留字段(name)*/
            var fieldList1 = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                                on p.TableNameId equals q.Id 
                              where q.TableName == "Recruitments" select p).ToList();
            ViewBag.fieldList = fieldList1;
            return View(recruitments);
        }

        // GET: Recruitments/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recruitments recruitments = db.Recruitments.Find(id);
            if (recruitments == null)
            {
                return HttpNotFound();
            }
              BillPropertyModels bill = db.BillProperties.Where(b => b.Type.Equals(recruitments.BillType)).SingleOrDefault();        
            ViewBag.BillType = recruitments.BillType + " " + bill.TypeName;
            ViewBag.BillCode = recruitments.BillCode;
            ViewBag.DepartmentsList = new SelectList(db.Departments.ToList(), "Name", "Name", recruitments.DepartmentName);

            ViewBag.PositionsList = new SelectList(this.GetParamsListByName("员工岗位"), recruitments.Position);//this.GetParamsByName("员工岗位");
            ViewBag.RequiredNumber = recruitments.RequiredNumber.ToString();
            ViewBag.GendersList = new MultiSelectList(this.GetParamsListByName("性别"), this.SelectedListByName(recruitments.Gender));
            ViewBag.Age = recruitments.Age.ToString();
            ViewBag.MaritalStatusList = new MultiSelectList(this.GetParamsListByName("婚姻状况"), this.SelectedListByName(recruitments.MaritalStatus));
            ViewBag.EduBackgroundsList = new MultiSelectList(this.GetParamsListByName("学历"), this.SelectedListByName(recruitments.EducationBackground));
            ViewBag.MajorsList = new MultiSelectList(this.GetParamsListByName("专业"), this.SelectedListByName(recruitments.Major));

            ViewBag.WorkExperience = recruitments.WorkExperience;
            ViewBag.Skill = recruitments.Skill;
            ViewBag.Others = recruitments.Others;
            ViewBag.Status = recruitments.Status;
            /*查找预留字段(value)*/
            var fieldValueList = (from rr in db.RecruitmentReserves
                                  join rf in db.ReserveFields on rr.FieldId equals rf.Id                                
                                  where rr.Number == id
                                  select new RecruitmentViewModel { Description = rf.Description, Value = rr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View();
        }

        // POST: Recruitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillType,BillCode,DepartmentName,Position,RequiredNumber,Gender,Age,MaritalStatus,EducationBackground,Major,WorkExperience,Skill,Others,Status")] Recruitments recruitments)
        {
            if (ModelState.IsValid)
            {
                Recruitments tmp = db.Recruitments.Find(recruitments.Id);
                tmp.BillType = Request["BillType"].Substring(0,4);
                tmp.BillCode = Request["BillCode"];
                tmp.DepartmentName = Request["DepartmentName"];
                tmp.Position = Request["Position"];
                tmp.RequiredNumber = int.Parse(Request["RequiredNumber"]);
                tmp.Gender = Request["Gender"];
                tmp.Age = Request["Age"];
                tmp.MaritalStatus = Request["MaritalStatus"];
                tmp.EducationBackground = Request["EducationBackground"];
                tmp.Major = Request["Major"];
                tmp.WorkExperience = Request["WorkExperience"];
                tmp.Skill = Request["Skill"];
                tmp.Others = Request["Others"];
                tmp.Status = Request["Status"];
                tmp.ChangePerson = this.Name;
                tmp.ChangeTime = DateTime.Now;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
                /*查找预留字段(value)*/
                var fieldValueList = (from rr in db.RecruitmentReserves
                                      join rf in db.ReserveFields on rr.FieldId equals rf.Id
                                      where rr.Number == recruitments.Id
                                      select new RecruitmentViewModel { Id = rr.Id, Description = rf.Description, Value = rr.Value }).ToList();
              
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    RecruitmentReserve rr = db.RecruitmentReserves.Find(temp.Id);
                    rr.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(recruitments);
        }

        public ActionResult TestEdit()
        {
            int id = 2;
            Recruitments recruitments = db.Recruitments.Find(id);
            if (recruitments == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillType = recruitments.BillType;
            ViewBag.BillCode = recruitments.BillCode;
            ViewBag.DepartmentsList = new SelectList(db.Departments.ToList(), "Name", "Name", recruitments.DepartmentName);

            ViewBag.PositionsList = new SelectList(this.GetParamsListByName("员工岗位"), recruitments.Position);//this.GetParamsByName("员工岗位");
            ViewBag.RequiredNumber = recruitments.RequiredNumber.ToString();
            ViewBag.GendersList = new MultiSelectList(this.GetParamsListByName("性别"), this.SelectedListByName(recruitments.Gender));
            ViewBag.Age = recruitments.Age.ToString();
            ViewBag.MaritalStatusList = new MultiSelectList(this.GetParamsListByName("婚姻状况"), this.SelectedListByName(recruitments.MaritalStatus));
            ViewBag.EduBackgroundsList = new MultiSelectList(this.GetParamsListByName("学历"), this.SelectedListByName(recruitments.EducationBackground));
            ViewBag.MajorsList = new MultiSelectList(this.GetParamsListByName("专业"), this.SelectedListByName(recruitments.Major));

            ViewBag.WorkExperience = recruitments.WorkExperience;
            ViewBag.Skill = recruitments.Skill;
            ViewBag.Others = recruitments.Others;
            ViewBag.Status = recruitments.Status;
            return View();
        }

        // GET: Recruitments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recruitments recruitments = db.Recruitments.Find(id);
            if (recruitments == null)
            {
                return HttpNotFound();
            }
            BillPropertyModels bill = db.BillProperties.Where(b => b.Type.Equals(recruitments.BillType)).SingleOrDefault();
            recruitments.BillType = recruitments.BillType+" "+bill.TypeName;
         
            /*查找预留字段(value)*/
            var fieldValueList = (from rr in db.RecruitmentReserves
                                  join rf in db.ReserveFields on rr.FieldId equals rf.Id
                                  where rr.Number == recruitments.Id
                                  select new RecruitmentViewModel { Id = rr.Number, Description = rf.Description, Value = rr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(recruitments);
        }

        // POST: Recruitments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from rr in db.RecruitmentReserves
                        where rr.Number == id
                        select new RecruitmentViewModel { Id = rr.Id }).ToList();
            foreach (var temp in item)
            {
                RecruitmentReserve rr = db.RecruitmentReserves.Find(temp.Id);
                db.RecruitmentReserves.Remove(rr);
            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            Recruitments recruitments = db.Recruitments.Find(id);
            db.Recruitments.Remove(recruitments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult BillTypeSearch(string number)
        {

            try
            {
                // var item = db.Staffs.Where(w => (w.StaffNumber).Contains(number)).ToList().Select(w => new { id=w.StaffNumber,name=w.StaffNumber});
                var items = (from p in db.BillProperties where p.Type.Contains(number) || p.TypeName.Contains(number) select p.Type + "-" + p.TypeName).ToList();//.ToList().Select p;
                return Json(
                    new
                    {
                        success = true,
                        data = items
                    });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message });
            }

        }
        /// <summary>
        /// 根据人事基本参数的名称，获取具体的参数内容（比如：参数为“婚姻状况”，返回值为“已婚”、“未婚”、”离异“）,用于构成下拉列表
        /// </summary>
        /// <param name="TypeName">人事参数的类型名称</param>
        /// <returns>返回具体的参数内容的列表</returns>
        public SelectList GetParamsByName(string TypeName)
        {
            List<string> tmp = (from x in db.StaffParams where (from d in db.StaffParamTypes where d.Name == TypeName select d.Id).Contains(x.StaffParamTypeId) select x.Value).ToList();
            return new SelectList(tmp);
        }
        /// <summary>
        /// 根据人事基本参数的名称，获取具体的参数内容（比如：参数为“婚姻状况”，返回值为“已婚”、“未婚”、”离异“），用于构成多选框选项
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public List<string> GetParamsListByName(string TypeName)
        {
            return (from x in db.StaffParams where (from d in db.StaffParamTypes where d.Name == TypeName select d.Id).Contains(x.StaffParamTypeId) select x.Value).ToList();
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

        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                string str = "tmp";
            }
            return View();
        }

        public ActionResult TestFileUpload()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult PublishRecruitments()
        {
            return View(db.Recruitments.Where(p => p.IsAudit == true&&p.AuditStatus==3).ToList().OrderBy(c=>c.BillCode));
        }

        public ActionResult EditRecruitments(int? id)
        {
            return View(db.Recruitments.Find(id));
        }

        public ActionResult PreviewRecruitments(int? id)
        {
            return View(db.Recruitments.Find(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveRecruitments(int? id)
        {
            Recruitments tmp = db.Recruitments.Find(id);
            /**
             * 这里要注意，MVC框架在默认情况不会运行获取表单中的包含“<>"的内容，为了能正常获取，需要采取两个措施：
             * 1、添加[ValidateInput(false)]注解
             * 2、需要在web.config配置中添加<httpRuntime requestValidationMode="2.0"/>配置
             * */
            tmp.PublishVersion = Request["content"];
            tmp.IsPublished = true;
            tmp.Status = "正在招聘";
            db.Entry(tmp).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PublishRecruitments");
        }


        public JsonResult CheckStaffSize()
        {
            int size = int.Parse(Request["number"]);
            string DepartmentName = Request["DepartmentName"];

            var StaffSize = db.Departments.Where(p => p.Name == DepartmentName).Select(p => p.StaffSize);
            int max = StaffSize.First();
            //该部门当前已有的员工数量
            int CurSize = db.Staffs.Where(p => p.Department == DepartmentName).ToList().Count();

            if (size <= (max - CurSize))
                return Json(new { result = "success" });
            else
                return Json(new { result = "该部门人员已满，请更换其他部门" });

        }
        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch()
        {

            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == "25"
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
    }
}
