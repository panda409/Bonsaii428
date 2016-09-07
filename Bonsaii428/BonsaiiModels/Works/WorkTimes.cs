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
        [Display(Name="�ϰ�ʱ��")]
        public TimeSpan StartTime { get; set; }
        [Display(Name = "�°�ʱ��")]
        public TimeSpan EndTime { get; set; }
        [Display(Name = "����ʱ��")]
        public int WorkHours { get; set; }
        [Display(Name = "�Ӱ�ʱ��")]
        public int? OvettimeHours { get; set; }
        [Display(Name = "��ǰ����")]
        public int AheadMinutes { get; set; }
        [Display(Name = "�ƺ����")]
        public int BackMinutes { get; set; }
        [Display(Name = "�ٵ�����")]
        public int LateMinutes { get; set; }
        [Display(Name = "���˷���")]
        public int LeaveEarlyMinutes { get; set; }
        [Display(Name = "��ǰ�ƼӰ�")]
        public bool IsAheadToOvertime { get; set; }
        [Display(Name = "�ƳټƼӰ�")]
        public bool IsBackToOvertime { get; set; }
        public virtual Works Works { get; set; }
    }
}
