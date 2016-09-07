using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models.Audit
{
    public class AuditStepViewModel
    {
        [Key]
        [Required]
        [Display(Name = "节点")]
        public int SId { get; set; }

        [Required]
        [Display(Name = "模板")]
        public int TId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "步骤名称")]
        public string Name { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "备注")]
        public string Description { get; set; }

        /*这里显示的是上级节点的名称*/
        //[Required]
        //[Display(Name = "上级节点")]
        //public string PreName { get; set; }

        [Required]
        [Display(Name = "下一个步骤")]
        public string ApprovedToSIdName { get; set; }

        [Required]
        [Display(Name = "若该步骤被驳回，则跳转到")]
        public string NotApprovedToSIdName { get; set; }


        [Required]
        [Display(Name = "审核天数")]
        public int Days { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "审核人员")]
        public string Approver { get; set; }

    }
}