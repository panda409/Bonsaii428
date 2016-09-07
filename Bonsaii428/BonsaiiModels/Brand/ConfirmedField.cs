
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

   namespace Bonsaii.Models
    { 
       [Table("ConfirmedFields")]
       public  class ConfirmedField
        {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
