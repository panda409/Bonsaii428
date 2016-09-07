namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StaffParamType")]
    public partial class StaffParamType
    {
        public StaffParamType()
        {
            StaffParam = new HashSet<StaffParam>();
        }

        [Display(Name="参数类型")]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "参数类型")]
        public string Name { get; set; }
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
        public virtual ICollection<StaffParam> StaffParam { get; set; }
    }
}
