using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using PagedList;
using Bonsaii.Models.Audit;
using System.Reflection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using BonsaiiModels;
using System.Data.Entity.Validation;
using Bonsaii.Models.Works;
namespace Bonsaii.Controllers
{
    public class StaffController : BaseController
    {
        //审批
        public byte AuditApplicationStaff(Staff staff)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == staff.BillTypeNumber select p).ToList().FirstOrDefault();

            if (item.IsAutoAudit == 1)//自动审核是1
            {
                return 3;//代表已审
            }

            if (item.IsAutoAudit == 2)//手动审核是2
            {
                return 6;//待审(手动)
            }

            if (item.IsAutoAudit == 3)//审核流程是3
            {
                AuditApplication auditApplication = new AuditApplication();
                auditApplication.BType = item.Type;
                auditApplication.TypeName = item.TypeName;
                auditApplication.CreateDate = DateTime.Now;

                var template = (from p in db.AuditTemplates
                                where (
                                    (staff.BillTypeNumber == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    //auditApplication.Remark = staff.;
                    auditApplication.BNumber = staff.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = base.UserName;
                    auditApplication.CreatorName = base.Name;
                    auditApplication.State = 0;//待审
                    string departmentName = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).SingleOrDefault();
                    if (departmentName == null)
                    {
                        staff.DepartmentName = this.CompanyFullName;
                    }
                    else
                    {
                        staff.DepartmentName = departmentName;
                    }
                    DateTime entryDate_dateTime = (DateTime)staff.Entrydate;

                    if (staff.BirthDate != null)
                    {
                        DateTime birthDate_dateTime = (DateTime)staff.BirthDate;

                        auditApplication.Info =
                       "单据名称：" + staff.BillTypeName + ";" +
                       "工号：" + staff.StaffNumber + ";" +
                       "员工名称：" + staff.Name + ";" +
                       "所在部门：" + staff.DepartmentName + ";" +
                       "性别：" + staff.Gender + ";" +
                       "职位：" + staff.Position + ";" +
                       "用工性质：" + staff.WorkProperty + ";" +
                       "入职日期：" + entryDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                       "出生日期：" + birthDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                       "员工来源：" + staff.Source + ";" +
                       "籍贯：" + staff.NativePlace + ";" +
                       "学历：" + staff.EducationBackground + ";" +
                       "专业：" + staff.SchoolMajor + ";" +
                       "个人手机：" + staff.IndividualTelNumber + ";";
                    }
                    else
                    {
                        auditApplication.Info =
                      "单据名称：" + staff.BillTypeName + ";" +
                      "工号：" + staff.StaffNumber + ";" +
                      "员工名称：" + staff.Name + ";" +
                      "所在部门：" + staff.DepartmentName + ";" +
                      "性别：" + staff.Gender + ";" +
                      "职位：" + staff.Position + ";" +
                      "用工性质：" + staff.WorkProperty + ";" +
                      "入职日期：" + entryDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                      "出生日期：" + staff.BirthDate + ";" +
                      "员工来源：" + staff.Source + ";" +
                      "籍贯：" + staff.NativePlace + ";" +
                      "学历：" + staff.EducationBackground + ";" +
                      "专业：" + staff.SchoolMajor + ";" +
                      "个人手机：" + staff.IndividualTelNumber + ";";
                    }
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
                        auditProcess.Info =
                            auditApplication.Info;//+
                        //"提交人员：" + auditApplication.CreatorName + "-" + auditApplication.Creator + ";" +
                        // "提交日期：" + auditApplication.CreateDate + ";";
                        auditProcess.AuditDate = DateTime.Now;
                        auditProcess.CreateDate = auditApplication.CreateDate;
                        auditProcess.Result = 0;
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
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
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
                    staff.AuditStatus = 3;//需要对原表做出的修改
                    if (staff.AuditStatus == 3)
                    {
                        AddDefaultWork(staff.StaffNumber, Int32.Parse(staff.ClassOrder), staff.Department);
                        SystemDbContext sysdb = new SystemDbContext();
                        /*BindCodes*/
                        var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == this.CompanyId && p.StaffNumber == staff.StaffNumber) select p).ToList();
                        if (Ucount.Count == 0)
                        {
                            //木有该员工
                            BindCode user = new BindCode();
                            string CompanyDbName = "Bonsaii" + this.CompanyId;
                            user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                            user.CompanyId = this.CompanyId;
                            user.StaffNumber = staff.StaffNumber;
                            user.RealName = staff.Name;
                            user.BindingCode = staff.BindingCode;
                            user.Phone = staff.IndividualTelNumber;
                            user.BindTag = false;
                            user.LastTime = DateTime.Now;
                            user.IsAvail = true;
                            sysdb.BindCodes.Add(user);
                            sysdb.SaveChanges();
                            db.SaveChanges();
                        }
                        else
                        {
                            db.SaveChanges();
                        }
                        //else if (Ucount.Count == 1)
                        //{
                        //    ModelState.AddModelError("", "需要更换工号！");
                        //    return View(staff);
                        //}
                        //else
                        //{
                        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        //}
                        //增加自定义字段
                        //AddReserveFields(staff);
                    }
                }
                else
                {
                    //不通过审批
                    staff.AuditStatus = 4;

                }
                staff.AuditPerson = this.UserName;
                staff.AuditTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw e;
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //GET: Staff/Submit/5
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationStaff(staff);
            //需要对原表做出的修改
            staff.AuditStatus = status;
            //if (status == 7) {
            //    //ViewBag.ErrorMessage = true;
            //    return RedirectToAction("Index");
            //}
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /*显示每个部门的员工列表 待改进*/
        public ActionResult List(string id)
        {
            // var Staffs = (from s in db.Staffs orderby s.BillNumber where s.JobState != "离职" select s).ToList();
            //找到孩子部门
            // var chidDepartment = (from q in db.Departments where q.ParentDepartmentId == id select q.DepartmentId).ToList();

            //var StaffsInThisDepartment = (
            //                              from q in db.Departments
            //                              where q.ParentDepartmentId == id
            //                              from p in db.Staffs where( p.Department == id || p.Department == q.ParentDepartmentId)
            //                              select p).ToList();
            // return View(StaffsInThisDepartment);
            List<Staff> Staffs = new List<Staff>();
            //var staffInDepartments = (from p in db.Staffs where( p.Department ==id&& p.JobState!="离职") select p).ToList();
            var staffInDepartments = (from p in db.Staffs
                                      where (p.Department == id && p.ArchiveTag == false)
                                      orderby p.BillNumber
                                      select p).ToList();
            foreach (Staff staffInDepartment in staffInDepartments)
            {
                Staffs.Add(staffInDepartment);
            }
            var childDepartments = (from q in db.Departments where q.ParentDepartmentId == id select q.DepartmentId).ToList();
            foreach (var childDepartment in childDepartments)
            {
                var staffInChidDepartments = (from p in db.Staffs where (p.Department == childDepartment && p.ArchiveTag == false) select p).ToList();
                foreach (Staff staffInChidDepartment in staffInChidDepartments)
                {
                    Staffs.Add(staffInChidDepartment);
                }
            }
            foreach (Staff tmp in Staffs)
            {
                tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
            }
            return View(Staffs);
            //foreach(var ditem in d)
            //{
            //      List<Staff>  item2 = (from p in db.Staffs
            //           where (p.Department == id.ToString() || ditem== id.ToString())
            //          select p).ToList();
            //      foreach (var i in item2) {
            //          item.Add(i);
            //      }
            //}

            //var item = (
            //     from p in db.Staffs
            //     join q in db.Departments on id.ToString() equals q.ParentDepartmentId
            //     where ((p.Department == id.ToString()) || (q.DepartmentId == p.Department))
            //     select p).ToList();


        }
        public ActionResult IndexMain()
        {
            return View();
        }

        /// <summary>
        /// 初始化Index，用于正确显示员工列表
        /// </summary>
        /// <returns></returns>
        public List<Staff> InitIndex()
        {
            List<Staff> Staffs = (from p in db.Staffs
                                  where p.ArchiveTag == false
                                  select p).OrderByDescending(c => c.Number).ToList();
            //p.JobState != "离职" select p).ToList();
            foreach (Staff tmp in Staffs)
            {
                tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
            }
            return Staffs;
        }

