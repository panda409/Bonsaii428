namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("HolidayTimeRecords")]
    public partial class HolidayTimeRecord
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="工号")]
        [StringLength(10)]
        public string Number { get; set; }

        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime RecordTimeHoliday { get; set; }
        [Display(Name = "")]
        public int? SortNumber { get; set; }
        [Display(Name = "休息状况")]
        [StringLength(2)]
        public string Tag { get; set; }
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
