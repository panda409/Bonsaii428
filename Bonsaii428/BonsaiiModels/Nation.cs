using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    [Table("StaffParam")]
   public class Nation
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
