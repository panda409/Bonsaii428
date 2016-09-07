using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Holidaies")]
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "假日名称")]
        public string JieJiaName { get; set; }
        [Display(Name = "开始时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime BeginTime { get; set; }
        [Display(Name = "结束时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndTime { get; set; }
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

    }
}