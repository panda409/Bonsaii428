namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MonthSignInViewModel
    {
        public int Id { get; set; }


        public DateTime Date { get; set; }

        [Display(Name="日期")]
        public string ShowDate { get { return String.Format("{0:yyyy-MM}", Date); } }

        [Display(Name="员工工号")]
        public string StaffNumber { get; set; }
        [Display(Name="员工姓名")]
        public string StaffName { get; set; }
        [Display(Name="部门名称")]
        public string DepartmentName { get; set; }
     
 

        [Display(Name="正班时数")]
        public double NormalWorkHours { get; set; }
        [Display(Name = "正班天数")]
        public int NormalWorkDays { get; set; }
        [Display(Name = "加班申请时数")]
        public double OvertimeApplyHours { get; set; }
        [Display(Name = "正常加班时数")]
        public double NormalOvertimeHours { get; set; }
        [Display(Name = "双休加班时数")]
        public double WeekendOvertimeHours { get; set; }
        [Display(Name = "节假日加班时数")]
        public double HolidayOvertimeHours { get; set; }
        [Display(Name = "其他加班时数")]
        public double OtherOvertimeHours { get; set; }
        [Display(Name = "总加班时数")]
        public double TotalOvertimeHours { get; set; }
        [Display(Name = "迟到分钟")]
        public int ComeLateMinutes { get; set; }
        [Display(Name = "迟到次数")]
        public int ComeLateTimes { get; set; }
        [Display(Name = "早退分钟")]
        public int LeaveEarlyMinutes { get; set; }
        [Display(Name = "早退次数")]
        public int LeaveEarlyTimes { get; set; }
        [Display(Name = "旷职时数")]
        public double AbsentHours { get; set; }
        [Display(Name = "旷职次数")]
        public int AbsentTimes { get; set; }
        [Display(Name = "休假天数")]
        public int HolidayDays { get; set; }
        [Display(Name = "请假时数")]
        public double VacateHours { get; set; }
        [Display(Name = "请假天数")]
        public int VacateDays { get; set; }
        [Display(Name = "请假次数")]
        public int VacateTimes { get; set; }
        [Display(Name = "夜班次数")]
        public int NightWorkTimes { get; set; }
        [Display(Name = "签卡次数")]
        public int ChargeCardTimes { get; set; }
        [Display(Name = "有薪假天数")]
        public int HolidayWithMoneyDays { get; set; }
        [Display(Name = "免卡")]
        public bool? FreeCard { get; set; }

        public byte? AuditStatus { get; set; }

        [StringLength(20)]
        public string AuditPerson { get; set; }

        public DateTime? AuditTime { get; set; }

        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }
        [Display(Name = "员工确认")]
        public bool? StaffConfirm { get; set; }
    }
}
