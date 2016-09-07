using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Controllers
{
    public class StaffParamAjaxSupportController : BaseController
    {
        // GET: StaffParamAjaxSupport
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 这段代码专门为StaffChangeCreate方法使用。用来查找人事参数下拉菜单的值以及某一个员工的参数值
        /// </summary>
        /// <param name="name">传的是员工的工号以及人事参数名字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StaffChangeCreateFindParam(string name,string staffNumber)
        {
            try
            {
                ///找到这个员工
                Staff staff = db.Staffs.Where(s => s.StaffNumber.Equals(staffNumber)).Single();
                ///反射
                ///获得属性列表
                string staffProperValue = null;
                PropertyInfo[]  staffInfos= typeof(Staff).GetProperties();
                foreach (PropertyInfo staffInfo in staffInfos)
                {
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
                    }
                    ///获取DisplayName(string)
                    Object[] some = staffInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                    var getDisplayName = ((DisplayAttribute)some[0]).Name;
                    ///如果找到需要的那个
                    if (getDisplayName == name)
                    {
                        staffProperValue = staffInfo.GetValue(staff,null).ToString();
                        break;
                    }
                }
               
               // var thisStaffValue = from p in db.Staffs where p.st
                var staffParamsList = (from sp in db.StaffParams
                                       join spt in db.StaffParamTypes on sp.StaffParamTypeId equals spt.Id
                                       where spt.Name == name
                                       select new
                                       {
                                           text = sp.Value,
                                           id = sp.Value,
                                           order = sp.StaffParamOrder,
                                           //thisStaff = staffProperValue
                                           thisStaff= staffProperValue
                                       }).OrderByDescending(c=>c.order).ToList();
                                  
                           
                //var items1 = (from sp in db.StaffParams
                //              join spt in db.StaffParamTypes on sp.StaffParamTypeId equals spt.Id
                //              where spt.Name == name
                //              select new
                //              {
                //                  text = sp.Value,
                //                  id = sp.Value,
                //                  order = sp.StaffParamOrder
                //              }).OrderBy(c => c.order).ToList();

                return Json(staffParamsList);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }
        /// <summary>
        /// 提供所有未离职且审核通过的员工的列表。请酌情调用。
        /// 显示的值是员工工号和姓名，实际保存的是员工的工号。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StaffList()
        {

            try
            {
                var items = (from s in db.Staffs
                             where s.ArchiveTag != true && s.AuditStatus == 3
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StaffNumber(string number)
        {
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
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
                                 head = p.Head,
                                 path = "/StaffChange/GetImage?Number=" + p.Number
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }

        }

    }
}