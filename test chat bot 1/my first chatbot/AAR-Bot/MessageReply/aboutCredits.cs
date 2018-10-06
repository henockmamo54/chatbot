using AAR_Bot.Dialogs;
using AAR_Bot.Helper.StoredStringValues;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AAR_Bot.MessageReply
{
    public static class aboutCredits
    {
        static int stuNum = 0;
        static StoredStringValuesMaster _storedvalues;
        public static async Task CreditsOptionSelected(IDialogContext context, bool isFirstTime = true)
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
                if (context.Activity.ChannelId == "facebook")
                {
                    if (isFirstTime)
                    {
                        var reply = context.MakeMessage();
                        reply.Text = _storedvalues._reply_CurrentCredits.Replace("Guide to my graduation.", "").Trim() + " = " + RootDialog.studentinfo.totalCredits(stuNum).ToString() +
                    "\n\n" + _storedvalues._reply_MajorCredits.Replace("Guide to major credit.", "").Trim() + " = " + RootDialog.studentinfo.totalMajorCredits(stuNum).ToString() +
                    "\n\n" + _storedvalues._reply_LiberalArtsCredits.Replace("Guide to major credit.", "").Trim() + " = " + RootDialog.studentinfo.totalElectiveCredits(stuNum).ToString();
                        await context.PostAsync(reply);
                    }

                    await showbuttonOptions(context);

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


        }

        public static async Task showbuttonOptions(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = _storedvalues._courseInfoSelected.Replace("\n", "\n\n ");
            activity.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = _storedvalues._currentCredits, Type=ActionTypes.ImBack, Value= _storedvalues._currentCredits },
                        new CardAction(){ Title = _storedvalues._majorCredits, Type=ActionTypes.ImBack, Value=_storedvalues._majorCredits },
                        new CardAction(){ Title = _storedvalues._liberalArtsCredits, Type=ActionTypes.ImBack, Value=_storedvalues._liberalArtsCredits },
                        new CardAction(){ Title = _storedvalues._courserecomendations, Type=ActionTypes.ImBack, Value=_storedvalues._courserecomendations },
                        new CardAction(){ Title = _storedvalues._changeStuNum, Type=ActionTypes.ImBack, Value=_storedvalues._changeStuNum},

                        new CardAction(){ Title = _storedvalues._gotostart, Type=ActionTypes.ImBack, Value="Go To Start" },
                        new CardAction(){ Title = "Help", Type=ActionTypes.ImBack, Value="Help" }
                    }
            };

            await context.PostAsync(activity);
            context.Wait(HandleCreditsOptionSelection);
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
                    else if (value.ToString() == _storedvalues._courserecomendations) await ReplyCourseRecomendationOptionSelected(context);


                    //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                    await CreditsOptionSelected(context, false);
                }
            }
        }

        //for facebook
        public static async Task HandleCreditsOptionSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var myresult = await result;
            string value = myresult.Text;

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
                    if (value.ToString() == _storedvalues._currentCredits) { await Reply_currentCredits(context); await CreditsOptionSelected(context, false); }
                    else if (value.ToString() == _storedvalues._majorCredits) { await Reply_majorCredits(context); await CreditsOptionSelected(context, false); }
                    else if (value.ToString() == _storedvalues._liberalArtsCredits) { await Reply_liberalArtsCredits(context); await CreditsOptionSelected(context, false); }
                    else if (value.ToString() == _storedvalues._courserecomendations || value.ToString()== "Course Recommendatio...") { await ReplyCourseRecomendationOptionSelected(context); await CreditsOptionSelected(context, false); }
                    else await LuisDialog.MessageReceivedAsync(context, result);
                }
            }
        }



        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_currentCredits(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();
            context.PrivateConversationData.TryGetValue<int>("stuNum", out stuNum);

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_CurrentCredits + RootDialog.studentinfo.totalCredits(stuNum);
            await context.PostAsync(activity);
        }

        public static async Task Reply_majorCredits(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();
            context.PrivateConversationData.TryGetValue<int>("stuNum", out stuNum);

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_MajorCredits + RootDialog.studentinfo.totalMajorCredits(stuNum);
            await context.PostAsync(activity);
        }

        public static async Task Reply_liberalArtsCredits(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();
            context.PrivateConversationData.TryGetValue<int>("stuNum", out stuNum);

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_LiberalArtsCredits + RootDialog.studentinfo.totalElectiveCredits(stuNum);

            await context.PostAsync(activity);
        }

        public static async Task Reply_changeStuNum(IDialogContext context)         //학번 재설정
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();
            context.PrivateConversationData.TryGetValue<int>("stuNum", out stuNum);

            await context.PostAsync(_storedvalues._reply_ChangeStuNum + stuNum);            //메시지를 보낸다.
            context.Call(new GetInfoDialog(), RootDialog.GetInfoDialogAfterResettingStudentNumber);     //바로 학번입력으로 간다.
        }
        public static async Task ReplyCourseRecomendationOptionSelected(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");

            var activity = context.MakeMessage();
            var reommenderreply = RootDialog.studentinfo.getrecommendedCourselist(stuNum).Trim().Replace("  ", ",");
            if (reommenderreply.Length == 0) activity.Text = "Sorry, we didn't get the appropriate recommendations for you.";
            else
                activity.Text = "The Recommended courses are: " + RootDialog.studentinfo.getrecommendedCourselist(stuNum).Trim().Replace("  ", ",");
            await context.PostAsync(activity);

        }
    }
}