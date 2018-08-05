using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using AAR_Bot.Helper;
using AAR_Bot.MessageReply;
using AAR_Bot.Helper.StoredStringValues;

namespace AAR_Bot.Dialogs
{
    [Serializable]
    public class LuisDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public static async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var value = await result;
            // first work check local data for the common words 
            FrequentlyUsedLuisSteatment fuls = new FrequentlyUsedLuisSteatment();
            Rootobject obj = fuls.getIntentOfString(value.Text);

            if (obj == null)
            {
                // second check  luis if the statement is not on the local files
                obj = await LUISService.GetEntityFromLUIS(value.Text);
            }

            //switch (value.Text.ToString())
            //{
            //    case "1": await aboutCourseRegistration.CourseRegistraionOptionSelected(context); break;
            //    case "2": await aboutCourseInfo.CourseInfoOptionSelected(context); break;
            //    case "3": await aboutCredits.CreditsOptionSelected(context); break;
            //    case "4": await aboutOthers.OtherOptionSelected(context); break;
            //    case "5": await aboutHelp.HelpOptionSelected(context); break;
            //    default: { await context.PostAsync("For now the chat bot works only for number inputs. \nWe are working on LUIS Integration. Thank you for your understanding.");
            //            context.Done(obj.topScoringIntent.ToString()); break; }
            //}

            switch (obj.topScoringIntent.intent)
            {
                case "Language":
                    {
                        switch (obj.entities[0].type)
                        {
                            case "english":
                                {
                                    RootDialog._storedvalues = new StoredValues_en();
                                    await RootDialog.ShowWelcomeOptions(context);
                                    break;
                                }
                            case "korean":
                                {
                                    RootDialog._storedvalues = new StoredValues_kr();
                                    await RootDialog.ShowWelcomeOptions(context);
                                    break;
                                }
                        }
                        break;
                    }
                case "Greetings":
                    {
                        await RootDialog.ShowWelcomeOptions(context);
                        break;
                    }
                case "Number":
                    {
                        switch (obj.entities[0].entity)
                        {
                            case "1": await aboutCourseRegistration.CourseRegistraionOptionSelected(context); break;
                            case "2": await aboutCourseInfo.CourseInfoOptionSelected(context); break;
                            case "3": await aboutCredits.CreditsOptionSelected(context); break;
                            case "4": await aboutOthers.OtherOptionSelected(context); break;
                            case "5": await aboutHelp.HelpOptionSelected(context); break;
                        }
                        break;
                    }
                case "Command":
                    {
                        switch (obj.entities[0].type)
                        {
                            case "restart": await RootDialog.ShowWelcomeOptions(context); break;
                            case "help": await aboutHelp.HelpOptionSelected(context); break;
                        }
                        break;
                    }
                case "CourseRegistration":
                    {
                        if (obj.entities.Count() > 0)
                        {
                            switch (obj.entities[0].type)
                            {
                                case "regulation": await aboutCourseRegistration.Reply_regulation(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "howtoregister": await aboutCourseRegistration.Reply_howToDoIt(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "schedule": await aboutCourseRegistration.Reply_schedule(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "terms": await aboutCourseRegistration.Reply_terms(context); await RootDialog.ShowWelcomeOptions(context); break;
                                default: { await aboutCourseRegistration.CourseRegistraionOptionSelected(context); break; }
                            }

                        }
                        else
                            await aboutCourseRegistration.CourseRegistraionOptionSelected(context);
                        break;
                    }
                case "CourseInformation":
                    {
                        if (obj.entities.Count() > 0)
                        {
                            switch (obj.entities[0].type)
                            {
                                case "openedmajor": await aboutCourseInfo.Reply_openedMajorCourses(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "openedliberalarts": await aboutCourseInfo.Reply_openedLiberalArts(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "syllabus": await aboutCourseInfo.Reply_syllabus(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "lectureinfo": await aboutCourseInfo.Reply_lecturerInfo(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "mandatorysubject": await aboutCourseInfo.Reply_mandatorySubject(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "prerequisite": await aboutCourseInfo.Reply_prerequisite(context); await RootDialog.ShowWelcomeOptions(context); break;
                                default: { await aboutCourseInfo.CourseInfoOptionSelected(context); break; }
                            }
                        }
                        else await aboutCourseInfo.CourseInfoOptionSelected(context);
                        break;
                    }
                case "CreditManagement":
                    {
                        await aboutCredits.CreditsOptionSelected(context);
                        break;
                    }
                case "OtherInformation":
                    {
                        if (obj.entities.Count() > 0)
                        {
                            switch (obj.entities[0].type)
                            {
                                case "foodmenu": await aboutOthers.Reply_restaurantMenu(context); await RootDialog.ShowWelcomeOptions(context); break;
                                case "libinfo": await aboutOthers.Reply_libraryInfo(context); await RootDialog.ShowWelcomeOptions(context); break;
                                default: { await aboutOthers.OtherOptionSelected(context); break; }
                            }
                        }
                        else await aboutOthers.OtherOptionSelected(context);
                        break;
                    }

                default:
                    {
                        await context.PostAsync("For now the chat bot works only for number inputs. \nWe are working on LUIS Integration. Thank you for your understanding.");
                        context.Done(obj.topScoringIntent.ToString()); break;
                    }
            }

            //context.Done(obj.topScoringIntent.ToString());
        }
    }
}