using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    [Table("BindCodes")]
    public class BindCode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyId { get; set; }
        [Required]
        public string ConnectionString { get; set; }
        [Required]
        public string StaffNumber { get; set; }
        [Required]
        public string RealName { get; set; }
       
        [Required]
        public string BindingCode { get; set; }
        public string Phone { get; set; }

        public bool BindTag { get; set; }
        public DateTime LastTime { get; set; }
        public bool IsAvail { get; set;
        }
    }
}
