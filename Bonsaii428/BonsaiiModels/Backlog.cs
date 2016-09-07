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
        [Display(Name = "事件名称")]
        public string AcciName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "APP消息标题/邮件主题")]
        public string MessTitle { get; set; }

        [Required]
        [Display(Name = "APP消息内容/邮件正文")]
        public string MessContent { get; set; }

        [Required]
        [Display(Name = "收件人")]
        [Column(TypeName = "text")]
        public string Recipient { get; set; }

        [Display(Name = "姓名")]
        [Column(TypeName = "text")]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "手机号码")]
        public string TelNum { get; set; }

        [Display(Name = "电子邮箱")]
        [Column(TypeName = "text")]
        public string EmailAddr { get; set; }

        [Display(Name = "类型")]
        [StringLength(50)]
        public string Type { get; set; }

        [Display(Name = "发送到APP")]
        public bool SendMess { get; set; }

        [Display(Name = "发送到邮箱")]
        public bool Email { get; set; }

        [Display(Name = "开始日期")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "提示时间")]
        public TimeSpan? RemindTime { get; set; }

        [Required]
       // [StringLength(1)]
        [Display(Name = "循环方式")]
        public byte Cycle { get; set; }

        [Display(Name = "退出日期")]
        public DateTime? QuitTime { get; set; }

        [Display(Name = "是否启用")]
        public bool IsUse { get; set; }

        [Display(Name = "提醒日期")]//注：如果只提醒一次选择这个日期
        public DateTime? OnlyOneDate { get; set; }
    }
}
