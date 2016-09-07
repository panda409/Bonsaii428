using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Bonsaii.Models
{
    [Table("BillProperties")]
    public partial class BillPropertyModels
    {
        public int Id { get; set; }

        [Display(Name = "单据类别")]
        public string BillSort { get; set; }

        [Display(Name = "单据性质")]
        public string Type { get; set; }
        [Required]
        [Display(Name = "单据性质名称")]
        [StringLength(50)]
        public string TypeName { get; set; }
        [Display(Name = "单据性质全称")]
        [StringLength(50)]
        public string TypeFullName { get; set; }

        [Display(Name = "单据编码方式")]
        public string CodeMethod { get; set; }

        [StringLength(10)]
        [Display(Name = "编码形式")]
        public string Code { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int SerialNumber { get; set; }
        [Display(Name = "审核方式")]
        public int IsAutoAudit { get; set; }
        [Display(Name = "走审批流程")]
        public bool? IsApprove { get; set; }
        [Display(Name = "单据限定输入用户")]
        public bool IsLimitInput { get; set; }
        [Display(Name = "增加减少")]
        public bool IsAscOrDesc { get; set; }

        public int Count { get; set; }
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

        [Display(Name = "是否需要打卡")]
        public bool NeedSignIn { get; set; }
    }
}
