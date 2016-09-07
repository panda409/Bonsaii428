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
        [Display(Name="��")]
        public int TableNameId { get; set; }

        [Display(Name = "����")]
        public virtual TableNameContrast TableName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "����ֶ�")]
        public string TableColumn { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "�ֶεĺ�����˼")]
        public string Description { get; set; }
    }
}
