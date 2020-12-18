using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniPaste
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            bool createdNew;
            //系统能够识别有名称的互斥，因此可以使用它禁止应用程序启动两次
            //第二个参数可以设置为产品的名称:Application.ProductName

            //每次启动应用程序，都会验证名称为SingletonWinAppMutex的互斥是否存在
            Mutex mutex = new Mutex(false, "MiniPaste", out createdNew);

            //如果已运行，则在前端显示
            //createdNew == false，说明程序已运行
            if (!createdNew)
            {
                Process instance = GetExistProcess();
                if (instance != null)
                {
                    Application.Exit();
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static Process GetExistProcess()
        {
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if ((process.Id != currentProcess.Id) &&
                    (Assembly.GetExecutingAssembly().Location == currentProcess.MainModule.FileName))
                {
                    return process;
                }
            }
            return null;
        }

    }
}
