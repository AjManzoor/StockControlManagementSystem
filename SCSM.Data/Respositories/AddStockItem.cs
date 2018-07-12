using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSM.Data.Respositories
{
    //object created for passing items when adding a new stock item
    public class AddStockItem
    {
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string itemPrice { get; set; }
        public int itemQuantity { get; set; }
        public DateTime stockArrivalDate { get; set; }
        public int minRequired { get; set; }
        public int maxRequired { get; set; }
    }
}
