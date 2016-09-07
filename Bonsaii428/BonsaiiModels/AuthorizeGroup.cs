namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuthorizeGroup")]
    public partial class AuthorizeGroup
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }
 //       [Key]
        [StringLength(50)]
        public string GroupName { get; set; }

 //       [Key]
        [StringLength(50)]
        public string ActionName { get; set; }
    }
}
