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
        [Display(Name = "培训单号")]
        public string BillNumber { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "工号")]
        public string StaffNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "员工姓名")]
        public string StaffName { get; set; }
       [Display(Name="签到")]
        public Boolean Tag { get; set; }

        public DateTime? RecordTime { get; set; }
        [Display(Name = "单据性质编号")]
        public string BillTypeNumber { get; set; }
        [Display(Name = "培训时间")]
        public string Time { get; set; }
        public string RecordPerson { get; set; }
        public int TrainId { get; set; }
    }
}
