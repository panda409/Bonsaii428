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

        [Display(Name="�û���")]
        [StringLength(256)]
        public string UserName { get; set; }

        [Display(Name="����Ա����")]
        public string Name { get; set; }
        public string BindingCode { get; set; }
        public string StaffNumber { get; set; }

        [Display(Name="��ҵ���")]
        [StringLength(10)]
        public string CompanyId { get; set; }
        [Display(Name="��ҵȫ��")]
        public string CompanyFullName { get; set; }

        public string ConnectionString { get; set; }

        [Display(Name="�Ƿ���Ч")]
        public bool IsAvailable { get; set; }
        [Display(Name="���״̬")]
        public bool IsProved { get; set; }

        [Display(Name="ע���û�")]
        public bool IsRoot { get; set; }


        [StringLength(100, ErrorMessage = "{0} �������ٰ��� {2} ���ַ���", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "������")]
        [NotMapped]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ȷ��������")]
        [Compare("Password", ErrorMessage = "�����ȷ�����벻ƥ�䡣")]
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
