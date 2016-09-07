namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ParentName { get; set; }

        [Required]
        [StringLength(50)]
        public string ParentClass { get; set; }
        [Required]
        public int ParentOrder { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Href { get; set; }

        public bool IsShow { get; set; }
    }
}