        [Authorize(Roles = "Admin,Staff_Index")]
        // GET: Staff
        public ActionResult Index()//string sortOrder, string currentFilter, string searchString, int? page)
        {
            List<Staff> Staffs = InitIndex();
            return View(Staffs);//使用ToPagedList方法时，需要引入using PagedList系统集成的分页函数。
        }
        public FilePathResult Download()
        {
            return File("../files/download/员工导入模板.xlsx", "application/excel", "员工导入模板.xlsx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase file)
        {
            List<Staff> Staffs = InitIndex();
            //下载文件
            var fileAddr = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
            //提示信息初始化
            string alert = null;
            List<string> alertMulFile = new List<string>();
            ViewBag.alertMul = null;
            //检验文件为空的情况
            if (file == null)
            {
                alert = "请上传文件！";
                alertMulFile.Add(alert);
                ViewBag.alertMul = alertMulFile;
                return View(Staffs);
            }
            //文件不为空则读入数据
            else
            {
                file.SaveAs(fileAddr);
                DataTable table = new DataTable();
                table = RenderFromExcel(fileAddr);
                int tableRowsCount = table.Rows.Count;
                if (tableRowsCount <= 1)
                {
                    alert = "不能上传空文件！";
                    alertMulFile.Add(alert);
                    ViewBag.alertMul = alertMulFile;
                    return View(Staffs);
                }
                else
                {
                    //逐行验证、导入
                    for (int i = 1; i < tableRowsCount; i++)
                    {
                        List<string> alertMul = new List<string>();
                        alertMul = Validate(table, i);//验证该行
                        if (alertMul.Count() == 0)//验证通过，可以进入下一步
                        {
                            alertMul = ToDatabase(table, alertMul, i);
                            if (alertMul.Count() == 0)
                            {
                                alert = "上传成功";
                                ViewBag.alertMul = alertMul;
                            }
                            else
                            {
                                foreach (var ItemAlertMul in alertMul)
                                {
                                    alertMulFile.Add(ItemAlertMul);
                                }
                            }
                        }
                        else //验证不通过，增加提示信息
                        {
                            foreach (var ItemAlertMul in alertMul)
                            {
                                alertMulFile.Add(ItemAlertMul);
                            }
                            continue;
                        }
                    }
                    ViewBag.alertMul = alertMulFile;
                    return View(Staffs);
                }
            }
        }
        /// <summary>
        /// 初始化staff模型
        /// </summary>
        /// <returns></returns>
        public Staff staffModelInit(DataTable table, int i)
        {
            Staff staff = new Staff();
            staff.BillTypeName = table.Rows[i][0].ToString();
            var billProperties = (from p in db.BillProperties where p.BillSort == "21" select p).ToList();
            staff.BillTypeNumber = (from p in billProperties
                                    where
                                        p.TypeName == staff.BillTypeName || p.TypeFullName == staff.BillTypeName
                                    select p.Type).Single();

            string billNumberCreate = GenerateBillNumber(staff.BillTypeNumber);
            staff.BillNumber = GenerateBillNumber(staff.BillTypeNumber);
            staff.Name = table.Rows[i][1].ToString();
            string departmenttmp = table.Rows[i][2].ToString();
            staff.Department = (from d in db.Departments
                                where d.Name == departmenttmp
                                select d.DepartmentId).First();//Single()
            staff.Gender = table.Rows[i][3].ToString();
            staff.Position = table.Rows[i][4].ToString();
            if (staff.Position == "") { staff.Position = null; }
            staff.WorkProperty = table.Rows[i][5].ToString();
            if (staff.WorkProperty == "") { staff.WorkProperty = null; }
            //  staff.ClassOrderName = table.Rows[i][6].ToString();
            string classordertmp = table.Rows[i][6].ToString();
            staff.ClassOrder = (from w in db.Works where w.Name == classordertmp select w.Id).Single().ToString();
            staff.StaffNumber = table.Rows[i][7].ToString();
            var phyNum = table.Rows[i][8].ToString();
            if (phyNum == "")
            {
                string str = (from p in db.StaffBasicParams where p.Id == 3 select p.Value).Single();
                int pram = 0;
                int.TryParse(str, out pram);
                staff.PhysicalCardNumber = GetRandomCode(pram);
            }
            else
            {
                staff.PhysicalCardNumber = table.Rows[i][8].ToString();
            }
            var entrydate = table.Rows[i][9].ToString();
            if (entrydate == "")
                staff.Entrydate = DateTime.Today;
            else
                staff.Entrydate = DateTime.Parse(entrydate);
            staff.IndividualTelNumber = table.Rows[i][10].ToString();
            if (staff.IndividualTelNumber == "")
            {
                staff.IndividualTelNumber = null;
            }
            staff.IDCardNumber = table.Rows[i][11].ToString();
            if (staff.IDCardNumber == "")
            {
                staff.IDCardNumber = null;
            }
            staff.Nationality = table.Rows[i][12].ToString();
            if (staff.Nationality == "")
            {
                staff.Nationality = null;
            }
            staff.Nation = table.Rows[i][13].ToString();
            if (staff.Nation == "") { staff.Nation = null; }
            staff.NativePlace = table.Rows[i][14].ToString();
            if (staff.NativePlace == "") { staff.NativePlace = null; }
            var birth = table.Rows[i][15].ToString();
            if (birth == "")
                staff.BirthDate = null;
            else
                staff.BirthDate = DateTime.Parse(entrydate);
            staff.IdentificationType = table.Rows[i][16].ToString();
            if (staff.IdentificationType == "") { staff.IdentificationType = null; }
            staff.IdentificationNumber = table.Rows[i][17].ToString();
            if (staff.IdentificationNumber == "") { staff.IdentificationNumber = null; }
            staff.VisaOffice = table.Rows[i][18].ToString();
            if (staff.VisaOffice == "") { staff.VisaOffice = null; }
            var deadline = table.Rows[i][19].ToString();
            if (deadline == "")
            {
                staff.DeadlineDate = DateTime.MaxValue.Date;
                staff.LongTime = true;
            }
            else
            {
                staff.DeadlineDate = DateTime.Parse(deadline);
                staff.LongTime = false;
            }
            staff.MaritalStatus = table.Rows[i][20].ToString();
            if (staff.MaritalStatus == "") { staff.MaritalStatus = null; }

            staff.BankCardNumber = table.Rows[i][21].ToString();
            if (staff.BankCardNumber == "") { staff.BankCardNumber = null; }

            staff.HomeTelNumber = table.Rows[i][22].ToString();
            if (staff.HomeTelNumber == "") { staff.HomeTelNumber = null; }

            staff.Address = table.Rows[i][23].ToString();
            if (staff.Address == "") { staff.Address = null; }

            staff.Source = table.Rows[i][24].ToString();
            if (staff.Source == "") { staff.Source = null; }

            staff.WorkType = table.Rows[i][25].ToString();
            if (staff.WorkType == "") { staff.WorkType = null; }

            staff.HealthCondition = table.Rows[i][26].ToString();
            if (staff.HealthCondition == "") { staff.HealthCondition = null; }

            staff.Introducer = table.Rows[i][27].ToString();
            if (staff.Introducer == "") { staff.Introducer = null; }

            staff.EducationBackground = table.Rows[i][28].ToString();
            if (staff.EducationBackground == "") { staff.EducationBackground = null; }


            staff.GraduationSchool = table.Rows[i][29].ToString();
            if (staff.GraduationSchool == "") { staff.GraduationSchool = null; }

            staff.SchoolMajor = table.Rows[i][30].ToString();
            if (staff.SchoolMajor == "") { staff.SchoolMajor = null; }

            staff.Degree = table.Rows[i][31].ToString();
            if (staff.Degree == "") { staff.Degree = null; }

            staff.UrgencyContactMan = table.Rows[i][32].ToString();
            if (staff.UrgencyContactMan == "") { staff.UrgencyContactMan = null; }

            staff.UrgencyContactAddress = table.Rows[i][33].ToString();
            if (staff.UrgencyContactAddress == "") { staff.UrgencyContactAddress = null; }

            staff.UrgencyContactPhoneNumber = table.Rows[i][34].ToString();
            if (staff.UrgencyContactPhoneNumber == "") { staff.UrgencyContactPhoneNumber = null; }

            staff.QualifyingPeriodFull = table.Rows[i][35].ToString();
            if (staff.QualifyingPeriodFull == "") { staff.QualifyingPeriodFull = null; }

            var freecard = table.Rows[i][36].ToString();
            if (freecard == "")
                staff.FreeCard = false;
            else
                staff.FreeCard = true;

            var apply = table.Rows[i][37].ToString();
            if (apply == "")
                staff.ApplyOvertimeSwitch = false;
            else
                staff.ApplyOvertimeSwitch = true;

            staff.BindingCode = GetRandomCode(8);

            staff.JobState = table.Rows[i][39].ToString();
            if (staff.JobState == "") { staff.JobState = null; }


            staff.RecordTime = DateTime.Now;
            staff.RecordPerson = base.Name;
            return staff;
        }

        /// <summary>
        /// 验证唯一性+保存到数据库
        /// </summary>
        /// <param name="table"></param>
        /// <param name="alertMul"></param>
        /// <returns></returns>
        public List<string> ToDatabase(DataTable table, List<String> alertMul, int i)
        {
            int rows = i + 1;
            string alert = null;
            Staff staff = staffModelInit(table, i);
            //物理卡号
            var physicalCardNumber = (from p in db.Staffs
                                      where (p.PhysicalCardNumber == staff.PhysicalCardNumber && staff.PhysicalCardNumber != null)
                                      select p).ToList();


            //身份证号
            var identicationNumber = (from p in db.Staffs
                                      where (p.IdentificationNumber == staff.IdentificationNumber && staff.IdentificationNumber != null)
                                      select p).ToList();
            //身份证号不唯一
            if (identicationNumber.Count() != 0)
            {
                alert = "第" + rows + "行" + "证件号码已存在，请重新输入！";
                alertMul.Add(alert);
            }

            //物理卡号不唯一
            if (physicalCardNumber.Count() != 0)
            {
                alert = "第" + rows + "行" + "指纹卡号已存在，请重新输入！";
                alertMul.Add(alert);
            }

            if ((staff.IdentificationNumber == null || identicationNumber.Count() == 0) && (staff.PhysicalCardNumber == null || physicalCardNumber.Count() == 0))
            {
                if (staff.IdentificationType == "身份证")
                {
                    if (staff.IdentificationNumber != null)
                    {
                        Regex regex = new Regex(@"^(\d{15}|\d{18}|\d{17}[X|x])$");
                        if (regex.IsMatch(staff.IdentificationNumber) == false)
                        {
                            alert = "第" + rows + "行" + "身份证不合法，请重新输入！";
                            alertMul.Add(alert);
                        }
                    }
                }
                if (staff.LongTime == true)
                {
                    staff.DeadlineDate = DateTime.MaxValue.Date;
                }
                else
                {
                    if (staff.DeadlineDate == null)
                    {
                        alert = "第" + rows + "行" + "请输入过期时间！";
                        alertMul.Add(alert);
                    }
                }
                var tmp = db.Staffs.Where(p => p.StaffNumber.Equals(staff.StaffNumber)).ToList();
                if (tmp.Count != 0)//如果表中已存在该员工的工号
                {
                    alert = "第" + rows + "行" + "抱歉，该工号已经被注册！";
                    alertMul.Add(alert);
                }
                else
                {
                    var tempStaffArchive = db.StaffArchives.Where(sa => sa.IdenticationNumber.Equals(staff.IdentificationNumber)).ToList();//存在离职档案中

                    foreach (var t in tempStaffArchive)
                    {
                        if (t.BlackList == true)
                        {
                            alert = "第" + rows + "行" + "该员工以前在本公司工作过，离职后进入黑名单。建议不要录用！如果现在录用，请到离职档案管理修改。！";
                            alertMul.Add(alert);
                        }
                        if (t.ReApplyDate > staff.Entrydate)
                        {
                            alert = "第" + rows + "行" + "该员工以前在本公司工作过，再次入职的日期是" + t.ReApplyDate + "以后！如果现在录用，请到离职档案管理修改。";
                            alertMul.Add(alert);
                        }
                    }

                    byte status = AuditApplicationStaff(staff);
                    staff.AuditStatus = status;
                    db.Staffs.Add(staff);
                    if (alertMul.Count == 0)
                    {
                        try
                        {
                            db.SaveChanges();

                        }
                        catch (DbEntityValidationException e)
                        {
                            throw e;
                        }


                        if (status == 7) //没有找到该单据的审批模板
                        {
                            alert = "第" + rows + "行" + "没有找到该单据的审批模板";
                            alertMul.Add(alert);
                        }
                        else if (status == 3)//审核通过
                        {
                            SystemDbContext sysdb = new SystemDbContext();
                            /*BindCodes*/
                            var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == this.CompanyId && p.StaffNumber == staff.StaffNumber) select p).ToList();
                            if (Ucount.Count == 0)
                            {//木有该员工
                                BindCode user = new BindCode();
                                string CompanyDbName = "Bonsaii" + this.CompanyId;
                                user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                                user.CompanyId = this.CompanyId;
                                user.StaffNumber = staff.StaffNumber;
                                user.RealName = staff.Name;
                                user.BindingCode = staff.BindingCode;
                                user.Phone = staff.IndividualTelNumber;
                                user.BindTag = false;
                                user.LastTime = DateTime.Now;
                                user.IsAvail = true;
                                sysdb.BindCodes.Add(user);
                                sysdb.SaveChanges();
                            }

                        }
                    }
                    return alertMul;
                }//else
            }
            return alertMul;
        }

