using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using webscraping.Helper;
using webscraping.Models;

namespace webscraping
{
    class Program
    {
        public static CosmosDBService myDbService = new CosmosDBService();

        public static List<FoodMenu> weeklyFoodMenu = new List<FoodMenu>();
        static void Main(string[] args)
        {
            // get restaurant menu
            var menu1 = getRestaurantMenu();
            menu1.Wait();
            var menu2 = getRestaurant2Menu();
            menu2.Wait();
            var menu3 = getRestaurant3Menu();
            menu3.Wait();
            var menu4 = getRestaurant4Menu();
            menu4.Wait();


            var y = getLibraryInfo();
            y.Wait();

        }
        public static async Task getRestaurantMenu()
        {
            var url = "http://www.mju.ac.kr/mbs/mjukr/jsp/restaurant/restaurant.jsp?configIdx=36337&id=mjukr_051002050000";

            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);



            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var cafeteria_container = htmlDoc.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("summary", "")
                .Equals("명진당 식당")).ToList();

            var menu_table = cafeteria_container[0].Descendants("table").
                Where(node => node.GetAttributeValue("summary", "")
                .Contains("식단내용")).ToList();

            weeklyFoodMenu = new List<FoodMenu>();
            //foreach (var menu in menu_table) {
            for (int i = 0; i < menu_table.Count; i++)
            {
                var menu = menu_table[i];
                FoodMenu todaysMenu = new FoodMenu();

                var date = menu.Descendants("th").FirstOrDefault().InnerText;
                todaysMenu.Date = date.Trim();

                var menudetails = menu.Descendants("tr").ToList();
                foreach (var menudetail in menudetails)
                {
                    try
                    {
                        var menudetail_subOption = menudetail.Descendants("td").ToList()[0].InnerText;

                        var menudetail_subOptionValue = menudetail.Descendants("td").ToList()[1].Descendants("p").ToList();
                        string actual_concatinatedValue = "";
                        foreach (var stringvalue in menudetail_subOptionValue)
                            actual_concatinatedValue += stringvalue.InnerText.Trim() + "\n";
                        string[] actualvalueswithsubmenu = { menudetail_subOption.Trim(), actual_concatinatedValue.Trim() };
                        todaysMenu.Details.Add(actualvalueswithsubmenu);
                    }
                    catch (Exception ee) { }
                }

                weeklyFoodMenu.Add(todaysMenu);

            }

            FoodMenuListModel foodmenuforjsonobj = new FoodMenuListModel();
            foodmenuforjsonobj.foodMenus = weeklyFoodMenu;
            ConversationInfo conversationinfo = new ConversationInfo
            {
                myList = JsonConvert.SerializeObject(foodmenuforjsonobj),
                id = "FoodMenu",
                timestamp = DateTimeOffset.Now,
                watermark = ""
            };

            await myDbService.SetInfoAsync(conversationinfo, "FoodMenu");

