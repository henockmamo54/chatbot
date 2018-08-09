using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AAR_Bot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        CancellationTokenSource _getTokenAsyncCancellation = new CancellationTokenSource();

        protected void Application_Start()
        {
            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    // Bot Storage: register state storage for your bot
                    // Default store: volatile in-memory store - Only for prototyping!
                    // var store = new InMemoryDataStore();

                    var uri = new Uri("https://mjukje.documents.azure.com:443/");
                    var key = "5MfRHpG8g1x534IXnilPh4DzhZOk94S440mTZ4rCSTiUFnyzq1Fr4lnOi9IscugVUNNFVFWahEQs4Yo06AqQDA==";

                    var store = new DocumentDbBotDataStore(uri, key);

                    builder.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                        .AsSelf()
                        .SingleInstance();
                });

            GlobalConfiguration.Configure(WebApiConfig.Register);

            //============================================ for optimization ============================================
            //==========================================================================================================

            
            var appID = ConfigurationManager.AppSettings["MicrosoftAppId"];
            var appPassword = ConfigurationManager.AppSettings["MicrosoftAppPassword"];
            if (!string.IsNullOrEmpty(appID) && !string.IsNullOrEmpty(appPassword))
            {
                var credentials = new MicrosoftAppCredentials(appID, appPassword);
                Task.Factory.StartNew(async () =>
                {
                    while (!_getTokenAsyncCancellation.IsCancellationRequested)
                    {
                        try
                        {
                            var token = await credentials.GetTokenAsync().ConfigureAwait(false);
                        }
                        catch (MicrosoftAppCredentials.OAuthException ex)
                        {
                            Trace.TraceError(ex.Message);
                        }
                        await Task.Delay(TimeSpan.FromMinutes(1), _getTokenAsyncCancellation.Token).ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);
            }



        }

        protected void Application_End()
        {
            _getTokenAsyncCancellation.Cancel();
        }
    }
}
