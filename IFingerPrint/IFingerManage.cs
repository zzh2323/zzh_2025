using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public interface IFingerManage
    {
        //开始采集指纹
        int AcquireFingerprint(IntPtr devHandle, byte[] imgBuffer, byte[] template, ref int size);
    }
}