        /// <summary>
        /// 验证导入的数据是否合理
        /// </summary>
        public List<string> Validate(DataTable table, int i)
        {
            string alert = null;
            List<string> alertMsg = new List<string>();
            int ii = i + 1;
            var departmentName = (from d in db.Departments //部门
                                  select d.Name).ToList();
            var workName = (from w in db.Works  //班次
                            select w.Name).ToList();
            try
            {
                string temp = table.Rows[i][0].ToString();
                var bType = (from p in db.BillProperties where p.BillSort == "21" && (temp == p.TypeName || temp == p.TypeFullName) select p.BillSort).Single();
            }
            catch
            {
                alert = "第" + ii + "行单据类型不存在！";
                alertMsg.Add(alert);
            }
            //2、姓名
            if (table.Rows[i][1].ToString() == "")
            {
                alert = "第" + ii + "行姓名不能为空！";
                alertMsg.Add(alert);
            }
            //3、性别
            if (table.Rows[i][3].ToString() == "")
            {
                alert = "第" + ii + "行性别为空，请重新输入！";
                alertMsg.Add(alert);
            }
            else
            {
                if (table.Rows[i][3].ToString() != "男")
                {
                    if (table.Rows[i][3].ToString() != "女")
                    {
                        alert = "第" + ii + "行性别不合法！";
                        alertMsg.Add(alert);
                    }
                }
            }
            //4、班次
            if (table.Rows[i][6].ToString() == "")
            {
                alert = "第" + ii + "行班次不能为空！";
                alertMsg.Add(alert);
            }
            else
                if (workName.Contains(table.Rows[i][6].ToString()) == false)
                {
                    alert = "第" + ii + "行不存在该班次！";
                    alertMsg.Add(alert);
                }
            //5、部门
            if (table.Rows[i][2].ToString() == "")
            {
                alert = "第" + ii + "行部门不能为空！";
                alertMsg.Add(alert);
            }
            else
            {
                if (departmentName.Contains(table.Rows[i][2].ToString()) == false)
                {
                    alert = "第" + ii + "行部门未找到！";
                    alertMsg.Add(alert);
                }
                //如果存在部门：若工号为空，生成工号；如果工号不为空，那么就使用该工号
                else
                {
                    if (table.Rows[i][7].ToString() == "")
                    {
                        var Name = table.Rows[i][2].ToString();

                        var department = (from p in db.Departments
                                          where p.Name == Name
                                          select p).ToList();
                        int count = department.Count();
                        if (count == 0)
                        {
                            alert = "第" + ii + "行无法生成工号！";
                            alertMsg.Add(alert);
                        }
                        else
                        {
                            table.Rows[i][7] = GenerateStaffParam("0001", department.First().DepartmentAbbr);
                        }
                        //db.Departments.Where(d => d.Name.Equals(departmentName)).SingleOrDefault();
                    }
                }
            }
            if (table.Rows[i][8].ToString() == "")
            {
                string str = (from p in db.StaffBasicParams where p.Id == 3 select p.Value).Single();
                int pram = 0;
                int.TryParse(str, out pram);
                table.Rows[i][8] = GetRandomCode(pram);
                //alert = "第" + ii + "行指纹卡号不能为空！";
                //alertMsg.Add(alert);
            }

            if (table.Rows[i][10].ToString() == "")
            {
                alert = "第" + ii + "行个人手机号不能为空！";
                alertMsg.Add(alert);
            }
            else
            {
                Regex mobileRegex = new Regex(@"^\d{11}$");
                var phone = table.Rows[i][10].ToString();
                if (mobileRegex.IsMatch(phone) == false)
                {
                    alert = "第" + ii + "行请输入正确的11位手机号码！";
                    alertMsg.Add(alert);
                }
            }
            string tmp = table.Rows[i][17].ToString();
            if (table.Rows[i][16].ToString() == "身份证")
            {
                Regex idenNumTegex = new Regex("^(\\d{15}|\\d{18}|\\d{17}[x])$");
                if (idenNumTegex.IsMatch(tmp) == false)
                {
                    alert = "第" + ii + "行请输入正确的身份证号码！";
                    alertMsg.Add(alert);
                }
            }

            if (table.Rows[i][22].ToString() != "")
            {
                Regex homeTelTegex = new Regex(@"^\d{11}$|0[0-9]{2,3}-[0-9]{7,8}(-[0-9]{1,4})?$");
                if (homeTelTegex.IsMatch(table.Rows[i][22].ToString()) == false)
                {
                    alert = "第" + ii + "行请输入正确的家庭电话号码！";
                    alertMsg.Add(alert);
                }
            }

            if (table.Rows[i][34].ToString() != "")
            {
                Regex homeTelTegex = new Regex(@"^\d{11}$|0[0-9]{2,3}-[0-9]{7,8}(-[0-9]{1,4})?$");
                if (homeTelTegex.IsMatch(table.Rows[i][34].ToString()) == false)
                {
                    alert = "第" + ii + "行请输入正确的紧急联系人号码！";
                    alertMsg.Add(alert);
                }
            }

            return alertMsg;
        }
        /// <summary>
        /// Index的post方法
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(HttpPostedFileBase file)
        //{
        //    List<Staff> Staffs = (from p in db.Staffs orderby p.BillNumber where p.ArchiveTag == false select p).ToList();

