
namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class HolidayTableViewModel
    {
        public int Id { get; set; }
        [Display(Name = "员工工号")]
        [Required]
        [StringLength(50)]
        public string StaffNumber { get; set; }
        [Display(Name="员工姓名")]
        public string StaffName { get; set; }

        public string DepartmentId { get; set; }
        [Display(Name = "部门名称")]
        public string DepartmentName { get; set; }
        [Display(Name = "放假日期")]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "假期类型")]
        public string Type { get; set; }
        [Display(Name = "开始时间")]
        public int StartHour { get; set; }
        [Display(Name = "结束时间")]
        public int EndHour { get; set; }
        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }
        public bool Flag { get; set; }
    }
}