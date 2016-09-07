namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeviceOriginalData")]
    public partial class DeviceOriginalData
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserID { get; set; }

        public int DeviceID { get; set; }

        public DateTime DateTime { get; set; }

        public int? Verify { get; set; }

        public int? Action { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [StringLength(100)]
        public string MDIN { get; set; }

        public int? DoorStatus { get; set; }

        public int? JobCode { get; set; }

        public int? Antipassback { get; set; }
    }
}
