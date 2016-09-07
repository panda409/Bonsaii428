namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DaysOffApplies
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "单别")]
        public string BillType { get; set; }
        [Display(Name="单号")]
        [StringLength(10)]
        public string BillNumber { get; set; }
        [NotMapped]
        [Display(Name = "可用调休时数")]
        public double AvailableHours { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "员工号")]
        public string StaffNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Display(Name = "开始时间")]
        [Required]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "结束时间")]
        [Required]
        public DateTime EndDateTime { get; set; }
        [Display(Name="调休时数")]
        public double Hours { get; set; }

        [Display(Name="调休原因")]
        [StringLength(255)]
        public string Reason { get; set; }
        [Display(Name="备注")]
        [StringLength(255)]
        public string Remark { get; set; }

        public bool IsRead { get; set; }
        [Display(Name="审批状态")]
        public byte? AuditStatus { get; set; }
        [Display(Name = "审核人员")]
        [StringLength(20)]
        public string AuditPerson { get; set; }
        [Display(Name="审核时间")]
        public DateTime? AuditTime { get; set; }
        [NotMapped]
        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }
    }
}
