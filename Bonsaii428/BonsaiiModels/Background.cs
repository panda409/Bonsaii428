using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Backgrounds")]
    public class Background
    {
        [Key]
        public int Id { get;  set;}
        [Required]
        [Display(Name="学历")]
        public string XueLi { get; set; }
    }
}