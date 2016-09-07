namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Users")]
    public partial class UserModels
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Display(Name="用户名")]
        [StringLength(256)]
        public string UserName { get; set; }

        [Display(Name="管理员名称")]
        public string Name { get; set; }
        public string BindingCode { get; set; }
        public string StaffNumber { get; set; }

        [Display(Name="企业编号")]
        [StringLength(10)]
        public string CompanyId { get; set; }
        [Display(Name="企业全称")]
        public string CompanyFullName { get; set; }

        public string ConnectionString { get; set; }

        [Display(Name="是否有效")]
        public bool IsAvailable { get; set; }
        [Display(Name="审核状态")]
        public bool IsProved { get; set; }

        [Display(Name="注册用户")]
        public bool IsRoot { get; set; }


        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        [NotMapped]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    
        public Boolean? BindTag { get; set; }
     
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Strict { get; set; }
        public byte[] Head { get; set; }
        public string HeadType { get; set; }
        public string NickName { get; set; }
        public string Personal { get; set; }
        public Boolean? HuanTag { get; set; }
    }
}
