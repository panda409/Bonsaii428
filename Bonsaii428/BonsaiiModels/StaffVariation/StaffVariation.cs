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
        [Display(Name="Ա������")]
        public int StaffId { get; set; }

        public int? StaffChangeId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="�����Ϣ")]
        public string FieldName { get; set; }

      
        [StringLength(255)]
        [Display(Name="�����")]    
        public string FieldValue { get; set; }

        [Required]
        [Display(Name = "���ʱ��")]    
        public DateTime VariationRecordTime { get; set; }

        [Display(Name = "�����Чʱ��")]    
        public DateTime? VariationEffectTime { get; set; }

         [Display(Name = "�����Чʱ��")]    
        public bool Initial { get; set; }

         [NotMapped]
         public string DepartmentName { get; set; }

    }
}
