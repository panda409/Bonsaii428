namespace BonsaiiModels.Subscribe
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscribe_Companies")]
    public partial class Subscribe_Company
    {
        [Key]
        public int Id { get; set; }

        public int? SubScribeId { get; set; }

        [StringLength(10)]
        public string CompanyId { get; set; }
    }
}
