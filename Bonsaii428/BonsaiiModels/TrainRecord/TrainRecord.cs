namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
   [Table("TrainRecords")]
    public class TrainRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "��ѵ����")]
        public string BillNumber { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "����")]
        public string StaffNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "Ա������")]
        public string StaffName { get; set; }
       [Display(Name="ǩ��")]
        public Boolean Tag { get; set; }

        public DateTime? RecordTime { get; set; }
        [Display(Name = "�������ʱ��")]
        public string BillTypeNumber { get; set; }
        [Display(Name = "��ѵʱ��")]
        public string Time { get; set; }
        public string RecordPerson { get; set; }
        public int TrainId { get; set; }
    }
}
