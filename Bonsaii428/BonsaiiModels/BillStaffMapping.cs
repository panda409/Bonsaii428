namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BillStaffMapping")]
    public partial class BillStaffMapping
    {
        public int Id { get; set; }

        
        [StringLength(4)]
        public string BillType { get; set; }

       
        [StringLength(10)]
        public string BillNumber { get; set; }

       
        [StringLength(10)]
        public string StaffNumber { get; set; }

        public string TelPhone { get; set; }
        public string Email { get; set; }
    }
}
