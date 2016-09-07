namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StaffBasicParam")]
    public partial class StaffBasicParam
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="参数名称")]
        [StringLength(20)]
        public string Name { get; set; }

        [Display(Name="参数值")]
        [StringLength(100)]
        public string Value { get; set; }

        public string Type { get; set; }
    }
}
