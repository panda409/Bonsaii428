namespace Bonsaii.Models.Works
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WorkTimes
    {
        public int Id { get; set; }

        public int WorksId { get; set; }
        [Display(Name="上班时间")]
        public TimeSpan StartTime { get; set; }
        [Display(Name = "下班时间")]
        public TimeSpan EndTime { get; set; }
        [Display(Name = "正班时数")]
        public int WorkHours { get; set; }
        [Display(Name = "加班时数")]
        public int? OvettimeHours { get; set; }
        [Display(Name = "提前分钟")]
        public int AheadMinutes { get; set; }
        [Display(Name = "推后分钟")]
        public int BackMinutes { get; set; }
        [Display(Name = "迟到分钟")]
        public int LateMinutes { get; set; }
        [Display(Name = "早退分钟")]
        public int LeaveEarlyMinutes { get; set; }
        [Display(Name = "提前计加班")]
        public bool IsAheadToOvertime { get; set; }
        [Display(Name = "推迟计加班")]
        public bool IsBackToOvertime { get; set; }
        public virtual Works Works { get; set; }
    }
}
