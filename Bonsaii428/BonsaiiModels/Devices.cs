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
        [Display(Name="������")]
        public int DeviceID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name="ͨѶ����")]
        public string CommKey { get; set; }

        [Required]
        [StringLength(16)]
        [Display(Name="�豸IP")]
    //    [RegularExpression("/^(\\d+).(\\d+).(\\d+).(\\d+)$/",ErrorMessage="������Ϸ���IP��ַ")]
        public string IP { get; set; }
        [Required]
        [Display(Name="�豸�˿ں�")]
        public int Port { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="�豸����")]
        public string DeviceType { get; set; }

        [StringLength(50)]
        [Display(Name="�豸����")]
        //����ֶ��Ǻͻ�������Ϣ�޹صģ�������Ϊ�����ǹ�������ϵķ��㣨�����ĳ����������Ϊ��ͼ��ݴ򿨻���
        public string DeviceName { get; set; }
    }
}
