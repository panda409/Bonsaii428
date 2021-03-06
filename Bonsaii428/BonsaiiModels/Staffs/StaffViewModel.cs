﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class StaffViewModel
    {
        [Key]
        public int Number { get; set; }

        [Required]
        [Display(Name = "单据类别编号和名称")]
        public string BillTypeNumber { get; set; }

        [Required]
        [Display(Name = "单据类别名称")]
        public string BillTypeName { get; set; }

        [Required]
        [Display(Name = "单号")]
        public string BillNumber { get; set; }

        [Required]
        [Display(Name = "工号")]
        public string StaffNumber { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "头像")]
        public byte[] Head { get; set; }

        public string HeadType { get; set; }

        [Display(Name = "性别")]
        public string Gender { get; set; }
         [Required]
        [Display(Name = "部门")]

        public string Department { get; set; }
        [Display(Name = "工种")]
        public string WorkType { get; set; }
        [Display(Name = "员工职位")]
        public string Position { get; set; }
        [Display(Name = "证件类型")]
        public string IdentificationType { get; set; }
        [Display(Name = "国籍")]
        public string Nationality { get; set; }
        [Display(Name = "证件号码")]
        [RegularExpression(@"^(\d{15}|\d{18}|\d{17}[x])$", ErrorMessage = "身份证号(15位或18位数字)")]
        public string IdentificationNumber { get; set; }
        [Display(Name = "入职日期")]
       // [Required]
       [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> Entrydate { get; set; }
        [Display(Name = "班次")]
        public string ClassOrder { get; set; }
        //[Display(Name = "手机APP")]
        //public string AppSwitch { get; set; }
        [Display(Name = "在职状态")]
        public string JobState { get; set; }
        [Display(Name = "异动类型")]
        public string AbnormalChange { get; set; }
        [Display(Name = "免卡")]
        public Boolean FreeCard { get; set; }
        [Display(Name = "用工性质")]
        public string WorkProperty { get; set; }
        [Display(Name = "加班需申请")]
        public Boolean ApplyOvertimeSwitch { get; set; }
        [Display(Name = "员工来源")]
        public string Source { get; set; }
        [Display(Name = "试用期满")]
        public string QualifyingPeriodFull { get; set; }
        [Display(Name = "婚姻状况")]
        public string MaritalStatus { get; set; }
        [Display(Name = "出生日期")]
        // [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> BirthDate { get; set; }
        [Display(Name = "籍贯")]
        public string NativePlace { get; set; }
        [Display(Name = "健康状况")]
        public string HealthCondition { get; set; }
        [Display(Name = "民族")]
        public string Nation { get; set; }
        [Display(Name = "家庭住址")]
        public string Address { get; set; }
        [Display(Name = "签证机关")]
        public string VisaOffice { get; set; }
        [Display(Name = "家庭电话")]
        //[RegularExpression(@"^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$",ErrorMessage="电话格式不正确")]
        public string HomeTelNumber { get; set; }
        [Display(Name = "学历")]
        public string EducationBackground { get; set; }
        [Display(Name = "毕业院校")]
        public string GraduationSchool { get; set; }
        [Display(Name = "专业")]
        public string SchoolMajor { get; set; }
        [Display(Name = "学位")]
        public string Degree { get; set; }
        [Display(Name = "介绍人")]
        public string Introducer { get; set; }
        [Display(Name = "个人手机")]
        [Phone]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "手机号码是11位数字")]
        public string IndividualTelNumber { get; set; }
        [Display(Name = "银行卡号")]
        public string BankCardNumber { get; set; }
        [Display(Name = "紧急联系人姓名")]
        public string UrgencyContactMan { get; set; }
        [Display(Name = "紧急联系人地址")]
        public string UrgencyContactAddress { get; set; }
        [Display(Name = "紧急联系人电话")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "手机号码是11位数字")]
        [Phone]
        public string UrgencyContactPhoneNumber { get; set; }
        [Display(Name = "黑名单")]
        public Boolean InBlacklist { get; set; }
        [Display(Name = "物理卡号")]
        public string PhysicalCardNumber { get; set; }
        [Display(Name = "离职日期")]
        public Nullable<DateTime> LeaveDate { get; set; }
        [Display(Name = "离职类型")]
        public string LeaveType { get; set; }
        [Display(Name = "离职原因")]
        public string LeaveReason { get; set; }
        // [Display(Name = "多版本")]
        //public string AccountVersion { get; set; }
        [Display(Name = "审核状态")]
        public string AuditStatus { get; set; }
        [Display(Name = "录入时间")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "录入人员")]
        public string RecordPerson { get; set; }
        [Display(Name = "更改时间")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "更改人员")]
        public string ChangePerson { get; set; }
        [Display(Name = "审核时间")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "审核人员")]
        public string AuditPerson { get; set; }
        //[Display(Name = "绑定码")]
        //public string BindingNumber { get; set; }
        // public virtual PagedList.IPagedList<Staff> Staffs { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public Boolean ArchiveTag { get; set; }

        public Boolean LongTime { get; set; }
       
    }
}