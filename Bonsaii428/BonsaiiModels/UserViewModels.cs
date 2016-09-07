using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class UserViewModels
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "管理员账户名")]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        public string UserName { get; set; }

        [Display(Name="管理员名称")]
        public string Name { get; set; }

        [Display(Name = "企业编号")]
        [StringLength(10)]
        public string CompanyId { get; set; }
        [Display(Name = "企业全称")]
        public string CompanyFullName { get; set; }

        public string ConnectionString { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPawword { get; set; }

        
        [Display(Name = "是否有效")]
        public bool IsAvailable { get; set; }
        [Display(Name = "审核状态")]
        public bool IsProved { get; set; }

        [Display(Name = "注册用户")]
        public bool IsRoot { get; set; }



    }
}