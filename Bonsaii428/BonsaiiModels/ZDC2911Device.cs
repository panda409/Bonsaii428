using Riss.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    class ZDC2911Device : BaseDevice
    {
        public ZDC2911Device(string DeviceType, int DeviceId, string CommKey, int Port, string IP) : base(DeviceType,DeviceId,CommKey,Port,IP)
        {
        }
        /// <summary>
        /// 针对具体设备的测视连接方法 
        /// </summary>
        /// <returns></returns>
        public override bool TestConnection()
        {
            //调用特定设备的测试连接的函数
            try
            {
                Device device = new Device();
                device.DN = this.DeviceID;
                device.Password = this.CommKey;
                device.Model = "ZDC2911";
                device.ConnectionModel = 5;//等于5时才能正确加载ZD2911通讯模块

                device.IpAddress = this.IP;  //txt_IP.Text.Trim();
                device.IpPort = this.Port;    //(int)nud_Port.Value;
                device.CommunicationType = CommunicationType.Tcp;


                DeviceConnection deviceConnection = DeviceConnection.CreateConnection(ref device);
                //设备连接成功
                if (deviceConnection.Open() > 0)
                {
                    //            DeviceCommEty deviceEty = new DeviceCommEty();
                    //                 deviceEty.Device = device;
                    //             deviceEty.DeviceConnection = deviceConnection;
                    //           btn_CloseDevice.Enabled = true;
                    //             SetButtonEnabled(true);
                    //             btn_OpenDevice.Enabled = false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
