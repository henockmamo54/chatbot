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
        public int stuNum = 0;
        public StoredStringValuesMaster _storedvalues;                           // to handle language localization StoredValues의 마스터를 만들어 둔다. 디폴트는 한국어로 되어있다.
        public static StudentInfoService studentinfo = new StudentInfoService();

        public async Task StartAsync(IDialogContext context)
        {

           string lang = "";
            if (context.PrivateConversationData.TryGetValue<string>("_storedvalues", out lang))
            {
                if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
                else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();
            }
            else
            {
                _storedvalues = new StoredValues_en();          //Default language is korean
                context.PrivateConversationData.SetValue("_storedvalues", "StoredValues_en");
            }

            context.Wait(MessageReceivedAsync);

            //return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            try
            {
                var value = await result;
                //if (value.Text.ToString() == "English") _storedvalues = new StoredValues_en();      //if user inputs english as text input or keyboard input language should change to english and the same holds for korean too
                //if (value.Text.ToString() == "한국어" || value.Text.ToString() == "korean") _storedvalues = new StoredValues_kr();      //if user inputs 한국어 as text input or keyboard input language should change to 한국어
                //*********here we have to add help,restart, reset and other options or have to be handled by Luis

                if (value.Text.ToString().Equals("English", StringComparison.InvariantCultureIgnoreCase))
                {
                   
                        //context.PrivateConversationData.SetValue("_storedvalues", new StoredValues_en());
                        context.PrivateConversationData.SetValue("_storedvalues", "StoredValues_en");
                        _storedvalues = new StoredValues_en();
                } // _storedvalues = new StoredValues_en();      //if user inputs english as text input or keyboard input language should change to english and the same holds for korean too
                else if (value.Text.ToString().Equals("한국어", StringComparison.InvariantCultureIgnoreCase) || value.Text.ToString().Equals("korean", StringComparison.InvariantCultureIgnoreCase)) 
                { //_storedvalues = new StoredValues_kr();      //if user inputs 한국어 as text input or keyboard input language should change to 한국어

                    context.PrivateConversationData.SetValue("_storedvalues", "StoredValues_kr");
                        _storedvalues = new StoredValues_kr();
                }

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
            //activity.Text = _storedvalues._typePleaseWelcome;
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");

            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) langtype = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) langtype = new StoredValues_kr();

            activity.Text = langtype._typePleaseWelcome;

            //activity.Attachments.Add(new HeroCard
            //{
            //    Title = "",
            //    Subtitle = "",          //Location of information in MJU homepage
            //    Text = "",
            //    //Images = new List<CardImage> { new CardImage("http://dynamicscrmcoe.com/wp-content/uploads/2016/08/chatbot-icon.png") },
            //    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
            //                                    "관련 페이지로 이동",
            //                                    value: "https://github.com/MJUKJE/chatbot/blob/dev/README.md") }
            //}.ToAttachment());

            await context.PostAsync(activity);

            context.Call(new LuisDialog(), LuisDialogResumeAfter);

        }

        private async static Task LuisDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {

            StoredStringValuesMaster _storedvalues = new StoredStringValuesMaster();
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            //await context.PostAsync(_storedvalues._goodByeMessage);
            await context.PostAsync(_storedvalues._goodByeMessage);

            await ShowWelcomeOptions(context);
        }


        public static async Task GetInfoDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                //stuNum = await result;
                var stuNum = await result;
                context.PrivateConversationData.SetValue("stuNum", stuNum);

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
                //stuNum = await result;
                var stuNum = await result;
                context.PrivateConversationData.SetValue("stuNum", stuNum);

                StoredStringValuesMaster _storedvalues= new StoredStringValuesMaster();
                string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
                if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
                else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

                //await context.PostAsync(_storedvalues._getStudentNumUpdateMessage + stuNum);
                await context.PostAsync(_storedvalues._getStudentNumUpdateMessage + stuNum);
                await aboutCredits.CreditsOptionSelected(context);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync(
                    $"I'm sorry, I'm having issues understanding you. Let's try again.\n" +
                    $"{ context.PrivateConversationData.GetValue<StoredStringValuesMaster>("_storedvalues")._printLine}");

                await ShowWelcomeOptions(context);
            }
        }
    }
}