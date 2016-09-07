using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Bonsaii.Models
{
    [Table("Brands")]
    public  class Brand
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name="厂牌模板")]
        public int BrandId { get; set; }

        [NotMapped]
        public string BrandName { get; set; }

        [Required]
        [Display(Name = "员工")]
        public int StaffId { get; set; }
        [Display(Name="员工姓名")]
        [NotMapped]
        public string StaffName { get; set; }
    }
}
