using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels.DeviceUser
{
    /// <summary>
    /// 这个视图模型主要用于展示考勤机当中存储的用户数据，
    /// 主要包括：用户名称，用户的物理卡号（也就是考勤机当中的编号），部门编号
    /// </summary>
    public class DeviceUserViewModel
    {
        /// <summary>
        /// 员工姓名是要通过系统导入到考勤机当中的
        /// </summary>
        [Display(Name="员工姓名")]
        public string Name { get; set; }

        //这个字段对应的是考勤机上面对于某一个员工的编号
        //这个编号的来源是员工入职的时候由系统自动生成的，然后手动录入到考勤机当中的
        [Display(Name="编号")]
        public string PhysicalCardNumber { get; set; }

        [Display(Name = "部门")]
        public string DepartmentName { get; set; }
    }
}
