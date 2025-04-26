using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libzkfpcsharp;
namespace IFingerPrint
{
    public class FingerLib : IFingerLib
    {
        //初始化库
        public int Initialize()
        {
            return zkfp2.Init();
        }
        //释放库资源
        public int Terminate()
        {
            return zkfp2.Terminate();
        }
    }
}
