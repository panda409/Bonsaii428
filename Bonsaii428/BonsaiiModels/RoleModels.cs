using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Roles")]
    public class RoleModels
    {
        [Key]
        public string Id { get; set; }
        public string Name{get;set;}
    }
}