using AAR_Bot.Dialogs;
using AAR_Bot.Helper.StoredStringValues;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace AAR_Bot.MessageReply
{
    public static class aboutCredits
    {
        static int stuNum = 0;
        static StoredStringValuesMaster _storedvalues;
        public static async Task CreditsOptionSelected(IDialogContext context)
        {
            context.PrivateConversationData.TryGetValue<int>("stuNum", out stuNum);
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            if (stuNum == 0)
            {
                //await context.PostAsync(_storedvalues._getStudentNumMessage);
                await context.PostAsync(_storedvalues._getStudentNumMessage);
                context.Call(new GetInfoDialog(), RootDialog.GetInfoDialogResumeAfter);                //get student number
            }
            else
            {
                PromptDialog.Choice<string>(
                context,
                HandleCreditsOptionSelection,
                _storedvalues._creditsOptions,
                _storedvalues._creditsOptionSelected + "\n현재 설정된 학번 : " + stuNum,                                                                                 //Course Registration
                _storedvalues._invalidSelectionMessage + "[ERROR] : CreditOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
            }


        }



        public static async Task HandleCreditsOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == _storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == _storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == _storedvalues._changeStuNum)
                {               //학번 재설정 요청일시
                    await Reply_changeStuNum(context);                                          //학번 재설정으로 연결
                }
                else
                {
                    if (value.ToString() == _storedvalues._currentCredits) await Reply_currentCredits(context);
                    else if (value.ToString() == _storedvalues._majorCredits) await Reply_majorCredits(context);
                    else if (value.ToString() == _storedvalues._liberalArtsCredits) await Reply_liberalArtsCredits(context);


                    //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                    aboutCredits.CreditsOptionSelected(context);
                }
            }
        }


        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_currentCredits(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_CurrentCredits + RootDialog.studentinfo.totalCredits(stuNum);
            await context.PostAsync(activity);
        }

        public static async Task Reply_majorCredits(IDialogContext context)
        {
            var activity = context.MakeMessage();

            activity.Text = _storedvalues._reply_MajorCredits + RootDialog.studentinfo.totalMajorCredits(stuNum);

            await context.PostAsync(activity);
        }

        public static async Task Reply_liberalArtsCredits(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_LiberalArtsCredits + RootDialog.studentinfo.totalElectiveCredits(stuNum);

            await context.PostAsync(activity);
        }

        public static async Task Reply_changeStuNum(IDialogContext context)         //학번 재설정
        {
            await context.PostAsync(_storedvalues._reply_ChangeStuNum + stuNum);            //메시지를 보낸다.
            context.Call(new GetInfoDialog(), RootDialog.GetInfoDialogAfterResettingStudentNumber);     //바로 학번입력으로 간다.
        }
    }
}