namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EveryDaySignInDate")]
    public partial class EveryDaySignInDate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Week { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }              //日考勤报表的日期

        [Required]
        [StringLength(50)]
        public string StaffNumber { get; set; }         //员工号

        public int WorksId { get; set; }                    //班次Id，用于找到员工对应的班次时间段信息（WorkTimes)
        public double WorkHours { get; set; }              //正班时数，最后计算考勤的时候由打卡表计算出来

        public double? WorkDays { get; set; }       //正班天数，暂时不考虑

        public double NormalWorkOvertimeHours { get; set; }    //正常加班时数,最后由打卡表计算出来

        public double WeekendWorkOvertimeHours { get; set; }   //双休加班时数，最后由打卡表计算出来

        [StringLength(50)]
        public string VacateType { get; set; }                          //请假类型，由请假单的单别获得，预生成的时候读取请假单，从单别获得。

        public double VacateHours { get; set; }                           //请假时数，预生成的时候读取请假单获得。

        public double HolidayHours { get; set; }                          //休假时数，预生成的时候读取请假单获得。

        public double TotalWorkOvertimeHours { get; set; }         //加班总时数，最后计算考勤的时候由打卡表计算出

        public int TotalComeLateMinutes { get; set; }            //迟到总分钟数，最后计算考勤的时候由打卡表计算出

        public int TotalLeaveEarlyMinutes { get; set; }             //早退总分钟数，最后计算考勤的时候由打卡表计算出

        public double AbsenteeismHours { get; set; }                    //旷职时数，最后计算考勤的时候由打卡表计算出

        [StringLength(20)]
        public string AuditStatus { get; set; }                            //审核状态，暂时不考虑

        [StringLength(50)]
        public string StaffConfirm { get; set; }                        //暂时不考虑

        public bool IsNightWork { get; set; }                         //是否夜班，暂时不考虑

        public double WorkOvertimeHours { get; set; }             //申请加班总时数，这个值从加班申请单中获取，

        [StringLength(50)]
        public string OriginalSignInData { get; set; }              //来源于机器的原始打卡数据,最后计算的时候写入

        [StringLength(255)]
        public string Remark { get; set; }


        public bool IsOnEvection { get; set; }

        public string OvertimeType { get; set; }

        //define a constructor with parameterless
        public EveryDaySignInDate()
        {
        }
        public EveryDaySignInDate(string StaffNumber, int WorksId,DateTime CurrentDate)
        {
            this.Week = DateTime.Now.DayOfWeek.ToString();
            this.Date = CurrentDate;
            this.StaffNumber = StaffNumber;
            this.WorksId = WorksId;
            this.WorkHours = 0;
            this.NormalWorkOvertimeHours = 0;
            this.WeekendWorkOvertimeHours = 0;
            this.TotalComeLateMinutes = 0;
            this.TotalWorkOvertimeHours = 0;
            this.TotalLeaveEarlyMinutes = 0;
            this.AbsenteeismHours = 0;
            this.WorkOvertimeHours = 0;
            this.IsOnEvection = false;
            this.IsNightWork = false;
            this.VacateHours = 0;
        }
    }
}