        //    foreach (Staff tmp in Staffs)
        //    {
        //        tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
        //        tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
        //    }
        //    string alert = null;
        //    List<string> alertMul = new List<string>();
        //    if (file == null)
        //    {
        //        // alert = "没有文件！";
        //        //  ViewBag.alert = alert;
        //        alert = "没有文件！";
        //        alertMul.Add(alert);
        //        ViewBag.alertMul = alertMul;
        //        return View(Staffs);
        //    }
        //    var fileAddr = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
        //    try
        //    {
        //        file.SaveAs(fileAddr);
        //        DataTable table = new DataTable();
        //        table = RenderFromExcel(fileAddr);

        //        alertMul = Validation(table);

        //        if (alertMul.Count == 0)
        //        {
        //            alertMul = toDataBase(table, alertMul);
        //            if (alertMul.Count == 0)
        //            {
        //                alert = "导入成功！";
        //                alertMul.Add(alert);
        //            }
        //            ViewBag.alertMul = alertMul;
        //        }
        //        else
        //        {
        //            ViewBag.alertMul = alertMul;
        //            return View(Staffs);
        //        }

        //        return View(Staffs);
        //    }
        //    catch (Exception e)
        //    {

        //        alert = "上传异常！";
        //        alertMul.Add(alert);
        //        ViewBag.alertMul = alertMul;
        //        return View(Staffs);
        //    }
        //}
        /// <summary>
        /// 验证上传的数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        //public list<string> validation(datatable table)
        //{
        //    string alert = null;
        //    list<string> alertmsg = new list<string>();
        //    //准备工作
        //    //var billtype = (from b in db.billproperties   //单据类型
        //    //                where b.billsort == "21"
        //    //                select new
        //    //                {
        //    //                    typename = b.typename,
        //    //                    typefullname = b.typefullname
        //    //                }).tolist();
        //    var departmentname = (from d in db.departments //部门
        //                          select d.name).tolist();
        //    var workname = (from w in db.works  //班次
        //                    select w.name).tolist();
        //    //逐行验证
        //    for (int i = 1; i < table.rows.count; i++)
        //    {
        //        int ii = i + 1;
        //        //1、单据类型
        //        //var btype = (from t in billtype
        //        //             select t.typename == temp || t.typefullname == temp).tolist();
        //        try
        //        {
        //            string temp = table.rows[i][0].tostring();
        //            var btype = (from p in db.billproperties where p.billsort == "21" && (temp == p.typename || temp == p.typefullname) select p.billsort).single();
        //        }
        //        catch {
        //            alert = "第" + ii+ "行单据类型出错！";
        //            alertmsg.add(alert);
        //        }
        //        //2、姓名
        //        if (table.rows[i][1].tostring() == "")
        //        {
        //            alert = "第" + ii + "行姓名不能为空！";
        //            alertmsg.add(alert);
        //        }
        //        //3、性别
        //        if (table.rows[i][3].tostring() == "")
        //        {
        //            alert = "第" + ii + "行性别为空，请重新输入！";
        //            alertmsg.add(alert);
        //        }
        //        else
        //        {
        //            if (table.rows[i][3].tostring() != "男")
        //            {
        //                if (table.rows[i][3].tostring() != "女")
        //                {
        //                    alert = "第" + ii + "行性别不合法！";
        //                    alertmsg.add(alert);
        //                }
        //            }
        //        }
        //        //4、班次
        //        if (table.rows[i][6].tostring() == "")
        //        {
        //            alert = "第" + ii + "行班次不能为空！";
        //            alertmsg.add(alert);
        //        }
        //        else
        //        if (workname.contains(table.rows[i][6].tostring()) == false)
        //        {
        //            alert = "第" + ii + "行班次未找到！";
        //            alertmsg.add(alert);
        //        }
        //        //5、部门
        //        if (table.rows[i][2].tostring() == "")
        //        {
        //            alert = "第" + ii + "行部门不能为空！";
        //            alertmsg.add(alert);
        //        }
        //        else
        //        {
        //            if (departmentname.contains(table.rows[i][2].tostring()) == false)
        //            {
        //                alert = "第" + i + "行部门未找到！";
        //                alertmsg.add(alert);
        //            }
        //            //如果存在部门：若工号为空，生成工号；如果工号不为空，那么就使用该工号
        //            else
        //            {
        //                if (table.rows[i][7].tostring() == "")
        //                {
        //                    var name = table.rows[i][2].tostring();

        //                    var department = (from p in db.departments
        //                                      where p.name == name
        //                                      select p).tolist();
        //                    int count = department.count();
        //                    if (count == 0)
        //                    {
        //                        alert = "第" + i + "行无法生成工号！";
        //                        alertmsg.add(alert);
        //                    }
        //                    if (count == 1)
        //                    {
        //                        table.rows[i][7] = generatestaffparam("0001", department.first().departmentabbr);
        //                    }
        //                    //db.departments.where(d => d.name.equals(departmentname)).singleordefault();
        //                }
        //            }
        //        }
        //        if (table.rows[i][8].tostring() == "")
        //        {
        //            alert = "第" + i + "行指纹卡号不能为空！";
        //            alertmsg.add(alert);
        //        }

        //        if (table.rows[i][10].tostring() == "")
        //        {
        //            alert = "第" + i + "行个人手机号不能为空！";
        //            alertmsg.add(alert);
        //        }
        //        else
        //        {
        //            regex mobileregex = new regex(@"^(\d{11})$");
        //            var phone = table.rows[i][10].tostring();
        //            if (mobileregex.ismatch(phone) == false)
        //            {
        //                alert = "第" + i + "行请输入正确的11位手机号码！";
        //                alertmsg.add(alert);
        //            }
        //        }
        //        string tmp = table.rows[i][17].tostring();
        //        if (tmp == "身份证")
        //        {
        //            regex idennumtegex = new regex("^(\\d{15}|\\d{18}|\\d{17}[x])$");
        //            if (idennumtegex.ismatch(tmp) == false)
        //            {
        //                alert = "第" + i + "行请输入正确的身份证号码！";
        //                alertmsg.add(alert);
        //            }
        //        }

        //        if (table.rows[i][22].tostring() != "")
        //        {
        //            regex hometeltegex = new regex("^([\\d-+]*)$");
        //            if (hometeltegex.ismatch(table.rows[i][22].tostring()) == false)
        //            {
        //                alert = "第" + i + "行请输入正确的家庭电话号码！";
        //                alertmsg.add(alert);
        //            }
        //        }

        //    }
        //    return alertmsg;
        //}
        //public List<string> ToDataBase(Datatable table, List<String> alertmul)
        //{
        //    string alert = null;
        //    try
        //    {
        //        Systemdbcontext sysdb = new systemdbcontext();
        //        for (int i = 1; i < table.rows.count; i++)
        //        {
        //            staff b = new staff();

