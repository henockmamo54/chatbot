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
            bool noOption2 = true;

            switch (mystr)
            {
                case "1": await aboutCourseRegistration.CourseRegistraionOptionSelected(context); noOption = false; break;
                case "2": await aboutCourseInfo.CourseInfoOptionSelected(context); noOption = false; break;
                case "3": await aboutCredits.CreditsOptionSelected(context); noOption = false; break;
                case "4": await aboutOthers.OtherOptionSelected(context); noOption = false; break;
                case "5": await aboutHelp.HelpOptionSelected(context); noOption = false; break;
                case "6": await RootDialog.ShowWelcomeButtonOptions(context); noOption = false; break;
                case "직접 입력하기": await RootDialog.ShowWelcomeOptions(context); noOption = false; break;
            }

            foreach (List<string> lst in RootDialog._storedvalues._welcomeOptionVocaList)
            {
                if (noOption == true)
                {
                    foreach (string str in lst)
                    {
                        if (mystr.Contains(str))
                        {
                            noOption = false;

                            //========================================================================================
                            //수강신청 일때.

                            if (lst == RootDialog._storedvalues._welcomeOptionVocaList[0])
                            {

                                //수강신청에서의 depth 3를 찾는다.
                                foreach (List<string> lst2 in RootDialog._storedvalues._courseRegistrationVocaList)
                                {
                                    if (noOption2 == true)
                                    {
                                        foreach (string str2 in lst2)
                                        {
                                            if (mystr.Contains(str2))
                                            {
                                                noOption2 = false;
                                                if (lst2 == RootDialog._storedvalues._courseRegistrationVocaList[0]) await aboutCourseRegistration.Reply_howToDoIt(context);
                                                else if (lst2 == RootDialog._storedvalues._courseRegistrationVocaList[1]) await aboutCourseRegistration.Reply_schedule(context);
                                                else if (lst2 == RootDialog._storedvalues._courseRegistrationVocaList[2]) await aboutCourseRegistration.Reply_regulation(context);
                                                else if (lst2 == RootDialog._storedvalues._courseRegistrationVocaList[3]) await aboutCourseRegistration.Reply_terms(context);
                                            }
                                        }
                                    }
                                }

                                if (noOption2 == true)
                                {
                                    await aboutCourseRegistration.CourseRegistraionOptionSelected(context); //depth 3를 찾지못했으면 2로 간다.
                                }

                            }

                            //========================================================================================
                            //강의정보 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[1])
                            {
                                //수강신청에서의 depth 3를 찾는다.
                                foreach (List<string> lst2 in RootDialog._storedvalues._courseInfoVocaList)
                                {
                                    if (noOption2 == true)
                                    {
                                        foreach (string str2 in lst2)
                                        {
                                            if (mystr.Contains(str2))
                                            {
                                                noOption2 = false;
                                                
                                                if (lst2 == RootDialog._storedvalues._courseInfoVocaList[0]) await aboutCourseInfo.Reply_openedMajorCourses(context);
                                                else if (lst2 == RootDialog._storedvalues._courseInfoVocaList[1]) await aboutCourseInfo.Reply_openedLiberalArts(context);
                                                else if (lst2 == RootDialog._storedvalues._courseInfoVocaList[2]) await aboutCourseInfo.Reply_syllabus(context);
                                                else if (lst2 == RootDialog._storedvalues._courseInfoVocaList[3]) await aboutCourseInfo.Reply_lecturerInfo(context);
                                                else if (lst2 == RootDialog._storedvalues._courseInfoVocaList[4]) await aboutCourseInfo.Reply_mandatorySubject(context);
                                                else if (lst2 == RootDialog._storedvalues._courseInfoVocaList[5]) await aboutCourseInfo.Reply_prerequisite(context);
                                                else if (lst2 == RootDialog._storedvalues._welcomeOptionVocaList[6]) await RootDialog.ShowWelcomeButtonOptions(context);
                                            }
                                        }
                                    }
                                }

                                if (noOption2 == true)
                                {
                                    await aboutCourseInfo.CourseInfoOptionSelected(context); //depth 3를 찾지못했으면 2로 간다.
                                }

                            }

                            //========================================================================================
                            //학점관리 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[2])
                            {
                                await aboutCredits.CreditsOptionSelected(context);
                            }

                            //========================================================================================
                            //기타정보 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[3])
                            {
                                await aboutOthers.OtherOptionSelected(context);
                            }

                            //========================================================================================
                            //도움말 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[4])
                            {
                                await aboutHelp.HelpOptionSelected(context);
                            }

                            //========================================================================================
                            //처음으로 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[5])
                            {
                                await RootDialog.ShowWelcomeOptions(context);
                            }

                            //========================================================================================
                            //버튼메뉴 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[6])
                            {
                                await RootDialog.ShowWelcomeButtonOptions(context);
                            }

                            //========================================================================================
                            //언어 변경 일때.

                            else if (lst == RootDialog._storedvalues._welcomeOptionVocaList[7])
                            {
                                if (mystr == "한국어" || mystr == "Korean" || mystr == "korean") RootDialog._storedvalues = new StoredValues_kr();
                                else if (mystr == "영어" || mystr == "English" || mystr == "english") RootDialog._storedvalues = new StoredValues_en();
                                await RootDialog.ShowWelcomeOptions(context);
                            }

                            //========================================================================================
                        }
                    }
                }
            }

            mystr = "";

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