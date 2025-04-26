using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public interface IFingerUtility
    {
        //Base64字符串转byte
        byte[] Base64ToBlob(String strBase64);
        //byte转Base64字符串
        string BlobToBase64(byte[] buf, int len);
        //获取设备参数
        int GetParameters(IntPtr devHandle, int code, byte[] paramValue, ref int size);
        //设置设备参数
        int SetParameters(IntPtr devHandle, int code, byte[] paramValue, int size);
        //4字节byte数组转int
        bool ByteArray2Int(byte[] buf, ref int value);
        //int转4字节byte数组
        bool Int2ByteArray(int value, byte[] buf);
    }
}
