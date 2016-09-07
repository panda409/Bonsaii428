namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("TrainStarts")]
    public partial class TrainStart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        [Display(Name="����������")]
        public string BillTypeNumber { get; set; }

        //[Required]
        [StringLength(10)]
        [Display(Name="�����������")]
        public string BillTypeName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="����")]
        public string BillNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="��ѵ����")]
        public string TrainType { get; set; }
         [Required]
        [Display(Name="��ʼʱ��")]
        public DateTime StartDate { get; set; }
         [Required]
        [Display(Name="����ʱ��")]
        public DateTime EndDate { get; set; }

        //[Display(Name = "...")]
        //public string ExcelObject { get; set; }

        //[Display(Name = "����")]
        //[ConfigurationPropertyAttribute("maxRequestLength", DefaultValue = "10240")]
        //public string ExcelAttachmentName { get; set; }

        //public string ExcelAttachmentUrl { get; set; }

        //public string ExcelAttachmentType { get; set; }
         [Required]
        [StringLength(50)]
        [Display(Name="��ѵ����")]
        public string TrainCost { get; set; }
         [Required]
        [StringLength(50)]
        [Display(Name="��ϵ�绰")]
        public string TellNumber { get; set; }

        [Display(Name="���״̬")]
        public byte AuditStatus { get; set; }
        [Display(Name="���״̬")]
        [NotMapped]
        public string AuditStatusName { get; set; }
        [StringLength(50)]
        [Display(Name="��ע")]
        public string Remark { get; set; }
         [Required]
        [Display(Name = "��ѵ����")]
        public string TrainTheme { get; set; }
         [Required]
        [Display(Name = "��ѵ�ص�")]
        public string TrainPlace { get; set; }
         [Required]
       [Display(Name = "��ѵ��ʦ")]
        public string TrainPerson { get; set; }
         [Required]
       [Display(Name = "��ѵ������Ա")]
        public string TrainManage { get; set; }
         [Required]
        [Display(Name = "�μ���Ա")]
        public string JoinPerson { get; set; }
         [Required]
        [Display(Name = "��ϯ��Ա")]
        public string ChoosePerson { get; set; }
        [Display(Name = "��ѵ��������")]
        public string TrainContent { get; set; }
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
    }
}
