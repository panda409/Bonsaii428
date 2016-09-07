namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OvertimeAppliesViewModel
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

        [Display(Name = "开始日期时间")]
        public DateTime StartDateTime { get; set; }


        [Display(Name = "加班时数")]
        public double Hours { get; set; }



        [NotMapped]
        [Display(Name = "审核状态")]
        public string AuditStatusName { get; set; }


    }
}
