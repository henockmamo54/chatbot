using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AAR_Bot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

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

        }
    }
}
