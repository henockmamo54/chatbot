﻿using AAR_Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AAR_Bot.Helper.webscraping;
using System.Linq;
using Newtonsoft.Json;
using AAR_Bot.Helper.StoredStringValues;

namespace AAR_Bot.MessageReply
{
    public static class aboutOthers
    {
		static StoredStringValuesMaster _storedvalues;
        public static async Task OtherOptionSelected(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            if (context.Activity.ChannelId == "facebook")
            {
                var activity = context.MakeMessage();
                activity.Text = _storedvalues._otherOptionSelected.Replace("\n", "\n\n ");
                activity.SuggestedActions = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = _storedvalues._leaveOrReadmission, Type=ActionTypes.ImBack, Value=_storedvalues._leaveOrReadmission },
                        new CardAction(){ Title = _storedvalues._scholarship, Type=ActionTypes.ImBack, Value=_storedvalues._scholarship },
                        new CardAction(){ Title = _storedvalues._restaurantMenu, Type=ActionTypes.ImBack, Value= _storedvalues._restaurantMenu},
                        new CardAction(){ Title = _storedvalues._restaurantMenu2, Type=ActionTypes.ImBack, Value= _storedvalues._restaurantMenu2},
                        new CardAction(){ Title = _storedvalues._libraryInfo, Type=ActionTypes.ImBack, Value=_storedvalues._libraryInfo },
                        new CardAction(){ Title = _storedvalues._gotostart, Type=ActionTypes.ImBack, Value=_storedvalues._gotostart },
                        new CardAction(){ Title = _storedvalues._help, Type=ActionTypes.ImBack, Value=_storedvalues._help }
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
                _storedvalues._othersOption,
                _storedvalues._otherOptionSelected,                                                                                 //Course Registration
                _storedvalues._invalidSelectionMessage + "[ERROR] : OtherOptionSelected",          //Ooops, what you wrote is not a valid option, please try again
                1,
                PromptStyle.Auto);

            }
		}
			
        public static async Task HandleOtherOptionSelection(IDialogContext context, IAwaitable<string> result)
        {
            var value = await result;

            if (value.ToString() == _storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == _storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == _storedvalues._leaveOrReadmission) await Reply_leaveOrReadmission(context);
                else if (value.ToString() == _storedvalues._scholarship) await Reply_scholarship(context);
                else if (value.ToString() == _storedvalues._restaurantMenu) await Reply_restaurantMenu(context);
                else if (value.ToString() == _storedvalues._restaurantMenu2) await Reply_restaurantMenu2(context);
                else if (value.ToString() == _storedvalues._libraryInfo) await Reply_libraryInfo(context);


                //await RootDialog.ShowWelcomeOptions(context);           //Return To Start
                await OtherOptionSelected(context);
            }
        }

        //for facebook
        public static async Task HandleOtherOptionSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var myresult = await result;
            string value = myresult.Text;

            if (value.ToString() == _storedvalues._gotostart) await RootDialog.ShowWelcomeOptions(context);

            else if (value.ToString() == _storedvalues._help) await aboutHelp.HelpOptionSelected(context);

            else
            {
                if (value.ToString() == _storedvalues._leaveOrReadmission) { await Reply_leaveOrReadmission(context); await OtherOptionSelected(context); }
                else if (value.ToString() == _storedvalues._scholarship) { await Reply_scholarship(context);await OtherOptionSelected(context); }
                else if (value.ToString() == _storedvalues._restaurantMenu) { await Reply_restaurantMenu(context); await OtherOptionSelected(context); }
                else if (value.ToString() == _storedvalues._restaurantMenu2) { await Reply_restaurantMenu2(context); await OtherOptionSelected(context); }
                else if (value.ToString() == _storedvalues._libraryInfo) { await Reply_libraryInfo(context); await OtherOptionSelected(context); }
                else await LuisDialog.MessageReceivedAsync(context, result);
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
                convinfo.id = "FoodMenu1"; //명진당

                var foodmenuinfo = await cd.readDataFromDocument(convinfo);

                var jsondata = JsonConvert.DeserializeObject<FoodMenuListModel>(foodmenuinfo.FirstOrDefault().myList.ToString());
                var activity = context.MakeMessage();

                foreach (var day in jsondata.foodMenus)
                {
                    activity.Text += day.Date + "\n";
                    foreach (var restaurant in day.Details)
                    {
                        int inBraket = 0;

                        
                        restaurant[1] = restaurant[1].Replace("&amp;<br>", "&");
                        restaurant[1] = restaurant[1].Replace("<br>&amp;", "&");
                        restaurant[1] = restaurant[1].Replace("&amp;", "&");
                        restaurant[1] = restaurant[1].Replace("&nbsp;", "");
                        restaurant[1] = restaurant[1].Replace("<br>", "\n");
                        restaurant[1] = restaurant[1].Replace("\r", "\n");


                        if (restaurant[0] != "자율한식(석식)" && restaurant[0] != "참미소")
                        {
                            activity.Text += "\n▶" + restaurant[0] + "◀\n\n"; //▶▣◀

                            activity.Text += restaurant[1] + "\n";
                        }
                        


                        
                        

                        //foreach (var text in restaurant[0])
                        //{
                        //    if (text == '<' || inBraket == 1)
                        //    {
                        //        inBraket = 1;
                        //        if (text == '>')
                        //        {
                        //            inBraket = 0;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        activity.Text += text;
                        //    }
                        //}
                        //activity.Text += "\n";
                    }
                    activity.Text += "\n__________\n\n";
                }


                await context.PostAsync(activity);
            }
            catch (Exception ee)
            {
                await context.PostAsync("Error reading cosmos db");
            }
        }

        public static async Task Reply_restaurantMenu2(IDialogContext context)
        {
            try
            {
                CosmosDBService cd = new CosmosDBService();
                ConversationInfo convinfo = new ConversationInfo();
                convinfo.id = "FoodMenu22"; //학생식당

                var foodmenuinfo = await cd.readDataFromDocument(convinfo);

                var jsondata = JsonConvert.DeserializeObject<FoodMenuListModel>(foodmenuinfo.FirstOrDefault().myList.ToString());
                var activity = context.MakeMessage();

                foreach (var day in jsondata.foodMenus)
                {

                    foreach (var restaurant in day.Details)
                    {
                        int inBraket = 0;




                        //activity.Text += restaurant[0];
                        restaurant[0] = restaurant[0].Replace(" ", "");
                        restaurant[0] = restaurant[0].Replace("\n", "");
                        restaurant[0] = restaurant[0].Replace("\t", "");
                        restaurant[0] = restaurant[0].Replace("\r", "\n");
                        restaurant[0] = restaurant[0].Replace(")", ")\n");
                        restaurant[0] = restaurant[0].Replace("&amp;<br>", "&");
                        restaurant[0] = restaurant[0].Replace("<br>&amp;", "&");
                        restaurant[0] = restaurant[0].Replace("&amp;", "&");
                        restaurant[0] = restaurant[0].Replace("&nbsp;", "");
                        restaurant[0] = restaurant[0].Replace("<br>", "\n");
                        restaurant[0] = restaurant[0].Replace("백반", "\n▶백반◀\n\n"); //▶▣◀
                        restaurant[0] = restaurant[0].Replace("일품", "\n\n▶일품◀\n\n");
                        restaurant[0] = restaurant[0].Replace("양식</td>", "\n\n▶양식◀\n\n");
                        restaurant[0] = restaurant[0].Replace("\n(", "("); 

                        foreach (var text in restaurant[0])
                        {
                            if (text == '<' || inBraket == 1)
                            {
                                inBraket = 1;
                                if (text == '>')
                                {
                                    inBraket = 0;
                                }
                            }
                            else
                            {
                                activity.Text += text;
                            }
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
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            /*
            var activity = context.MakeMessage();
            activity.Text = RootDialog._storedvalues._reply_leaveOrReadmission;
            await context.PostAsync(activity);
            */

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_leaveOrReadmission;

            activity.Attachments.Add(new HeroCard
            {
                Title = "휴학 및 복학관련 정보입니다.",
                Subtitle = "휴학 및 복학관련 정보입니다.",          //Location of information in MJU homepage
                Text = "휴학 및 복학관련 정보입니다.",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") }, 
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=1YXE91epV_0_l8_lsgkXn1f9rYeF4_DfG") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

        public static async Task Reply_scholarship(IDialogContext context)
        {
            string lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._reply_Scholarship;

            activity.Attachments.Add(new HeroCard
            {
                Title = "장학금관련 정보입니다.",
                Subtitle = "장학금관련 정보입니다.",          //Location of information in MJU homepage
                Text = "장학금관련 정보입니다.",
                //Images = new List<CardImage> { new CardImage("http://www.kimaworld.net/data/file/char/3076632059_6ySVa5o9_EBAA85ECA7801.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,
                                                _storedvalues._goToButton,
                                                value: "https://drive.google.com/open?id=112Fs5ZE3ek3AzQBiCrKLXLuLxkWCPvo_") }
            }.ToAttachment());

            await context.PostAsync(activity);
        }

    }
}