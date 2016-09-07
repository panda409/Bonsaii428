namespace BonsaiiModels.Subscribe
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubscribeAndWarning")]
    public partial class SubscribeAndWarning
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "�¼�����")]
        public string EventName { get; set; }

        [StringLength(50)]
        [Display(Name = "APP��Ϣ����/�ʼ�����")]
        public string MessageTitle { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "APP��Ϣ����/�ʼ�����")]
        public string MessageBody { get; set; }

        [StringLength(50)]
        [Display(Name = "APP��Ϣ������")]
        public string MessageAlert { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "�ռ���")]
        public string Receiver { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "����")]
        public string ReceiverName { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "�ֻ�����")]
        public string ReceiverTel { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "��������")]
        public string ReceiverEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "�ռ�������")]
        public string ReceiverType { get; set; }

        [Display(Name = "�Ƿ��͵�App")]
        public bool SendToApp { get; set; }

        [Display(Name = "�Ƿ��͵�����")]
        public bool IsEmail { get; set; }

        [Display(Name = "��ʼ����")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "����ʱ��")]
        public TimeSpan? RemindDate { get; set; }

        [Display(Name = "ѭ����ʽ")]

        public byte CirculateMethod { get; set; }

        [Display(Name = "��������")]
        public DateTime? EndDate { get; set; }

        

        [Column(TypeName = "text")]
        [Display(Name = "����")]
        public string SubscribeContent { get; set; }

        [Display(Name = "�Ƿ�����")]
        public bool IsAvailable { get; set; }

        [Display(Name = "��������")]//ע�����ֻ����һ��ѡ���������
        public DateTime? OnlyOneDate { get; set; }
    }
}
