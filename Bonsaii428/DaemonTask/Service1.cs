using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleTest
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private void t_Elapsed(Object sender, EventArgs e)
        {
            new CommonTool().WriteTestLog("Debug message.....");
        }
        protected override void OnStart(string[] args)
        {
            Timer t = new Timer(1000*60);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.AutoReset = true;
            t.Enabled = true;
        }

        protected override void OnStop()
        {
        }
    }
}
