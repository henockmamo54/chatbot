using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAR_Bot.Helper.webscraping
{

        public class Rootobject
        {
            public bool success { get; set; }
            public string code { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public int totalCount { get; set; }
            public List[] list { get; set; }
        }

        public class List
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool isActive { get; set; }
            public string note { get; set; }
            public int total { get; set; }
            public int activeTotal { get; set; }
            public int occupied { get; set; }
            public int available { get; set; }
            public object disablePeriod { get; set; }
        }

}
