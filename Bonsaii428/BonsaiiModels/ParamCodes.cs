namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParamCodes
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Display(Name="编码方式")]
        [StringLength(20)]
        public string CodeMethod { get; set; }

        [Display(Name="编码形式")]
       
        [StringLength(10)]
        public string Code { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int SerialNumber { get; set; }

        [Display(Name="参数名称")]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ParamName { get; set; }

        public int Count { get; set; }
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

        public string Type { get; set; }
    }
}
