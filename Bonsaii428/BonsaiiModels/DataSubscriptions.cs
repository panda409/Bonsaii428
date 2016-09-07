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
        [Display(Name = "单别")]
        public string BillType { get; set; }

 //       [Required]
        [StringLength(10)]
        [Display(Name = "单号")]
        public string BillCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "事件名称")]
        public string EventName { get; set; }

        [StringLength(50)]
        [Display(Name = "消息标题")]
        public string MessageTitle { get; set; }

        [StringLength(255)]
        [Display(Name = "消息内容（头）")]
        public string MessageHead { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "消息内容（身）")]
        public string MessageBody { get; set; }

        [StringLength(255)]
        [Display(Name = "消息内容（尾）")]
        public string MessageTail { get; set; }
        [Display(Name = "消息")]
        public bool IsMessage { get; set; }
        [Display(Name = "邮件")]
        public bool IsEmail { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "开始日期")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "提醒时间")]
        public DateTime? RemindDate { get; set; }


        [Display(Name = "循环方式")]
        public int CirculateMethod { get; set; }

        public DateTime NextSendDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "退出日期")]
        public DateTime? EndDate { get; set; }

        [StringLength(255)]
        [Display(Name = "SQL条件")]
        public string SQL { get; set; }
        [Display(Name = "没有符合不发")]
        public bool IsSQLLegal { get; set; }

        [Display(Name="显示字段")]
        [StringLength(255)]
        public string FieldName { get; set; }
        [Display(Name="是否启用")]
        public bool IsAvailable { get; set; }
    }
}
