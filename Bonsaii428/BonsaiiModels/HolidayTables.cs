namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HolidayTables
    {
        public int Id { get; set; }
        [Display(Name="Ա������")]

        [StringLength(50)]
        public string StaffNumber { get; set; }

        [Display(Name="Ա��")]
        [NotMapped]
        public string Staffs { get; set; }
        [Display(Name="����")]
        [NotMapped]
        public string DptIds { get; set; }
        [Display(Name="�ż�����")]
        [Column(TypeName = "date")]
        [Required]
        public DateTime Date { get; set; }
        [Display(Name = "��������")]
        public string DepartmentId { get; set; }
        [StringLength(20)]
        [Display(Name="��������")]
        public string Type { get; set; }
        [Required]
        [Display(Name="��ʼʱ��")]
        public int StartHour { get; set; }
        [Required]
        [Display(Name="����ʱ��")]
        public int EndHour { get; set; }

        [Display(Name="��ע")]
        [StringLength(255)]
        public string Remark { get; set; }

        public bool Flag { get; set; }

    }


    public class DepartmentHolidayViewModel
    {
        public string DepartmentId { get; set; }
        [Display(Name="��������")]
        public string DepartmentName { get; set; }
        [Display(Name = "��������")]
        public DateTime Date { get; set; }
        [Display(Name = "���տ�ʼʱ��")]
        public int StartHour { get; set; }
        [Display(Name = "���ս���ʱ��")]
        public int EndHour { get; set; }
        [Display(Name="��������")]
        public string Type { get; set; }
        [Display(Name="��ע")]
        public string Remark { get; set; }
    }
}
