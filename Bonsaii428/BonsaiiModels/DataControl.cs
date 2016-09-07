namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("DataControls")]
    public partial class DataControl
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

       // [Required]
       // [StringLength(50)]
        [Display(Name="表")]
        public int TableNameId { get; set; }

        [Display(Name = "表名")]
        public virtual TableNameContrast TableName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "表的字段")]
        public string TableColumn { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "字段的汉语意思")]
        public string Description { get; set; }
    }
}
