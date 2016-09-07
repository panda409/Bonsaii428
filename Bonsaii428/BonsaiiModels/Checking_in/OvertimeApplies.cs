namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OvertimeApplies
    {
        public int Id { get; set; }
        [Display(Name = "单别")]
        [Required]
        [StringLength(4)]
        public string BillType { get; set; }

        [Display(Name = "单号")]                                                                  
       
        public string BillNumber { get; set; }

        [Display(Name = "员工号")]
        [Required]
        [StringLength(10)]
        public string StaffNumber { get; set; }

        [Display(Name="已处理")]
        public bool IsRead { get; set; }

        [Display(Name = "开始日期时间")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "结束日期时间")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "加班时数")]
        public double Hours { get; set; }

        [Display(Name = "加班理由")]
        [StringLength(255)]
        public string Reason { get; set; }

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
        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }

        public DateTime Date { get; set; }
    }
}
