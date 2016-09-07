using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii.Models.Checking_in
{
    public class ChargeCardAppliesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "员工号")]
        public string StaffNumber { get; set; }
        [Display(Name="员工名称")]
        public string StaffName { get; set; }
        [Display(Name="单据名称")]
        public string BillTypeName { get; set; }
        [Display(Name = "部门名称")]
        public string DepartmentName { get; set; }

        [Display(Name = "签卡日期")]
        public DateTime DateTime { get; set; }
        [Display(Name = "补签时间点")]
        public DateTime Date { get; set; }

        [Display(Name = "签卡原因")]
        [StringLength(255)]
        public string Reason { get; set; }


        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }





        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }
    }
}
