namespace BonsaiiModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Devices
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="机器号")]
        public int DeviceID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name="通讯密码")]
        public string CommKey { get; set; }

        [Required]
        [StringLength(16)]
        [Display(Name="设备IP")]
    //    [RegularExpression("/^(\\d+).(\\d+).(\\d+).(\\d+)$/",ErrorMessage="请输入合法的IP地址")]
        public string IP { get; set; }
        [Required]
        [Display(Name="设备端口号")]
        public int Port { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="设备类型")]
        public string DeviceType { get; set; }

        [StringLength(50)]
        [Display(Name="设备名称")]
        //这个字段是和机器的信息无关的，仅仅是为了我们管理机器上的方便（比如把某个机器命名为：图书馆打卡机）
        public string DeviceName { get; set; }
    }
}
