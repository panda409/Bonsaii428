using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii.Models.Audit
{
    class AuditApplicationStepViewModel
    {
        [Display(Name = "步骤")]
        public int AuditApplicationStep { get; set; }
    
        [Display(Name = "状态")]
        public string AuditApplicationStepStatus { get; set; }
    }
}
