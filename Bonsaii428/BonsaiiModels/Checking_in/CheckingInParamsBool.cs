namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckingInParamsBool")]
    public partial class CheckingInParamsBool
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool Value { get; set; }

        [Required]
        [StringLength(50)]
        public string Remark { get; set; }
    }
}
