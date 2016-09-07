using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models.Works
{
    public class WorkArrangeViewModel
    {
        [Display(Name="开始日期")]
        public string StartDate { get; set; }
        [Display(Name="结束日期")]
        public string EndDate { get;set;}

        [Display(Name = "日期")]
        public DateTime Date { get; set; }
        public int WorksId { get; set; }
        [Display(Name="班次名称")]
        public string WorksName { get; set; }
        public string DepartmentId { get; set; }
        [Display(Name="部门名称")]
        public string DepartmentName { get; set; }
         [Display(Name = "员工编号")]
        public string StaffNumber { get; set; }
        [Display(Name="员工姓名")]
        public string StaffName { get; set; }
        [Display(Name="备注")]
        public string Remark { get; set; }

    }
}