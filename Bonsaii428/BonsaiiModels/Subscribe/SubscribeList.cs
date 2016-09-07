namespace BonsaiiModels
{
  
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubscribeLists")]
    public partial class SubscribeList
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="订阅名称")]
        [StringLength(50)]
        public string SubscribeName { get; set; }

         [Display(Name = "SQL查询")]
         [Column(TypeName = "text")]
        public string SQL { get; set; }

         [Display(Name = "SQL查询是否有效")]
        public bool IsSQLLegal { get; set; }

         [Display(Name = "是否启动")]
        public bool IsAvailable { get; set; }

         [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }

         [Display(Name = "APP消息标题/邮件主题")]
         [Column(TypeName = "text")]
         public string MessTitle { get; set; }

         [Display(Name = "APP消息内容/邮件正文")]
         [Column(TypeName = "text")]
         public string MessContent { get; set; }


       // public virtual ICollection<Subscribe_Company> Subscribe_Company { get; set; }
    }
}
