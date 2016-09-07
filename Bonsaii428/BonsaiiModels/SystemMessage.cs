namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemMessages")]
    public partial class SystemMessage
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name="��Ϣ����")]
        public string MessTitle { get; set; }

        [Column(TypeName = "text")]
        [Display(Name="��Ϣ����")]
        public string MessBody { get; set; }

        [Column(TypeName = "date")]
        [Display(Name="ʱ��")]
        public DateTime MessTime { get; set; }

        public bool? IsRead { get; set; }
        
        public string CompanyId { get; set; }

        [StringLength(11)]
        public string UserName { get; set; }

        [Display(Name="�Ƿ��ͳɹ�")]
        public byte? SendStatus { get; set; }


        [Display(Name = "����")]
        [StringLength(50)]
        public string MessType { get; set; }

        [Display(Name = "�ռ���")]
       
        public string MessReceiver { get; set; }

      
    }
}
