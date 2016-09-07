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
   
        [Display(Name="�û���")]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "������Ϸ���{0}")]
        public string UserName { get; set; }
      
        [StringLength(50)]
        [Display(Name="��ҵ���")]
        public string CompanyId { get; set; }
        
        [Column(TypeName = "image")]
        [Display(Name="Ӫҵִ��")]
        public byte[] BusinessLicense { get; set; }
  
        [HiddenInput(DisplayValue = false)]
        [StringLength(50)]
        public string BusinessLicenseType { get; set; }

        [Required]
        //[RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "������Ϸ���{0}")]
        //[RegularExpression("^((d{3,4}-)|d{3.4}-)?d{7,8}$", ErrorMessage = "������Ϸ���{0}")]
        [Display(Name="��ϵ�绰")]
        public string TelNumber { get; set; }

         [Display(Name = "�ύ����ʱ��")]
        public DateTime SubmitTime { get; set; }
        
     //   [StringLength(50)]
        [Display(Name="���״̬")]
        public byte AuditStatus { get; set; }
    }
}
