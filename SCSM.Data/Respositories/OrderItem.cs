using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSM.Data.Respositories
{
    //object for for ordering a new item
    public class OrderItem
    {
        public string itemCode { get; set; }
        public int itemQuantity { get; set; }
        public DateTime stockArrivalDate { get; set; }
    }
}
