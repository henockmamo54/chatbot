using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AAR_Bot.Dialogs
{
    [Serializable]
    public class GetInfoDialog : IDialog<int>
    {
        private int attempts = 3;
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            int stuNum;

            if (Int32.TryParse(message.Text, out stuNum) && (message.Text.Length == 8))
            {
                context.Done(stuNum);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync(RootDialog._storedvalues._getStudentNumFail);

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not valid."));
                }
            }
        }
    }
}