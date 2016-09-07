using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii.Models
{
    [Table("TableNameContrasts")]
    public class TableNameContrast
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="表名")]
        public string TableName { get; set; }

        [Display(Name = "描述")]
        public string TableDescription { get; set; }

    }
}
