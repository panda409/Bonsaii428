using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class StaffApplicationViewModel
    {
       
            [Key]
            [Required]
            public int Id { get; set; }

            [Required]
            [Display(Name = "单据类别编号")]
            [StringLength(4)]
            public string BillTypeNumber { get; set; }

            [Required]
            [Display(Name = "单据类别名称")]
            [StringLength(10)]
            public string BillTypeName { get; set; }

            [Required]
            [Display(Name = "单号")]
            [StringLength(50)]
            public string BillNumber { get; set; }
            [Required]
            [Display(Name = "员工工号")]
            [StringLength(50)]
            public string StaffNumber { get; set; }
            [Display(Name = "姓名")]
            [StringLength(100)]
            public string StaffName { get; set; }
            [Display(Name = "申请日期")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
            [Column(TypeName = "date")]
            public DateTime? ApplyDate { get; set; }
            [Display(Name = "期望离职日期")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
            [Column(TypeName = "date")]
            public DateTime? HopeLeaveDate { get; set; }
            [Display(Name = "离职类别")]
            [StringLength(50)]
            public string LeaveType { get; set; }
            [Display(Name = "离职原因")]
            [StringLength(200)]
            public string LeaveReason { get; set; }
            [Display(Name = "备注")]
            [StringLength(200)]
            public string Remark { get; set; }
            [Display(Name = "状态")]
          
            public byte AuditStatus { get; set; }
            [Display(Name = "录入时间")]
            public Nullable<DateTime> RecordTime { get; set; }
            [Display(Name = "录入人员")]
            public string RecordPerson { get; set; }
            [Display(Name = "更改时间")]
            public Nullable<DateTime> ChangeTime { get; set; }
            [Display(Name = "更改人员")]
            public string ChangePerson { get; set; }
            [Display(Name = "审核时间")]
            public Nullable<DateTime> AuditTime { get; set; }
            [Display(Name = "审核人员")]
            public string AuditPerson { get; set; }

            public string Description { get; set; }
            public string Value { get; set; }
            [Display(Name = "部门")]
            public string Department { get; set; }
        
    }
}