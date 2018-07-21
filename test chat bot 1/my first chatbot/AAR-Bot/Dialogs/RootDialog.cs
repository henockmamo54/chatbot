using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AAR_Bot.Helper;
using AAR_Bot.Helper.StoredStringValues;
using System.Collections.Generic;
using AAR_Bot.MessageReply;

namespace AAR_Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public static int stuNum = 0;
        public static StoredStringValuesMaster _storedvalues;                           // to handle language localization StoredValues의 마스터를 만들어 둔다. 디폴트는 한국어로 되어있다.
        public static StudentInfoService studentinfo = new StudentInfoService();

        public async Task StartAsync(IDialogContext context)
        {
            _storedvalues = new StoredValues_en();          //Default language is korean
            context.Wait(MessageReceivedAsync);

            //return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            try
            {
                var value = await result;
                if (value.Text.ToString() == "English") _storedvalues = new StoredValues_en();      //if user inputs english as text input or keyboard input language should change to english and the same holds for korean too
                if (value.Text.ToString() == "한국어" || value.Text.ToString() == "korean") _storedvalues = new StoredValues_kr();      //if user inputs 한국어 as text input or keyboard input language should change to 한국어
                //*********here we have to add help,restart, reset and other options or have to be handled by Luis

                await ShowWelcomeOptions(context);
            }
            catch (Exception ee)                                        //Exception 잡아주기
            {
                string msg = ee.Message;
            }
        }


        public static async Task ShowWelcomeOptions(IDialogContext context)
        {
            var activity = context.MakeMessage();

            if (activity.ChannelId == "facebook")
            {
                activity.Text = _storedvalues._typePleaseWelcome.Replace("\n", "\n\n");
                activity.SuggestedActions = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = "Registration info.", Type=ActionTypes.ImBack, Value="Course Registration" },
                        new CardAction(){ Title = "Course related info", Type=ActionTypes.ImBack, Value="Subject related information" },
                        new CardAction(){ Title = "Credit management", Type=ActionTypes.ImBack, Value="credit" },
                        new CardAction(){ Title = "Help", Type=ActionTypes.ImBack, Value="Help" },
                        new CardAction(){ Title = "English", Type=ActionTypes.ImBack, Value="Help" },
                        new CardAction(){ Title = "Korean", Type=ActionTypes.ImBack, Value="Help" }
                    }
                };
            }
            else activity.Text = _storedvalues._typePleaseWelcome;
                        
            await context.PostAsync(activity);

            context.Call(new LuisDialog(), LuisDialogResumeAfter);

        }

        private async static Task LuisDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(_storedvalues._goodByeMessage);
            await ShowWelcomeOptions(context);
        }


        public static async Task GetInfoDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                stuNum = await result;

                await aboutCredits.CreditsOptionSelected(context);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await ShowWelcomeOptions(context);
            }
            //throw new NotImplementedException();
        }

        public static async Task GetInfoDialogAfterResettingStudentNumber(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                stuNum = await result;

                await context.PostAsync(_storedvalues._getStudentNumUpdateMessage + stuNum);
                await aboutCredits.CreditsOptionSelected(context);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync(
                    $"I'm sorry, I'm having issues understanding you. Let's try again.\n" +
                    $"{ _storedvalues._printLine}");

                await ShowWelcomeOptions(context);
            }
        }
    }
}