using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    //定义设备的抽象类，封装设备的属性和方法
    public abstract class BaseDevice
    {
        [Display(Name="设备类型")]
        public string DeviceType { get; set; }
        [Display(Name = "机器号")]
        public int DeviceID { get; set; }
        [Display(Name = "通讯密码")]
        public string CommKey { get; set; }
        [Display(Name = "设备端口号")]
        public int Port { get; set; }
        public string IP { get; set; }

        

        public BaseDevice() { }
        public BaseDevice(string DeviceType, int DeviceId, string CommKey, int Port, string IP)
        {
            this.DeviceType = DeviceType;
            this.DeviceID = DeviceId;
            this.CommKey = CommKey;
            this.Port = Port;
            this.IP = IP;
        }
        /// <summary>
        /// 针对具体设备的测视连接方法
        /// </summary>
        /// <returns>返回测试链接的结果</returns>
        public abstract bool TestConnection();
    }

}
