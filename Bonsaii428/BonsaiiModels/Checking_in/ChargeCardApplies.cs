namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChargeCardApplies
    {
        public int Id { get; set; }
        [Display(Name = "单别")]
        [Required]
        [StringLength(4)]
        public string BillType { get; set; }

        [Display(Name = "单号")]  
        public string BillNumber { get; set; }
        [NotMapped]
        [Display(Name="单据类型")]
        public string BillTypeName { get; set; }
        [Display(Name = "员工号")]
        [Required]
        [StringLength(10)]
        public string StaffNumber { get; set; }

        [Display(Name="员工名称")]
        [NotMapped]
        public string StaffName{get;set;}
        [NotMapped]
        [Display(Name="部门名称")]
        public string DepartmentName { get; set; }

        [Display(Name = "签卡日期")]
        public DateTime DateTime { get; set; }
        [Display(Name="补签时间点")]
        public DateTime Date { get; set; }

        [Display(Name = "签卡原因")]
        [StringLength(255)]
        public string Reason { get; set; }

        [Display(Name="是否已处理")]
        public bool IsRead { get; set; }

        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }

        [Display(Name = "审核时间")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "审核人员")]
        public string AuditPerson { get; set; }

        [Display(Name = "审核状态")]
        public byte AuditStatus { get; set; }
        [NotMapped]
        [Display(Name ="审核状态")]
        public string AuditStatusName { get; set; }

    }
}
