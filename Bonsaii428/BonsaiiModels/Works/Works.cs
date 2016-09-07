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
        [Display(Name = "�������")]
        public string Name { get; set; }

        [Display(Name = "���ܰ��")]
        public bool IsAutoWork { get; set; }
        [Display(Name = "�Զ���ʱ��")]
        public int? AutoWorkHours { get; set; }
        [Display(Name = "�Զ��೬�Ӱ�")]
        public bool AutoWorkExtraToOvertime { get; set; }
        [Display(Name = "������")]
        public bool IsOverDays { get; set; }
        [Display(Name = "����ʱ������")]
        public int TotalWorkHours { get; set; }
        [Display(Name = "�Ӱ�ʱ������")]
        public int? TotalOvertimeHours { get; set; }
        [Display(Name = "�ٵ�����")]
        public int? LatePunishment { get; set; }
        [Display(Name = "��ע")]
        [StringLength(255)]
        public string Remark { get; set; }
        [Display(Name = "")]
        public virtual ICollection<WorkTimes> WorkTimes { get; set; }
    }
}
