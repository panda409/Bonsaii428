namespace BonsaiiModels.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AAKaoQin")]
    public class AAKaoQin
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string StaffNumber { get; set; }

        [StringLength(255)]
        public string Latitude { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [StringLength(255)]
        public string Reason { get; set; }
        public string PhotoType { get; set; }
        public DateTime Date { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string BillNumber { get; set; }
       // public Boolean IsRead { get; set; }
        [Display(Name = "���ʱ��")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "�����Ա")]
        public string AuditPerson { get; set; }

        [Display(Name = "���״̬")]
        public byte AuditStatus { get; set; }

    }
}
