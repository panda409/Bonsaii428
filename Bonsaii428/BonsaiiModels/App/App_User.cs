using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Bonsaii.Models.App
{
    [Table("App_Users")]
    public class App_User
    {
        [Key]
        [Display(Name = "用户名")]
       // [StringLength(256)]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

       // public string Code { get; set; }

        [Display(Name = "企业编号")]
        [StringLength(10)]
        public string CompanyId { get; set; }
      
        public string ConnectionString { get; set; }

        public string StaffNumber { get; set; }

        public bool Status { get; set; }
        public byte[] Head { get; set; }
        public string HeadType { get; set; }
        public string Nickname { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Strict { get; set; }
        public string Personal { get; set; }
        public Boolean? BindApp { get; set; }
    }
}
