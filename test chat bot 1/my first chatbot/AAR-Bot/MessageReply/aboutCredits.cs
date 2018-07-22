using AAR_Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AAR_Bot.MessageReply
{
    public static class aboutCredits
    {

        public static async Task CreditsOptionSelected(IDialogContext context)
        {
            if (RootDialog.stuNum == 0)
            {
                await context.PostAsync(RootDialog._storedvalues._getStudentNumMessage);
                context.Call(new GetInfoDialog(), RootDialog.GetInfoDialogResumeAfter);                //get student number
            }
            else
            {
                if (context.Activity.ChannelId == "facebook")
                {
                    var reply = context.MakeMessage();
                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    reply.Attachments.Add(GetReceiptCard());
                    await context.PostAsync(reply);


                    var activity = context.MakeMessage();
                    activity.Text = RootDialog._storedvalues._courseInfoSelected.Replace("\n", "\n\n ");
                    activity.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = RootDialog._storedvalues._currentCredits, Type=ActionTypes.ImBack, Value= RootDialog._storedvalues._currentCredits },
                        new CardAction(){ Title = RootDialog._storedvalues._majorCredits, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._majorCredits },
                        new CardAction(){ Title = RootDialog._storedvalues._liberalArtsCredits, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._liberalArtsCredits },
                        new CardAction(){ Title = RootDialog._storedvalues._changeStuNum, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._changeStuNum},
                       
                        new CardAction(){ Title = RootDialog._storedvalues._gotostart, Type=ActionTypes.ImBack, Value="Go To Start" },
                        new CardAction(){ Title = "Help", Type=ActionTypes.ImBack, Value="Help" }
                    }
                    };

                    await context.PostAsync(activity);
                    context.Wait(HandleCreditsOptionSelection);
                }
                else
                {
                    PromptDialog.Choice<string>(
                context,
                HandleCreditsOptionSelection,
                RootDialog._storedvalues._creditsOptions,
                RootDialog._storedvalues._creditsOptionSelected + "\n현재 설정된 학번 : " + RootDialog.stuNum,                                                                                 //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : CreditOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
                }
            }


        }


        public static Attachment GetReceiptCard()
        {
            var receiptCard = new ReceiptCard
            {
                //Title = "John Doe",
                //Facts = new List<Fact> { new Fact("Order Number", "1234"), new Fact("Payment Method", "VISA 5555-****") },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem(RootDialog._storedvalues._reply_CurrentCredits.Replace("Guide to my graduation.","").Trim(), price: RootDialog.studentinfo.totalCredits(RootDialog.stuNum).ToString()),
                    new ReceiptItem(RootDialog._storedvalues._reply_MajorCredits.Replace("Guide to major credit.","").Trim(), price: RootDialog.studentinfo.totalMajorCredits(RootDialog.stuNum).ToString()),
                    new ReceiptItem(RootDialog._storedvalues._reply_LiberalArtsCredits.Replace("Guidance on Liberal Arts credits.".Trim(),""), price: RootDialog.studentinfo.totalElectiveCredits(RootDialog.stuNum).ToString()),
                    //new ReceiptItem("App Service", price: "$ 45.00", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                //Tax = "$ 7.50",
                //Total = "$ 90.95",
            };

            return receiptCard.ToAttachment();
        }

        public static async Task HandleCreditsOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._changeStuNum)
                {               //학번 재설정 요청일시
                    await Reply_changeStuNum(context);                                          //학번 재설정으로 연결
                }
                else
                {
                    if (value.ToString() == RootDialog._storedvalues._currentCredits) await Reply_currentCredits(context);
                    else if (value.ToString() == RootDialog._storedvalues._majorCredits) await Reply_majorCredits(context);
                    else if (value.ToString() == RootDialog._storedvalues._liberalArtsCredits) await Reply_liberalArtsCredits(context);


                    //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                    aboutCredits.CreditsOptionSelected(context);
                }
            }
        }

        //for facebook
        public static async Task HandleCreditsOptionSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var myresult = await result;
            string value = myresult.Text;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._changeStuNum)
                {               //학번 재설정 요청일시
                    await Reply_changeStuNum(context);                                          //학번 재설정으로 연결
                }
                else
                {
                    if (value.ToString() == RootDialog._storedvalues._currentCredits) await Reply_currentCredits(context);
                    else if (value.ToString() == RootDialog._storedvalues._majorCredits) await Reply_majorCredits(context);
                    else if (value.ToString() == RootDialog._storedvalues._liberalArtsCredits) await Reply_liberalArtsCredits(context);


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
            activity.Text = RootDialog._storedvalues._reply_CurrentCredits + RootDialog.studentinfo.totalCredits(RootDialog.stuNum);
            await context.PostAsync(activity);
        }

        public static async Task Reply_majorCredits(IDialogContext context)
        {
            var activity = context.MakeMessage();

            activity.Text = RootDialog._storedvalues._reply_MajorCredits + RootDialog.studentinfo.totalMajorCredits(RootDialog.stuNum);

            await context.PostAsync(activity);
        }

        public static async Task Reply_liberalArtsCredits(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_LiberalArtsCredits + RootDialog.studentinfo.totalElectiveCredits(RootDialog.stuNum);

            await context.PostAsync(activity);
        }

        public static async Task Reply_changeStuNum(IDialogContext context)         //학번 재설정
        {
            await context.PostAsync(RootDialog._storedvalues._reply_ChangeStuNum + RootDialog.stuNum);            //메시지를 보낸다.
            context.Call(new GetInfoDialog(), RootDialog.GetInfoDialogAfterResettingStudentNumber);     //바로 학번입력으로 간다.
        }
    }
}