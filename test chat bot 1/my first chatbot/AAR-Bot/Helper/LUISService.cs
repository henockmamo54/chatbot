using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AAR_Bot.Helper
{

    public class Rootobject
    {
        public string query { get; set; }
        public Topscoringintent topScoringIntent { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }
    }

    public class Topscoringintent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }

    public class LUISService
    {

        //public static async Task<Rootobject> parseUserInput(string strInput)
        //{
        //    string strRet = string.Empty;
        //    string strEscape = Uri.EscapeDataString(strInput);

        //    using (var client = new HttpClient())
        //    {

        //        HttpResponseMessage msg = await client.GetAsync(uri);

        //        if (msg.IsSuccessStatusCode)
        //        {
        //            var jsonResponse = await msg.Content.ReadAsStringAsync();
        //            var _data = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
        //            return _data;
        //        }
        //    }
        //}

        public static async Task<Rootobject> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            Rootobject Data = new Rootobject();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://eastasia.api.cognitive.microsoft.com/luis/v2.0/apps/c32ed705-90f4-420d-b670-c354c5062829?subscription-key=2ea8ca4be2b041b9b8b4390f4f3a87f5&timezoneOffset=-360&q=" + Query;
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