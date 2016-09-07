namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OriginalDataofCheckOn")]
    public partial class OriginalDataofCheckOn
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string StaffNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string StaffName { get; set; }

        [StringLength(50)]
        public string LogicCardNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string PhysicalCardNumber { get; set; }

        public TimeSpan Time { get; set; }

        [Required]
        [StringLength(100)]
        public string OriginalData { get; set; }

        [Required]
        [StringLength(10)]
        public string MachineNumber { get; set; }

        [StringLength(50)]
        public string Place { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }
        [Required]
        public bool IsRead{ get; set; }
    }
}
