namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HolidayTables
    {
        public int Id { get; set; }
        [Display(Name="员工工号")]

        [StringLength(50)]
        public string StaffNumber { get; set; }

        [Display(Name="员工")]
        [NotMapped]
        public string Staffs { get; set; }
        [Display(Name="部门")]
        [NotMapped]
        public string DptIds { get; set; }
        [Display(Name="放假日期")]
        [Column(TypeName = "date")]
        [Required]
        public DateTime Date { get; set; }
        [Display(Name = "部门名称")]
        public string DepartmentId { get; set; }
        [StringLength(20)]
        [Display(Name="假期类型")]
        public string Type { get; set; }
        [Required]
        [Display(Name="开始时间")]
        public int StartHour { get; set; }
        [Required]
        [Display(Name="结束时间")]
        public int EndHour { get; set; }

        [Display(Name="备注")]
        [StringLength(255)]
        public string Remark { get; set; }

        public bool Flag { get; set; }

    }


    public class DepartmentHolidayViewModel
    {
        public string DepartmentId { get; set; }
        [Display(Name="部门名称")]
        public string DepartmentName { get; set; }
        [Display(Name = "假日日期")]
        public DateTime Date { get; set; }
        [Display(Name = "假日开始时数")]
        public int StartHour { get; set; }
        [Display(Name = "假日结束时数")]
        public int EndHour { get; set; }
        [Display(Name="假日类型")]
        public string Type { get; set; }
        [Display(Name="备注")]
        public string Remark { get; set; }
    }
}
