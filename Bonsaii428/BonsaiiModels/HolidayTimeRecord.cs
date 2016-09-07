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
        [Display(Name="����")]
        [StringLength(10)]
        public string Number { get; set; }

        [Display(Name = "ʱ��")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime RecordTimeHoliday { get; set; }
        [Display(Name = "")]
        public int? SortNumber { get; set; }
        [Display(Name = "��Ϣ״��")]
        [StringLength(2)]
        public string Tag { get; set; }
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
