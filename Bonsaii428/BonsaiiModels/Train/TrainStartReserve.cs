namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("TrainStartReserves")]
    public partial class TrainStartReserve
    {
        [Key]
        public int Id { get; set; }
        public int FieldId { get; set; }

        [Display(Name = "×Ö¶ÎÃû")]
        public int Number { get; set; }

        [StringLength(50)]
        [Display(Name = "ÃèÊö")]
        public string Value { get; set; }
    }
}
