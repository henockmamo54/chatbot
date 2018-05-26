using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using my_first_chatbot.Helper;
using my_first_chatbot.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace my_first_chatbot.MessageReply
{
    public static class aboutTypeSelf
    {

        public static async Task TypeSelfOptionSelected(IDialogContext context)
        {

            await context.PostAsync(RootDialog._storedvalues._typePleaseWelcome);
            context.Call<Activity>(new LuisDialog(), RootDialog.LuisDialogResumeAfter);                //get student number
            
        }

        


        
    }
}