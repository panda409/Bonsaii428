namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Backlog")]
    public partial class Backlog
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "�¼�����")]
        public string AcciName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "APP��Ϣ����/�ʼ�����")]
        public string MessTitle { get; set; }

        [Required]
        [Display(Name = "APP��Ϣ����/�ʼ�����")]
        public string MessContent { get; set; }

        [Required]
        [Display(Name = "�ռ���")]
        [Column(TypeName = "text")]
        public string Recipient { get; set; }

        [Display(Name = "����")]
        [Column(TypeName = "text")]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "�ֻ�����")]
        public string TelNum { get; set; }

        [Display(Name = "��������")]
        [Column(TypeName = "text")]
        public string EmailAddr { get; set; }

        [Display(Name = "����")]
        [StringLength(50)]
        public string Type { get; set; }

        [Display(Name = "���͵�APP")]
        public bool SendMess { get; set; }

        [Display(Name = "���͵�����")]
        public bool Email { get; set; }

        [Display(Name = "��ʼ����")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "��ʾʱ��")]
        public TimeSpan? RemindTime { get; set; }

        [Required]
       // [StringLength(1)]
        [Display(Name = "ѭ����ʽ")]
        public byte Cycle { get; set; }

        [Display(Name = "�˳�����")]
        public DateTime? QuitTime { get; set; }

        [Display(Name = "�Ƿ�����")]
        public bool IsUse { get; set; }

        [Display(Name = "��������")]//ע�����ֻ����һ��ѡ���������
        public DateTime? OnlyOneDate { get; set; }
    }
}
