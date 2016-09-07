using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    public class BindCodeViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name="员工工号")]
        public string StaffNumber { get; set; }
        [Required]
        [Display(Name = "姓名")]
        public string RealName { get; set; }

        [Required]
        [Display(Name = "绑定码")]
        public string BindingCode { get; set; }
        [Display(Name = "电话")]
        public string Phone { get; set; }
        [Display(Name = "是否绑定App")]
        public bool BindTag { get; set; }
        [Display(Name = "最后登录时间")]
        public DateTime LastTime { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }
        [Display(Name = "职位")]
        public string Position { get; set; }
 
    }
}
