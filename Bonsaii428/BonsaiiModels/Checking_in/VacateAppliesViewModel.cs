using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii.Models.Checking_in
{
    public class VacateAppliesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "员工号")]
        public string StaffNumber { get; set; }
        [Display(Name = "员工名称")]
        public string StaffName { get; set; }
        [Display(Name = "单据名称")]
        public string BillTypeName { get; set; }

        [Display(Name = "部门名称")]
        public string DepartmentName { get; set; }
        [Display(Name = "开始日期")]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "请假时数")]
        [Required]
        public int Hours { get; set; }
        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }
    }
}
