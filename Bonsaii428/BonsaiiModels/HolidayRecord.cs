namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("HolidayRecords")]
    public partial class HolidayRecord
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(50)]
        public string HolidayName { get; set; }
    }
}
