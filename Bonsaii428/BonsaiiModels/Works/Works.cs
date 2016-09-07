namespace Bonsaii.Models.Works
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Works
    {
        public Works()
        {
            WorkTimes = new HashSet<WorkTimes>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "班次名称")]
        public string Name { get; set; }

        [Display(Name = "智能班次")]
        public bool IsAutoWork { get; set; }
        [Display(Name = "自动班时数")]
        public int? AutoWorkHours { get; set; }
        [Display(Name = "自动班超加班")]
        public bool AutoWorkExtraToOvertime { get; set; }
        [Display(Name = "跨天班次")]
        public bool IsOverDays { get; set; }
        [Display(Name = "正班时数汇总")]
        public int TotalWorkHours { get; set; }
        [Display(Name = "加班时数汇总")]
        public int? TotalOvertimeHours { get; set; }
        [Display(Name = "迟到扣数")]
        public int? LatePunishment { get; set; }
        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }
        [Display(Name = "")]
        public virtual ICollection<WorkTimes> WorkTimes { get; set; }
    }
}
