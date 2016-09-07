using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class DepartmentViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="序号")]
        public int DepartmentOrder { get; set; }

        [Display(Name = "部门编号")]
        public string DepartmentId { get; set; }

        [Display(Name = "部门名称")]
        public string Name { get; set; }
        public string ParentDepartmentId { get; set; }
        [Display(Name = "上级部门")]
        public string ParentDepartmentName { get; set; }
        [Required]
        [Display(Name = "部门缩写")]
        public string DepartmentAbbr { get; set; }
        [Display(Name = "编制人数")]
        public int StaffSize { get; set; }
        [Display(Name = "录入时间")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "录入人员")]
        public string RecordPerson { get; set; }
        [Display(Name = "更改时间")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "更改人员")]
        public string ChangePerson { get; set; }
        [Display(Name = "审核时间")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "审核人员")]
        public string AuditPerson { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public string Description { get; set; }
        public string Value { get; set; }
    }
}