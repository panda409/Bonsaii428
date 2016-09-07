namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReserveFields")]
    public partial class ReserveField
    {
        [Key]
        //[Required]
        public int Id { get; set; }
      
        [Display(Name="����")]
        public int TableNameId { get; set; }

        [Display(Name = "����")]
        public virtual TableNameContrast TableName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "�ֶ���")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "��������ȷ��Ӣ������")]
        public string FieldName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "����")]
        public string Description { get; set; }


        [Display(Name = "�Ƿ�Ӧ��")]
        public Boolean Status { get; set; }
        [Display(Name = "¼��ʱ��")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "¼����Ա")]
        public string RecordPerson { get; set; }
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "������Ա")]
        public string ChangePerson { get; set; }
        [Display(Name = "���ʱ��")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "�����Ա")]
        public string AuditPerson { get; set; }
    }
}
