namespace Bonsaii.Models.Audit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditProcesses")]
    public partial class AuditProcess
    {
        [Key]
        [Required]
        [Display(Name="序号")]
        public int Id { get; set; }

        [Display(Name = "单据类别")]
        public int AId { get; set; }

        [Display(Name="节点")]
        public int SId { get; set; }

        [Display(Name = "单别编号")]
        [StringLength(4)]
        public string BType { get; set; }

        [Display(Name = "单别名称")]
        [StringLength(50)]
        public string TypeName { get; set; }

        [Display(Name = "单号")]
        [StringLength(50)]
        public string BNumber { get; set; }

        [Display(Name = "审核内容")]
        public string Info { get; set; }

        [Display(Name="模板")]
        public int TId { get; set; }

        [Display(Name = "审核截止日期")]
        public DateTime DeadlineDate { get; set; }

        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }

       [Display(Name = "审核日期")]
        public DateTime AuditDate { get; set; }

       [Display(Name = "审核人员")]
       public string AuditPerson { get; set; }

        [Display(Name = "审核状态")]
        public byte Result { get; set; }

        [Display(Name = "审核意见")]
        [StringLength(255)]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "所有审核人员")]
        public string Approver { get; set; }

        [NotMapped]
        [Display(Name = "审核状态")]
        public string ResultDescription { get; set; }

        [NotMapped]
        [Display(Name = "模板名称")]
        public string TemplateName { get; set; }

        [NotMapped]
        [Display(Name="部门")]
        public string DepartmentName { get; set; }

        [NotMapped]
        [Display(Name = "姓名/职务")]
        public string NameOrPosition { get; set; }

       // public virtual AuditApplication AuditApplications { get; set; }
    }
}
