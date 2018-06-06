using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webscraping.Models
{
    public class FoodMenu
    {
        private string date;
        private List<string[]> details = new List<string[]>();


        public string Date { get { return date; } set { date = value; } }
        public List<string[]> Details { get { return details; } set { details = value; } }
    }
}
