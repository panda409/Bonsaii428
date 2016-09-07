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
        [Display(Name = "事件名称")]
        public string EventName { get; set; }

        [StringLength(50)]
        [Display(Name = "APP消息标题/邮件主题")]
        public string MessageTitle { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "APP消息内容/邮件正文")]
        public string MessageBody { get; set; }

        [StringLength(50)]
        [Display(Name = "APP消息副标题")]
        public string MessageAlert { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "收件人")]
        public string Receiver { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "姓名")]
        public string ReceiverName { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "手机号码")]
        public string ReceiverTel { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "电子邮箱")]
        public string ReceiverEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "收件人类型")]
        public string ReceiverType { get; set; }

        [Display(Name = "是否发送到App")]
        public bool SendToApp { get; set; }

        [Display(Name = "是否发送到邮箱")]
        public bool IsEmail { get; set; }

        [Display(Name = "开始日期")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "提醒时间")]
        public TimeSpan? RemindDate { get; set; }

        [Display(Name = "循环方式")]

        public byte CirculateMethod { get; set; }

        [Display(Name = "结束日期")]
        public DateTime? EndDate { get; set; }

        

        [Column(TypeName = "text")]
        [Display(Name = "订阅")]
        public string SubscribeContent { get; set; }

        [Display(Name = "是否启用")]
        public bool IsAvailable { get; set; }

        [Display(Name = "提醒日期")]//注：如果只提醒一次选择这个日期
        public DateTime? OnlyOneDate { get; set; }
    }
}
