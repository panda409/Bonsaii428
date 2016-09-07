using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("RecordDatetimes")]
    public class RecordDatetime
    {
        
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Recordtime { get; set; }
        public string Tag { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
    }
}