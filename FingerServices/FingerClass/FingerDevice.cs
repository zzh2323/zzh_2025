using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public class FingerDevice : IFingerDevice
    {
        // 获取连接设备的数量
        public int GetDeviceCount()
        {
            return zkfp2.GetDeviceCount();
        }

        // 打开设备
        public IntPtr OpenDevice(int index)
        {
            return zkfp2.OpenDevice(index);
        }

        // 关闭设备
        public int CloseDevice(IntPtr devHandle)
        {
            return zkfp2.CloseDevice(devHandle);
        }

    }
}
