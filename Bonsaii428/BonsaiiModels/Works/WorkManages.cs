namespace Bonsaii.Models.Works
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WorkManages
    {
        public int Id { get; set; }

        [NotMapped]
        [Display(Name="开始日期")]
        public DateTime StartDate { get; set; }
        [Display(Name="结束日期")]
        [NotMapped]
        public DateTime EndDate { get; set; }

        public DateTime Date { get; set; }
        [Required]
        [Display(Name="班次")]
        public int WorksId { get; set; }

        [Display(Name="班次名称")]
        [NotMapped]
        public string WorksName { get; set; }
        [Display(Name = "审批状态")]
        public byte AuditStatus { get; set; }
        [Display(Name="员工编号")]
        [StringLength(50)]
        public string StaffNumber { get; set; }
        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }  
        [Display(Name="部门")]
        public string DepartmentId { get; set; }
        [Display(Name="部门名称")]
        [NotMapped]
        public string DepartmentName { get; set; }
        public bool Flag { get; set; }
    }
}
