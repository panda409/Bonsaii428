
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Bonsaii.Models
{
    [Table("BrandTemplateReserves")]
    public  class BrandTemplateReserve
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="�ֶ�")]
        public int FieldId { get; set; }

        [Display(Name = "ģ��")]
        public int Number { get; set; }

        [StringLength(50)]
        [Display(Name="�ֶε�ֵ")]
        public string Value { get; set; }

        [NotMapped]
        public string FieldDescription { get; set; }
    }
}
