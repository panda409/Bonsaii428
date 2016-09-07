using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bonsaii.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "电子邮件")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Key]
        public string CompanyId { get; set; }
        [Required]
        [Display(Name = "企业全称")]
        public string CompanyFullName { get; set; }

        [Required]
        [Display(Name = "联系电话")]
        [RegularExpression("([0-9]|-){5,}", ErrorMessage = "请输入合法的{0}")]
        public string PhoneNumber { get; set; }
        public string ConnectionString { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
      
        public string AddressDetail { get; set; }
        public string PersonNumber { get; set; }
        [Required]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "短信验证码")]
        [RegularExpression("^[0-9]{4}$",ErrorMessage="请输入合法的{0}")]
        public string Code { get; set; }



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


        public bool cbox { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "联系电话")]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "验证码")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "请输入合法的{0}")]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }


    public class ModifyPasswordViewModel
    {
        [Required]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "原密码")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmNewPassword { get; set; }

    }
}
