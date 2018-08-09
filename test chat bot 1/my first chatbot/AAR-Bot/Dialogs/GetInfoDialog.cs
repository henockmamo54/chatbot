using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AAR_Bot.Helper.StoredStringValues;

namespace AAR_Bot.Dialogs
{
    [Serializable]
    public class GetInfoDialog : IDialog<int>
    {
        private int attempts = 3;
        static StoredStringValuesMaster _storedvalues;
        public async Task StartAsync(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

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
                    await context.PostAsync(_storedvalues._getStudentNumFail);

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