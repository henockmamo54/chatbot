using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using my_first_chatbot.Helper;
using my_first_chatbot.Forms;
using my_first_chatbot.MessageReply;
using Microsoft.Bot.Builder.FormFlow;
using my_first_chatbot.Helper.StoredStringValues;

namespace my_first_chatbot.Dialogs
{
    [Serializable]
    public class FakeLuisDialog : IDialog<IMessageActivity>
    {
        static string mystr = "";
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            mystr = message.Text;
            await ShowFakeOptions(context);
            
        }

        public static async Task ShowFakeOptions(IDialogContext context)
        {
            bool noOption = true;

            switch(mystr)
            {
                case "1": await aboutCourseRegistration.CourseRegistraionOptionSelected(context); noOption = false; break;
                case "2": await aboutCourseInfo.CourseInfoOptionSelected(context); noOption = false; break;
                case "3": await aboutCredits.CreditsOptionSelected(context); noOption = false; break;
                case "4": await aboutOthers.OtherOptionSelected(context); noOption = false; break;
                case "5": await aboutHelp.HelpOptionSelected(context); noOption = false; break;
                case "6": await RootDialog.ShowWelcomeButtonOptions(context); noOption = false; break;
            }

            foreach (List<string> lst in RootDialog._storedvalues._welcomeOptionVocaList)
            {
                if(noOption == true)
                {
                    foreach (string str in lst)
                    {
                        if (mystr.Contains(str))
                        {
                            noOption = false;
                            if (lst == RootDialog._storedvalues._welcomeOptionVocaList[0]) await aboutCourseRegistration.CourseRegistraionOptionSelected(context);
                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[1]) await aboutCourseInfo.CourseInfoOptionSelected(context);
                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[2]) await aboutCredits.CreditsOptionSelected(context);
                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[3]) await aboutOthers.OtherOptionSelected(context);
                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[4]) await aboutHelp.HelpOptionSelected(context);
                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[5]) await RootDialog.ShowWelcomeButtonOptions(context);
                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[6])
                            {
                                if(mystr == "한국어" || mystr == "Korean" || mystr == "korean") RootDialog._storedvalues = new StoredValues_kr();    
                                else if (mystr == "영어" || mystr == "English" || mystr == "english") RootDialog._storedvalues = new StoredValues_en();
                                await RootDialog.ShowWelcomeOptions(context);
                            }
                        }
                    }
                }
            }

            if (noOption == true)
            {
                PromptDialog.Choice<string>(
                context,
                RootDialog.HandleWelcomeOptionSelected,
                RootDialog._storedvalues._welcomeOptionsList,
                RootDialog._storedvalues._sorryMessage,                                                                                 //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage,          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
            }

        }


        
    }
}