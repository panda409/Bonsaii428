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


        [Display(Name = "�������ʱ��")]
        public string BillTypeNumber { get; set; }
          [Display(Name = "��������")]
        public string BillTypeName { get; set; }
          [Display(Name = "��ѵ����")]
        public string BillNumber { get; set; }
          [Display(Name = "����")]
        public string DepartmentName { get; set; }

          [Display(Name = "����")]
        public string StaffNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "Ա������")]
        public string StaffName { get; set; }
       [Display(Name="ǩ��")]
        public Boolean Tag { get; set; }

        public DateTime? RecordTime { get; set; }
        public string RecordPerson { get; set; }
          [Display(Name = "��ѵʱ��")]
        public string Time { get; set; }
          [Display(Name = "ְλ")]
        public string Position { get; set; }

          [Display(Name = "��ѵ����")]
          public string TrainTheme { get; set; }

          [Display(Name = "��ѵ����")]
          public string TrainType { get; set; }

          [Display(Name = "��ѵ�ص�")]
          public string TrainPlace { get; set; }
    }
}
