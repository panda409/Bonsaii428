using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class TestModels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="姓名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "方法")]
        public string Method { get; set; }
        [Required]
        [Display(Name = "是否审核")]
        public bool IsChecked { get; set; }
        [Display(Name = "文件")]
        public string FilePath { get; set; }
    }
}