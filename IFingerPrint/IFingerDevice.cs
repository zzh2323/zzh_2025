using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public interface IFingerDevice
    {
        //获取连接设备的数量
        int GetDeviceCount();
        //打开设备
        IntPtr OpenDevice(int index);
        //关闭设备
        int CloseDevice(IntPtr devHandle);
    }
}
