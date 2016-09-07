using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models
{
    public class BrandTemplateViewModels
    {
        public int Id { get; set; }

        [Display(Name = "厂牌模板")]
     
        public byte[] BrandTemplate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string BrandTemplateType { get; set; }

        [Required]
        [Display(Name = "模板名称")]
        public string TemplateDescription { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

      
        [Display(Name = "工号")]
        public string StaffNumber { get; set; }

       [Display(Name = "姓名")]
       public string Name { get; set; }

        [Display(Name = "性别")]
        public string Gender { get; set; }

        [Display(Name = "部门")]
        public string Department { get; set; }

        [Display(Name = "工种")]
        public string WorkType { get; set; }

        [Display(Name = "员工职位")]
        public string Position { get; set; }

        public byte[] Head { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string HeadType { get; set; }
        //[StringLength(50)]
        //[Display(Name="字段")]
        //public string FieldName { get; set; }

        //[StringLength(50)]
        //public string Status { get; set; }
    }
}