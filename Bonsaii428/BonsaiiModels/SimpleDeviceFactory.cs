using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    public class SimpleDeviceFactory
    {
        /// <summary>
        /// 针对某一类型的设备，返回具体的设备对象
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public static BaseDevice createDevice(string DeviceType, int DeviceId, string CommKey, int Port, string IP)
        {
            BaseDevice device = null;
            if (DeviceType.Equals("ZDC2911"))
                device = new ZDC2911Device(DeviceType,DeviceId,CommKey,Port,IP);
            return device;
        }
    }
}
