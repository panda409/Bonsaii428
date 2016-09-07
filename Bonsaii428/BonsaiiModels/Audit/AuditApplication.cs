namespace Bonsaii.Models.Audit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditApplications")]
    public partial class AuditApplication
    {
        [Key]
        [Display(Name="序号")]
        public int Id { get; set; }

        [Display(Name="单别编号")]
        [StringLength(4)]
        public string BType { get; set; }

        [Display(Name="单别名称")]
        [StringLength(50)]
        public string TypeName { get; set; }

        [Display(Name = "单号")]
        [StringLength(50)]
        public string BNumber { get; set; }

        [Required]
        [Display(Name = "模板")]
        [Range(0, 1000)]
        public int TId { get; set; }

        [Display(Name="审核内容")]
        public string Info { get; set; }

        [Required]
        [Display(Name="提交人员")]
        [StringLength(11)]
        public string Creator { get; set; }

        [Display(Name = "提交人员")]
        public string CreatorName { get; set; }

         [Display(Name = "提交日期")]
        public DateTime CreateDate { get; set; }

         [Display(Name="状态")]
        public byte State { get; set; }

        [StringLength(50)]
         [Display(Name="备注")]
        public string Remark { get; set; }


        [NotMapped]
        [Display(Name = "审核状态")]
        public string StateDescription { get; set; }

        [NotMapped]
        [Display(Name = "模板名称")]
        public string TemplateName { get; set; }

        //public virtual AuditTemplate AuditTemplates { get; set; }

     //  public virtual ICollection<AuditProcesse> AuditProcesses { get; set; }
    }
}
