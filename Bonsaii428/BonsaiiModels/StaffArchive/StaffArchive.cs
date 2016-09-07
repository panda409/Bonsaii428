 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Bonsaii.Models
{
    [Table("StaffArchives")]
    public class StaffArchive
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "单据类别编号")]
        //[StringLength(4)]
        public string BillTypeNumber { get; set; }

     //   [Required]
        [Display(Name = "单据类别名称")]
       // [StringLength(10)]
        public string BillTypeName { get; set; }

    //    [Required]
        [Display(Name = "单号")]
       // [StringLength(50)]
        public string BillNumber { get; set; }

        [Required]
        [Display(Name = "员工工号")]
        [StringLength(50)]
        public string StaffNumber { get; set; }

        [Required]
        [Display(Name = "姓名")]
        [StringLength(50)]
        public string StaffName { get; set; }

        [Display(Name = "离职日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? LeaveDate { get; set; }

       // [Required]
        [Display(Name = "再次入职日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? ReApplyDate { get; set; }
        [Display(Name = "备注")]
        [StringLength(200)]
        public string Remark { get; set; }
        [Display(Name = "证件号码")]
        public string IdenticationNumber { get; set; }
        [Display(Name = "黑名单")]
        public bool BlackList { get; set; }
        [Display(Name = "工龄是否相加")]
        public bool WorkPlus { get; set; }
        [Display(Name = "标识")]
        public bool Tag { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }
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
    }
}
