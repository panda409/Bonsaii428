namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EvectionApplies
    {
        public int Id { get; set; }
        [StringLength(4)]
        [Display(Name = "����")]
        public string BillType { get; set; }

        [Display(Name = "����")]
        //[StringLength(10)]
        public string BillNumber { get; set; }

         [Display(Name = "Ա����")]
        [Required]
        [StringLength(10)]
        public string StaffNumber { get; set; }

         [Display(Name = "��ʼʱ��")]
        public DateTime StartDateTime { get; set; }

         [Display(Name="�Ѵ���")]
         public bool IsRead { get; set; }
         [Display(Name = "����ʱ��")]
        public DateTime EndDateTime { get; set; }

         [Display(Name = "��������")]
        public int Days { get; set; }

         [Display(Name = "��������")]

        [StringLength(255)]
        public string Reason { get; set; }

         [Display(Name = "����ص�")]
        [StringLength(255)]
        public string Location { get; set; }

         [Display(Name = "ͼƬ")]
        [StringLength(255)]
        public string Picture { get; set; }

         [Display(Name = "��ע")]
        [StringLength(255)]
        public string Remark { get; set; }

         [Display(Name = "���ʱ��")]
         public Nullable<DateTime> AuditTime { get; set; }
         [Display(Name = "�����Ա")]
         public string AuditPerson { get; set; }

         [Display(Name = "���״̬")]
         public byte AuditStatus { get; set; }
         [NotMapped]
         [Display(Name = "���״̬")]
         public string AuditStatusName { get; set; }
    }
}
