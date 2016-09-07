namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemMessages")]
    public partial class SystemMessage
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name="消息标题")]
        public string MessTitle { get; set; }

        [Column(TypeName = "text")]
        [Display(Name="消息内容")]
        public string MessBody { get; set; }

        [Column(TypeName = "date")]
        [Display(Name="时间")]
        public DateTime MessTime { get; set; }

        public bool? IsRead { get; set; }
        
        public string CompanyId { get; set; }

        [StringLength(11)]
        public string UserName { get; set; }

        [Display(Name="是否发送成功")]
        public byte? SendStatus { get; set; }


        [Display(Name = "类型")]
        [StringLength(50)]
        public string MessType { get; set; }

        [Display(Name = "收件人")]
       
        public string MessReceiver { get; set; }

      
    }
}
