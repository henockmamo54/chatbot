﻿using AAR_Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AAR_Bot.Helper.webscraping;
using System.Linq;
using Newtonsoft.Json;

namespace AAR_Bot.MessageReply
{
    public static class aboutOthers
    {

        public static async Task OtherOptionSelected(IDialogContext context)
        {
            if (context.Activity.ChannelId == "facebook")
            {
                var activity = context.MakeMessage();
                activity.Text = RootDialog._storedvalues._otherOptionSelected.Replace("\n", "\n\n ");
                activity.SuggestedActions = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = RootDialog._storedvalues._leaveOrReadmission, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._leaveOrReadmission },
                        new CardAction(){ Title = RootDialog._storedvalues._scholarship, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._scholarship },
                        new CardAction(){ Title = RootDialog._storedvalues._restaurantMenu, Type=ActionTypes.ImBack, Value= RootDialog._storedvalues._restaurantMenu},
                        new CardAction(){ Title = RootDialog._storedvalues._libraryInfo, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._libraryInfo },
                        new CardAction(){ Title = RootDialog._storedvalues._gotostart, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._gotostart },
                        new CardAction(){ Title = RootDialog._storedvalues._help, Type=ActionTypes.ImBack, Value=RootDialog._storedvalues._help }
                    }
                };

                await context.PostAsync(activity);
                context.Wait(HandleOtherOptionSelection);
            }
            else
            {

                PromptDialog.Choice<string>(
                context,
                HandleOtherOptionSelection,
                RootDialog._storedvalues._othersOption,
                RootDialog._storedvalues._otherOptionSelected,                                                                                 //Course Registration
                RootDialog._storedvalues._invalidSelectionMessage + "[ERROR] : OtherOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);

            }

        }
        public static async Task HandleOtherOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._leaveOrReadmission) await Reply_leaveOrReadmission(context);
                else if (value.ToString() == RootDialog._storedvalues._scholarship) await Reply_scholarship(context);
                else if (value.ToString() == RootDialog._storedvalues._restaurantMenu) await Reply_restaurantMenu(context);
                else if (value.ToString() == RootDialog._storedvalues._libraryInfo) await Reply_libraryInfo(context);


                //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                await OtherOptionSelected(context);
            }
        }

        //for facebook
        public static async Task HandleOtherOptionSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var myresult = await result;
            string value = myresult.Text;

            if (value.ToString() == RootDialog._storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == RootDialog._storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == RootDialog._storedvalues._leaveOrReadmission) await Reply_leaveOrReadmission(context);
                else if (value.ToString() == RootDialog._storedvalues._scholarship) await Reply_scholarship(context);
                else if (value.ToString() == RootDialog._storedvalues._restaurantMenu) await Reply_restaurantMenu(context);
                else if (value.ToString() == RootDialog._storedvalues._libraryInfo) await Reply_libraryInfo(context);


                //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                await OtherOptionSelected(context);
            }
        }

        public static async Task Reply_libraryInfo(IDialogContext context)
        {
            try
            {
                CosmosDBService cd = new CosmosDBService();
                ConversationInfo convinfo = new ConversationInfo();
                convinfo.id = "Library"; //FoodMenu

                var libinfo = await cd.readDataFromDocument(convinfo);
                var jsondata = JsonConvert.DeserializeObject<Rootobject>(libinfo.FirstOrDefault().myList.ToString());
                var activity = context.MakeMessage();

                foreach (var a in jsondata.data.list)
                {
                    activity.Text += a.name + " : " + a.available + " / " + a.total + "\n";
                }

                await context.PostAsync(activity);
            }
            catch (Exception ee)
            {
                await context.PostAsync("Error reading cosmos db");
            }

        }

        public static async Task Reply_restaurantMenu(IDialogContext context)
        {
            try
            {
                CosmosDBService cd = new CosmosDBService();
                ConversationInfo convinfo = new ConversationInfo();
                convinfo.id = "FoodMenu"; //FoodMenu

                var foodmenuinfo = await cd.readDataFromDocument(convinfo);

                var jsondata = JsonConvert.DeserializeObject<FoodMenuListModel>(foodmenuinfo.FirstOrDefault().myList.ToString());
                var activity = context.MakeMessage();

                foreach (var day in jsondata.foodMenus)
                {
                    activity.Text += day.Date + "\n";
                    foreach (var restaurant in day.Details)
                    {
                        activity.Text += "\n" + restaurant[0] + "\n";
                        for (int i = 1; i < restaurant.Length; i++)
                        {
                            var food = restaurant[i];
                            activity.Text += "\n" + food;
                        }
                        activity.Text += "\n";
                    }
                    activity.Text += "\n";
                }


                await context.PostAsync(activity);
            }
            catch (Exception ee)
            {
                await context.PostAsync("Error reading cosmos db");
            }
        }


        //================================================================================================================================================
        //Last Phase Option

        public static async Task Reply_leaveOrReadmission(IDialogContext context)
        {
            /*
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_leaveOrReadmission;
            await context.PostAsync(activity);
            */

            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_leaveOrReadmission;

            activity.Attachments.Add(new HeroCard
            {
                Title = "휴학 및 복학관련 정보입니다.",
                Subtitle = "휴학 및 복학관련 정보입니다.",          //Location of information in MJU homepage
                Text = "휴학 및 복학관련 정보입니다.",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") }, 
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1YXE91epV_0_l8_lsgkXn1f9rYeF4_DfG") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_scholarship(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_Scholarship;

            activity.Attachments.Add(new HeroCard
            {
                Title = "장학금관련 정보입니다.",
                Subtitle = "장학금관련 정보입니다.",          //Location of information in MJU homepage
                Text = "장학금관련 정보입니다.",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                RootDialog._storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=112Fs5ZE3ek3AzQBiCrKLXLuLxkWCPvo_") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

    }
}