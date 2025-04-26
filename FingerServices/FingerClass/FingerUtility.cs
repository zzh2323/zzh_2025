using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public class FingerUtility : IFingerUtility
    {
        // Base64字符串转byte
        public byte[] Base64ToBlob(string strBase64)
        {
            return Convert.FromBase64String(strBase64);
        }

        // byte转Base64字符串
        public string BlobToBase64(byte[] buf, int len)
        {
            return Convert.ToBase64String(buf, 0, len);
        }

        //获取设备参数
        public int GetParameters(IntPtr devHandle, int code, byte[] paramValue, ref int size)
        {
            return zkfp2.GetParameters(devHandle, code, paramValue, ref size);
        }
        //设置设备参数
        public int SetParameters(IntPtr devHandle, int code, byte[] paramValue, int size)
        {
            return zkfp2.SetParameters(devHandle, code, paramValue, size);
        }
        //4字节byte数组转int
        public bool ByteArray2Int(byte[] buf, ref int value)
        {
            return zkfp2.ByteArray2Int(buf, ref value);
        }
        //int转4字节byte数组
        public bool Int2ByteArray(int value, byte[] buf)
        {
            return zkfp2.Int2ByteArray(value, buf);
        }
    }
}
