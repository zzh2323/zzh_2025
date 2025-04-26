using IFingerPrint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace FingerTest
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 配置服务容器
            var services = new ServiceCollection();
            ConfigureServices(services);

            // 构建服务提供器
            var serviceProvider = services.BuildServiceProvider();

            // 从容器中解析 Form1 并运行
            var form1 = serviceProvider.GetRequiredService<Form1>();
            Application.Run(form1);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // 注册接口和实现
            services.AddSingleton<IFingerLib, FingerLib>();
            services.AddSingleton<IFingerDevice, FingerDevice>();
            services.AddSingleton<IFingerDB, FingerDB>();
            services.AddSingleton<IFingerManage, FingerManage>();
            services.AddSingleton<IFingerUtility, FingerUtility>();

            // 注册 Form1
            services.AddSingleton<Form1>();
        }
    }
}
