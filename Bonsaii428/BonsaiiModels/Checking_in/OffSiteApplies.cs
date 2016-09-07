namespace BonsaiiModels.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OffSiteApplies")]
    public class OffSiteApplies
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string StaffNumber { get; set; }

        [StringLength(255)]
        public string Latitude { get; set; }

      
        [StringLength(255)]
        public string Reason { get; set; }
    
        public DateTime Date { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string BillNumber { get; set; }
       // public Boolean IsRead { get; set; }
        [Display(Name = "…Û∫À ±º‰")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "…Û∫À»À‘±")]
        public string AuditPerson { get; set; }

        [Display(Name = "…Û∫À◊¥Ã¨")]
        public byte AuditStatus { get; set; }
        public bool IsRead { get; set; }

    }
}
