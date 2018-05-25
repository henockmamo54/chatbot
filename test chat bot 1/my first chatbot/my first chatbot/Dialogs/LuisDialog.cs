using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using my_first_chatbot.MessageReply;

namespace my_first_chatbot.Dialogs
{
    [Serializable]
    [LuisModel("1c2971cf-2e31-4abb-9f03-0932c48fb838", "adc70e51f80e4c6a8431de30d094042b")]
    public class LuisDialog : LuisDialog<Activity>
    {
        /*
            string strtemp = "";
            for (int i = 0; i < result.Entities.Count; i++)
            {
                strtemp = strtemp + (i + 1) + " Entitie의 타입은 : " + result.Entities[i].Type + "  Entitie의 Target Text는 " + result.Entities[i].Entity + " 입니다.###";
            }
            var activity = context.MakeMessage();
            activity.Text = $"{strtemp} Intent는 {result.Intents[0].Intent}. 해당 Intent일 확률은 {result.Intents[0].Score}% 입니다. {result.Entities.Count} is Entities count //";
            context.Done(activity);
        */

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var activity = context.MakeMessage();
            activity.Text = $"말씀을 이해하지 못했습니다..";
            //await context.PostAsync(activity);
            context.Done(activity);
        }



        [LuisIntent("CourseRegistration")]
        public async Task CourseRegistrationIntent(IDialogContext context, LuisResult result)
        {
            await aboutCourseRegistration.CourseRegistraionOptionSelected(context);
        }



        [LuisIntent("CourseInformation")]
        public async Task CourseInformationIntent(IDialogContext context, LuisResult result)
        {
            await aboutCourseInfo.CourseInfoOptionSelected(context);
        }



        [LuisIntent("Credits")]
        public async Task CreditsIntent(IDialogContext context, LuisResult result)
        {
            if(result.Query == "1") await aboutCourseRegistration.CourseRegistraionOptionSelected(context);
            else if (result.Query == "2") await aboutCourseInfo.CourseInfoOptionSelected(context);
            else if (result.Query == "3") await aboutCredits.CreditsOptionSelected(context);
            else if (result.Query == "4") await aboutOthers.OtherOptionSelected(context);
            else if (result.Query == "5") await aboutHelp.HelpOptionSelected(context);
            else if (result.Query == "6")
            {
                PromptDialog.Choice<string>(
                    context,
                    RootDialog.HandleWelcomeOptionSelected,
                    RootDialog._storedvalues._welcomeOptionsList,
                    RootDialog._storedvalues._welcomeMessage,                          //선택시 출력되는 메시지 정의
                    RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : showWelcomeOptions",    //오류시 표시될 메시지 정의
                    1,
                    PromptStyle.Auto);
            }
            else await aboutCredits.CreditsOptionSelected(context);
        }



        [LuisIntent("Others")]
        public async Task OthersIntent(IDialogContext context, LuisResult result)
        {
            await aboutOthers.OtherOptionSelected(context);
        }



        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await aboutHelp.HelpOptionSelected(context);
        }



        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            var activity = context.MakeMessage();
            activity.Text = $"인사해주셔서 감사해요." +
                             $"좋은하루 되시길 바랄게요 :)\n";
            
            context.Done(activity);
        }



        [LuisIntent("GoToStart")]
        public async Task GoToStartIntent(IDialogContext context, LuisResult result)
        {
            var activity = context.MakeMessage();
            activity.Text = $"시작메뉴로 이동합니다.";

            context.Done(activity);
        }
        
    }
}