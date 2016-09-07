 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
 using System.Data.Entity.Spatial;

namespace Bonsaii.Models
{
    [Table("StaffApplications")]
    public class StaffApplication
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name="���������")]
       
        public string BillTypeNumber { get; set; }

       
        [Display(Name="�����������")]
        [StringLength(10)]
        public string BillTypeName { get; set; }


        [Display(Name="����")]
        [StringLength(50)]
        public string BillNumber { get; set; }
        [Required]
        [Display(Name = "Ա������")]
        [StringLength(50)]
        public string StaffNumber { get; set; }
        [Display(Name = "����")]
        [StringLength(100)]
        public string StaffName { get; set; }
        [Display(Name="��������")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }
        [Display(Name = "������ְ����")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? HopeLeaveDate { get; set; }

        [Required]
        [Display(Name="��ְ���")]
        [StringLength(50)]
        public string LeaveType { get; set; }

        [Display(Name = "��ְԭ��")]
        [StringLength(200)]
        public string LeaveReason { get; set; }
        [Display(Name="��ע")]
        [StringLength(200)]
        public string Remark { get; set; }
        [Display(Name="״̬")]
        public byte AuditStatus { get; set; }

        [NotMapped]
        [Display(Name = "״̬")]
        public string AuditStatusName { get; set; }

        [Display(Name = "¼��ʱ��")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "¼����Ա")]
        public string RecordPerson { get; set; }
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "������Ա")]
        public string ChangePerson { get; set; }
        [Display(Name = "���ʱ��")]
        public DateTime? AuditTime { get; set; }
        [Display(Name = "�����Ա")]
        public string AuditPerson { get; set; }
        [Display(Name="����")]
        public string Department { get; set; }


    }
}
