using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using my_first_chatbot.Helper;
using my_first_chatbot.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace my_first_chatbot.MessageReply
{
    public static class aboutCourseRegistration
    {
        public static async Task CourseRegistraionOptionSelected(IDialogContext context)
        {
            /*
            PromptDialog.Choice<string>(
                context,
                HandleCourseRegistrationOptionSelection,
                RootDialog._storedvalues._courseRegistrationOptions,
                RootDialog._storedvalues._courseRegistrationSelected,                                                                                        //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : CourseRegistraionOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
            */
            PromptDialog.Text(context, HandleCourseRegistrationOptionSelection, RootDialog._storedvalues._typePleaseCourseRegistration);
        }

        public static async Task CourseRegistraionButtonOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice<string>(
                context,
                HandleCourseRegistrationOptionSelection,
                RootDialog._storedvalues._courseRegistrationOptions,
                RootDialog._storedvalues._courseRegistrationSelected,                                                                                        //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : CourseRegistraionOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
        }

        public static async Task HandleCourseRegistrationOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            bool noOption = true;
            var message = await result;
            

            switch (message)
            {
                case "1": await Reply_howToDoIt(context); noOption = false; break;
                case "2": await Reply_schedule(context); noOption = false; break;
                case "3": await Reply_regulation(context); noOption = false; break;
                case "4": await Reply_terms(context); noOption = false; break;
                case "5": await RootDialog.ShowWelcomeOptions(context); noOption = false; break;
                case "6": await aboutHelp.HelpOptionSelected(context); noOption = false; break;
                case "직접 입력하기": await RootDialog.ShowWelcomeOptions(context); noOption = false; break;
            }

            foreach (List<string> lst in RootDialog._storedvalues._courseRegistrationVocaList)
            {
                if (noOption == true)
                {
                    foreach (string str in lst)
                    {
                        if (message.Contains(str))
                        {
                            noOption = false;

                            if (lst == RootDialog._storedvalues._courseRegistrationVocaList[4]) await aboutHelp.HelpOptionSelected(context);

                            else if (lst == RootDialog._storedvalues._courseRegistrationVocaList[5]) await RootDialog.ShowWelcomeOptions(context);

                            else if (lst == RootDialog._storedvalues._courseRegistrationVocaList[6])
                            {
                                if (message == "한국어" || message == "Korean" || message == "korean") RootDialog._storedvalues = new StoredValues_kr();
                                else if(message == "영어" || message == "English" || message == "english") RootDialog._storedvalues = new StoredValues_en();
                                await RootDialog.ShowWelcomeOptions(context);
                            }
                            //처음으로 돌아가야 하는 아이들
                            else
                            {
                                if (lst == RootDialog._storedvalues._courseRegistrationVocaList[0]) await Reply_howToDoIt(context);
                                else if (lst == RootDialog._storedvalues._courseRegistrationVocaList[1]) await Reply_schedule(context);
                                else if (lst == RootDialog._storedvalues._courseRegistrationVocaList[2]) await Reply_regulation(context);
                                else if (lst == RootDialog._storedvalues._courseRegistrationVocaList[3]) await Reply_terms(context);
                                else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[6]) await RootDialog.ShowWelcomeButtonOptions(context);
                            }
                        }
                    }
                }
            }

            message = "";

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



            

            /*
            var value = await result;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._howToDoIt) await Reply_howToDoIt(context);      //각각의 메서드에 연결
                else if (value.ToString() == RootDialog._storedvalues._schedule) await Reply_schedule(context);     //각각의 Dialog로 연결하는 것 보다 편한듯
                else if (value.ToString() == RootDialog._storedvalues._regulation) await Reply_regulation(context);
                else if (value.ToString() == RootDialog._storedvalues._terms) await Reply_terms(context);


                //await RootDialog.ShowWelcomeOptions(context);                  //Return To Start
                await aboutCourseRegistration.CourseRegistraionOptionSelected(context);
            }
            */
        }


        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_howToDoIt(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_HowToDoIt;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseRegistrationOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "수강신청 방법",
                Subtitle = "온라인서비스-학사운영-수강신청",          //Location of information in MJU homepage
                Text = "수강신청 방법",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1G4Vnh3vDnpZ5AXgwgSQy88k7wEgPermE") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_schedule(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Schedule;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseRegistrationOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "수강신청 일정",
                Subtitle = "온라인서비스-공지사항-일반공지",
                Text = "수강신청 일정",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1hkUuDnWVq4LgS5odnhda4CeZSkhdiET2") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_regulation(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Regulation;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseRegistrationOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "명지대학교 학칙",
                Subtitle = "2018.05.01 개정",
                Text = "명지대학교 학칙 [ 2018.05.01 ]",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "http://law.mju.ac.kr/lmxsrv/law/lawviewer.srv?lawseq=69&hseq=1571&refid=undefined") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_terms(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Terms;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseRegistrationOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "수강신청관련 용어정리",
                Subtitle = "수강신청관련 용어정리",
                Text = "수강신청관련 용어정리",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=13K60TUyp8Cim21w5jFmPZ-CWR5Ub-0iHDLtl8wbN0D0") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }
    }
}