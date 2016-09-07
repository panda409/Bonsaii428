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
        /// ��¼�򿨵����ͣ�0����ͨ�ϰ�򿨣�1�ǼӰ�򿨣�2�ǳ����
        /// </summary>
        public int SignInType { get; set; }
    }
}
