using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity.Spatial;

namespace Bonsaii.Models
{
    [Table("Contracts")]
    public  class Contract
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [StringLength(4)]
        [Display(Name="�������")]
        public string BillTypeNumber { get; set; }

        [Display(Name = "�������")]
        public string BillTypeName { get; set; }
     
     
        [StringLength(50)]
        [Display(Name = "����")]
        public string BillNumber { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "��ͬ���")]
        public string ContractNumber { get; set; }

        [Display(Name = "�׷�")]
        public string FirstParty { get; set; }

        [StringLength(30)]
        [Display(Name = "�ҷ�")]
        public string SecondParty { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "ǩ��ʱ��")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SignDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "����ʱ��")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DueDate { get; set; }

        [StringLength(20)]
        [Display(Name = "���")]
        public string Amount { get; set; }

        [StringLength(50)]
        [Display(Name = "��ͬ����")]
        public string ContractObject { get; set; }
        [Display(Name = "��ͬ����")]
        [ConfigurationPropertyAttribute("maxRequestLength", DefaultValue = "10240")]
        public string ContractAttachmentName { get; set; }
       
        public string ContractAttachmentUrl { get; set; }
        public string ContractAttachmentType { get; set; }

        [StringLength(200)]
        [Display(Name = "��ע")]
        public string Remark { get; set; }
        [Display(Name = "¼��ʱ��")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "¼����Ա")]
        public string RecordPerson { get; set; }
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "������Ա")]
        public string ChangePerson { get; set; }
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
