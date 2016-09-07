namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Actions
    {
        [Key]
        public int ActionId { get; set; }

        [Required]
        [StringLength(30)]
        public string ActionName { get; set; }

        [Required]
        [StringLength(30)]
        public string ActionShowName { get; set; }

        [Required]
        [StringLength(30)]
        public string ControllerName { get; set; }

        [Required]
        [StringLength(30)]
        public string ControllerShowName { get; set; }
    }
}
