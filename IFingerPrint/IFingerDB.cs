using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public interface IFingerDB
    {
        //初始化算法库
        IntPtr DBInit();
        //指纹1:1对比
        int DBMatch(IntPtr dbHandle, byte[] temp1, byte[] temp2);
        //释放算法库资源
        int DBFree(IntPtr dbHandle);
        //指纹模板三合一
        int DBMerge(IntPtr dbHandle, byte[] temp1, byte[] temp2, byte[] temp3, byte[] regTemp, ref int regTempLen);
        //指纹模板添加到内存
        int DBAdd(IntPtr dbHandle, int fid, byte[] regTemp);
        //指纹模板1:N匹配
        int DBIdentify(IntPtr dbHandle, byte[] temp, ref int fid, ref int score);
        //指纹模板删除
        int DBDel(IntPtr dbHandle, int fid);
        //清空内存
        int DBClear(IntPtr dbHandle);
    }
}
