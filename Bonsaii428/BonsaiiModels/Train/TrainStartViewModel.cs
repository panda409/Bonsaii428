using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii.Models
{
    public class TrainStartViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "单据类别编码")]
        public string BillTypeNumber { get; set; }

        //[Required]
        [StringLength(10)]
        [Display(Name = "单据类别名称")]
        public string BillTypeName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "单号")]
        public string BillNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "培训类型")]
        public string TrainType { get; set; }
        [Required]
        [Display(Name = "开始时间")]
        public DateTime? StartDate { get; set; }
        [Required]
        [Display(Name = "结束时间")]
        public DateTime? EndDate { get; set; }

        //[Display(Name = "...")]
        //public string ExcelObject { get; set; }

        //[Display(Name = "附件")]
        //[ConfigurationPropertyAttribute("maxRequestLength", DefaultValue = "10240")]
        //public string ExcelAttachmentName { get; set; }

        //public string ExcelAttachmentUrl { get; set; }

        //public string ExcelAttachmentType { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "培训费用")]
        public string TrainCost { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "联系电话")]
        public string TellNumber { get; set; }

        [Display(Name = "审核状态")]
        public byte AuditStatus { get; set; }
        [Display(Name="审核状态")]
        [NotMapped]
        public string AuditStatusName{get;set;}
        [StringLength(50)]
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Required]
        [Display(Name = "培训主题")]
        public string TrainTheme { get; set; }
        [Required]
        [Display(Name = "培训地点")]
        public string TrainPlace { get; set; }
        [Required]
        [Display(Name = "培训讲师")]
        public string TrainPerson { get; set; }
        [Required]
        [Display(Name = "培训管理人员")]
        public string TrainManage { get; set; }
        [Required]
        [Display(Name = "必须参加人员")]
        public string JoinPerson { get; set; }
        [Required]
        [Display(Name = "可以参加人员")]
        public string ChoosePerson { get; set; }
        [Display(Name = "培训内容描述")]
        public string TrainContent { get; set; }
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
        public string Description { get; set; }
        public string Value { get; set; }
         
    }
}
