namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DataSubscriptions
    {
        public int Id { get; set; }

 //       [Required]
        [StringLength(4)]
        [Display(Name = "����")]
        public string BillType { get; set; }

 //       [Required]
        [StringLength(10)]
        [Display(Name = "����")]
        public string BillCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "�¼�����")]
        public string EventName { get; set; }

        [StringLength(50)]
        [Display(Name = "��Ϣ����")]
        public string MessageTitle { get; set; }

        [StringLength(255)]
        [Display(Name = "��Ϣ���ݣ�ͷ��")]
        public string MessageHead { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "��Ϣ���ݣ���")]
        public string MessageBody { get; set; }

        [StringLength(255)]
        [Display(Name = "��Ϣ���ݣ�β��")]
        public string MessageTail { get; set; }
        [Display(Name = "��Ϣ")]
        public bool IsMessage { get; set; }
        [Display(Name = "�ʼ�")]
        public bool IsEmail { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "��ʼ����")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "����ʱ��")]
        public DateTime? RemindDate { get; set; }


        [Display(Name = "ѭ����ʽ")]
        public int CirculateMethod { get; set; }

        public DateTime NextSendDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "�˳�����")]
        public DateTime? EndDate { get; set; }

        [StringLength(255)]
        [Display(Name = "SQL����")]
        public string SQL { get; set; }
        [Display(Name = "û�з��ϲ���")]
        public bool IsSQLLegal { get; set; }

        [Display(Name="��ʾ�ֶ�")]
        [StringLength(255)]
        public string FieldName { get; set; }
        [Display(Name="�Ƿ�����")]
        public bool IsAvailable { get; set; }
    }
}
