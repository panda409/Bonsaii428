namespace Bonsaii.Models.Audit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuditStep
    {
        [Key]
        [Required]
        [Display(Name = "步骤")]
        public int SId { get; set; }

        [Required]
        [Display(Name = "模板")]
        public int TId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="步骤名称")]
        public string Name { get; set; }

      
        [StringLength(50)]
        [Display(Name = "备注")]
        public string Description { get; set; }

        ////[Required]
        //[Display(Name = "上级节点")]
        //public int PreId { get; set; }

        [Required]
        [Display(Name = "下一个步骤")]
        public int ApprovedToSId { get; set; }

        [Required]
        [Display(Name = "若该步骤被驳回，则跳转到")]
        public int NotApprovedToSId { get; set; }

        [Required]
        [Range(1,30)]
        [Display(Name = "审核天数")]
        public int Days { get; set; }

     //   [Required]
        [Display(Name = "审核人员")]
        public string Approver { get; set; }

      

      
    }
}
