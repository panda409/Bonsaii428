using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models
{
    public class BrandTemplateReserveViewModels
    {
        /*BrandTemplateReserve*/
        [Key]
        public int Id { get; set; }

        [Display(Name = "字段")]
        public int FieldId { get; set; }

        [Display(Name = "模板")]
        public int Number { get; set; }

        /*用来在Index方法中显示*/
        public string FieldIdDescription { get; set; }

        public string NumberDescription { get; set; }

        public string Value { get; set; }

        /*BrandTemplateModels*/
        [Display(Name = "厂牌模板")]
        public byte[] BrandTemplate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string BrandTemplateType { get; set; }

        [Required]
        [Display(Name = "说明")]
        public string TemplateDescription { get; set; }

        /*ConfirmedField*/
        [Display(Name="字段名称")]
        public string FieldName { get; set; }
    }
}