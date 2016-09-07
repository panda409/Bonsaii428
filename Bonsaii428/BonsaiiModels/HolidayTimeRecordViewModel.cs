using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class HolidayTimeRecordViewModel
    {
        public int Id { get; set; }
        [Display(Name="工号")]
        public string Number { get; set; }
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }
        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime RecordTimeHoliday { get; set; }
        [Display(Name = "休息状况")]
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