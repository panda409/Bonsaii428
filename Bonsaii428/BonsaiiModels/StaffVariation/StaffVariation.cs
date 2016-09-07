namespace BonsaiiModels.StaffVariation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StaffVariations")]
    public partial class StaffVariation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="员工工号")]
        public int StaffId { get; set; }

        public int? StaffChangeId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="变更信息")]
        public string FieldName { get; set; }

      
        [StringLength(255)]
        [Display(Name="变更后")]    
        public string FieldValue { get; set; }

        [Required]
        [Display(Name = "变更时间")]    
        public DateTime VariationRecordTime { get; set; }

        [Display(Name = "变更生效时间")]    
        public DateTime? VariationEffectTime { get; set; }

         [Display(Name = "变更生效时间")]    
        public bool Initial { get; set; }

         [NotMapped]
         public string DepartmentName { get; set; }

    }
}
