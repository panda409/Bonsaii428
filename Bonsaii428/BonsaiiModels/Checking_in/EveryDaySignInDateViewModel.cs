
namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using BonsaiiModels.GlobalStaticVaribles;
    public  class EveryDaySignInDateViewModel
    {
        public int Id { get; set; }
        [Display(Name = "星期")]
        public string Week { get; set; }
        public DateTime Date { get; set; }              //日考勤报表的日期

        [Display(Name="日期")]
        public string ShowDate { get{
            return String.Format("{0:D}",Date);
        }}


        [Display (Name ="员工号")]
        public string StaffNumber { get; set; }         //员工号

        [StringLength(50)]
        [Display (Name ="员工姓名")]
        public  string StaffName{get;set;}

        [Display (Name ="公司")]
        public string CompanyName{get;set;}

        [Display (Name ="部门")]
        public string DepartmentName{get;set;}
        [Display (Name ="职务")]
        public string Position{get;set;}
        //在职是什么鬼public string  
        [Display(Name = "班次ID")]
        public int WorksId { get; set; }                         ////班次Id，用于找到员工对应的班次时间段信息（WorkTimes)
        [Display (Name= "班次名称")]
        [StringLength(50)]
        public string WorkName{get;set;}                                                                
        [Display(Name = "正班时数")]
        public double WorkHours { get; set; }              //正班时数，最后计算考勤的时候由打卡表计算出来
        [Display (Name ="正班天数")]
        public double? WorkDays { get; set; }       //正班天数，暂时不考虑
        [Display(Name ="正常加班")]
        public double NormalWorkOvertimeHours { get; set; }    //正常加班时数,最后由打卡表计算出来
        [Display(Name = "双休加班")]
        public double WeekendWorkOvertimeHours { get; set; }   //双休加班时数，最后由打卡表计算出来
        [StringLength(50)]
        [Display(Name = "请假类别")]
        public string VacateType { get; set; }                          //请假类型，由请假单的单别获得，预生成的时候读取请假单，从单别获得。
        [Display(Name = "请假时数")]
        public double VacateHours { get; set; }                           //请假时数，预生成的时候读取请假单获得。

        [Display(Name = "休假时数")]
        public double HolidayHours { get; set; }                          //休假时数，预生成的时候读取请假单获得。
        [Display(Name = "总加班时数")]
        public double TotalWorkOvertimeHours { get; set; }         //加班总时数，最后计算考勤的时候由打卡表计算出

        [Display(Name = "总迟到分钟")]
        public int TotalComeLateMinutes { get; set; }            //迟到总分钟数，最后计算考勤的时候由打卡表计算出

        [Display(Name = "总早退分钟")]
        public int TotalLeaveEarlyMinutes { get; set; }             //早退总分钟数，最后计算考勤的时候由打卡表计算出

        [Display(Name = "旷职时数")]
        public double AbsenteeismHours { get; set; }                    //旷职时数，最后计算考勤的时候由打卡表计算出

        [StringLength(20)]
        [Display(Name = "审核标识")]
        public string AuditStatus { get; set; }                            //审核状态，暂时不考虑
        [StringLength(50)]
        [Display(Name = "员工确认")]
        public string StaffConfirm { get; set; }                        //暂时不考虑

        [Display(Name = "是否是夜班")]
        public bool IsNightWork { get; set; }                         //是否夜班，暂时不考虑

        [Display(Name = "申请加班总时数" )]
        public double? WorkOvertimeHours { get; set; }             //申请加班总时数，这个值从加班申请单中获取，

        [Display(Name = "原始数据")]
        public string OriginalSignInData { get; set; }              //来源于机器的原始打卡数据,最后计算的时候写入


        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "是否出差")]
        public bool IsOnEvection { get; set; }
        public List<SignInCardStatus> SignInCard { get; set; }



    }
      }

