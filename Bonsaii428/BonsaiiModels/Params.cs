namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Params
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }


        [Column(Order = 1)]
        [StringLength(20)]
        public string ParamName { get; set; }
    }
}
