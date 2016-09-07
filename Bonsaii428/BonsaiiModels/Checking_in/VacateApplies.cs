namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VacateApplies
    {
        public int Id { get; set; }
        [Display(Name = "单别")]
        [Required]
        [StringLength(4)]
        public string BillType { get; set; }

        [Display(Name = "单号")]
      //  [StringLength(10)]
        public string BillNumber { get; set; }

        [Display(Name = "员工号")]
        [Required]
        [StringLength(10)]
        public string StaffNumber { get; set; }

        [Display(Name = "开始日期")]
        [Required]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "结束日期")]
        [Required]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "请假时数")]
        [Required]
        public int Hours { get; set; }

        [Display(Name = "请假理由")]
        [StringLength(255)]
        public string Reason { get; set; }

        [Display(Name = "图片")]
        [StringLength(255)]
        public string Picture { get; set; }

        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [Display(Name = "审核时间")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "审核人员")]
        public string AuditPerson { get; set; }

        [Display(Name = "审核状态")]
        public byte AuditStatus { get; set; }

        [NotMapped]
         [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }
    }
}
