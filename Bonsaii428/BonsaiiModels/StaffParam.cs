namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StaffParam")]
    public partial class StaffParam
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name="参数值")]
        public string Value { get; set; }

        [Display(Name="参数类型")]
        public int StaffParamTypeId { get; set; }
        [Display(Name = "参数类型")]
        public virtual StaffParamType StaffParamType { get; set; }
        [Display(Name="排序")]
        public int StaffParamOrder { get; set; }

        [Display(Name="默认值")]
        public bool IsDefault { get; set; }
        [StringLength(30)]
        public string Extra { get; set; }
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