            Console.WriteLine();
        }

        public static async Task getRestaurant2Menu()
        {
            var url = "http://www.mju.ac.kr/mbs/mjukr/jsp/restaurant/restaurant.jsp?configIdx=36548&id=mjukr_051002020000";

            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);



            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var cafeteria_container = htmlDoc.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("summary", "")
                .Equals("학생회관 식당 ")).ToList();

            //var menu_table = cafeteria_container[0].Descendants("table").
            //    Where(node => node.GetAttributeValue("summary", "")
            //    .Contains("식단내용")).ToList();

            weeklyFoodMenu = new List<FoodMenu>();
            //foreach (var menu in menu_table) {
            //for (int i = 0; i < menu_table.Count; i++)
            //{
            //var menu = menu_table[i];
            var menu = cafeteria_container[0];
            FoodMenu todaysMenu = new FoodMenu();

            var date = menu.Descendants("th").FirstOrDefault().InnerText;
            todaysMenu.Date = date.Trim();

            var menudetails = menu.Descendants("tr").ToList();
            foreach (var menudetail in menudetails)
            {
                try
                {
                    var menudetail_subOption = menudetail.Descendants("td").ToList()[0].InnerText;
                    var menudetail_headerOption = "http://www.mju.ac.kr/" +menudetail.Descendants("th").ToList()[0].Descendants("img").FirstOrDefault().Attributes["src"].Value;

                   string[] actualvalueswithsubmenu = { menudetail_subOption.Trim(), menudetail_headerOption.Trim() };
                    todaysMenu.Details.Add(actualvalueswithsubmenu);
                }
                catch (Exception ee) { }
            }

            weeklyFoodMenu.Add(todaysMenu);

            //}

            FoodMenuListModel foodmenuforjsonobj = new FoodMenuListModel();
            foodmenuforjsonobj.foodMenus = weeklyFoodMenu;
            ConversationInfo conversationinfo = new ConversationInfo
            {
                myList = JsonConvert.SerializeObject(foodmenuforjsonobj),
                id = "FoodMenu2",
                timestamp = DateTimeOffset.Now,
                watermark = ""
            };

            await myDbService.SetInfoAsync(conversationinfo, "FoodMenu");

            Console.WriteLine();
        }
        
        public static async Task getRestaurant3Menu()
        {
            var url = "http://www.mju.ac.kr/mbs/mjukr/jsp/restaurant/restaurant.jsp?configIdx=36550&id=mjukr_051002030000";

            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);



            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var cafeteria_container = htmlDoc.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("summary", "")
                .Equals("생활관_신관(자연)")).ToList();
            
            weeklyFoodMenu = new List<FoodMenu>();
            var menu = cafeteria_container[0];
            FoodMenu todaysMenu = new FoodMenu();

            var date = menu.Descendants("th").FirstOrDefault().InnerText;
            todaysMenu.Date = date.Trim();

            var menudetails = menu.Descendants("tr").ToList();
            foreach (var menudetail in menudetails)
            {
                try
                {
                    var menudetail_subOption = menudetail.Descendants("td").ToList()[0].InnerText;
                    var menudetail_headerOption = "http://www.mju.ac.kr/" + menudetail.Descendants("th").ToList()[0].Descendants("img").FirstOrDefault().Attributes["src"].Value;

                    string[] actualvalueswithsubmenu = { menudetail_subOption.Trim(), menudetail_headerOption.Trim() };
                    todaysMenu.Details.Add(actualvalueswithsubmenu);
                }
                catch (Exception ee) { }
            }

            weeklyFoodMenu.Add(todaysMenu);
            

            FoodMenuListModel foodmenuforjsonobj = new FoodMenuListModel();
            foodmenuforjsonobj.foodMenus = weeklyFoodMenu;
            ConversationInfo conversationinfo = new ConversationInfo
            {
                myList = JsonConvert.SerializeObject(foodmenuforjsonobj),
                id = "FoodMenu3",
                timestamp = DateTimeOffset.Now,
                watermark = ""
            };

            await myDbService.SetInfoAsync(conversationinfo, "FoodMenu");

            Console.WriteLine();
        }
        
        public static async Task<Rootobject> getLibraryInfo()
        {
            Rootobject Data = new Rootobject();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "http://seat.mju.ac.kr:8000/smufu-api/pc/2/rooms-at-seat?branchGroupId=2&buildingId=3"; ;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    ConversationInfo conversationinfo = new ConversationInfo
                    {
                        myList = JsonDataResponse,
                        id = "Library",
                        timestamp = DateTimeOffset.Now,
                        watermark = ""
                    };

                    await myDbService.SetInfoAsync(conversationinfo, "Library");

                    Data = JsonConvert.DeserializeObject<Rootobject>(JsonDataResponse);
                }
            }


            return Data;
        }


        public static async Task getRestaurant4Menu()
        {
            var url = "http://www.mju.ac.kr/mbs/mjukr/jsp/restaurant/restaurant.jsp?configIdx=58976&id=mjukr_051002040000";

            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);



            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var cafeteria_container = htmlDoc.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("summary", "")
                .Equals("자연캠퍼스 교직원식당")).ToList();

            weeklyFoodMenu = new List<FoodMenu>();
            var menu = cafeteria_container[0];
            FoodMenu todaysMenu = new FoodMenu();

            var date = menu.Descendants("th").FirstOrDefault().InnerText;
            todaysMenu.Date = date.Trim();

            var menudetails = menu.Descendants("tr").ToList();
            foreach (var menudetail in menudetails)
            {
                try
                {
                    var menudetail_subOption = menudetail.Descendants("td").ToList()[0].InnerText;
                    var menudetail_headerOption = "http://www.mju.ac.kr/" + menudetail.Descendants("th").ToList()[0].Descendants("img").FirstOrDefault().Attributes["src"].Value;

                    string[] actualvalueswithsubmenu = { menudetail_subOption.Trim(), menudetail_headerOption.Trim() };
                    todaysMenu.Details.Add(actualvalueswithsubmenu);
                }
                catch (Exception ee) { }
            }

            weeklyFoodMenu.Add(todaysMenu);


            FoodMenuListModel foodmenuforjsonobj = new FoodMenuListModel();
            foodmenuforjsonobj.foodMenus = weeklyFoodMenu;
            ConversationInfo conversationinfo = new ConversationInfo
            {
                myList = JsonConvert.SerializeObject(foodmenuforjsonobj),
                id = "FoodMenu4",
                timestamp = DateTimeOffset.Now,
                watermark = ""
            };

            await myDbService.SetInfoAsync(conversationinfo, "FoodMenu");

            Console.WriteLine();
        }

    }
}
