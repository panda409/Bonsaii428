namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Recruitments")]
    public partial class Recruitments
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "单别编号")]
        public string BillType { get; set; }

        [Display(Name = "单别名称")]
        [StringLength(50)]
        public string BillTypeName { get; set; }
        [Display(Name = "单号")]
     //   [StringLength(10)]
      //  [RegularExpression("[0-9]{10}", ErrorMessage = "请输入合法的{0}")]
        public string BillCode { get; set; }

        [Display(Name = "申请部门")]
        public string DepartmentName { get; set; }
        [Display(Name = "招聘职务")]
        public string Position { get; set; }
        [Required]
        [RegularExpression("[0-9]*", ErrorMessage = "请输入合法的{0}")]
        [Display(Name = "需求人数")]
        public int? RequiredNumber { get; set; }
         [Required]
        [Display(Name = "性别")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "年龄")]
        [RegularExpression("[1-9][0-9](-[1-9][0-9])?", ErrorMessage = "请输入合法的{0}")]
        public string Age { get; set; }
         [Required]
        [Display(Name = "婚姻状况")]
        public string MaritalStatus { get; set; }
         [Required]
        [Display(Name = "学历")]
        public string EducationBackground { get; set; }
         [Required]
        [Display(Name = "专业")]
        public string Major { get; set; }
        [Required]
        [Display(Name = "工作经验")]
        public string WorkExperience { get; set; }
        [Required]
        [Display(Name = "技能")]
        public string Skill { get; set; }
        [Display(Name = "其他条件")]
        public string Others { get; set; }
        [Display(Name = "状态")]
        [StringLength(50)]
        public string Status { get; set; }

        public bool IsAudit { get; set; }

        public string PublishVersion { get; set; }

        public bool IsPublished { get; set; }
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
        public byte? AuditStatus { get; set; }

        [NotMapped]
        [Display(Name="审核状态")]
        public string AuditStatusName { get; set; }
    }
}
