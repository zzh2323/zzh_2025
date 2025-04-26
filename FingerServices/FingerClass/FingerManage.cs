using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public class FingerManage : IFingerManage
    {
        // 开始采集指纹
        public int AcquireFingerprint(IntPtr devHandle, byte[] imgBuffer, byte[] template, ref int size)
        {
            return zkfp2.AcquireFingerprint(devHandle, imgBuffer, template, ref size);
        }
    }
}
