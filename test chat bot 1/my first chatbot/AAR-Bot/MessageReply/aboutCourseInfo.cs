using AAR_Bot.Dialogs;
using AAR_Bot.Helper.StoredStringValues;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AAR_Bot.MessageReply
{
    public static class aboutCourseInfo
    {
        static StoredStringValuesMaster _storedvalues;
        public static async Task CourseInfoOptionSelected(IDialogContext context)
        {
            if (context.Activity.ChannelId == "facebook")
            {
                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCardsAttachments();
                await context.PostAsync(reply);

                await showOptions(context);
            }
            else
            {
                 string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            PromptDialog.Choice<string>(
                context,
                HandleCourseInfoOptionSelection,
                _storedvalues._courseInfoOptions,
                _storedvalues._courseInfoSelected,                                                                                 //Course Registration
                _storedvalues._invalidSelectionMessage + "[ERROR] : CourseInfoOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);
            }

        }


        public static async Task showOptions(IDialogContext context) {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._courseInfoSelected.Replace("\n", "\n\n ");
            activity.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = RootDialog._storedvalues._openedMajorCourses, Type=ActionTypes.ImBack, Value="Opened LiberalArts" },
                        new CardAction(){ Title = RootDialog._storedvalues._openedLiberalArts, Type=ActionTypes.ImBack, Value="Opened Major" },
                        new CardAction(){ Title = RootDialog._storedvalues._syllabus, Type=ActionTypes.ImBack, Value="Syllabus" },
                        new CardAction(){ Title = RootDialog._storedvalues._lecturerInfo, Type=ActionTypes.ImBack, Value="Lecture Info" },
                        new CardAction(){ Title = RootDialog._storedvalues._mandatorySubject, Type=ActionTypes.ImBack, Value="Mandatory Subject" },
                        new CardAction(){ Title = RootDialog._storedvalues._prerequisite, Type=ActionTypes.ImBack, Value="Prerequisite" },
                        new CardAction(){ Title = RootDialog._storedvalues._gotostart, Type=ActionTypes.ImBack, Value="Go To Start" },
                        new CardAction(){ Title = "Help", Type=ActionTypes.ImBack, Value="Help" }
                    }
            };

            await context.PostAsync(activity);
            context.Wait(HandleCourseInfoOptionSelection);
        }

        public static async Task HandleCourseInfoOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == _storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == _storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == _storedvalues._openedMajorCourses) await Reply_openedMajorCourses(context);
                else if (value.ToString() == _storedvalues._openedLiberalArts) await Reply_openedLiberalArts(context);
                else if (value.ToString() == _storedvalues._syllabus) await Reply_syllabus(context);
                else if (value.ToString() == _storedvalues._lecturerInfo) await Reply_lecturerInfo(context);
                else if (value.ToString() == _storedvalues._mandatorySubject) await Reply_mandatorySubject(context);
                else if (value.ToString() == _storedvalues._prerequisite) await Reply_prerequisite(context);
                else if (value.ToString() == _storedvalues._help) await RootDialog.ShowWelcomeOptions(context);
                else if (value.ToString() == _storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

                //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                await CourseInfoOptionSelected(context);
            }
        }

        // for facebook
        public static async Task HandleCourseInfoOptionSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var myresult = await result;
            string value = myresult.Text;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);
            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._openedMajorCourses) { await Reply_openedMajorCourses(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._openedLiberalArts) { await Reply_openedLiberalArts(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._syllabus) { await Reply_syllabus(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._lecturerInfo) { await Reply_lecturerInfo(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._mandatorySubject) { await Reply_mandatorySubject(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._prerequisite) { await Reply_prerequisite(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._help) { await RootDialog.ShowWelcomeOptions(context); await CourseInfoOptionSelected(context); }
                else if (value.ToString() == RootDialog._storedvalues._gotostart) { await RootDialog.ShowWelcomeOptions(context); await CourseInfoOptionSelected(context); }
                else await LuisDialog.MessageReceivedAsync(context, result);
            }
        }



        private static IList<Attachment> GetCardsAttachments()
        {
            var openedMajorCourses = new HeroCard
            {
                Title = "이번학기 전공개설강의",
                Subtitle = "이번학기 전공개설강의",          //Location of information in MJU homepage
                Text = "이번학기 전공개설강의정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1iVNvUHc2-Qs_AXWGgnXpsPx3mp0BWCK7") }
            };

            var openedLiberalArts = new HeroCard
            {
                Title = "이번학기 교양개설강의",
                Subtitle = "이번학기 교양개설강의",          //Location of information in MJU homepage
                Text = "이번학기 교양개설강의정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Q7Ej1JB2OHcBP-TjXEdZYWz8H7ncUtpd") }
            };

            var syllabus = new HeroCard
            {
                Title = "강의계획서",
                Subtitle = "강의별 강의계획서",
                Text = "강의계획서 열람 정보입니다.",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Yn5FBeBVedQdodPsnM3I_1kWcKyL2abM") }
            };

            var lecturerInfo = new HeroCard
            {
                Title = "교수 정보",
                Subtitle = "교수 홈페이지 검색",
                Text = "교수 정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "http://home.mju.ac.kr/mainIndex/searchHomepage.action") }
            };

            var mandatorySubject = new HeroCard
            {
                Title = "정보통신공학과 선후수 과목정보",
                Subtitle = "정보통신공학과 선후수 과목정보",
                Text = "정보통신공학과 선후수 과목정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Fy7bAxihUXqlNLLToimYcKSiTHg_XdGe") }
            };

            var prerequisite = new HeroCard
            {
                Title = "정보통신공학과 선후수 과목정보",
                Subtitle = "정보통신공학과 선후수 과목정보",
                Text = "정보통신공학과 선후수 과목정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "http://www.mju.ac.kr/mbs/mjukr/images/editor/1406095802964_img_2017.jpg") }
            };

            return new List<Attachment>()
            {
                openedMajorCourses.ToAttachment(),
                openedLiberalArts.ToAttachment(),
                syllabus.ToAttachment(),
                mandatorySubject.ToAttachment(),
                prerequisite.ToAttachment()
            };

            }


        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_openedMajorCourses(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_openedLiberalArts;
            //activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "이번학기 전공개설강의",
                Subtitle = "이번학기 전공개설강의",          //Location of information in MJU homepage
                Text = "이번학기 전공개설강의정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1iVNvUHc2-Qs_AXWGgnXpsPx3mp0BWCK7") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_openedLiberalArts(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_openedLiberalArts;
            //activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "이번학기 교양개설강의",
                Subtitle = "이번학기 교양개설강의",          //Location of information in MJU homepage
                Text = "이번학기 교양개설강의정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Q7Ej1JB2OHcBP-TjXEdZYWz8H7ncUtpd") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_syllabus(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_Syllabus;
            //activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "강의계획서",
                Subtitle = "강의별 강의계획서",
                Text = "강의계획서 열람 정보입니다.",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Yn5FBeBVedQdodPsnM3I_1kWcKyL2abM") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_lecturerInfo(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_LecturerInfo;
            //activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "교수 정보",
                Subtitle = "교수 홈페이지 검색",
                Text = "교수 정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "http://home.mju.ac.kr/mainIndex/searchHomepage.action") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_mandatorySubject(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();

            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();
            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_MandatorySubject;
            //activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "정보통신공학과 선후수 과목정보",
                Subtitle = "정보통신공학과 선후수 과목정보",
                Text = "정보통신공학과 선후수 과목정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1Fy7bAxihUXqlNLLToimYcKSiTHg_XdGe") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_prerequisite(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_Prerequisite;
            //activity.AddKeyboardCard<string>("", RootDialog._storedvalues._courseInfoOptions);
            activity.Attachments.Add(new HeroCard
            {
                Title = "정보통신공학과 선후수 과목정보",
                Subtitle = "정보통신공학과 선후수 과목정보",
                Text = "정보통신공학과 선후수 과목정보",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "http://www.mju.ac.kr/mbs/mjukr/images/editor/1406095802964_img_2017.jpg") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }
    }
}