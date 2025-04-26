using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libzkfpcsharp;

namespace IFingerPrint
{
    public interface IFingerLib
    {
        //初始化库
        int Initialize();
        //释放库资源
        int Terminate();
    }
}
