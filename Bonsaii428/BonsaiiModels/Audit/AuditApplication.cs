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
        [Display(Name="���")]
        public int Id { get; set; }

        [Display(Name="������")]
        [StringLength(4)]
        public string BType { get; set; }

        [Display(Name="��������")]
        [StringLength(50)]
        public string TypeName { get; set; }

        [Display(Name = "����")]
        [StringLength(50)]
        public string BNumber { get; set; }

        [Required]
        [Display(Name = "ģ��")]
        [Range(0, 1000)]
        public int TId { get; set; }

        [Display(Name="�������")]
        public string Info { get; set; }

        [Required]
        [Display(Name="�ύ��Ա")]
        [StringLength(11)]
        public string Creator { get; set; }

        [Display(Name = "�ύ��Ա")]
        public string CreatorName { get; set; }

         [Display(Name = "�ύ����")]
        public DateTime CreateDate { get; set; }

         [Display(Name="״̬")]
        public byte State { get; set; }

        [StringLength(50)]
         [Display(Name="��ע")]
        public string Remark { get; set; }


        [NotMapped]
        [Display(Name = "���״̬")]
        public string StateDescription { get; set; }

        [NotMapped]
        [Display(Name = "ģ������")]
        public string TemplateName { get; set; }

        //public virtual AuditTemplate AuditTemplates { get; set; }

     //  public virtual ICollection<AuditProcesse> AuditProcesses { get; set; }
    }
}
