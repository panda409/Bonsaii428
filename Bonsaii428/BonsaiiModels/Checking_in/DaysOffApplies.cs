namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DaysOffApplies
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "����")]
        public string BillType { get; set; }
        [Display(Name="����")]
        [StringLength(10)]
        public string BillNumber { get; set; }
        [NotMapped]
        [Display(Name = "���õ���ʱ��")]
        public double AvailableHours { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Ա����")]
        public string StaffNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Display(Name = "��ʼʱ��")]
        [Required]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "����ʱ��")]
        [Required]
        public DateTime EndDateTime { get; set; }
        [Display(Name="����ʱ��")]
        public double Hours { get; set; }

        [Display(Name="����ԭ��")]
        [StringLength(255)]
        public string Reason { get; set; }
        [Display(Name="��ע")]
        [StringLength(255)]
        public string Remark { get; set; }

        public bool IsRead { get; set; }
        [Display(Name="����״̬")]
        public byte? AuditStatus { get; set; }
        [Display(Name = "�����Ա")]
        [StringLength(20)]
        public string AuditPerson { get; set; }
        [Display(Name="���ʱ��")]
        public DateTime? AuditTime { get; set; }
        [NotMapped]
        [Display(Name = "���״̬")]
        public string AuditStatusName { get; set; }
    }
}
