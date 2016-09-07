using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models
{
    public class BrandViewModel
    {
        public int Id { get; set; }

        [Display(Name = "厂牌模板")]
      
        public byte[] BrandTemplate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string BrandTemplateType { get; set; }

        [Display(Name = "说明")]
        public string Description { get; set; }
    }
}