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
      
        [Display(Name="表名")]
        public int TableNameId { get; set; }

        [Display(Name = "表名")]
        public virtual TableNameContrast TableName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "字段名")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "请输入正确的英文名称")]
        public string FieldName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "描述")]
        public string Description { get; set; }


        [Display(Name = "是否应用")]
        public Boolean Status { get; set; }
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
    }
}
