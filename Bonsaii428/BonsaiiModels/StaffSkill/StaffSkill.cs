using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("StaffSkills")]
    public class StaffSkill
    {
        [Key]
        public int Id { get; set; }
       // [Required]
        [Display(Name = "单据类别编号")]
        public string BillTypeNumber { get; set; }

      
       // [Required]
        [Display(Name = "单号")]
        public string BillNumber { get; set; }

        [Required]
        [Display(Name = "工号")]
        public string StaffNumber { get; set; }
       
        [Required]
        [Display(Name = "技能")]
        public string SkillNumber { get; set; }

       
   //     [Required]
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

        [Display(Name = "审核状态")]
        public byte AuditStatus { get; set; }

    }
}