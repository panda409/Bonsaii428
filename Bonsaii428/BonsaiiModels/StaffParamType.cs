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

        [Display(Name="��������")]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "��������")]
        public string Name { get; set; }
        [Display(Name = "¼��ʱ��")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "¼����Ա")]
        public string RecordPerson { get; set; }
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "������Ա")]
        public string ChangePerson { get; set; }
        [Display(Name = "���ʱ��")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "�����Ա")]
        public string AuditPerson { get; set; }
        public virtual ICollection<StaffParam> StaffParam { get; set; }
    }
}
