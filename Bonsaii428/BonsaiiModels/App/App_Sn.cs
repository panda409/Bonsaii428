using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Bonsaii.Models.App
{
    [Table("App_Sns")]
    public class App_Sn
    {
       
        [Key]
        [Display(Name = "用户名")]
        [StringLength(256)]
        public string UserName { get; set; }
       // [Key,Column(Order=2)]
        public string Sn { get; set; }

        public DateTime DateTime { get; set; }
    }
}
