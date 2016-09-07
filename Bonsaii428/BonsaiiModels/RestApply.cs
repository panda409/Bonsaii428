namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("RestApplies")]
    public class RestApply
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string BillType { get; set; }

        [Required]
       
        public string BillNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string StaffNumber { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int Hours { get; set; }

        //[StringLength(255)]
        public string Reason { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        public byte AuditStatus { get; set; }

        public bool IsRead { get; set; }

        [StringLength(20)]
        public string AuditPerson { get; set; }

        public DateTime? AuditTime { get; set; }
    }
}
