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
        [Display(Name="���")]
        public int Id { get; set; }

        [Display(Name = "�������")]
        public int AId { get; set; }

        [Display(Name="�ڵ�")]
        public int SId { get; set; }

        [Display(Name = "������")]
        [StringLength(4)]
        public string BType { get; set; }

        [Display(Name = "��������")]
        [StringLength(50)]
        public string TypeName { get; set; }

        [Display(Name = "����")]
        [StringLength(50)]
        public string BNumber { get; set; }

        [Display(Name = "�������")]
        public string Info { get; set; }

        [Display(Name="ģ��")]
        public int TId { get; set; }

        [Display(Name = "��˽�ֹ����")]
        public DateTime DeadlineDate { get; set; }

        [Display(Name = "��������")]
        public DateTime CreateDate { get; set; }

       [Display(Name = "�������")]
        public DateTime AuditDate { get; set; }

       [Display(Name = "�����Ա")]
       public string AuditPerson { get; set; }

        [Display(Name = "���״̬")]
        public byte Result { get; set; }

        [Display(Name = "������")]
        [StringLength(255)]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "���������Ա")]
        public string Approver { get; set; }

        [NotMapped]
        [Display(Name = "���״̬")]
        public string ResultDescription { get; set; }

        [NotMapped]
        [Display(Name = "ģ������")]
        public string TemplateName { get; set; }

        [NotMapped]
        [Display(Name="����")]
        public string DepartmentName { get; set; }

        [NotMapped]
        [Display(Name = "����/ְ��")]
        public string NameOrPosition { get; set; }

       // public virtual AuditApplication AuditApplications { get; set; }
    }
}
