namespace BonsaiiModels.Staffs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("SerialCodeCounts")]
    public partial class SerialCodeCount
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Length { get; set; }
        
        public int Count { get; set; }


    }
}
