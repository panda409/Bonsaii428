namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SignInCardStatus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StaffNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime WorkDate { get; set; }

        public TimeSpan NeedStartTime { get; set; }

        public TimeSpan NeedEndTime { get; set; }

        public TimeSpan NeedWorkTime { get; set; }

        public TimeSpan? SignInTime { get; set; }

        public int ComeLateMinutes { get; set; }

        public int LeaveEarlyMinutes { get; set; }

        public string Type { get; set; }

        public int AbsenteeismHours { get; set; }


        public bool IsRead { get; set; }

        /// <summary>
        /// 记录打卡的类型，0是普通上班打卡，1是加班打卡，2是出差打卡
        /// </summary>
        public int SignInType { get; set; }
    }
}
