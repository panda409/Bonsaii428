namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Dimensions")]
    public partial class Dimension
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DimensionName { get; set; }
    }
}
