using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels.App
{
   public class ApplicationDetailViewModel
    {
       public int Id { get; set; }
       public string BillSort { get; set; }
       public string Application { get; set; }
       public string Date { get; set; }
       public string Time { get; set; }
       public string BillNumber { get; set; }
    }
}
