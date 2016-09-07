using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii.Models.Checking_in
{
    public class OriginalSignInDataViewModel
    {
        [Display(Name="员工工号")]
        public string StaffNumber { get; set; }
        [Display(Name="员工姓名")]
        public string StaffName { get; set; }
        [Display(Name="所属部门")]
        public string DepartmentName { get; set; }
        [Display(Name="打卡时间")]
        public DateTime Date { get; set; }
        public string ShowDate { get; set; }

        [Display(Name="打卡类型")]
        public string Type { get; set; }

    }
}