        //            b.billtypename = table.rows[i][0].tostring();
        //            b.billtypenumber = (from bp in db.billproperties
        //                                where (bp.typename == b.billtypename) || (bp.typefullname == b.billtypename)
        //                                select bp.type).singleordefault();
        //            b.billnumber = generatebillnumber(b.billtypenumber);
        //            b.name = table.rows[i][1].tostring();
        //            string departmenttmp = table.rows[i][2].tostring();
        //            b.department = (from d in db.departments
        //                            where d.name == departmenttmp
        //                            select d.departmentid).singleordefault();
        //            b.gender = table.rows[i][3].tostring();
        //            b.position = table.rows[i][4].tostring();
        //            b.workproperty = table.rows[i][5].tostring();
        //            b.classordername = table.rows[i][6].tostring();
        //            string classordertmp = table.rows[i][6].tostring();
        //            b.classorder = (from w in db.works
        //                            where w.name == classordertmp
        //                            select w.id).singleordefault().tostring();

        //            b.staffnumber = table.rows[i][7].tostring();
        //            var staffnum = (from n in db.staffs
        //                            where (n.staffnumber == b.staffnumber && b.staffnumber != "")
        //                            select n).tolist();
        //            if (staffnum.count() != 0)
        //            {
        //                alert = "第" + i + "行该工号已被占用，请重新输入！";
        //                alertmul.add(alert);
        //            }

        //            b.physicalcardnumber = table.rows[i][8].tostring();
        //            物理卡号
        //            var physicalcardnumber = (from p in db.staffs
        //                                      where (p.physicalcardnumber == b.physicalcardnumber && b.physicalcardnumber != "")
        //                                      select p).tolist();
        //            物理卡号不唯一
        //            if (physicalcardnumber.count() != 0)
        //            {
        //                alert = "第" + i + "行该物理卡号已存在，请重新输入！";
        //                alertmul.add(alert);
        //            }
        //            var entrydate = table.rows[i][9].tostring();
        //            if (entrydate == "")
        //                b.entrydate = null;
        //            else
        //                b.entrydate = datetime.parse(entrydate);

        //            b.individualtelnumber = table.rows[i][10].tostring();

        //            var phonenumber = (from p in db.staffs
        //                               where (p.individualtelnumber == b.individualtelnumber && b.individualtelnumber != "")
        //                               select p).tolist();
        //            if (phonenumber.count() != 0)
        //            {
        //                alert = "第" + i + "行该手机号已经被占用，请重新输入！"; 
        //                alertmul.add(alert);
        //            }

        //            b.idcardnumber = table.rows[i][11].tostring();
        //            var idcard = (from d in db.staffs
        //                          where (d.idcardnumber == b.idcardnumber && b.idcardnumber != "")
        //                          select d).tolist();
        //            if (idcard.count() != 0)
        //            {
        //                alert = "第" + i + "行该id卡号已被占用，请重新输入！";
        //                alertmul.add(alert);
        //            }

        //            b.nationality = table.rows[i][12].tostring();
        //            b.nation = table.rows[i][13].tostring();
        //            b.nativeplace = table.rows[i][14].tostring();

        //            var birth = table.rows[i][15].tostring();
        //            if (birth == "")
        //                b.birthdate = null;
        //            else
        //                b.birthdate = datetime.parse(birth);

        //            b.identificationtype = table.rows[i][16].tostring();
        //            b.identificationnumber = table.rows[i][17].tostring();
        //            身份证号
        //            var identicationnumber = (from p in db.staffs
        //                                      where (p.identificationnumber == b.identificationnumber && b.identificationnumber != "")
        //                                      select p).tolist();
        //            身份证号不唯一
        //            if (identicationnumber.count() != 0)
        //            {

        //                alert = "第" + i + "行该身份证已存在，请重新输入！";
        //                alertmul.add(alert);
        //            }
        //            b.visaoffice = table.rows[i][18].tostring();

        //            var deadline = table.rows[i][19].tostring();
        //            if (deadline == "")
        //                b.deadlinedate = datetime.maxvalue.date;
        //            else
        //                 b.deadlinedate = (nullable<datetime>)table.rows[i][19];
        //                b.deadlinedate = datetime.parse(deadline);

        //            b.maritalstatus = table.rows[i][20].tostring();
        //            b.bankcardnumber = table.rows[i][21].tostring();
        //            b.hometelnumber = table.rows[i][22].tostring();
        //            b.address = table.rows[i][23].tostring();
        //            b.source = table.rows[i][24].tostring();
        //            b.worktype = table.rows[i][25].tostring();
        //            b.healthcondition = table.rows[i][26].tostring();
        //            b.introducer = table.rows[i][27].tostring();
        //            b.educationbackground = table.rows[i][28].tostring();
        //            b.graduationschool = table.rows[i][29].tostring();
        //            b.schoolmajor = table.rows[i][30].tostring();
        //            b.degree = table.rows[i][31].tostring();
        //            b.urgencycontactman = table.rows[i][32].tostring();
        //            b.urgencycontactaddress = table.rows[i][33].tostring();
        //            b.urgencycontactphonenumber = table.rows[i][34].tostring();
        //            b.qualifyingperiodfull = table.rows[i][35].tostring();
        //             b.freecard = (bool)table.rows[i][36];
        //            if (table.rows[i][36].tostring() == "")
        //                b.freecard = false;
        //            else
        //                b.freecard = true;
        //             b.applyovertimeswitch = (bool)table.rows[i][37];
        //            if (table.rows[i][37].tostring() == "")
        //                b.applyovertimeswitch = false;
        //            else
        //            {
        //                b.applyovertimeswitch = true;
        //            }

        //            b.bindingcode = getrandomcode(8);
        //            b.jobstate = table.rows[i][39].tostring();
        //            byte status = auditapplicationstaff(b);
        //            b.auditstatus = status;
        //            db.staffs.add(b);
        //            db.savechanges();
        //            if (status == 3)//审核通过
        //            {
        //                /*bindcodes*/
        //                var ucount = (from p in sysdb.bindcodes where (p.companyid == this.companyid && p.staffnumber == b.staffnumber) select p).tolist();
        //                if (ucount.count == 0)
        //                {//木有该员工
        //                    bindcode user = new bindcode();
        //                    string companydbname = "bonsaii" + user.companyid;
        //                    user.connectionstring = configurationmanager.appsettings["userdbconnectionstring"] + companydbname + ";";   //"data source = localhost,1433;network library = dbmssocn;initial catalog = " + companydbname + ";user id = test;password = admin;";
        //                    user.companyid = this.companyid;
        //                    user.staffnumber = b.staffnumber;
        //                    user.realname = b.name;
        //                    user.bindingcode = b.bindingcode;
        //                    user.phone = b.individualtelnumber;
        //                    user.bindtag = false;
        //                    user.lasttime = datetime.now;
        //                    user.isavail = true;
        //                    sysdb.bindcodes.add(user);
        //                    sysdb.savechanges();

