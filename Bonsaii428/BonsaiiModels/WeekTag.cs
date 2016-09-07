using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("WeekTags")]
    public class WeekTag
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "年份")]
        public string Nian { get; set; }
        [Display(Name = "类别")]
        public string Range { get; set; }
        [Display(Name = "星期一")]
        public string Week1 { get; set; }
        [Display(Name = "星期二")]
        public string Week2 { get; set; }
        [Display(Name = "星期三")]
        public string Week3 { get; set; }
        [Display(Name = "星期四")]
        public string Week4 { get; set; }
        [Display(Name = "星期五")]
        public string Week5 { get; set; }
        [Display(Name = "星期六")]
        public string Week6 { get; set; }
        [Display(Name = "星期日")]
        public string Week7 { get; set; }
    }
}