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
        [Display(Name = "����")]
        public int SId { get; set; }

        [Required]
        [Display(Name = "ģ��")]
        public int TId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="��������")]
        public string Name { get; set; }

      
        [StringLength(50)]
        [Display(Name = "��ע")]
        public string Description { get; set; }

        ////[Required]
        //[Display(Name = "�ϼ��ڵ�")]
        //public int PreId { get; set; }

        [Required]
        [Display(Name = "��һ������")]
        public int ApprovedToSId { get; set; }

        [Required]
        [Display(Name = "���ò��豻���أ�����ת��")]
        public int NotApprovedToSId { get; set; }

        [Required]
        [Range(1,30)]
        [Display(Name = "�������")]
        public int Days { get; set; }

     //   [Required]
        [Display(Name = "�����Ա")]
        public string Approver { get; set; }

      

      
    }
}
