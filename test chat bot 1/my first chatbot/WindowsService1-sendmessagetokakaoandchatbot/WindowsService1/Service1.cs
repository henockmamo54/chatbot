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
            await client.Conversations.PostActivityAsync(this.conversation.ConversationId, usermessage);

            var url = "https://mjukjeweb.azurewebsites.net/keyboard";
            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);

            using (StreamWriter writer =
            new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
            {
                //writer.WriteLine("Important data line 1");
                //writer.WriteLine("Line 2  ==== > Datasend");
                writer.WriteLine("Data Send: "+DateTime.Now.ToString());
                //writer.WriteLine("key board API");
                //writer.WriteLine(html.ToString());
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
            new System.Threading.Thread(async () => await ReadBotMessageAsync(client, conversation.ConversationId)).Start();
        }


        private async Task ReadBotMessageAsync(DirectLineClient client, string conversationId)
        {
            string watermark = null;
            while (true)
            {

                var activityset = await client.Conversations.GetActivitiesAsync(conversationId, watermark);
                watermark = activityset.Watermark;

                var activities = from x in activityset.Activities where x.From.Id == botID select x;

                foreach (Activity activity in activities)
                {
                    if (activity.Text != null)
                    {
                        string message = activity.Text;
                        //System.Diagnostics.Debug.WriteLine(message);

                        //using (StreamWriter writer =
                        //new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
                        //{
                        //    writer.WriteLine("======= data received");
                        //}
                        //// Append line to the file.
                        //using (StreamWriter writer =
                        //    new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
                        //{
                        //    writer.WriteLine(message);
                        //    writer.WriteLine(DateTime.Now.ToString());
                        //}


                    }
                }

            }
        }


        protected override void OnStart(string[] args)
        {
            InitClient();
            
            //try
            //{
            //    using (StreamWriter writer =
            //new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
            //    {
            //        writer.WriteLine("Important data line 1");
            //    }
            //    // Append line to the file.
            //    using (StreamWriter writer =
            //        new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
            //    {
            //        writer.WriteLine("Line 2");
            //        writer.WriteLine(DateTime.Now.ToString());
            //    }

            //}
            //catch (Exception ex)
            //{
            //    // Append line to the file.
            //    using (StreamWriter writer =
            //        new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt", true))
            //    {
            //        writer.WriteLine("errror");
            //        writer.WriteLine(ex.Message);
            //        writer.WriteLine(DateTime.Now.ToString());
            //    }
            //}

            thread = new Thread(this.DoWork);
            thread.Start();
        }

        private void DoWork()
        {
            while (true)
            {
                timer_elasped();
                //Thread.Sleep(1500000);
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
