using AAR_Bot.Dialogs;
using AAR_Bot.Helper.StoredStringValues;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace AAR_Bot.MessageReply
{
    public static class aboutHelp
    {
        public static async Task HelpOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice<string>(
                context,
                HandleHelpOptionSelected,
                RootDialog._storedvalues._helpOptionsList,
                RootDialog._storedvalues._helpOptionSelected,           //선택시 출력되는 메시지 정의
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : ShowHelpOptions",    //오류시 표시될 메시지 정의
                1,
                PromptStyle.Auto);
        }


        public static async Task HandleHelpOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);          //웰컴이 두번 불러지는 문제인가?

            else
            {
                if (value.ToString() == RootDialog._storedvalues._introduction) await Reply_introduction(context);     //이거 룻다이알로그에 스토얼 가져와서 인듯
                else if (value.ToString() == RootDialog._storedvalues._requestInformationCorrection) await Reply_requestInformationCorrection(context);
                else if (value.ToString() == RootDialog._storedvalues._contactMaster) await Reply_contactMaster(context);
                else if (value.ToString() == RootDialog._storedvalues._convertLanguage)
                {
                    if (RootDialog._storedvalues._convertLanguage == "한국어") RootDialog._storedvalues = new StoredValues_kr();   //for convert en to kr
                    else RootDialog._storedvalues = new StoredValues_en();                                  //for convert kr to en
                }

                await RootDialog.ShowWelcomeOptions(context);                  //Return To Start
            }

        }


        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_introduction(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Introduction;
            await context.PostAsync(activity);
        }

        public static async Task Reply_requestInformationCorrection(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_RequestInformationCorrection;

            await context.PostAsync(activity);
        }

        public static async Task Reply_contactMaster(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_ContactMaster;

            await context.PostAsync(activity);
        }


    }
}