        //                }
        //            }
        //        }
        //        if (alertmul.count == 0)
        //        {
        //            sysdb.savechanges();
        //            db.savechanges();
        //        }
        //        return alertmul;
        //    }
        //    catch (exception e)
        //    {
        //        alert = "数据导入错误！"; alertmul.add(alert);
        //        return alertmul;
        //    }
        //}
        /// <summary>
        /// id是Staffs表的主键;找到StaffChanges的
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Submit"></param>
        /// <returns></returns>
        public ActionResult MulDetails(int? id, int? Submit)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.staffId = id;
                Staff staff = db.Staffs.Find(id);
                //var staffChangeId = (from p in db.StaffChanges where p.StaffNumber == staff.StaffNumber select p).ToList();
                ViewBag.staffNumber = staff.StaffNumber;

            }
            return View();
        }
        // GET: Staff/Details/5
        public ActionResult Details(int? id, int? Submit)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            //var staffDepartment = (from s in db.Staffs
            //                       join d in db.Departments on s.Department equals d.DepartmentId
            //                        into gc    /*左联：显示所有员工表的字段*/
            //                       from d in gc.DefaultIfEmpty()
            //                       where s.Department == staff.Department
            //                       select new StaffViewModel { Number = s.Number, Department = s.Department, Value = d.Name }).ToList();
            //ViewBag.staffDepartment = staffDepartment;
            /*查找员工基本信息表预留字段(value)*/
            var fieldValueList = (from sr in db.StaffReserves
                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                  where staff.Number == sr.Number && rf.Status == true
                                  select new StaffViewModel { Number = sr.Number, Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            var classOrders = Generate.GetWorks(base.ConnectionString);

            staff.DepartmentName = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).ToList().FirstOrDefault();
            staff.ClassOrderName = (from classOrder in classOrders where classOrder.Value == staff.ClassOrder select classOrder.Text).ToList().FirstOrDefault();
            staff.AuditStatusName = db.States.Find(staff.AuditStatus).Description;

            if (Submit == 1)
            {
                //提交审批
                byte status = AuditApplicationStaff(staff);
                //需要对原表做出的修改
                staff.AuditStatus = status;
                //没有找到该单据的审批模板
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    return View(staff);
                }
                else if (status == 3)//审核通过
                {
                    //需要对原表做出的修改
                    staff.AuditStatus = status;
                    SystemDbContext sysdb = new SystemDbContext();
                    /*BindCodes*/
                    var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == this.CompanyId && p.StaffNumber == staff.StaffNumber) select p).ToList();
                    if (Ucount.Count == 0)
                    {//木有该员工
                        BindCode user = new BindCode();
                        string CompanyDbName = "Bonsaii" + this.CompanyId;
                        user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                        user.CompanyId = this.CompanyId;
                        user.StaffNumber = staff.StaffNumber;
                        user.RealName = staff.Name;
                        user.BindingCode = staff.BindingCode;
                        user.Phone = staff.IndividualTelNumber;
                        user.BindTag = false;
                        user.LastTime = DateTime.Now;
                        user.IsAvail = true;
                        sysdb.SaveChanges();
                        // db.SaveChanges();
                    }
                    // else 
                    // {
                    // db.SaveChanges();
                    // }
                    //else if (Ucount.Count == 1)
                    //{
                    //    ModelState.AddModelError("", "需要更换工号！");
                    //    return View(staff);
                    //}
                    //else
                    //{
                    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    //}
                    //增加自定义字段
                    AddReserveFields(staff);
                }
                else
                {
                    //db.SaveChanges();
                    AddReserveFields(staff);
                }
            }
            return View(staff);
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public FileContentResult GetImage(int Number)
        {
            Staff s = db.Staffs.FirstOrDefault(p => p.Number == Number);
            if (s != null)
            {
                return File(s.Head, s.HeadType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// AddReserveFields 增加自定义字段
        /// </summary>
        /// <param name="staff"></param>
        public void AddReserveFields(Staff staff)
        {
            db.SaveChanges();
            /*查找员工基本信息表预留字段(name)*/
            var fieldList1 = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                              on p.TableNameId equals q.Id
                              where q.TableName == "Staffs" && p.Status == true
                              select p).ToList();
            ViewBag.fieldList = fieldList1;

            /*遍历，保存员工基本信息预留字段*/
            foreach (var temp in fieldList1)
            {
                StaffReserve sr = new StaffReserve();
                sr.Number = staff.Number;
                sr.FieldId = temp.Id;
                sr.Value = Request[temp.FieldName];
                /*占位，为了在Index中显示整齐的格式*/
                if (sr.Value == null) sr.Value = " ";
                db.StaffReserves.Add(sr);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 初始化Create方法
        /// </summary>
        public static void IniteCreate()
        {

        }

        [Authorize(Roles = "Admin,Staff_Create")]
        // GET: Staff/Create
        public ActionResult Create()
        {

            List<SelectListItem> billNumber = db.BillProperties.Where(b => b.BillSort.Equals("21")).ToList().Select(c => new SelectListItem
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
            /*查找员工基本信息表预留字段*/
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                                 on p.TableNameId equals q.Id
                             where q.TableName == "Staffs" && p.Status == true
                             select p).ToList();
            ViewBag.fieldList = fieldList;
            ViewBag.WorksList = Generate.GetWorks(base.ConnectionString);
            ViewBag.today = DateTime.Today;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public FileContentResult ImageTmp(Staff staff)
        {
            //Staff s = db.Staffs.FirstOrDefault(p => p.Number == Number);
            //staff = 
            //if (s != null)
            //{
            //    return File(staff.Head, staff.HeadType);//File方法直接将二进制转化为指定类型了。
            //}
            //else
            //{
            return null;
            ////}
        }
        // POST: Staff/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Staff staff, HttpPostedFileBase FileData)
        {

            ViewBag.WorksList = Generate.GetWorks(base.ConnectionString);
            /*查找员工基本信息表预留字段*/
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                                on p.TableNameId equals q.Id
                             where q.TableName == "Staffs" && p.Status == true
                             select p).ToList();
            ViewBag.fieldList = fieldList;

            if (FileData != null)
            {
                staff.HeadType = FileData.ContentType;//获取图片类型
                staff.Head = new byte[FileData.ContentLength];//新建一个长度等于图片大小的二进制地址
                FileData.InputStream.Read(staff.Head, 0, FileData.ContentLength);//将image读取到Logo中
            }
            //查看员工列表的部门人数
            //var staffsSameDepartment = (from p in db.Staffs where p.Department == staff.Department select p).ToList();
            //var StaffSize = (from p in db.Departments where p.DepartmentId == staff.Department select p.StaffSize).ToList().FirstOrDefault();
            //if (staffsSameDepartment.Count() >= StaffSize) {
            //    ModelState.AddModelError("", "该部门人数已满！请修改部门编制人数或选择其他部门。");
            //    return View(staff);
            //}

            //物理卡号
            var physicalCardNumber = (from p in db.Staffs
                                      where (p.PhysicalCardNumber == staff.PhysicalCardNumber && staff.PhysicalCardNumber != null)
                                      select p).ToList();
            //身份证号
            var identicationNumber = (from p in db.Staffs
                                      where (p.IdentificationNumber == staff.IdentificationNumber && staff.IdentificationNumber != null)
                                      select p).ToList();
            //身份证号不唯一
            if (identicationNumber.Count() != 0)
            {
                ModelState.AddModelError("", "该证件号码已存在，请重新输入！");
                return View(staff);
            }
            //物理卡号不唯一
            if (physicalCardNumber.Count() != 0)
            {
                ModelState.AddModelError("", "该指纹卡号已存在，请重新输入！");
                return View(staff);
            }
            ///单号不能重复
            //var billNumber = (from p in db.Staffs where p.BillNumber == staff.BillNumber && staff.BillNumber != null select p).ToList();
            //if (billNumber.Count() != 0) { ModelState.AddModelError("", "该单号已存在，请重新生成！"); return View(staff); }

            if ((staff.IdentificationNumber == null || identicationNumber.Count() == 0) && (staff.PhysicalCardNumber == null || physicalCardNumber.Count() == 0))
            {
                if (ModelState.IsValid)
                {
                    if (staff.IdentificationType == "身份证")
                    {
                        if (staff.IdentificationNumber != null)
                        {
                            Regex regex = new Regex(@"^(\d{15}|\d{18}|\d{17}[X|x])$");
                            if (regex.IsMatch(staff.IdentificationNumber) == false)
                            {
                                ModelState.AddModelError("", "身份证不合法，请重新输入！");
                                return View(staff);
                            }
                        }
                    }
                    if (staff.LongTime == true)
                    {
                        staff.DeadlineDate = DateTime.MaxValue.Date;
                    }
                    //else
                    //{
                    //    if (staff.DeadlineDate == null)
                    //    {

                    //ModelState.AddModelError("", "请输入过期时间！");
                    //return View(staff);
                    //    }
                    //}
                    var tmp = db.Staffs.Where(p => p.StaffNumber.Equals(staff.StaffNumber)).ToList();
                    if (tmp.Count != 0)//如果表中已存在该员工的工号
                    {
                        List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
                        {
                            Value = c.Name,//保存的值
                            Text = c.Name//显示的值
                        }).ToList();
                        ViewBag.List = item;

                        ModelState.AddModelError("", "抱歉，该工号已经被注册！");

                        return View(staff);
                    }
                    else
                    {
                        var tempStaffArchive = db.StaffArchives.Where(sa => sa.IdenticationNumber.Equals(staff.IdentificationNumber)).ToList();//存在离职档案中

                        foreach (var t in tempStaffArchive)
                        {
                            if (t.BlackList == true)
                            {
                                ModelState.AddModelError("", "该员工以前在本公司工作过，离职后进入黑名单。建议不要录用！如果现在录用，请到离职档案管理修改。");
                                return View(staff);
                            }
                            if (t.ReApplyDate > staff.Entrydate)
                            {
                                ModelState.AddModelError("", "该员工以前在本公司工作过，再次入职的日期是" + t.ReApplyDate + "以后！如果现在录用，请到离职档案管理修改。");
                                return View(staff);
                            }
                        }


                        staff.RecordTime = DateTime.Now;
                        staff.RecordPerson = base.Name;
                        staff.AuditStatus = 0;
                        //查找单据类别名称
                        var billTypeName = (from p in db.BillProperties where p.Type == staff.BillTypeNumber select p.TypeName).ToList().FirstOrDefault();
                        staff.BillTypeName = billTypeName;
                        /*员工绑定码*/
                        staff.BindingCode = GetRandomCode(8);//8位随机
                        staff.BillNumber = GenerateBillNumber(staff.BillTypeNumber);
                        //生成物理卡号
                        //string str = (from p in db.StaffBasicParams where p.Id == 3 select p.Value).Single();
                        //int pram = 0;
                        //int.TryParse(str, out pram);
                        //staff.PhysicalCardNumber = GetRandomCode(pram);

                        byte status = AuditApplicationStaff(staff);
                        staff.AuditStatus = status;
                        /*Step1：先保存员工固定字段*/
                        db.Staffs.Add(staff);
                        /*审核通过*/
                        //提交审核
                        AddReserveFields(staff);
                        if (status == 7) //没有找到该单据的审批模板
                        {
                            ViewBag.alertMessage = true;
                            return View(staff);
                        }
                        else if (status == 3)//审核通过
                        {
                            //为员工添加默认排班，默认是添加当前月份的排班
                            AddDefaultWork(staff.StaffNumber, Int32.Parse(staff.ClassOrder), staff.Department);

                            SystemDbContext sysdb = new SystemDbContext();
                            /*BindCodes*/
                            var Ucount = (from p in sysdb.BindCodes where (p.CompanyId == this.CompanyId && p.StaffNumber == staff.StaffNumber) select p).ToList();
                            if (Ucount.Count == 0)
                            {   //木有该员工
                                BindCode user = new BindCode();
                                string CompanyDbName = "Bonsaii" + this.CompanyId;
                                user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";";   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                                user.CompanyId = this.CompanyId;
                                user.StaffNumber = staff.StaffNumber;
                                user.RealName = staff.Name;
                                user.BindingCode = staff.BindingCode;
                                user.Phone = staff.IndividualTelNumber;
                                user.BindTag = false;
                                user.LastTime = DateTime.Now;
                                user.IsAvail = true;
                                sysdb.BindCodes.Add(user);
                                sysdb.SaveChanges();
                                // db.SaveChanges();
                            }
                            // else 
                            // {
                            // db.SaveChanges();
                            // }
                            //else if (Ucount.Count == 1)
                            //{
                            //    ModelState.AddModelError("", "需要更换工号！");
                            //    return View(staff);
                            //}
                            //else
                            //{
                            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            //}
                            //增加自定义字段

                        }
                        return RedirectToAction("Index");
                    }//else
                }
            }//if
            else
            {
                var temp = db.StaffArchives.Where(sa => sa.IdenticationNumber.Equals(staff.IdentificationNumber)).ToList();
                foreach (var t in temp)
                {
                    if (t.BlackList == true)
                    {
                        ModelState.AddModelError("", "该员工以前在本公司工作过，离职后进入黑名单。建议不要录用！如果现在录用，请到离职档案管理修改。");
                        return View(staff);
                    }
                    if (t.ReApplyDate > staff.Entrydate)
                    {
                        ModelState.AddModelError("", "该员工以前在本公司工作过，再次入职的日期是" + t.ReApplyDate + "以后！如果现在录用，请到离职档案管理修改。");
                        return View(staff);
                    }
                }
            }
            if (FileData != null)
            {
                ViewBag.photo = FileData;//File(staff.Head, staff.HeadType).FileContents;
            }
            else
            {
                ViewBag.photo = null;
            }
            return View(staff);//File方法直接将二进制转化为指定类型了。);
        }





        [Authorize(Roles = "Admin,Staff_Edit")]
        // GET: Staff/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);

            //实现下拉列表
            var item = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.DepartmentId,//保存的值
                Text = c.Name//显示的值
            }).ToList();

            //增加一个null选项
            SelectListItem i = new SelectListItem();
            i.Value = "";
            i.Text = "-请选择-";
            i.Selected = true;
            item.Add(i);

            //传值到页面
            ViewBag.List = item;

            if (staff == null)
            {
                return HttpNotFound();
            }
            ////1.部门信息
            //List<SelectListItem> department = db.Departments.ToList().Select(c => new SelectListItem
            //{
            //    Value = c.DepartmentId,//保存的值
            //    Text = c.Name,//显示的值
            //}).ToList();
            ////增加一个null选项
            //SelectListItem ii = new SelectListItem();
            //ii.Value = " ";
            //ii.Text = "-请选择-";
            //ii.Selected = true;
            //department.Add(ii);
            //ViewBag.departmentList = department;//ViewBag.departmentList里的departmentList名称与字段名称不能一样（字母大小写也不行）。不然Selected属性会失效的。
            var classOrders = Generate.GetWorks(base.ConnectionString);

            /*查找员工基本信息表预留字段*/
            var fieldList = (from sr in db.StaffReserves
                             join rf in db.ReserveFields on sr.FieldId equals rf.Id
                             where staff.Number == sr.Number && rf.Status == true
                             select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldList = fieldList;
            return View(staff);
        }

        // POST: Staff/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。

        [Authorize(Roles = "Admin,Staff_Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Staff staff, HttpPostedFileBase FileData)
        {
            if (ModelState.IsValid)
            {
                //查看员工列表的部门人数
                var staffsSameDepartment = (from p in db.Staffs where p.Department == staff.Department && p.Number != staff.Number select p).ToList();
                var StaffSize = (from p in db.Departments where p.DepartmentId == staff.Department select p.StaffSize).ToList().FirstOrDefault();
                if (staffsSameDepartment.Count() >= StaffSize)
                {
                    /*查找员工基本信息表预留字段*/
                    var fieldList = (from sr in db.StaffReserves
                                     join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                     where staff.Number == sr.Number && rf.Status == true
                                     select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
                    ViewBag.fieldList = fieldList;
                    ModelState.AddModelError("", "该部门人数已满！请修改部门编制人数或选择其他部门。");
                    return View(staff);
                }

                //物理卡号
                var physicalCardNumber = (from p in db.Staffs
                                          where (p.PhysicalCardNumber == staff.PhysicalCardNumber && staff.PhysicalCardNumber != null && staff.Number != p.Number)
                                          select p).ToList();
                //身份证号
                var identicationNumber = (from p in db.Staffs
                                          where (p.IdentificationNumber == staff.IdentificationNumber && staff.IdentificationNumber != null && staff.Number != p.Number)
                                          select p).ToList();
                //身份证号不唯一
                if (identicationNumber.Count() != 0)
                {
                    /*查找员工基本信息表预留字段*/
                    var fieldList = (from sr in db.StaffReserves
                                     join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                     where staff.Number == sr.Number && rf.Status == true
                                     select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
                    ViewBag.fieldList = fieldList;
                    ModelState.AddModelError("", "该身份证已存在，请重新输入！");
                    return View(staff);
                }
                //物理卡号不唯一
                if (physicalCardNumber.Count() != 0)
                {
                    /*查找员工基本信息表预留字段*/
                    var fieldList = (from sr in db.StaffReserves
                                     join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                     where staff.Number == sr.Number && rf.Status == true
                                     select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
                    ViewBag.fieldList = fieldList;
                    ModelState.AddModelError("", "该物理卡号已存在，请重新输入！");
                    return View(staff);
                }

                if ((staff.IdentificationNumber == null || identicationNumber.Count() == 0) && (staff.PhysicalCardNumber == null || physicalCardNumber.Count() == 0))
                {
                    var tmp = (from p in db.Staffs where (p.StaffNumber == staff.StaffNumber && staff.Number != p.Number) select p).ToList();

                    if (tmp.Count != 0)
                    {
                        List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
                        {
                            Value = c.Name,//保存的值
                            Text = c.Name//显示的值
                        }).ToList();
                        ViewBag.List = item;
                        /*查找员工基本信息表预留字段*/
                        var fieldList = (from sr in db.StaffReserves
                                         join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                         where staff.Number == sr.Number && rf.Status == true
                                         select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
                        ViewBag.fieldList = fieldList;

                        ModelState.AddModelError("", "抱歉，该工号已经被注册！");
                        return View(staff);
                    }
                    else
                    {
                        Staff staff1 = db.Staffs.Find(staff.Number);
                        if (ModelState.IsValid)
                        {
                            if (FileData != null)
                            {
                                staff1.HeadType = FileData.ContentType;//获取图片类型
                                staff1.Head = new byte[FileData.ContentLength];//新建一个长度等于图片大小的二进制地址
                                FileData.InputStream.Read(staff1.Head, 0, FileData.ContentLength);//将image读取到Logo中

                            }
                            /*查找员工基本信息表预留字段*/
                            var fieldList = (from sr in db.StaffReserves
                                             join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                             where staff.Number == sr.Number && rf.Status == true
                                             select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
                            ViewBag.fieldList = fieldList;
                            /*查找员工信息预留字段(value)*/
                            var fieldValueList = (from sr in db.StaffReserves
                                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                                  where sr.Number == staff.Number && rf.Status == true
                                                  select new StaffViewModel { Number = sr.Id, Description = rf.Description, Value = sr.Value }).ToList();
                            /*给预留字段赋值*/
                            foreach (var temp in fieldValueList)
                            {
                                StaffReserve sr = db.StaffReserves.Find(temp.Number);
                                sr.Value = Request[temp.Description];
                                db.SaveChanges();
                            }

                            staff1.StaffNumber = staff.StaffNumber;
                            staff1.Name = staff.Name;
                            staff1.Gender = staff.Gender;
                            staff1.Department = staff.Department;
                            staff1.WorkType = staff.WorkType;
                            staff1.Position = staff.Position;
                            staff1.IdentificationNumber = staff.IdentificationNumber;
                            staff1.Entrydate = staff.Entrydate;
                            staff1.IdentificationType = staff.IdentificationType;
                            staff1.Nationality = staff.Nationality;
                            staff1.ClassOrder = staff.ClassOrder;
                            //staff1.JobState = staff.JobState;
                            staff1.AbnormalChange = staff.AbnormalChange;
                            staff1.FreeCard = staff.FreeCard;
                            staff1.WorkProperty = staff.WorkProperty;
                            staff1.ApplyOvertimeSwitch = staff.ApplyOvertimeSwitch;
                            staff1.Source = staff.Source;
                            staff1.QualifyingPeriodFull = staff.QualifyingPeriodFull;
                            staff1.MaritalStatus = staff.MaritalStatus;
                            staff1.BirthDate = staff.BirthDate;
                            staff1.NativePlace = staff.NativePlace;
                            staff1.HealthCondition = staff.HealthCondition;
                            staff1.Nation = staff.Nation;
                            staff1.Address = staff.Address;
                            staff1.VisaOffice = staff.VisaOffice;
                            staff1.HomeTelNumber = staff.HomeTelNumber;
                            staff1.EducationBackground = staff.EducationBackground;
                            staff1.GraduationSchool = staff.GraduationSchool;
                            staff1.SchoolMajor = staff.SchoolMajor;
                            staff1.Degree = staff.Degree;
                            staff1.Introducer = staff.Introducer;
                            staff1.IndividualTelNumber = staff.IndividualTelNumber;
                            staff1.BankCardNumber = staff.BankCardNumber;
                            staff1.UrgencyContactMan = staff.UrgencyContactMan;
                            staff1.UrgencyContactAddress = staff.UrgencyContactAddress;
                            staff1.UrgencyContactPhoneNumber = staff.UrgencyContactPhoneNumber;
                            staff1.InBlacklist = staff.InBlacklist;
                            staff1.PhysicalCardNumber = staff.PhysicalCardNumber;
                            staff1.IDCardNumber = staff.IDCardNumber;
                            staff1.ChangePerson = base.Name;
                            staff1.ChangeTime = DateTime.Now;
                            staff1.AuditStatus = staff.AuditStatus;
                            staff1.BankCardNumber = staff.BankCardNumber;
                            // staff1.BindingCode = staff.BindingCode;

                            /*保存固定字段*/
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }

                    return View(staff);
                }
                else
                {
                    ModelState.AddModelError("", "抱歉，该身份证已存在！！");
                    return View(staff);
                }
            }
            else
            {
                List<SelectListItem> item = db.Departments.ToList().Select(c => new SelectListItem
                {
                    Value = c.Name,//保存的值
                    Text = c.Name//显示的值
                }).ToList();
                ViewBag.List = item;
                /*查找员工基本信息表预留字段*/
                var fieldList = (from sr in db.StaffReserves
                                 join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                 where staff.Number == sr.Number && rf.Status == true
                                 select new StaffViewModel { Description = rf.Description, Value = sr.Value }).ToList();
                ViewBag.fieldList = fieldList;

                return View(staff);
            }
        }

        [Authorize(Roles = "Admin,Staff_Delete")]
        // GET: Staff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            //var staffDepartment = (from s in db.Staffs
            //                       join d in db.Departments on s.Department equals d.DepartmentId
            //                       into gc    /*左联：显示所有员工表的字段*/
            //                       from d in gc.DefaultIfEmpty()
            //                       where s.Department == staff.Department
            //                       select new StaffViewModel { Number = s.Number, Department = s.Department, Value = d.Name }).ToList();
            //ViewBag.staffDepartment = staffDepartment;
            /*查找员工基本信息表预留字段(value)*/
            var fieldValueList = (from sr in db.StaffReserves
                                  join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                  where sr.Number == id && rf.Status == true
                                  select new StaffViewModel { Number = sr.Number, Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;


            var classOrders = Generate.GetWorks(base.ConnectionString);

            staff.DepartmentName = (from p in db.Departments where p.DepartmentId == staff.Department select p.Name).ToList().FirstOrDefault();
            staff.ClassOrderName = (from classOrder in classOrders where classOrder.Value == staff.ClassOrder select classOrder.Text).ToList().FirstOrDefault();
            staff.AuditStatusName = db.States.Find(staff.AuditStatus).Description;

            return View(staff);
        }


        [Authorize(Roles = "Admin,Staff_Delete")]
        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from sr in db.StaffReserves
                        where sr.Number == id
                        select new StaffViewModel { Number = sr.Id }).ToList();
            foreach (var temp in item)
            {
                StaffReserve sr = db.StaffReserves.Find(temp.Number);
                db.StaffReserves.Remove(sr);
            }
            db.SaveChanges();
            /*Step1.1：删除变更信息*/
            var staffvariation = (from p in db.StaffVariations where p.StaffId == id select p).ToList();
            foreach (var temp in staffvariation)
            {
                db.StaffVariations.Remove(temp);
            }
            /*Step2：删除固定字段*/
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);

            /*Step3：需要把厂牌也删了*/
            var brands = db.Brands.Where(s => s.StaffId.Equals(staff.Number)).ToList();
            if (brands.Count() != 0)
            {
                foreach (var brand in brands)
                {
                    db.Brands.Remove(brand);
                }
            }

            ///*Step3：需要把厂牌也删了*/
            //Brand brand = db.Brands.Where(s => s.StaffId.Equals(staff.Number)).ToList().FirstOrDefault();
            //db.Brands.Remove(brand);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 员工工号的生成
        /// 物理卡号的生成
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StaffNumber(string number)
        {
            Department department = db.Departments.Where(d => d.DepartmentId.Equals(number)).SingleOrDefault();
            //生成员工工号
            string temp = GenerateStaffParam("0001", department.DepartmentAbbr);
            //生成物理卡号
            string str = (from p in db.StaffBasicParams where p.Id == 3 select p.Value).Single();
            int pram = 0;
            int.TryParse(str, out pram);
            string temp2 = GetSerialNumber(pram);///物理卡号传的参数是长度
            try
            {
                var items = (from d in db.Departments
                             where d.DepartmentId == number
                             select new
                             {
                                 staffNumber = temp,
                                 PhysicalCardNumber = temp2
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }
        /// <summary>
        /// 选择部门，有一定的顺序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DepartmentSearch()
        {
            try
            {
                var items = (from d in db.Departments

                             select new
                             {
                                 text = d.Name,
                                 id = d.DepartmentId,
                                 order = d.DepartmentOrder
                             }).OrderBy(c => c.order).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch(string billSort)
        {
            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == billSort
                             select new
                             {
                                 text = b.Type + " " + b.TypeName,
                                 id = b.Type
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }
        /// <summary>
        /// 可以提取成公共方法
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 班次搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClassOrderSearch()
        {
            try
            {
                var classOrders = Generate.GetWorks(base.ConnectionString);


                var items = from classOrder in classOrders
                            select
                                new
                                {
                                    text = classOrder.Text,
                                    id = classOrder.Value
                                };

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }
        /// <summary>
        /// 这段代码用来查找人事参数下拉菜单的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SimpleParamSearch(string name)
        {
            try
            {
                var items1 = (from sp in db.StaffParams
                              join spt in db.StaffParamTypes on sp.StaffParamTypeId equals spt.Id
                              where spt.Name == name
                              select new
                              {
                                  text = sp.Value,
                                  id = sp.Value,
                                  order = sp.StaffParamOrder,
                                  isDefault = sp.IsDefault
                              }).OrderBy(c => c.order).ToList();

                return Json(items1);
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
