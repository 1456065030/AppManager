using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppManage
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //让程序只能运行一个
            bool createdNew;
            System.Threading.Mutex instance = new System.Threading.Mutex(true, "AppManager_Veasion", out createdNew);
            if (createdNew)
            {
                try
                {
                    Application.Run(new Form1());
                }
                catch(Exception ex)
                {
                    MessageBox.Show("未预知错误：\n"+ex.Message+"\n正在重新启动...","错误-main",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                instance.ReleaseMutex();
            }
            else
            {
                Application.Exit();
            } 
            
        }
    }
}
