namespace BonsaiiModels.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AAYiDiKaoQin")]
    public partial class AAYiDiKaoQin
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string StaffNumber { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [StringLength(30)]
        public string PhotoType { get; set; }

        public DateTime? Date { get; set; }
    }
}
