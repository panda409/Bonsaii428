namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StaffReserve
    {
        [Key]
        public int Id { get; set; }

        public int FieldId { get; set; }

        [Display(Name = "�ֶ���")]
        public int Number { get; set; }

         [Display(Name = "����")]
        [StringLength(50)]
        public string Value { get; set; }
    }
}
