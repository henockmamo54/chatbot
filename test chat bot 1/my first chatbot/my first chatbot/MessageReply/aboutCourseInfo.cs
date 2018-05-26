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
    public static class aboutCourseInfo
    {
        public static async Task CourseInfoOptionSelected(IDialogContext context)
        {
            /*
            PromptDialog.Choice<string>(
                context,
                HandleCourseInfoOptionSelection,
                RootDialog._storedvalues._courseInfoOptions,
                RootDialog._storedvalues._courseInfoSelected,                                                                                 //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : CourseInfoOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
                */
            PromptDialog.Text(context, HandleCourseInfoOptionSelection, RootDialog._storedvalues._typePleaseCourseInfo);
        }


        public static async Task CourseInfoButtonOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice<string>(
                context,
                HandleCourseInfoOptionSelection,
                RootDialog._storedvalues._courseInfoOptions,
                RootDialog._storedvalues._courseInfoSelected,                                                                                 //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : CourseInfoOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
        }


        public static async Task HandleCourseInfoOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            bool noOption = true;
            var message = await result;
            
            switch (message)
            {
                case "1": await Reply_openedMajorCourses(context); noOption = false; break;
                case "2": await Reply_openedLiberalArts(context); noOption = false; break;
                case "3": await Reply_syllabus(context); noOption = false; break;
                case "4": await Reply_lecturerInfo(context); noOption = false; break;
                case "5": await Reply_mandatorySubject(context); noOption = false; break;
                case "6": await Reply_prerequisite(context); noOption = false; break;
                case "7": await RootDialog.ShowWelcomeOptions(context); noOption = false; break;
                case "8": await aboutHelp.HelpOptionSelected(context); noOption = false; break;
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

                            if (lst == RootDialog._storedvalues._courseInfoVocaList[6]) await aboutHelp.HelpOptionSelected(context);

                            else if (lst == RootDialog._storedvalues._courseInfoVocaList[7]) await RootDialog.ShowWelcomeOptions(context);

                            else if (lst == RootDialog._storedvalues._courseInfoVocaList[8])
                            {
                                if (message == "한국어" || message == "Korean" || message == "korean") RootDialog._storedvalues = new StoredValues_kr();
                                else if (message == "영어" || message == "English" || message == "english") RootDialog._storedvalues = new StoredValues_en();
                                await RootDialog.ShowWelcomeOptions(context);
                            }
                            //처음으로 돌아가야 하는 아이들
                            else
                            {
                                if (lst == RootDialog._storedvalues._courseInfoVocaList[0]) await Reply_openedMajorCourses(context);
                                else if (lst == RootDialog._storedvalues._courseInfoVocaList[1]) await Reply_openedLiberalArts(context);
                                else if (lst == RootDialog._storedvalues._courseInfoVocaList[2]) await Reply_syllabus(context);
                                else if (lst == RootDialog._storedvalues._courseInfoVocaList[3]) await Reply_lecturerInfo(context);
                                else if (lst == RootDialog._storedvalues._courseInfoVocaList[4]) await Reply_mandatorySubject(context);
                                else if (lst == RootDialog._storedvalues._courseInfoVocaList[5]) await Reply_prerequisite(context);
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
                if (value.ToString() == RootDialog._storedvalues._openedMajorCourses) await Reply_openedMajorCourses(context);
                else if (value.ToString() == RootDialog._storedvalues._openedLiberalArts) await Reply_openedLiberalArts(context);
                else if (value.ToString() == RootDialog._storedvalues._syllabus) await Reply_syllabus(context);
                else if (value.ToString() == RootDialog._storedvalues._lecturerInfo) await Reply_lecturerInfo(context);
                else if (value.ToString() == RootDialog._storedvalues._mandatorySubject) await Reply_mandatorySubject(context);
                else if (value.ToString() == RootDialog._storedvalues._prerequisite) await Reply_prerequisite(context);


                //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                await aboutCourseInfo.CourseInfoOptionSelected(context);
            }
            */
        }


        //================================================================================================================================================
        //Last Phase Option
        
        public static async Task Reply_openedMajorCourses(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_openedLiberalArts;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "이번학기 전공개설강의",
                Subtitle = "이번학기 전공개설강의",          //Location of information in MJU homepage
                Text = "이번학기 전공개설강의정보",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1iVNvUHc2-Qs_AXWGgnXpsPx3mp0BWCK7") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_openedLiberalArts(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_openedLiberalArts;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "이번학기 교양개설강의",
                Subtitle = "이번학기 교양개설강의",          //Location of information in MJU homepage
                Text = "이번학기 교양개설강의정보",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Q7Ej1JB2OHcBP-TjXEdZYWz8H7ncUtpd") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_syllabus(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Syllabus;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "강의계획서",
                Subtitle = "강의별 강의계획서",
                Text = "강의계획서 열람 정보입니다.",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Yn5FBeBVedQdodPsnM3I_1kWcKyL2abM") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_lecturerInfo(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_LecturerInfo;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "교수 정보",
                Subtitle = "교수 홈페이지 검색",
                Text = "교수 정보",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "http://home.mju.ac.kr/mainIndex/searchHomepage.action") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_mandatorySubject(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_MandatorySubject;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "정보통신공학과 선후수 과목정보",
                Subtitle = "정보통신공학과 선후수 과목정보",
                Text = "정보통신공학과 선후수 과목정보",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Fy7bAxihUXqlNLLToimYcKSiTHg_XdGe") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_prerequisite(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Prerequisite;
            activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "정보통신공학과 선후수 과목정보",
                Subtitle = "정보통신공학과 선후수 과목정보",
                Text = "정보통신공학과 선후수 과목정보",
                Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "http://www.mju.ac.kr/mbs/mjukr/images/editor/1406095802964_img_2017.jpg") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }
    }
}