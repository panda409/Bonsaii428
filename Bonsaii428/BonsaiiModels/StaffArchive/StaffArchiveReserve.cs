using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("StaffArchiveReserves")]
    public class StaffArchiveReserve
    {
        [Key]
        public int Id { get; set; }
        public int FieldId { get; set; }

        [Display(Name = "字段名")]
        public int Number { get; set; }

        [StringLength(50)]
        [Display(Name = "描述")]
        public string Value { get; set; }
    }
}