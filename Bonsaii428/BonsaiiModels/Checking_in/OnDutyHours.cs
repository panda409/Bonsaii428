namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OnDutyHours
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string StaffNumber { get; set; }

        public double AvailableHours { get; set; }
    }
}
