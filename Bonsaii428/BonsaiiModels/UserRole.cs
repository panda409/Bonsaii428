using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        [Column("UserId", Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column("RoleId", Order = 2)]
        public string RoleId { get; set; }
        public string IdentityUser_Id{get;set;}
    }
}