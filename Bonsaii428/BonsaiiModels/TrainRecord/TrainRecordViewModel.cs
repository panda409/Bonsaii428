namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
   
    public class TrainRecordViewModel
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "单据性质编号")]
        public string BillTypeNumber { get; set; }
          [Display(Name = "单据性质")]
        public string BillTypeName { get; set; }
          [Display(Name = "培训单号")]
        public string BillNumber { get; set; }
          [Display(Name = "部门")]
        public string DepartmentName { get; set; }

          [Display(Name = "工号")]
        public string StaffNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "员工姓名")]
        public string StaffName { get; set; }
       [Display(Name="签到")]
        public Boolean Tag { get; set; }

        public DateTime? RecordTime { get; set; }
        public string RecordPerson { get; set; }
          [Display(Name = "培训时间")]
        public string Time { get; set; }
          [Display(Name = "职位")]
        public string Position { get; set; }

          [Display(Name = "培训主题")]
          public string TrainTheme { get; set; }

          [Display(Name = "培训类型")]
          public string TrainType { get; set; }

          [Display(Name = "培训地点")]
          public string TrainPlace { get; set; }
    }
}
