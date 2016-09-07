    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

   namespace Bonsaii.Models
    { 
    
       [Table("BrandTemplates")]
    public  class BrandTemplateModels
    {
        [Key]
         [Required]
        public int Id { get; set; }

        [Display(Name="����ģ��")]
        [Column(TypeName = "image")]
        public byte[] BrandTemplate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string BrandTemplateType { get; set; }

        [Required]
        [Display(Name="ģ������")]
        public string Description { get; set; }
    }
}
