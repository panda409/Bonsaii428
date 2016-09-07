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
        [Display(Name="����ֵ")]
        public string Value { get; set; }

        [Display(Name="��������")]
        public int StaffParamTypeId { get; set; }
        [Display(Name = "��������")]
        public virtual StaffParamType StaffParamType { get; set; }
        [Display(Name="����")]
        public int StaffParamOrder { get; set; }

        [Display(Name="Ĭ��ֵ")]
        public bool IsDefault { get; set; }
        [StringLength(30)]
        public string Extra { get; set; }
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
    }
}
