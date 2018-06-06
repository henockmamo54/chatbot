using AAR_Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AAR_Bot.MessageReply
{
    public static class aboutOthers
    {
        public static async Task OtherOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice<string>(
                context,
                HandleOtherOptionSelection,
                RootDialog._storedvalues._othersOption,
                RootDialog._storedvalues._otherOptionSelected,                                                                                 //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : OtherOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);

        }
        public static async Task HandleOtherOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._leaveOrReadmission) await Reply_leaveOrReadmission(context);
                else if (value.ToString() == RootDialog._storedvalues._scholarship) await Reply_scholarship(context);


                //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                await aboutOthers.OtherOptionSelected(context);
            }
        }


        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_leaveOrReadmission(IDialogContext context)
        {
            /*
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_leaveOrReadmission;
            await context.PostAsync(activity);
            */

            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_leaveOrReadmission;

            activity.Attachments.Add(new HeroCard
            {
                Title = "휴학 및 복학관련 정보입니다.",
                Subtitle = "휴학 및 복학관련 정보입니다.",          //Location of information in MJU homepage
                Text = "휴학 및 복학관련 정보입니다.",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") }, 
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1YXE91epV_0_l8_lsgkXn1f9rYeF4_DfG") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_scholarship(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Scholarship;

            activity.Attachments.Add(new HeroCard
            {
                Title = "장학금관련 정보입니다.",
                Subtitle = "장학금관련 정보입니다.",          //Location of information in MJU homepage
                Text = "장학금관련 정보입니다.",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=112Fs5ZE3ek3AzQBiCrKLXLuLxkWCPvo_") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

    }
}