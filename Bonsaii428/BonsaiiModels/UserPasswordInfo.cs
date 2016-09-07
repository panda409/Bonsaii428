namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("UserPasswordInfo")]
    public partial class UserPasswordInfo
    {
        [Key]
        public int Id { get; set; }
   
        [Display(Name="用户名")]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        public string UserName { get; set; }
      
        [StringLength(50)]
        [Display(Name="企业编号")]
        public string CompanyId { get; set; }
        
        [Column(TypeName = "image")]
        [Display(Name="营业执照")]
        public byte[] BusinessLicense { get; set; }
  
        [HiddenInput(DisplayValue = false)]
        [StringLength(50)]
        public string BusinessLicenseType { get; set; }

        [Required]
        //[RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        //[RegularExpression("^((d{3,4}-)|d{3.4}-)?d{7,8}$", ErrorMessage = "请输入合法的{0}")]
        [Display(Name="联系电话")]
        public string TelNumber { get; set; }

         [Display(Name = "提交申请时间")]
        public DateTime SubmitTime { get; set; }
        
     //   [StringLength(50)]
        [Display(Name="审核状态")]
        public byte AuditStatus { get; set; }
    }
}
