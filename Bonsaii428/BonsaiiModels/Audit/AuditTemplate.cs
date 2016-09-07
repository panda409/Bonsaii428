namespace Bonsaii.Models.Audit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditTemplates")]
    public partial class AuditTemplate
    {
        [Key]

        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "�������")]
        public string BType { get; set; }

        [StringLength(50)]
        public string TypeName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "ģ������")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "����")]
        public string Description { get; set; }


        [Display(Name = "��������")]
        public DateTime CreateDate { get; set; }


        [StringLength(11)]
        [Display(Name = "������Ա")]
        public string Creator { get; set; }

        public int? FirstStepSId { get; set; }

        [Required]
        [Display(Name = "��Чʱ��")]
        public DateTime StartTime { get; set; }


        [Display(Name = "����ʱ��")]
        public DateTime? ExpireTime { get; set; }


        [Required]
        [Display(Name = "��������й���")]
        [Range(1, 10)]
        public int StepCount { get; set; }

        [Display(Name = "������Ч")]
        public bool? LongTime { get; set; }

        //public virtual ICollection<AuditApplication> AuditApplications { get; set; }
    }
}
