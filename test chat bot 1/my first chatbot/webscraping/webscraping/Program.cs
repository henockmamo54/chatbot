using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using webscraping.Models;

namespace webscraping
{
    class Program
    {
        public static List<FoodMenu> weeklyFoodMenu = new List<FoodMenu>();
        static void Main(string[] args)
        {
            getRestaurantMenu();
            getLibraryInfo();
            Console.ReadLine();
        }

        public static async void getHtmlAsync() {
            var url = "https://www.ebay.com/sch/i.html?_nkw=x+box+one&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=&_sargn=-1%26saslc%3D1&_salic=1&_sop=12&_dmd=1&_ipg=50&_fosrp=1";

            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);
            

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var productsHtml = htmlDoc.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("ListViewInner")).ToList();

            var productlist = productsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                .Contains("item")).ToList();

            foreach (var productlistitem in productlist) {
                Console.WriteLine(productlistitem.GetAttributeValue("listingid", ""));
                Console.WriteLine(productlistitem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r','\n','\t'));
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static async void getRestaurantMenu()
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

            foreach (var menu in menu_table) {

                FoodMenu todaysMenu = new FoodMenu();

                var date = menu.Descendants("th").FirstOrDefault().InnerText;
                todaysMenu.Date = date.Trim();

                var menudetails = menu.Descendants("tr").ToList();
                foreach (var menudetail in menudetails) {
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

            //var productlist = productsHtml[0].Descendants("li")
            //    .Where(node => node.GetAttributeValue("id", "")
            //    .Contains("item")).ToList();

            //foreach (var productlistitem in productlist)
            //{
            //    Console.WriteLine(productlistitem.GetAttributeValue("listingid", ""));
            //    Console.WriteLine(productlistitem.Descendants("h3")
            //        .Where(node => node.GetAttributeValue("class", "")
            //        .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));
            //    Console.WriteLine();
            //}

            Console.WriteLine();
        }


        //public static async void getHtmlAsync3()
        //{
        //    var url = "http://lib.mju.ac.kr/service/Smuf.ax?br=02&userId=60131937#";

        //    var httpclient = new HttpClient();
        //    var html = await httpclient.GetStringAsync(url);


        //    var htmlDoc = new HtmlDocument();
        //    htmlDoc.LoadHtml(html);

        //    var productsHtml = htmlDoc.DocumentNode.Descendants("table")
        //        .Where(node => node.GetAttributeValue("class", "")
        //        .Equals("ikc-tablelist ikc-seat-status")).ToList();

        //    //var productlist = productsHtml[0].Descendants("li")
        //    //    .Where(node => node.GetAttributeValue("id", "")
        //    //    .Contains("item")).ToList();

        //    //foreach (var productlistitem in productlist)
        //    //{
        //    //    Console.WriteLine(productlistitem.GetAttributeValue("listingid", ""));
        //    //    Console.WriteLine(productlistitem.Descendants("h3")
        //    //        .Where(node => node.GetAttributeValue("class", "")
        //    //        .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));
        //    //    Console.WriteLine();
        //    //}

        //    Console.WriteLine();
        //}

        

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
                    Data = JsonConvert.DeserializeObject<Rootobject>(JsonDataResponse);
                }
            }
            return Data;
        }

    }
}
