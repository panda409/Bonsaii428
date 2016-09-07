namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DepartmentReserves")]
    public partial class DepartmentReserve
    {
        [Key]
        public int Id { get; set; }
        public int FieldId { get; set; }

        [Display(Name = "�ֶ���")]
        public int Number { get; set; }
      
        [StringLength(50)]
        [Display(Name = "����")]
        public string Value { get; set; }
    }
}
