namespace Bonsaii.Models.Works
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WorkManages
    {
        public int Id { get; set; }

        [NotMapped]
        [Display(Name="��ʼ����")]
        public DateTime StartDate { get; set; }
        [Display(Name="��������")]
        [NotMapped]
        public DateTime EndDate { get; set; }

        public DateTime Date { get; set; }
        [Required]
        [Display(Name="���")]
        public int WorksId { get; set; }

        [Display(Name="�������")]
        [NotMapped]
        public string WorksName { get; set; }
        [Display(Name = "����״̬")]
        public byte AuditStatus { get; set; }
        [Display(Name="Ա�����")]
        [StringLength(50)]
        public string StaffNumber { get; set; }
        [Display(Name = "��ע")]
        [StringLength(255)]
        public string Remark { get; set; }  
        [Display(Name="����")]
        public string DepartmentId { get; set; }
        [Display(Name="��������")]
        [NotMapped]
        public string DepartmentName { get; set; }
        public bool Flag { get; set; }
    }
}
