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

        [Display(Name="����")]
        public string ShowDate { get { return String.Format("{0:yyyy-MM}", Date); } }

        [Display(Name="Ա������")]
        public string StaffNumber { get; set; }
        [Display(Name="Ա������")]
        public string StaffName { get; set; }
        [Display(Name="��������")]
        public string DepartmentName { get; set; }
     
 

        [Display(Name="����ʱ��")]
        public double NormalWorkHours { get; set; }
        [Display(Name = "��������")]
        public int NormalWorkDays { get; set; }
        [Display(Name = "�Ӱ�����ʱ��")]
        public double OvertimeApplyHours { get; set; }
        [Display(Name = "�����Ӱ�ʱ��")]
        public double NormalOvertimeHours { get; set; }
        [Display(Name = "˫�ݼӰ�ʱ��")]
        public double WeekendOvertimeHours { get; set; }
        [Display(Name = "�ڼ��ռӰ�ʱ��")]
        public double HolidayOvertimeHours { get; set; }
        [Display(Name = "�����Ӱ�ʱ��")]
        public double OtherOvertimeHours { get; set; }
        [Display(Name = "�ܼӰ�ʱ��")]
        public double TotalOvertimeHours { get; set; }
        [Display(Name = "�ٵ�����")]
        public int ComeLateMinutes { get; set; }
        [Display(Name = "�ٵ�����")]
        public int ComeLateTimes { get; set; }
        [Display(Name = "���˷���")]
        public int LeaveEarlyMinutes { get; set; }
        [Display(Name = "���˴���")]
        public int LeaveEarlyTimes { get; set; }
        [Display(Name = "��ְʱ��")]
        public double AbsentHours { get; set; }
        [Display(Name = "��ְ����")]
        public int AbsentTimes { get; set; }
        [Display(Name = "�ݼ�����")]
        public int HolidayDays { get; set; }
        [Display(Name = "���ʱ��")]
        public double VacateHours { get; set; }
        [Display(Name = "�������")]
        public int VacateDays { get; set; }
        [Display(Name = "��ٴ���")]
        public int VacateTimes { get; set; }
        [Display(Name = "ҹ�����")]
        public int NightWorkTimes { get; set; }
        [Display(Name = "ǩ������")]
        public int ChargeCardTimes { get; set; }
        [Display(Name = "��н������")]
        public int HolidayWithMoneyDays { get; set; }
        [Display(Name = "�⿨")]
        public bool? FreeCard { get; set; }

        public byte? AuditStatus { get; set; }

        [StringLength(20)]
        public string AuditPerson { get; set; }

        public DateTime? AuditTime { get; set; }

        [Display(Name = "��ע")]
        [StringLength(255)]
        public string Remark { get; set; }
        [Display(Name = "Ա��ȷ��")]
        public bool? StaffConfirm { get; set; }
    }
}
