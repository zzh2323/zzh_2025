using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFingerPrint
{
    public class FingerDB : IFingerDB
    {
        // 初始化算法库
        public IntPtr DBInit()
        {
            return zkfp2.DBInit();
        }

        // 释放算法库资源
        public int DBFree(IntPtr dbHandle)
        {
            return zkfp2.DBFree(dbHandle);
        }

        //指纹1:1对比
        public int DBMatch(IntPtr dbHandle, byte[] temp1, byte[] temp2)
        {
            return zkfp2.DBMatch(dbHandle, temp1, temp2);
        }

        // 指纹模板三合一
        public int DBMerge(IntPtr dbHandle, byte[] temp1, byte[] temp2, byte[] temp3, byte[] regTemp, ref int regTempLen)
        {
            return zkfp2.DBMerge(dbHandle, temp1, temp2, temp3, regTemp, ref regTempLen);
        }

        // 指纹模板添加到内存
        public int DBAdd(IntPtr dbHandle, int fid, byte[] regTemp)
        {
            return zkfp2.DBAdd(dbHandle, fid, regTemp);
        }

        // 指纹模板1:N匹配
        public int DBIdentify(IntPtr dbHandle, byte[] temp, ref int fid, ref int score)
        {
            return zkfp2.DBIdentify(dbHandle, temp, ref fid, ref score);
        }

        // 指纹模板删除
        public int DBDel(IntPtr dbHandle, int fid)
        {
            return zkfp2.DBDel(dbHandle, fid);
        }

        //清空内存
        public int DBClear(IntPtr dbHandle)
        {
            return zkfp2.DBClear(dbHandle);
        }
    }
}
