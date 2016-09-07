using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("VerifyCode")]
    public class VerifyCode
    {
        [Key]
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        [Column(TypeName = "datetime")]
        public  DateTime OverTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

    }
}