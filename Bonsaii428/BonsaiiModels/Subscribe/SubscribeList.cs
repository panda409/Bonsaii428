namespace BonsaiiModels
{
  
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubscribeLists")]
    public partial class SubscribeList
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="��������")]
        [StringLength(50)]
        public string SubscribeName { get; set; }

         [Display(Name = "SQL��ѯ")]
         [Column(TypeName = "text")]
        public string SQL { get; set; }

         [Display(Name = "SQL��ѯ�Ƿ���Ч")]
        public bool IsSQLLegal { get; set; }

         [Display(Name = "�Ƿ�����")]
        public bool IsAvailable { get; set; }

         [Display(Name = "��������")]
        public DateTime CreateDate { get; set; }

         [Display(Name = "APP��Ϣ����/�ʼ�����")]
         [Column(TypeName = "text")]
         public string MessTitle { get; set; }

         [Display(Name = "APP��Ϣ����/�ʼ�����")]
         [Column(TypeName = "text")]
         public string MessContent { get; set; }


       // public virtual ICollection<Subscribe_Company> Subscribe_Company { get; set; }
    }
}
