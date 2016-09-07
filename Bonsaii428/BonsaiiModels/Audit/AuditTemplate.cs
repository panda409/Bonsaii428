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
        [Display(Name = "单据类别")]
        public string BType { get; set; }

        [StringLength(50)]
        public string TypeName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "模板名称")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "描述")]
        public string Description { get; set; }


        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }


        [StringLength(11)]
        [Display(Name = "创建人员")]
        public string Creator { get; set; }

        public int? FirstStepSId { get; set; }

        [Required]
        [Display(Name = "生效时间")]
        public DateTime StartTime { get; set; }


        [Display(Name = "过期时间")]
        public DateTime? ExpireTime { get; set; }


        [Required]
        [Display(Name = "审核流程中共有")]
        [Range(1, 10)]
        public int StepCount { get; set; }

        [Display(Name = "长期有效")]
        public bool? LongTime { get; set; }

        //public virtual ICollection<AuditApplication> AuditApplications { get; set; }
    }
}
