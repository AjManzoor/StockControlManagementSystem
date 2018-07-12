using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSM.Data
{
    //object which will contain all the values of all the items in our database. Will then be used to for the data table
    public class StockInventoryObject
    {
        public List<string> itemCode { get; set; }
        public List<string> itemName { get; set; }
        public List<double> price { get; set; }
        public List<int> itemQuantity { get; set; }
        public List<DateTime> stockArrivalDate { get; set; }
        public List<int> minRequired { get; set; }
        public List<int> maxRequired { get; set; }


    }
}
