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
using BonsaiiModels.StaffVariation;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bonsaii.Controllers
{
    public class StaffChangeController : BaseController
    {
        //审批
        public byte AuditApplicationStaffChange(StaffChange staffChange)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == staffChange.BillTypeNumber select p).ToList().FirstOrDefault();

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
                                    (staffChange.BillTypeNumber == p.BType) && (DateTime.Now > p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    var staff = db.Staffs.Where(s => s.StaffNumber.Equals(staffChange.StaffNumber)).ToList().Single();//找到某个员工
                    //var staffVariations = db.StaffVariations.Where(s => s.StaffId.Equals(staff.Number)).ToList();
                    var staffVariationsBefore = (from p in db.StaffVariations
                                           where p.StaffId == staff.Number && p.Initial == true&&p.StaffChangeId==staffChange.Id
                                           select p).ToList();
                    var staffVariationsAfter = (from p in db.StaffVariations
                                                 where p.StaffId == staff.Number && p.Initial == false && p.StaffChangeId == staffChange.Id
                                                 select p).ToList();
                    string staffVariationBefore = null;
                    string staffVariationAfter = null;
                    //string staffVariationValue = null;
                    foreach (var temp in staffVariationsBefore)
                    {
                        staffVariationBefore += temp.FieldName + "：" + temp.FieldValue + ";";
                        //staffVariationValue += temp.FieldValue + ";";
                    }   
                    //string staffVariationValue = null;
                    foreach (var temp in staffVariationsAfter)
                    {
                        staffVariationAfter += temp.FieldName + "：" + temp.FieldValue + ";";
                        //staffVariationValue += temp.FieldValue + ";";
                    }

                 

                    auditApplication.BNumber = staffChange.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = base.UserName;
                    auditApplication.CreatorName = base.Name;
                    auditApplication.State = 0;//待审

                    DateTime entryDate_dateTime = (DateTime)staffChange.Entrydate;

                    if (staffChange.BirthDate != null)
                    {
                        DateTime birthDate_dateTime = (DateTime)staffChange.BirthDate;
                        auditApplication.Info =
                       "单据名称：" + staffChange.BillTypeName + ";" +
                       "工       号：" + staffChange.StaffNumber + ";" +
                       "姓       名：" + staffChange.Name + ";" +
                       "当前部门：" + staffChange.DepartmentName + ";" +
                       "性       别：" + staffChange.Gender + ";" +
                       "当前职位：" + staffChange.Position + ";" +
                       "用工性质：" + staffChange.WorkProperty + ";" +
                       "入职日期：" + entryDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                       "出生日期：" + birthDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                       "员工来源：" + staffChange.Source + ";" +
                       "籍       贯：" + staffChange.NativePlace + ";" +
                       "学       历：" + staffChange.EducationBackground + ";" +
                       "专       业：" + staffChange.SchoolMajor + ";" +
                       "个人手机：" + staffChange.IndividualTelNumber + ";" +
                       "人事变更前：" + ";" +
                       staffVariationBefore +
                       "人事变更后：" + ";" +
                       staffVariationAfter
                            //staffVariationName+staffVariationValue+";"
                       ;
                    }
                    else 
                    {

                        auditApplication.Info =
                       "单据名称：" + staffChange.BillTypeName + ";" +
                       "工       号：" + staffChange.StaffNumber + ";" +
                       "姓       名：" + staffChange.Name + ";" +
                       "当前部门：" + staffChange.DepartmentName + ";" +
                       "性       别：" + staffChange.Gender + ";" +
                       "当前职位：" + staffChange.Position + ";" +
                       "用工性质：" + staffChange.WorkProperty + ";" +
                       "入职日期：" + entryDate_dateTime.Date.ToString("yyyy/MM/dd") + ";" +
                       "出生日期：" + staffChange.BirthDate + ";" +
                       "员工来源：" + staffChange.Source + ";" +
                       "籍       贯：" + staffChange.NativePlace + ";" +
                       "学       历：" + staffChange.EducationBackground + ";" +
                       "专       业：" + staffChange.SchoolMajor + ";" +
                       "个人手机：" + staffChange.IndividualTelNumber + ";" +
                       "人事变更前：" + staffVariationBefore + ";" +

                       "人事变更后：" + staffVariationAfter + ";";
                      
                    }
                   // "人事变更后"+"人事变更前"+
                   // ";"+
                   //"单别：" + staffChange.BillTypeNumber + "-" + staffChange.BillTypeName +"——"+
                   //staff.BillTypeNumber + "-" + staff.BillTypeName + 
                   //";" +
                   //"单号：" + staffChange.BillNumber + "——" +
                   //staff.BillNumber + 
                   //";" +
                   //"员工：" + staffChange.StaffNumber + "-" + staffChange.Name + "——" +
                   //staff.StaffNumber +  "-" + staff.Name +
                   //";" +
                   //"性别：" + staffChange.Gender + "——" +
                   //staff.Gender +
                   //";" +
                   //"部门：" + staffChange.Department + "——" +
                   //staff.Department +
                   //";" +
                   //"工种：" + staffChange.WorkType + "——" +
                   //staff.WorkType +
                   //";" +
                   //"职位：" + staffChange.Position + "——" +
                   //staff.Position + 
                   //";" +
                   //"证件类型：" + staffChange.IdentificationType + "——" +
                   //staff.IdentificationType +
                   //";" +
                   //"国籍：" + staffChange.Nationality + "——" +
                   //staff.Nationality + 
                   //";" +
                   //"身份证号：" + staffChange.IdentificationNumber + "——" +
                   //staff.IdentificationNumber + 
                   //";" +
                   //"入职日期：" + staffChange.Entrydate + "——" +
                   //staff.Entrydate + 
                   //";" +
                   //"班次：" + staffChange.ClassOrder + "——" +
                   //staff.ClassOrder + 
                   //";" +
                   //"在职状态：" + staffChange.JobState + "——" +
                   //staff.JobState+
                   //";" +
                   //"异动类型：" + staffChange.AbnormalChange + "——" +
                   //staff.AbnormalChange+
                   //";" +
                   //"免卡：" + staffChange.FreeCard + "——" +
                   //staff.FreeCard+
                   //";" +
                   //"用工性质：" + staffChange.WorkProperty + "——" +
                   //staff.WorkProperty +
                   //";" +
                   //"加班需申请：" + staffChange.ApplyOvertimeSwitch + "——" +
                   //staff.ApplyOvertimeSwitch + ";" +
                   //"员工来源：" + staffChange.Source + "——" +
                   //staff.Source + ";" +
                   //"试用期满：" + staffChange.QualifyingPeriodFull + "——" +
                   //staff.QualifyingPeriodFull + ";" +
                   //"婚姻状况：" + staffChange.MaritalStatus + "——" +
                   //staff.MaritalStatus + ";" +
                   //"出生日期：" + staffChange.BirthDate + "——" +
                   //staff.BirthDate + ";" +
                   //"籍贯：" + staffChange.NativePlace + "——" +
                   //staff.NativePlace + ";" +
                   //"健康状况：" + staffChange.HealthCondition + "——" +
                   //staff.HealthCondition + ";" +
                   //"民族：" + staffChange.Nation + "——" +
                   //staff.Nation + ";" +
                   //"家庭住址：" + staffChange.Address + "——" +
                   //staff.Address + ";" +
                   //"签证机关：" + staffChange.VisaOffice + "——" +
                   //staff.VisaOffice + ";" +
                   //"家庭电话：" + staffChange.HomeTelNumber + "——" +
                   //staff.HomeTelNumber + ";" +
                   //"学历：" + staffChange.EducationBackground + "——" +
                   //staff.EducationBackground + ";" +
                   //"毕业院校：" + staffChange.GraduationSchool + "——" + 
                   //staff.GraduationSchool + ";" +
                   //"专业：" + staffChange.SchoolMajor + "——" +
                   //staff.SchoolMajor + ";" +
                   //"学位：" + staffChange.Degree + "——" +
                   //staff.Degree + ";" +
                   //"介绍人：" + staffChange.Introducer + "——" +
                   //staff.Introducer + ";" +
                   //"个人手机：" + staffChange.IndividualTelNumber + "——" +
                   //staff.IndividualTelNumber + ";" +
                   //"银行卡号：" + staffChange.BankCardNumber + "——" +
                   //staff.BankCardNumber + ";" +
                   //"紧急联系人姓名：" + staffChange.UrgencyContactMan + "——" +
                   //staff.UrgencyContactMan + ";" +
                   //"紧急联系人地址：" + staffChange.UrgencyContactAddress + "——" +
                   //staff.UrgencyContactAddress + ";" +
                   //"紧急联系人电话：" + staffChange.UrgencyContactPhoneNumber + "——" +
                   //staff.UrgencyContactPhoneNumber + ";" +
                   //"黑名单：" + staffChange.InBlacklist + "——" +
                   //staff.InBlacklist + ";" +
                   //"物理卡号：" + staffChange.PhysicalCardNumber + "——" +
                   //staff.PhysicalCardNumber + ";" +
                   //"离职日期：" + staffChange.LeaveDate + "——" +
                   //staff.LeaveDate + ";" +
                   //"离职类型：" + staffChange.LeaveType + "——" +
                   //staff.LeaveType + ";" +
                   //"离职原因：" + staffChange.LeaveReason + "——" +
                   //staff.LeaveReason + ";" +
                   //"审核状态：" + staffChange.AuditStatus + "——" +
                   //staff.AuditStatus + ";" +
                   //"录入时间：" + staffChange.RecordTime + "——" +
                   //staff.RecordTime + ";" +
                   //"录入人员：" + staffChange.RecordPerson + "——" +
                   //staff.RecordPerson + ";"
                   //;
                 
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
                        auditProcess.Info = auditApplication.Info; //+
                            //"提交人员：" + auditApplication.CreatorName + "———" + auditApplication.Creator + ";" +
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
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
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
                    staffChange.AuditStatus = 3;
                    if (staffChange.AuditStatus == 3)
                    {
                        var staffId = (from p in db.Staffs where p.StaffNumber == staffChange.StaffNumber && p.ArchiveTag != true select p.Number).ToList().FirstOrDefault();
                        Staff staff = db.Staffs.Find(staffId);
                        staff.Name = staffChange.Name;
                        staff.Gender = staffChange.Gender;
                        staff.Department = staffChange.Department;
                        staff.WorkType = staffChange.WorkType;
                        staff.Position = staffChange.Position;
                        staff.IdentificationNumber = staffChange.IdentificationNumber;
                        staff.Nationality = staffChange.Nationality;
                        staff.IdentificationNumber = staffChange.IdentificationNumber;
                        staff.Entrydate = staffChange.Entrydate;
                        staff.ClassOrder = staffChange.ClassOrder;
                        staff.ApplyOvertimeSwitch = staff.ApplyOvertimeSwitch;
                        staff.JobState = staffChange.JobState;
                        staff.AbnormalChange = staffChange.AbnormalChange;
                        staff.FreeCard = staffChange.FreeCard;
                        staff.WorkProperty = staffChange.WorkProperty;
                        staff.WorkType = staffChange.WorkType;
                        staff.Source = staffChange.Source;
                        staff.QualifyingPeriodFull = staffChange.QualifyingPeriodFull;
                        staff.MaritalStatus = staffChange.MaritalStatus;
                        staff.BirthDate = staffChange.BirthDate;
                        staff.NativePlace = staffChange.NativePlace;
                        staff.HealthCondition = staffChange.HealthCondition;
                        staff.Nation = staffChange.Nation;
                        staff.Address = staffChange.Address;
                        staff.VisaOffice = staffChange.VisaOffice;
                        staff.HomeTelNumber = staffChange.HomeTelNumber;
                        staff.EducationBackground = staffChange.EducationBackground;
                        staff.GraduationSchool = staffChange.GraduationSchool;
                        staff.SchoolMajor = staffChange.SchoolMajor;
                        staff.Degree = staffChange.SchoolMajor;
                        staff.Introducer = staffChange.Introducer;
                        staff.IndividualTelNumber = staffChange.IndividualTelNumber;
                        staff.BankCardNumber = staffChange.BankCardNumber;
                        staff.UrgencyContactMan = staffChange.UrgencyContactMan;
                        staff.UrgencyContactAddress = staffChange.UrgencyContactAddress;
                        staff.UrgencyContactPhoneNumber = staffChange.UrgencyContactPhoneNumber;
                        staff.PhysicalCardNumber = staffChange.PhysicalCardNumber;
                        staff.LeaveDate = staffChange.LeaveDate;
                        staff.LeaveType = staffChange.LeaveType;
                        staff.LeaveReason = staffChange.LeaveReason;
                        staff.AuditStatus = staffChange.AuditStatus;
                        staff.HealthCondition = staffChange.HealthCondition;
                        staff.ChangeTime = staffChange.RecordTime;
                        staff.ChangePerson = staffChange.RecordPerson;
                        staff.AuditTime = DateTime.Now;
                        staff.AuditPerson = this.UserName;
                        staff.Head = staffChange.Head;
                        staff.LogicCardNumber = staffChange.LogicCardNumber;
                        staff.HeadType = staffChange.HeadType;
                        staff.IDCardNumber = staffChange.IDCardNumber;
                        staff.DeadlineDate = staffChange.DeadlineDate;
                    }

                }
                else
                {
                    //不通过审批
                    staffChange.AuditStatus = 4;

                }
                staffChange.AuditPerson = this.UserName;
                staffChange.AuditTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //GET: Staff/Submit/5
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationStaffChange(staffChange);
            //需要对原表做出的修改
            staffChange.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,StaffChange_Index")]
        public ActionResult IndexS(string staffNumber)
        {
               // staffNumber = RouteData.Values["staffNumber"].ToString();
                List<StaffChange> StaffChanges = (from p in db.StaffChanges where p.StaffNumber == staffNumber 
                                                  orderby p.BillNumber
                                                  select p).ToList();
                foreach (StaffChange tmp in StaffChanges)
                {
                    tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
                    tmp.AuditStatusName = (from p in db.States where p.Id == tmp.AuditStatus select p.Description).ToList().FirstOrDefault();
                }
                return View(StaffChanges);
        
        }
        [Authorize(Roles = "Admin,StaffChange_Index")]
        public ActionResult Index()
        {
            //var recordList = (from p in db.ReserveFields
            //                  join q in db.TableNameContrasts
            //                      on p.TableNameId equals q.Id
            //                  where q.TableName == "StaffChanges" && p.Status == true
            //                  select p).ToList();
            //ViewBag.recordList = recordList;

            //var pp = (from scf in db.StaffChangeReserves
            //          join rf in db.ReserveFields on scf.FieldId equals rf.Id
            //          select new StaffChangeViewModel
            //          {
            //              Id = scf.Number,
            //              Description = rf.Description,
            //              Value = scf.Value
            //          }).ToList();
            //ViewBag.List = pp;
       

                List<StaffChange> StaffChanges = (from p in db.StaffChanges orderby p.BillNumber select p ).ToList();
                foreach (StaffChange tmp in StaffChanges)
                {
                    tmp.DepartmentName = (from p in db.Departments where p.DepartmentId == tmp.Department select p.Name).ToList().FirstOrDefault();
                    tmp.AuditStatusName = (from p in db.States where p.Id == tmp.AuditStatus select p.Description).ToList().FirstOrDefault();
                }
                return View(StaffChanges.OrderByDescending(c=>c.Id));
          
           // return View(db.StaffChanges.ToList());
        }

        [Authorize(Roles = "Admin,StaffChange_Details")]
        // GET: StaffChange/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
            {
                return HttpNotFound();

            }
            var fieldValueList = (from scr in db.StaffChangeReserves
                                  join rf in db.ReserveFields on scr.FieldId equals rf.Id
                                  where scr.Number == id && rf.Status== true
                                  select new StaffChangeViewModel { Id = scr.Number, Description = rf.Description, Value = scr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;


            var classOrders = Generate.GetWorks(base.ConnectionString);
            staffChange.DepartmentName = (from p in db.Departments where p.DepartmentId == staffChange.Department select p.Name).ToList().FirstOrDefault();
            staffChange.ClassOrderName = (from classOrder in classOrders where classOrder.Value == staffChange.ClassOrder select classOrder.Text).ToList().FirstOrDefault();
            staffChange.AuditStatusName = db.States.Find(staffChange.AuditStatus).Description;

            return View(staffChange);
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
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public FileContentResult GetImageChange(int Id)
        {
            StaffChange s = db.StaffChanges.FirstOrDefault(p => p.Id == Id);
            if (s != null)
            {
                return File(s.Head, s.HeadType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }

       [Authorize(Roles = "Admin,StaffChange_Create")]
        // GET: StaffChange/Create
        public ActionResult Create()
        {
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                                on p.TableNameId equals q.Id
                             where q.TableName == "StaffChanges" && p.Status == true
                             select p).ToList();
            ViewBag.fieldList = fieldList;
        //     public ActionResult Create()
        //{            
        //    /*查找员工基本信息表预留字段*/
        //    var fieldList = (from p in db.ReserveFields where p.TableName == "Staffs" select p).ToList();
        //    ViewBag.fieldList = fieldList;
                //0.班次信息
            var classOrders = Generate.GetWorks(base.ConnectionString);
            List<SelectListItem> classorder = classOrders.ToList().Select(c => new SelectListItem
             {
                Value = c.Value,//保存的值
                Text = c.Text,//显示的值
            }).ToList();
       
            ViewBag.classorderlist = classorder;//ViewBag.departmentList里的departmentList名称与字段名称不能一样（字母大小写也不行）。不然Selected属性会失效的。
           
            //1.部门信息
            List<SelectListItem> department = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.DepartmentId,//保存的值
                Text = c.Name,//显示的值
            }).ToList();
            //增加一个null选项
            SelectListItem ii = new SelectListItem();
            ii.Value = this.CompanyId;
            ii.Text = "请选择";
            ii.Selected = true;
            department.Add(ii);
            ViewBag.departmentList = department;//ViewBag.departmentList里的departmentList名称与字段名称不能一样（字母大小写也不行）。不然Selected属性会失效的。
            //2.性别信息
            List<SelectListItem> gender = new List<SelectListItem>();
            ///linq多表查询
            var gender1 = from spt in db.StaffParamTypes
                       join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                       where spt.Name == "性别"
                       select new { value = sp.Value};

            foreach (var tt in gender1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                gender.Add(i);

            }
            ViewBag.genderList = gender;
            //3.工种信息
            List<SelectListItem> staff = new List<SelectListItem>();
            ///linq多表查询
            var staff1 = from spt in db.StaffParamTypes
                       join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                         where spt.Name == "工种"
                       select new { value = sp.Value };

            foreach (var tt in staff1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                staff.Add(i);

            }
            ViewBag.staff = staff;
            //4.国籍信息
            List<SelectListItem> nationality = new List<SelectListItem>();
            ///linq多表查询
            var nationality1 = from spt in db.StaffParamTypes
                       join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                       where spt.Name == "国籍"
                       select new { value = sp.Value};

            foreach (var tt in nationality1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                nationality.Add(i);

            }
            ViewBag.nationality = nationality;
            //5.籍贯信息
            List<SelectListItem> nativeplace = new List<SelectListItem>();
            ///linq多表查询
            var nativeplace1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "籍贯"
                          select new { value = sp.Value };

            foreach (var tt in nativeplace1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                nativeplace.Add(i);

            }
            ViewBag.nativeplace = nativeplace;
            //6.健康信息
            List<SelectListItem> health = new List<SelectListItem>();
            ///linq多表查询
            var health1 = from spt in db.StaffParamTypes
                               join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                               where spt.Name == "健康状况"
                               select new { value = sp.Value };

            foreach (var tt in health1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                health.Add(i);

            }
            ViewBag.health = health;
            //7.民族信息
            List<SelectListItem> nation = new List<SelectListItem>();
            ///linq多表查询
            var nation1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "民族"
                          select new { value = sp.Value };

            foreach (var tt in nation1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                nation.Add(i);

            }
            ViewBag.nation = nation;
            //8.学历信息
            List<SelectListItem> background = new List<SelectListItem>();
            ///linq多表查询
            var background1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "学历"
                          select new { value = sp.Value };

            foreach (var tt in background1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                background.Add(i);

            }
            ViewBag.background = background;
            //9.异动信息
            List<SelectListItem> abnormal = new List<SelectListItem>();
            ///linq多表查询
            var abnormal1 = from spt in db.StaffParamTypes
                              join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                              where spt.Name == "异动类型"
                              select new { value = sp.Value };

            foreach (var tt in abnormal1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                abnormal.Add(i);

            }
            ViewBag.abnormal = abnormal;
            //10.用工性质信息
            List<SelectListItem> workproperty = new List<SelectListItem>();
            ///linq多表查询
            var workproperty1 = from spt in db.StaffParamTypes
                            join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                            where spt.Name == "用工性质"
                            select new { value = sp.Value };

            foreach (var tt in workproperty1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                workproperty.Add(i);

            }
            ViewBag.workproperty = workproperty;
            //11.员工来源信息
            List<SelectListItem> staffsource = new List<SelectListItem>();
            ///linq多表查询
            var staffsource1 = from spt in db.StaffParamTypes
                        join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                        where spt.Name == "员工来源"
                        select new { value = sp.Value };

            foreach (var tt in staffsource1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                staffsource.Add(i);

            }
            ViewBag.staffsource = staffsource;
            //12.员工职务信息
            List<SelectListItem> position = new List<SelectListItem>();
            ///linq多表查询
            var position1 = from spt in db.StaffParamTypes
                               join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                               where spt.Name == "员工职务"
                               select new { value = sp.Value };

            foreach (var tt in position1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                position.Add(i);

            }
            ViewBag.position = position;
            //13.证件类型信息
            List<SelectListItem> identificationtype = new List<SelectListItem>();
            ///linq多表查询
            var identificationtype1 = from spt in db.StaffParamTypes
                            join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                            where spt.Name == "证件类型"
                            select new { value = sp.Value };

            foreach (var tt in identificationtype1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                identificationtype.Add(i);

            }
            ViewBag.identificationtype = identificationtype;
            //14.学位信息
            List<SelectListItem> degree = new List<SelectListItem>();
            ///linq多表查询
            var degree1 = from spt in db.StaffParamTypes
                                      join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "学位"
                                      select new { value = sp.Value };

            foreach (var tt in degree1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                degree.Add(i);

            }
            ViewBag.degree = degree;
            //15.婚姻状况信息
            List<SelectListItem> maritalstatus = new List<SelectListItem>();
            ///linq多表查询
            var maritalstatus1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "婚姻状况"
                          select new { value = sp.Value };

            foreach (var tt in maritalstatus1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                maritalstatus.Add(i);

            }
            ViewBag.maritalstatus = maritalstatus;
            //16.专业信息
            List<SelectListItem> major = new List<SelectListItem>();
            ///linq多表查询
            var major1 = from spt in db.StaffParamTypes
                                 join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                 where spt.Name == "专业"
                                 select new { value = sp.Value };

            foreach (var tt in major1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                major.Add(i);

            }
            ViewBag.major = major;
                //List<SelectListItem> item2 = db.Healths.ToList().Select(c => new SelectListItem
                //{
                //    Value = c.HealthCondition,//保存的值
                //    Text = c.HealthCondition//显示的值
                //}).ToList();

                //List<SelectListItem> item3 = db.Nations.ToList().Select(c => new SelectListItem
                //{
                //    Value = c.Nationality,//保存的值
                //    Text = c.Nationality//显示的值
                //}).ToList();
                //List<SelectListItem> item4 = db.Backgrounds.ToList().Select(c => new SelectListItem
                //{
                //    Value = c.XueLi,//保存的值
                //    Text = c.XueLi//显示的值
                //}).ToList();
           
        //    return View();
        //}
            return View();
        }

       void init()
       {
           var fieldList = (from p in db.ReserveFields
                            join q in db.TableNameContrasts
                           on p.TableNameId equals q.Id
                            where q.TableName == "StaffChanges" && p.Status == true
                            select p).ToList();
           ViewBag.fieldList = fieldList;
                     //0.班次信息
                     var classOrders = Generate.GetWorks(base.ConnectionString);
                     List<SelectListItem> classorder = classOrders.ToList().Select(c => new SelectListItem
                     {
                         Value = c.Value,//保存的值
                         Text = c.Text,//显示的值
                     }).ToList();

                     ViewBag.classorderlist = classorder;//ViewBag.departmentList里的departmentList名称与字段名称不能一样（字母大小写也不行）。不然Selected属性会失效的。
                     //1.部门信息
                     List<SelectListItem> department = db.Departments.ToList().Select(c => new SelectListItem
                     {
                         Value = c.DepartmentId,//保存的值
                         Text = c.Name,//显示的值
                     }).ToList();
                     //增加一个null选项
                     SelectListItem ii = new SelectListItem();
                     ii.Value = this.CompanyId;
                     ii.Text = "请选择";
                     ii.Selected = true;
                     department.Add(ii);
                     ViewBag.departmentList = department;//ViewBag.departmentList里的departmentList名称与字段名称不能一样（字母大小写也不行）。不然Selected属性会失效的。
                     //2.性别信息
                     List<SelectListItem> gender = new List<SelectListItem>();
                     ///linq多表查询
                     var gender1 = from spt in db.StaffParamTypes
                                   join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                   where spt.Name == "性别"
                                   select new { value = sp.Value };

                     foreach (var tt in gender1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         gender.Add(i);

                     }
                     ViewBag.genderList = gender;
                     //3.工种信息
                     List<SelectListItem> staff = new List<SelectListItem>();
                     ///linq多表查询
                     var staff1 = from spt in db.StaffParamTypes
                                  join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                  where spt.Name == "工种"
                                  select new { value = sp.Value };

                     foreach (var tt in staff1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         staff.Add(i);

                     }
                     ViewBag.staff = staff;
                     //4.国籍信息
                     List<SelectListItem> nationality = new List<SelectListItem>();
                     ///linq多表查询
                     var nationality1 = from spt in db.StaffParamTypes
                                        join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                        where spt.Name == "国籍"
                                        select new { value = sp.Value };

                     foreach (var tt in nationality1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         nationality.Add(i);

                     }
                     ViewBag.nationality = nationality;
                     //5.籍贯信息
                     List<SelectListItem> nativeplace = new List<SelectListItem>();
                     ///linq多表查询
                     var nativeplace1 = from spt in db.StaffParamTypes
                                        join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                        where spt.Name == "籍贯"
                                        select new { value = sp.Value };

                     foreach (var tt in nativeplace1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         nativeplace.Add(i);

                     }
                     ViewBag.nativeplace = nativeplace;
                     //6.健康信息
                     List<SelectListItem> health = new List<SelectListItem>();
                     ///linq多表查询
                     var health1 = from spt in db.StaffParamTypes
                                   join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                   where spt.Name == "健康状况"
                                   select new { value = sp.Value };

                     foreach (var tt in health1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         health.Add(i);

                     }
                     ViewBag.health = health;
                     //7.民族信息
                     List<SelectListItem> nation = new List<SelectListItem>();
                     ///linq多表查询
                     var nation1 = from spt in db.StaffParamTypes
                                   join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                   where spt.Name == "民族"
                                   select new { value = sp.Value };

                     foreach (var tt in nation1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         nation.Add(i);

                     }
                     ViewBag.nation = nation;
                     //8.学历信息
                     List<SelectListItem> background = new List<SelectListItem>();
                     ///linq多表查询
                     var background1 = from spt in db.StaffParamTypes
                                       join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                       where spt.Name == "学历"
                                       select new { value = sp.Value };

                     foreach (var tt in background1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         background.Add(i);

                     }
                     ViewBag.background = background;
                     //9.异动信息
                     List<SelectListItem> abnormal = new List<SelectListItem>();
                     ///linq多表查询
                     var abnormal1 = from spt in db.StaffParamTypes
                                     join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                     where spt.Name == "异动类型"
                                     select new { value = sp.Value };

                     foreach (var tt in abnormal1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         abnormal.Add(i);

                     }
                     ViewBag.abnormal = abnormal;
                     //10.用工性质信息
                     List<SelectListItem> workproperty = new List<SelectListItem>();
                     ///linq多表查询
                     var workproperty1 = from spt in db.StaffParamTypes
                                         join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                         where spt.Name == "用工性质"
                                         select new { value = sp.Value };

                     foreach (var tt in workproperty1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         workproperty.Add(i);

                     }
                     ViewBag.workproperty = workproperty;
                     //11.员工来源信息
                     List<SelectListItem> staffsource = new List<SelectListItem>();
                     ///linq多表查询
                     var staffsource1 = from spt in db.StaffParamTypes
                                        join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                        where spt.Name == "员工来源"
                                        select new { value = sp.Value };

                     foreach (var tt in staffsource1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         staffsource.Add(i);

                     }
                     ViewBag.staffsource = staffsource;
                     //12.员工职务信息
                     List<SelectListItem> position = new List<SelectListItem>();
                     ///linq多表查询
                     var position1 = from spt in db.StaffParamTypes
                                     join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                     where spt.Name == "员工职务"
                                     select new { value = sp.Value };

                     foreach (var tt in position1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         position.Add(i);

                     }
                     ViewBag.position = position;
                     //13.证件类型信息
                     List<SelectListItem> identificationtype = new List<SelectListItem>();
                     ///linq多表查询
                     var identificationtype1 = from spt in db.StaffParamTypes
                                               join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                               where spt.Name == "证件类型"
                                               select new { value = sp.Value };

                     foreach (var tt in identificationtype1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         identificationtype.Add(i);

                     }
                     ViewBag.identificationtype = identificationtype;
                     //14.学位信息
                     List<SelectListItem> degree = new List<SelectListItem>();
                     ///linq多表查询
                     var degree1 = from spt in db.StaffParamTypes
                                   join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                   where spt.Name == "学位"
                                   select new { value = sp.Value };

                     foreach (var tt in degree1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         degree.Add(i);

                     }
                     ViewBag.degree = degree;
                     //15.婚姻状况信息
                     List<SelectListItem> maritalstatus = new List<SelectListItem>();
                     ///linq多表查询
                     var maritalstatus1 = from spt in db.StaffParamTypes
                                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                          where spt.Name == "婚姻状况"
                                          select new { value = sp.Value };

                     foreach (var tt in maritalstatus1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         maritalstatus.Add(i);

                     }
                     ViewBag.maritalstatus = maritalstatus;
                     //16.专业信息
                     List<SelectListItem> major = new List<SelectListItem>();
                     ///linq多表查询
                     var major1 = from spt in db.StaffParamTypes
                                  join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                  where spt.Name == "专业"
                                  select new { value = sp.Value };

                     foreach (var tt in major1)
                     {
                         SelectListItem i = new SelectListItem();
                         i.Value = tt.value;
                         i.Text = tt.value;
                         major.Add(i);

                     }
                     ViewBag.major = major;
                  
        }
       [Authorize(Roles = "Admin,StaffChange_Create")]
        // POST: StaffChange/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffChange staffChange,HttpPostedFileBase FileData)
        {
            //查找单据类别名称
            var billTypeName = (from p in db.BillProperties where p.Type == staffChange.BillTypeNumber select p.TypeName).ToList().FirstOrDefault();
            staffChange.BillTypeName = billTypeName;
            staffChange.BillNumber = GenerateBillNumber(staffChange.BillTypeNumber);
            if (ModelState.IsValid)
            {
                if (FileData != null)
                {
                    staffChange.HeadType = FileData.ContentType;//获取图片类型
                    staffChange.Head = new byte[FileData.ContentLength];//新建一个长度等于图片大小的二进制地址
                   FileData.InputStream.Read(staffChange.Head, 0, FileData.ContentLength);//将image读取到Logo中
               }
              
                staffChange.AuditStatus = 0;
                staffChange.RecordTime = DateTime.Now;
                staffChange.RecordPerson = base.Name;
                staffChange.ChangeTime = DateTime.Now;
                staffChange.ChangePerson = base.Name;
            
                db.StaffChanges.Add(staffChange); 
                //保存变化的字段到另一张表中
                RecordVariation(staffChange);
                byte status   = AuditApplicationStaffChange(staffChange);
                 //db.SaveChanges();
                 //没有找到该单据的审批模板
                 if (status == 7)
                 {
                     ViewBag.alertMessage = true;
                     init();
                     return View(staffChange);
                 }
                 else {//如果存在模板
                     staffChange.AuditStatus = status;
                     db.SaveChanges();
                     var staffId = (from p in db.Staffs where p.StaffNumber == staffChange.StaffNumber && p.ArchiveTag != true select p.Number).ToList().FirstOrDefault();
                     
                     if (staffChange.AuditStatus == 3){
                       
                         if(staffId!=0) {
                         Staff staff = db.Staffs.Find(staffId);
                         staff.Name = staffChange.Name;
                         staff.Gender = staffChange.Gender;
                         staff.Department = staffChange.Department;
                         staff.WorkType = staffChange.WorkType;
                         staff.Position = staffChange.Position;
                         staff.IdentificationNumber = staffChange.IdentificationNumber;
                         staff.Nationality = staffChange.Nationality;
                         staff.IdentificationNumber = staffChange.IdentificationNumber;
                         staff.Entrydate = staffChange.Entrydate;
                         staff.ClassOrder = staffChange.ClassOrder;
                         staff.ApplyOvertimeSwitch = staff.ApplyOvertimeSwitch;
                         staff.JobState = staffChange.JobState;
                         staff.AbnormalChange = staffChange.AbnormalChange;
                         staff.FreeCard = staffChange.FreeCard;
                         staff.WorkProperty = staffChange.WorkProperty;
                         staff.WorkType = staffChange.WorkType;
                         staff.Source = staffChange.Source;
                         staff.QualifyingPeriodFull = staffChange.QualifyingPeriodFull;
                         staff.MaritalStatus = staffChange.MaritalStatus;
                         staff.BirthDate = staffChange.BirthDate;
                         staff.NativePlace = staffChange.NativePlace;
                         staff.HealthCondition = staffChange.HealthCondition;
                         staff.Nation = staffChange.Nation;
                         staff.Address = staffChange.Address;
                         staff.VisaOffice = staffChange.VisaOffice;
                         staff.HomeTelNumber = staffChange.HomeTelNumber;
                         staff.EducationBackground = staffChange.EducationBackground;
                         staff.GraduationSchool = staffChange.GraduationSchool;
                         staff.SchoolMajor = staffChange.SchoolMajor;
                         staff.Degree = staffChange.Degree;
                         staff.Introducer = staffChange.Introducer;
                         staff.IndividualTelNumber = staffChange.IndividualTelNumber;
                         staff.BankCardNumber = staffChange.BankCardNumber;
                         staff.UrgencyContactMan = staffChange.UrgencyContactMan;
                         staff.UrgencyContactAddress = staffChange.UrgencyContactAddress;
                         staff.UrgencyContactPhoneNumber = staffChange.UrgencyContactPhoneNumber;
                         staff.PhysicalCardNumber = staffChange.PhysicalCardNumber;
                         staff.LeaveDate = staffChange.LeaveDate;
                         staff.LeaveType = staffChange.LeaveType;
                         staff.LeaveReason = staffChange.LeaveReason;
                         //staff.AuditStatus = staffChange.AuditStatus;
                         staff.HealthCondition = staffChange.HealthCondition;
                         staff.ChangeTime = staffChange.RecordTime;
                         staff.ChangePerson = staffChange.RecordPerson;
                         //staff.AuditTime = DateTime.Now;
                       //  staff.AuditPerson = this.UserName;

                         staff.LogicCardNumber = staffChange.LogicCardNumber;
                         if (FileData != null)
                         {
                             staff.Head = staffChange.Head;
                             staff.HeadType = staffChange.HeadType;

                         }
                         else {
                             staffChange.Head = staff.Head;
                             staffChange.HeadType = staff.HeadType;
                         }
                         staff.IDCardNumber = staffChange.IDCardNumber;
                         staff.DeadlineDate = staffChange.DeadlineDate;

                         db.SaveChanges();
                         }
                     }
                     var fieldListA = (from p in db.ReserveFields
                                       join q in db.TableNameContrasts
                                       on p.TableNameId equals q.Id
                                       where q.TableName == "StaffChanges" && p.Status == true
                                       select p).ToList();
                     ViewBag.fieldList = fieldListA;

                     /*遍历，保存员工基本信息预留字段*/
                     foreach (var temp in fieldListA)
                     {
                         StaffChangeReserve scr = new StaffChangeReserve();
                         scr.Number = staffChange.Id;
                         scr.FieldId = temp.Id;
                         scr.Value = Request[temp.FieldName];
                         /*占位，为了在Index中显示整齐的格式*/
                        // if (scr.Value == null) scr.Value = "";
                         db.StaffChangeReserves.Add(scr);
                         db.SaveChanges();
                     }
                     return RedirectToAction("Index");
                 }
            }
            else//如果没有
            {
                init();
              return View(staffChange);
            }
           
        }

        public JsonResult BillTypeNumberSearch()
        {
            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == "22"
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
                                 billTypeName = p.TypeName
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
                var items = (from s in db.Staffs where s.ArchiveTag!=true&&s.AuditStatus==3
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
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                //var staff = (from  p in db.Staffs
                //             join d in db.Departments on p.Department equals d.DepartmentId 
                //             where p.ArchiveTag!=true&&p.StaffNumber==number
                //             select p.Number
                //            ).FirstOrDefault();
                //ViewBag.Number = staff;
                var items = (from p in db.Staffs
                             where p.ArchiveTag != true && p.AuditStatus == 3
                             join d in db.Departments on p.Department equals d.DepartmentId
                             where p.StaffNumber == number

                             select new
                             {
                                 name = p.StaffNumber + " " + p.Name,
                                 staffNumber = p.StaffNumber,
                                 staffName = p.Name,
                                 gender = p.Gender,
                                 department = p.Department,
                                 workType = p.WorkType,
                                 position = p.Position,
                                 identificationNumber = p.IdentificationNumber,
                                 entrydate = p.Entrydate.ToString(),
                                 identificationType = p.IdentificationType,
                                 nationality = p.Nationality,
                                 classOrder = p.ClassOrder,
                                 jobState = p.JobState,
                                 abnormalChange = p.AbnormalChange,
                                 freeCard = p.FreeCard,
                                 workProperty = p.WorkProperty,
                                 applyOvertimeSwitch = p.ApplyOvertimeSwitch,
                                 source = p.Source,
                                 qualifyingPeriodFull = p.QualifyingPeriodFull,
                                 maritalStatus = p.MaritalStatus,
                                 birthDate = p.BirthDate.ToString(),
                                 nativePlace = p.NativePlace,
                                 healthCondition = p.HealthCondition,
                                 nation = p.Nation,
                                 address = p.Address,
                                 visaOffice = p.VisaOffice,
                                 homeTelNumber = p.HomeTelNumber,
                                 educationBackground = p.EducationBackground,
                                 graduationSchool = p.GraduationSchool,
                                 schoolMajor = p.SchoolMajor,
                                 degree = p.Degree,
                                 introducer = p.Introducer,
                                 individualTelNumber = p.IndividualTelNumber,
                                 bankCardNumber = p.BankCardNumber,
                                 urgencyContactMan = p.UrgencyContactMan,
                                 urgencyContactAddress = p.UrgencyContactAddress,
                                 urgencyContactPhoneNumber = p.UrgencyContactPhoneNumber,
                                 inBlacklist = p.InBlacklist,
                                 physicalCardNumber = p.PhysicalCardNumber,
                                 effectiveDate = date,
                                 deadlineDate = p.DeadlineDate.ToString(),
                                 idCardNumber = p.IDCardNumber,
                                 head=p.Head,
                                 path = "/StaffChange/GetImage?Number=" + p.Number
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }

        /*实现员工工号搜索：显示姓名和工号*/
        [HttpPost]
        public JsonResult StaffNumberSearch(string number)
        {
            string date=DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                //var staff = (from p in db.Staffs
                //             join d in db.Departments on p.Department equals d.DepartmentId
                //             where p.ArchiveTag != true && p.StaffNumber == number
                //             select p.Number
                //             ).FirstOrDefault();
                //ViewBag.Number = staff;
                var items = (from p in db.Staffs where p.ArchiveTag!=true&&p.AuditStatus==3
                             join d in db.Departments on p.Department equals d.DepartmentId 
                             into gc 
                             from d in gc.DefaultIfEmpty()
                           
                             select new
                             {
                                 name = p.StaffNumber + " " + p.Name,
                                 staffNumber = p.StaffNumber,
                                 staffName = p.Name,
                                 gender = p.Gender,
                                 department = p.Department,
                                 workType = p.WorkType,
                                 position = p.Position,
                                 identificationNumber = p.IdentificationNumber,
                                 entrydate = p.Entrydate.ToString(),
                                 identificationType = p.IdentificationType,
                                 nationality = p.Nationality,
                                 classOrder = p.ClassOrder,
                                 jobState = p.JobState,
                                 abnormalChange = p.AbnormalChange,
                                 freeCard = p.FreeCard,
                                 workProperty = p.WorkProperty,
                                 applyOvertimeSwitch = p.ApplyOvertimeSwitch,
                                 source = p.Source,
                                 qualifyingPeriodFull = p.QualifyingPeriodFull,
                                 maritalStatus = p.MaritalStatus,
                                 birthDate = p.BirthDate.ToString(),
                                 nativePlace = p.NativePlace,
                                 healthCondition = p.HealthCondition,
                                 nation = p.Nation,
                                 address = p.Address,
                                 visaOffice = p.VisaOffice,
                                 homeTelNumber = p.HomeTelNumber,
                                 educationBackground = p.EducationBackground,
                                 graduationSchool = p.GraduationSchool,
                                 schoolMajor = p.SchoolMajor,
                                 degree = p.Degree,
                                 introducer = p.Introducer,
                                 individualTelNumber = p.IndividualTelNumber,
                                 bankCardNumber = p.BankCardNumber,
                                 urgencyContactMan = p.UrgencyContactMan,
                                 urgencyContactAddress = p.UrgencyContactAddress,
                                 urgencyContactPhoneNumber = p.UrgencyContactPhoneNumber,
                                 inBlacklist = p.InBlacklist,
                                 physicalCardNumber = p.PhysicalCardNumber,
                                 effectiveDate = date,
                                 deadlineDate = p.DeadlineDate.ToString(),
                                 idCardNumber = p.IDCardNumber,
                                 id=p.Number,
                                 head = p.Head,
                                 path= "/StaffChange/GetImage?Number="+p.Number
                                // id=p.Number
                             }).ToList();

               
                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }

         [Authorize(Roles = "Admin,StaffChange_Edit")]
        // GET: StaffChange/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
            {
                return HttpNotFound();
            }
              //1.部门信息
            List<SelectListItem> department = db.Departments.ToList().Select(c => new SelectListItem
            {
                Value = c.DepartmentId,//保存的值
                Text = c.Name,//显示的值
            }).ToList();
            //增加一个null选项
            SelectListItem ii = new SelectListItem();
            ii.Value = this.CompanyId;
            ii.Text = "-请选择-";
            ii.Selected = true;
            department.Add(ii);
            ViewBag.departmentList = department;//ViewBag.departmentList里的departmentList名称与字段名称不能一样（字母大小写也不行）。不然Selected属性会失效的。
            //2.性别信息
            List<SelectListItem> gender = new List<SelectListItem>();
            ///linq多表查询
            var gender1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "性别"
                          select new { value = sp.Value };

            foreach (var tt in gender1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                gender.Add(i);

            }
            ViewBag.genderList = gender;
            //3.工种信息
            List<SelectListItem> staffparam = new List<SelectListItem>();
            ///linq多表查询
            var staff1 = from spt in db.StaffParamTypes
                         join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                         where spt.Name == "工种"
                         select new { value = sp.Value };

            foreach (var tt in staff1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                staffparam.Add(i);

            }
            ViewBag.staff = staffparam;
            //4.国籍信息
            List<SelectListItem> nationality = new List<SelectListItem>();
            ///linq多表查询
            var nationality1 = from spt in db.StaffParamTypes
                               join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                               where spt.Name == "国籍"
                               select new { value = sp.Value };

            foreach (var tt in nationality1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                nationality.Add(i);

            }
            ViewBag.nationality = nationality;
            //5.籍贯信息
            List<SelectListItem> nativeplace = new List<SelectListItem>();
            ///linq多表查询
            var nativeplace1 = from spt in db.StaffParamTypes
                               join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                               where spt.Name == "籍贯"
                               select new { value = sp.Value };

            foreach (var tt in nativeplace1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                nativeplace.Add(i);

            }
            ViewBag.nativeplace = nativeplace;
            //6.健康信息
            List<SelectListItem> health = new List<SelectListItem>();
            ///linq多表查询
            var health1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "健康状况"
                          select new { value = sp.Value };

            foreach (var tt in health1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                health.Add(i);

            }
            ViewBag.health = health;
            //7.民族信息
            List<SelectListItem> nation = new List<SelectListItem>();
            ///linq多表查询
            var nation1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "民族"
                          select new { value = sp.Value };

            foreach (var tt in nation1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                nation.Add(i);

            }
            ViewBag.nation = nation;
            //8.学历信息
            List<SelectListItem> background = new List<SelectListItem>();
            ///linq多表查询
            var background1 = from spt in db.StaffParamTypes
                              join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                              where spt.Name == "学历"
                              select new { value = sp.Value };

            foreach (var tt in background1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                background.Add(i);

            }
            ViewBag.background = background;
            //9.异动信息
            List<SelectListItem> abnormal = new List<SelectListItem>();
            ///linq多表查询
            var abnormal1 = from spt in db.StaffParamTypes
                            join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                            where spt.Name == "异动类型"
                            select new { value = sp.Value };

            foreach (var tt in abnormal1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                abnormal.Add(i);

            }
            ViewBag.abnormal = abnormal;
            //10.用工性质信息
            List<SelectListItem> workproperty = new List<SelectListItem>();
            ///linq多表查询
            var workproperty1 = from spt in db.StaffParamTypes
                                join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                where spt.Name == "用工性质"
                                select new { value = sp.Value };

            foreach (var tt in workproperty1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                workproperty.Add(i);

            }
            ViewBag.workproperty = workproperty;
            //11.员工来源信息
            List<SelectListItem> staffsource = new List<SelectListItem>();
            ///linq多表查询
            var staffsource1 = from spt in db.StaffParamTypes
                               join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                               where spt.Name == "员工来源"
                               select new { value = sp.Value };

            foreach (var tt in staffsource1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                staffsource.Add(i);

            }
            ViewBag.staffsource = staffsource;
            //12.员工职务信息
            List<SelectListItem> position = new List<SelectListItem>();
            ///linq多表查询
            var position1 = from spt in db.StaffParamTypes
                            join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                            where spt.Name == "员工职务"
                            select new { value = sp.Value };

            foreach (var tt in position1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                position.Add(i);

            }
            ViewBag.position = position;
            //13.证件类型信息
            List<SelectListItem> identificationtype = new List<SelectListItem>();
            ///linq多表查询
            var identificationtype1 = from spt in db.StaffParamTypes
                                      join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                      where spt.Name == "证件类型"
                                      select new { value = sp.Value };

            foreach (var tt in identificationtype1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                identificationtype.Add(i);

            }
            ViewBag.identificationtype = identificationtype;
            //14.学位信息
            List<SelectListItem> degree = new List<SelectListItem>();
            ///linq多表查询
            var degree1 = from spt in db.StaffParamTypes
                          join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                          where spt.Name == "学位"
                          select new { value = sp.Value };

            foreach (var tt in degree1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                degree.Add(i);

            }
            ViewBag.degree = degree;
            //15.婚姻状况信息
            List<SelectListItem> maritalstatus = new List<SelectListItem>();
            ///linq多表查询
            var maritalstatus1 = from spt in db.StaffParamTypes
                                 join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                                 where spt.Name == "婚姻状况"
                                 select new { value = sp.Value };

            foreach (var tt in maritalstatus1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                maritalstatus.Add(i);

            }
            ViewBag.maritalstatus = maritalstatus;
            //16.专业信息
            List<SelectListItem> major = new List<SelectListItem>();
            ///linq多表查询
            var major1 = from spt in db.StaffParamTypes
                         join sp in db.StaffParams on spt.Id equals sp.StaffParamTypeId
                         where spt.Name == "专业"
                         select new { value = sp.Value };

            foreach (var tt in major1)
            {
                SelectListItem i = new SelectListItem();
                i.Value = tt.value;
                i.Text = tt.value;
                major.Add(i);

            }
            ViewBag.major = major;

            /*查找员工基本信息表预留字段*/
            var fieldList = (from sr in db.StaffChangeReserves
                             join rf in db.ReserveFields on sr.FieldId equals rf.Id
                             where staffChange.Id == sr.Number && rf.Status == true
                             select new StaffChangeViewModel { Description = rf.Description, Value = sr.Value }).ToList();
            ViewBag.fieldList = fieldList;
            return View(staffChange);
        }

       [Authorize(Roles = "Admin,StaffChange_Edit")]
        // POST: StaffChange/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult Edit(StaffChange staffChange, HttpPostedFileBase FileData)
        {
            StaffChange staff1 = db.StaffChanges.Find(staffChange.Id);
            if (ModelState.IsValid)
            {
                if (FileData != null)
                {
                    staff1.HeadType = FileData.ContentType;//获取图片类型
                    staff1.Head = new byte[FileData.ContentLength];//新建一个长度等于图片大小的二进制地址
                    FileData.InputStream.Read(staff1.Head, 0, FileData.ContentLength);//将image读取到Logo中

                }
                /*查找员工信息预留字段(value)*/
                var fieldValueList = (from sr in db.StaffChangeReserves
                                      join rf in db.ReserveFields on sr.FieldId equals rf.Id
                                      where sr.Number == staffChange.Id && rf.Status == true
                                      select new StaffChangeViewModel { Id = sr.Id, Description = rf.Description, Value = sr.Value }).ToList();
                /*给预留字段赋值*/
                foreach (var temp in fieldValueList)
                {
                    StaffChangeReserve sr = db.StaffChangeReserves.Find(temp.Id);
                    sr.Value = Request[temp.Description];
                    db.SaveChanges();
                }
                staff1.StaffNumber = staffChange.StaffNumber;
                staff1.Name = staffChange.Name;
                staff1.Gender = staffChange.Gender;
                staff1.Department = staffChange.Department;
                staff1.WorkType = staffChange.WorkType;
                staff1.Position = staffChange.Position;
                staff1.IdentificationNumber = staffChange.IdentificationNumber;
                staff1.Entrydate = staffChange.Entrydate;
                staff1.IdentificationType = staffChange.IdentificationType;
                staff1.Nationality = staffChange.Nationality;
                staff1.ClassOrder = staffChange.ClassOrder;
                staff1.JobState = staffChange.JobState;
                staff1.AbnormalChange = staffChange.AbnormalChange;
                staff1.FreeCard = staffChange.FreeCard;
                staff1.WorkProperty = staffChange.WorkProperty;
                staff1.ApplyOvertimeSwitch = staffChange.ApplyOvertimeSwitch;
                staff1.Source = staffChange.Source;
                staff1.QualifyingPeriodFull = staffChange.QualifyingPeriodFull;
                staff1.MaritalStatus = staffChange.MaritalStatus;
                staff1.BirthDate = staffChange.BirthDate;
                staff1.NativePlace = staffChange.NativePlace;
                staff1.HealthCondition = staffChange.HealthCondition;
                staff1.Nation = staffChange.Nation;
                staff1.Address = staffChange.Address;
                staff1.VisaOffice = staffChange.VisaOffice;
                staff1.HomeTelNumber = staffChange.HomeTelNumber;
                staff1.EducationBackground = staffChange.EducationBackground;
                staff1.GraduationSchool = staffChange.GraduationSchool;
                staff1.SchoolMajor = staffChange.SchoolMajor;
                staff1.Degree = staffChange.Degree;
                staff1.Introducer = staffChange.Introducer;
                staff1.IndividualTelNumber = staffChange.IndividualTelNumber;
                staff1.BankCardNumber = staffChange.BankCardNumber;
                staff1.UrgencyContactMan = staffChange.UrgencyContactMan;
                staff1.UrgencyContactAddress = staffChange.UrgencyContactAddress;
                staff1.UrgencyContactPhoneNumber = staffChange.UrgencyContactPhoneNumber;
                staff1.InBlacklist = staffChange.InBlacklist;
                staff1.PhysicalCardNumber = staffChange.PhysicalCardNumber;

                staff1.ChangePerson = base.Name;
                staff1.ChangeTime = DateTime.Now;
                db.SaveChanges();
                RecordVariation(staff1);
                return RedirectToAction("Index");
            }
            return View(staffChange);
        }

        [Authorize(Roles = "Admin,StaffChange_Delete")]
        // GET: StaffChange/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffChange staffChange = db.StaffChanges.Find(id);
            if (staffChange == null)
            {
                return HttpNotFound();
            }
            /*查找表预留字段*/
            var fieldValueList = (from scr in db.StaffChangeReserves
                                  join rf in db.ReserveFields on scr.FieldId equals rf.Id
                                  where scr.Number == id && rf.Status == true
                                  select new StaffChangeViewModel { Id = scr.Number, Description = rf.Description, Value = scr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;

            var classOrders = Generate.GetWorks(base.ConnectionString);
            staffChange.DepartmentName = (from p in db.Departments where p.DepartmentId == staffChange.Department select p.Name).ToList().FirstOrDefault();
            staffChange.ClassOrderName = (from classOrder in classOrders where classOrder.Value == staffChange.ClassOrder select classOrder.Text).ToList().FirstOrDefault();
            staffChange.AuditStatusName = db.States.Find(staffChange.AuditStatus).Description;
          

            return View(staffChange);
        }

         [Authorize(Roles = "Admin,StaffChange_Delete")]
        // POST: StaffChange/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from scr in db.StaffChangeReserves
                        where scr.Number == id
                        select new StaffChangeViewModel { Id = scr.Id }).ToList();
            foreach (var temp in item)
            {
                StaffChangeReserve scr = db.StaffChangeReserves.Find(temp.Id);
                db.StaffChangeReserves.Remove(scr);

            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            StaffChange staffChange = db.StaffChanges.Find(id);
            db.StaffChanges.Remove(staffChange);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Look(int staffChangeId,string id)
        {
            var staff = db.Staffs.Where(s => s.StaffNumber.Equals(id)).ToList();//找到某个员工
        
           foreach(var temp in staff)
           {
               Department department1 = (from d in db.Departments where d.DepartmentId == temp.Department select d).SingleOrDefault();
               //Department department1 = (from d in db.Departments where d.DepartmentId == temp.Department select d).ToList().FirstOrDefault();
               ViewBag.department = department1.Name;
               BillPropertyModels bp = db.BillProperties.Where(b => b.Type.Equals(temp.BillTypeNumber)).SingleOrDefault();
               ViewBag.billproperty = bp.TypeName;
           }
           
            // StaffChange staffChange = db.StaffChanges.Where(s => s.StaffNumber.Equals(id)).SingleOrDefault();
            StaffChange staffChange = db.StaffChanges.Where(s => s.Id.Equals(staffChangeId)).ToList().FirstOrDefault();
            Department department = (from d in db.Departments where d.DepartmentId == staffChange.Department select d).SingleOrDefault();
            staffChange.Department = department.Name;
            ViewBag.staff = staff;
            //ViewBag.staffChange = staffChange;
            return View(staffChange);
        }
       /// <summary>
       /// 记录变更的内容
       /// </summary>
       /// <param name="staffChangeId"></param>
       /// <param name="id"></param>
        public void RecordVariation(StaffChange staffChange)
        {
            //根据员工工号找到某个员工，如果没找到或者找到多个员工则报异常；
            try
            {
                Staff staff = db.Staffs.Where(s => s.StaffNumber.Equals(staffChange.StaffNumber)).Single();
                //用反射的方式
                StaffVariation staffVariationBefore = new StaffVariation();
                StaffVariation staffVariationAfter = new StaffVariation();
                PropertyInfo[]  staffInfos= typeof(Staff).GetProperties();
                PropertyInfo[] staffChangeInfos = typeof(StaffChange).GetProperties();
                int flag = 0;
                foreach (PropertyInfo staffInfo in staffInfos)
                {
                  
                    //如果是Number那么跳过
                    switch (staffInfo.Name)
                    {
                        case "Number": continue;
                        case "BillTypeNumber": continue;
                        case "BillTypeName": continue;
                        case "BillNumber": continue;
                        case "Head": continue;
                        case "HeadType": continue;
                        //case "Entrydate": continue;
                        //case "BirthDate": continue;
                        //case "LeaveDate": continue;
                        //case "DeadlineDate": continue;
                        case "RecordTime": continue;
                        case "ChangeTime": continue;
                        case "AuditTime": continue;
                        case "DepartmentName": continue;
                        case "ClassOrderName": continue;
                        case "AuditStatusName": continue;
                        case "ChangePerson": continue;
                        case "AuditPerson": continue;
                        case "LongTime": continue;
                        case "BindingCode": continue;
                        case "AuditStatus": continue;
                        case "RecordPerson": continue;

                           
                    }

                    //if (tItem.Name == "Number") continue;
                    //if (tItem.Name == "BillTypeNumber") continue;
                    //if (tItem.Name == "BillTypeName") continue;
                    //if (tItem.Name == "Head") continue;
                    //if(tItem.Name ==)
                    int flag2 = 0;
                   
                    foreach (PropertyInfo staffChangeInfo in staffChangeInfos)
                    {
                        switch (staffChangeInfo.Name)
                        {
                            case "Id": continue;
                            case "BillTypeNumber": continue;
                            case "BillTypeName": continue;
                            case "BillNumber": continue;
                            case "Head": continue;
                            case "HeadType": continue;
                            //case "Entrydate": continue;
                            //case "BirthDate": continue;
                            //case "LeaveDate": continue;
                            //case "DeadlineDate": continue;
                            case "RecordTime": continue;
                            case "ChangeTime": continue;
                            case "AuditTime": continue;
                            case "DepartmentName": continue;
                            case "ClassOrderName": continue;
                            case "AuditStatusName": continue;
                            case "ChangePerson": continue;
                            case "AuditPerson": continue;
                            case "LongTime": continue;
                            case "EffectiveDate": continue;
                            case "BindingCode": continue;
                            case "AuditStatus": continue;
                            case "Abnormal": continue;
                        }
                        
                        //if (vItem.Name == "Id") continue;
                        if (staffInfo.Name == staffChangeInfo.Name)
                        {
                            flag2++;

                            ///获取DisplayName
                            Object[] some = staffInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                            var getDisplayName = ((DisplayAttribute)some[0]).Name;
                            Object[] some2 = staffChangeInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                            var getDisplayName2 = ((DisplayAttribute)some[0]).Name;
                            string staffValue = null; string staffChangeValue = null;
                            
                            if (staffInfo.GetValue(staff) == null)
                            {
                                if (staffChangeInfo.GetValue(staffChange) == null)//两个都为null
                                {
                                    break;
                                }
                                else//Staff为null,StaffChange不为null
                                {
                                   staffChangeValue = staffChangeInfo.GetValue(staffChange).ToString();
                                   flag++;
                               


                                   //DateTime test = new DateTime();
                                   //staffVariationBefore.FieldValue = test.ToString();
                                   //Boolean btest = true;
                                   //staffVariationBefore.FieldValue = btest.ToString();
                                   //string a = "aaa"; staffVariationBefore.FieldValue = a.ToString();

                                   //变更前信息
                                   staffVariationBefore.Initial = true;
                                   //staffVariationBefore.FieldName =staffInfo.Name;
                                   staffVariationBefore.FieldName = getDisplayName;
                                   staffVariationBefore.FieldValue = staffValue;//Null不能转成string
                                   staffVariationBefore.VariationRecordTime = DateTime.Now;
                                   staffVariationBefore.VariationEffectTime = staffChange.EffectiveDate;
                                   staffVariationBefore.StaffId = staff.Number;
                                   staffVariationAfter.StaffChangeId = staffChange.Id;
                                   db.StaffVariations.Add(staffVariationBefore);
                                   //变更后信息  
                                   staffVariationAfter.FieldValue = staffChangeValue;//Null不能转成string
                                   //staffVariationAfter.FieldName = staffChangeInfo.Name;
                                   staffVariationAfter.FieldName = getDisplayName2;
                                   staffVariationAfter.Initial = false;//不是初值
                                   staffVariationAfter.VariationRecordTime = DateTime.Now;
                                   staffVariationAfter.VariationEffectTime = staffChange.EffectiveDate;
                                   staffVariationAfter.StaffId = staff.Number;
                                   staffVariationAfter.StaffChangeId = staffChange.Id;
                                   db.StaffVariations.Add(staffVariationAfter);
                                   db.SaveChanges();
                                }
                            }
                            else
                            {
                                staffValue = staffInfo.GetValue(staff).ToString();  
                                if (staffChangeInfo.GetValue(staffChange) != null) //两个都不为null
                                {
                                    
                                    staffChangeValue = staffChangeInfo.GetValue(staffChange).ToString();
                                    if (staffValue != staffChangeValue) 
                                    {
                                        flag++;
                                        //变更前信息
                                        staffVariationBefore.Initial = true;
                                        staffVariationBefore.FieldName = getDisplayName;
                                        staffVariationBefore.FieldValue = staffValue.ToString();//不为Null可以转成string
                                        staffVariationBefore.VariationRecordTime = DateTime.Now;
                                        staffVariationBefore.VariationEffectTime = staffChange.EffectiveDate;
                                        staffVariationBefore.StaffId = staff.Number;
                                        db.StaffVariations.Add(staffVariationBefore);
                                        //变更后信息  
                                        staffVariationAfter.FieldValue = staffChangeValue.ToString();//为Null不可以转成string
                                        staffVariationAfter.FieldName = getDisplayName2;
                                        staffVariationAfter.Initial = false;//不是初值
                                        staffVariationAfter.VariationRecordTime = DateTime.Now;
                                        staffVariationAfter.VariationEffectTime = staffChange.EffectiveDate;
                                        staffVariationAfter.StaffId = staff.Number;
                                        db.StaffVariations.Add(staffVariationAfter);
                                        db.SaveChanges();
                                    
                                    }

                                }
                                else //StaffChange 为null，Staff不为null
                                {
                                    flag++;
                                    //变更前信息
                                    staffVariationBefore.Initial = true;
                                    staffVariationBefore.FieldName = getDisplayName;
                                    staffVariationBefore.FieldValue = staffValue.ToString();
                                    staffVariationBefore.VariationRecordTime = DateTime.Now;
                                    staffVariationBefore.VariationEffectTime = staffChange.EffectiveDate;
                                    staffVariationBefore.StaffId = staff.Number;
                                    db.StaffVariations.Add(staffVariationBefore);
                                    //变更后信息  
                                    staffVariationAfter.FieldValue = staffChangeValue;
                                    staffVariationAfter.FieldName = getDisplayName2;
                                    staffVariationAfter.Initial = false;//不是初值
                                    staffVariationAfter.VariationRecordTime = DateTime.Now;
                                    staffVariationAfter.VariationEffectTime = staffChange.EffectiveDate;
                                    staffVariationAfter.StaffId = staff.Number;
                                    db.StaffVariations.Add(staffVariationAfter);
                                    db.SaveChanges();
                                }
                            }
                        } 
                        
                        if (flag2 != 0)
                        {
                            break;
                        }
                    }
                
                }
            }
            catch(Exception e){
                throw e;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public ActionResult StaffChangeVariationIndex(int staffChangeId)
        {
            var staffChange =db.StaffChanges.Find(staffChangeId);
            var staff = (from p in db.Staffs where p.StaffNumber == staffChange.StaffNumber select p).SingleOrDefault();
            var variationView = (from p in db.StaffVariations
                                 //join q in db.Staffs on p.StaffId equals q.Number
                                 where p.Initial == false && staff.Number == p.StaffId
                                 select p).ToList();
            var name = db.Staffs.Find(staff.Number);
            ViewBag.name = name.Name;
            foreach (var item in variationView)
            {
                if (item.FieldName == "部门")
                {
                    var temp = (from p in db.Departments where p.DepartmentId == item.FieldValue select p.Name).Single();
                    item.FieldValue = temp;
                    // ViewBag.DepartmentName = fro
                }
                if (item.FieldName == "班次")
                {
                    int m = 0;
                    int.TryParse(item.FieldValue, out m);
                    var temp = db.Works.Find(m);//(from p in db.Works where p.Nam)
                    item.FieldValue = temp.Name;
                }
                //string fieldName = item.FieldName;
                // var propertyInfo = typeof(Staff).GetProperty(fieldName);
                //item.FieldName = propertyInfo.GetValue(item).ToString(); 
                //var displayName = propertyInfo.GetCustomAttributes();//<DisplayNameAttribute>();

                //item.FieldName = displayName.DisplayName;
                //propertyInfo [] props = typeof(StaffVariation).GetProperties();
                //foreach (var prop in props) 
                //{
                //    prop.GetCustomAttributes[]
                //}
                //object[] attrs = item.FieldName.GetCustomAttributes(true);
                //switch (item.FieldName) 
                //{
                //    case "Department":{item.FieldName="部门";continue; }
                //    case "ClassOrder": { item.FieldName = "班次"; continue; }
                //    case "Gender": { item.FieldName = "性别"; continue; }
                //    case "WorkType": { item.FieldName = "工种"; continue; }
                //    case "Position": { item.FieldName = "职位"; continue; }

                //}
            }

            return View(variationView);
        }


        public ActionResult StaffVariationIndex(int staffId)
        {
            var variationView = (from p in db.StaffVariations
                                 //join q in db.Staffs on p.StaffId equals q.Number
                                 where p.Initial ==false && staffId==p.StaffId
                                 select p).ToList();
            var name = db.Staffs.Find(staffId);
            ViewBag.name = name.Name;
            foreach (var item in variationView) {
                if (item.FieldName == "部门") 
                {
                   var temp= (from p in db.Departments where p.DepartmentId==item.FieldValue select p.Name).Single();
                   item.FieldValue = temp;
                   // ViewBag.DepartmentName = fro
                }
                if (item.FieldName == "班次") 
                {
                    int m = 0;
                    int.TryParse(item.FieldValue, out m);
                    var temp = db.Works.Find(m);//(from p in db.Works where p.Nam)
                    item.FieldValue = temp.Name;
                }
                //string fieldName = item.FieldName;
               // var propertyInfo = typeof(Staff).GetProperty(fieldName);
                //item.FieldName = propertyInfo.GetValue(item).ToString(); 
                //var displayName = propertyInfo.GetCustomAttributes();//<DisplayNameAttribute>();
                
                //item.FieldName = displayName.DisplayName;
                //propertyInfo [] props = typeof(StaffVariation).GetProperties();
                //foreach (var prop in props) 
                //{
                //    prop.GetCustomAttributes[]
                //}
                //object[] attrs = item.FieldName.GetCustomAttributes(true);
                //switch (item.FieldName) 
                //{
                //    case "Department":{item.FieldName="部门";continue; }
                //    case "ClassOrder": { item.FieldName = "班次"; continue; }
                //    case "Gender": { item.FieldName = "性别"; continue; }
                //    case "WorkType": { item.FieldName = "工种"; continue; }
                //    case "Position": { item.FieldName = "职位"; continue; }

                //}
            }

            return View(variationView);
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
