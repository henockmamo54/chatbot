using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using AAR_Bot.Helper;
using AAR_Bot.MessageReply;

namespace AAR_Bot.Dialogs
{
    [Serializable]
    public class LuisDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var value = await result;
            Rootobject obj = await LUISService.GetEntityFromLUIS(value.Text);

            switch (value.Text.ToString())
            {
                case "1": await aboutCourseRegistration.CourseRegistraionOptionSelected(context); break;
                case "2": await aboutCourseInfo.CourseInfoOptionSelected(context); break;
                case "3": await aboutCredits.CreditsOptionSelected(context); break;
                case "4": await aboutOthers.OtherOptionSelected(context); break;
                case "5": await aboutHelp.HelpOptionSelected(context); break;
                default: { await context.PostAsync("For now the chat bot works only for number inputs. \nWe are working on LUIS Integration. Thank you for your understanding.");
                        context.Done(obj.topScoringIntent.ToString()); break; }
            }

            //context.Done(obj.topScoringIntent.ToString());
        }
    }
}