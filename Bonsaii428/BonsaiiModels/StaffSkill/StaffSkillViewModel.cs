using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class StaffSkillViewModel
    {
       
        public int Id { get; set; }
        [Required]
        [Display(Name = "单据类别编号")]
        public string BillTypeNumber { get; set; }

       
        [Display(Name = "单据类别名称")]
        public string BillTypeName { get; set; }
        [Required]
        [Display(Name = "单号")]
        public string BillNumber { get; set; }
        [Required]
        [Display(Name = "员工工号")]
        public string StaffNumber { get; set; }
        
        [Display(Name = "姓名")]
        public string StaffName { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }
        [Required]
        [Display(Name = "技能")]
        public string SkillNumber { get; set; }

        [Display(Name = "技能名称")]
        public string SkillName { get; set; }
      
        [Required]
        [Display(Name = "生效日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ValidDate { get; set; }
        [Display(Name = "备注")]
        public string SkillRemark { get; set; }
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


        [Display(Name = "审核状态")]
        public byte AuditStatus { get; set; }

        [NotMapped]
        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }
    }
}