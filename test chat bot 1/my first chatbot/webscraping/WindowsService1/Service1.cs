using System;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        Thread thread;

        public Service1()
        {
            InitializeComponent();
            
        }
        public void onDebug()
        {
            OnStart(null);
        }
        private void timer_elasped()
        {
            webscraping.Program.runTheAction();
        }

        private void DoWork()
        {
            while (true)
            {
                timer_elasped();
                Thread.Sleep(900000);
            }
        }

        protected override void OnStart(string[] args)
        {
            thread = new Thread(this.DoWork);
            thread.Start();
        }

        protected override void OnStop()
        {
            
        }
    }
}
