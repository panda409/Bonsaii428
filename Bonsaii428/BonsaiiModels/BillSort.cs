namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BillSort")]
    public partial class BillSort
    {
        [Key]
        [StringLength(2)]
        public string Sort { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int SerialNumber { get; set; }


    }
}
