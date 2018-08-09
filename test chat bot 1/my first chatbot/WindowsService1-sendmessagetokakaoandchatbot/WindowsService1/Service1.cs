using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private static string directlinesecrte = "xH9A5lssbfo.cwA.Wes.vxPHmR6lEWw_LOZCr2OfiOMfl6TTmGLa6MurY3TxVds";
        private static string botID = "MJUKJECHATBOT";
        private static string fromUser = "fadsfa";
        private static string id = "dsfasdf";

        public DirectLineClient client;
        private Conversation conversation;
        Thread thread;

        public Service1()
        {
            InitializeComponent();

        }


        private async void timer_elasped()
        {
            Activity usermessage = new Activity
            {
                From = new ChannelAccount(id, fromUser),
                Text = "Hello",
                Type = ActivityTypes.Message,
                TextFormat = "plain"
            };
            //chat bot service
            await client.Conversations.PostActivityAsync(this.conversation.ConversationId, usermessage);

            //kakao web service
            var url = "https://mjukjeweb.azurewebsites.net/keyboard";
            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);

            using (StreamWriter writer =
            new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
            {
                writer.WriteLine("Data Send: " + DateTime.Now.ToString());
            }

        }

        public void onDebug()
        {
            OnStart(null);
        }
        private void InitClient()
        {
            client = new DirectLineClient(directlinesecrte);
            conversation = client.Conversations.StartConversation();
        }



        protected override void OnStart(string[] args)
        {
            InitClient();

            thread = new Thread(this.DoWork);
            thread.Start();
        }

        private void DoWork()
        {
            while (true)
            {
                timer_elasped();
                Thread.Sleep(900000);
            }
        }

        protected override void OnStop()
        {
            using (StreamWriter writer =
            new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStop.txt", true))
            {
                writer.WriteLine("Important data line 1 " + DateTime.Now.ToString());
            }

        }
    }
}
