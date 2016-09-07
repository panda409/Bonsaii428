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
        [Display(Name="单据类别")]
        public string BillTypeNumber { get; set; }

        [Display(Name = "单据类别")]
        public string BillTypeName { get; set; }
     
     
        [StringLength(50)]
        [Display(Name = "单号")]
        public string BillNumber { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "合同编号")]
        public string ContractNumber { get; set; }

        [Display(Name = "甲方")]
        public string FirstParty { get; set; }

        [StringLength(30)]
        [Display(Name = "乙方")]
        public string SecondParty { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "签订时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SignDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "到期时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DueDate { get; set; }

        [StringLength(20)]
        [Display(Name = "金额")]
        public string Amount { get; set; }

        [StringLength(50)]
        [Display(Name = "合同标题")]
        public string ContractObject { get; set; }
        [Display(Name = "合同附件")]
        [ConfigurationPropertyAttribute("maxRequestLength", DefaultValue = "10240")]
        public string ContractAttachmentName { get; set; }
       
        public string ContractAttachmentUrl { get; set; }
        public string ContractAttachmentType { get; set; }

        [StringLength(200)]
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "录入时间")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "录入人员")]
        public string RecordPerson { get; set; }
        [Display(Name = "更改时间")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "更改人员")]
        public string ChangePerson { get; set; }
        [Display(Name = "审核时间")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "审核人员")]
        public string AuditPerson { get; set; }

        [Display(Name = "审核状态")]
        public byte AuditStatus { get; set; }
        [NotMapped]
        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }
    }
}
