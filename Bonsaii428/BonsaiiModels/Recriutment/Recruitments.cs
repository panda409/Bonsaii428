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
        [Display(Name = "������")]
        public string BillType { get; set; }

        [Display(Name = "��������")]
        [StringLength(50)]
        public string BillTypeName { get; set; }
        [Display(Name = "����")]
     //   [StringLength(10)]
      //  [RegularExpression("[0-9]{10}", ErrorMessage = "������Ϸ���{0}")]
        public string BillCode { get; set; }

        [Display(Name = "���벿��")]
        public string DepartmentName { get; set; }
        [Display(Name = "��Ƹְ��")]
        public string Position { get; set; }
        [Required]
        [RegularExpression("[0-9]*", ErrorMessage = "������Ϸ���{0}")]
        [Display(Name = "��������")]
        public int? RequiredNumber { get; set; }
         [Required]
        [Display(Name = "�Ա�")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "����")]
        [RegularExpression("[1-9][0-9](-[1-9][0-9])?", ErrorMessage = "������Ϸ���{0}")]
        public string Age { get; set; }
         [Required]
        [Display(Name = "����״��")]
        public string MaritalStatus { get; set; }
         [Required]
        [Display(Name = "ѧ��")]
        public string EducationBackground { get; set; }
         [Required]
        [Display(Name = "רҵ")]
        public string Major { get; set; }
        [Required]
        [Display(Name = "��������")]
        public string WorkExperience { get; set; }
        [Required]
        [Display(Name = "����")]
        public string Skill { get; set; }
        [Display(Name = "��������")]
        public string Others { get; set; }
        [Display(Name = "״̬")]
        [StringLength(50)]
        public string Status { get; set; }

        public bool IsAudit { get; set; }

        public string PublishVersion { get; set; }

        public bool IsPublished { get; set; }
        [Display(Name = "¼��ʱ��")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "¼����Ա")]
        public string RecordPerson { get; set; }
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "������Ա")]
        public string ChangePerson { get; set; }
        [Display(Name = "���ʱ��")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "�����Ա")]
        public string AuditPerson { get; set; }
        [Display(Name = "���״̬")]
        public byte? AuditStatus { get; set; }

        [NotMapped]
        [Display(Name="���״̬")]
        public string AuditStatusName { get; set; }
    }
}
