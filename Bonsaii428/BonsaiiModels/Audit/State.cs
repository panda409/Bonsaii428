namespace Bonsaii.Models.Audit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("States")]
    public partial class State
    {
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

      //  public virtual States States1 { get; set; }

       // public virtual States States2 { get; set; }
    }
}
