using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using ZDC2911Demo.UI;

namespace ZDC2911Demo
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

            Application.Run(new CMForm());
        }
    }
}
