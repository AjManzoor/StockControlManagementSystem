using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCSM.Data;
using SCSM.Data.Respositories;

namespace SCSM.Business
{
    //controller (MVC pattern) links the model to the view as well as dealing with any other calls from the data layer
    public class DatabaseCalls
    {
        SCSM.Data.Respositories.StockRepository db = new SCSM.Data.Respositories.StockRepository();
        SCSM.Data.Respositories.PreviousMonthSalesRepository sdb = new SCSM.Data.Respositories.PreviousMonthSalesRepository();
        SCSM.Data.Respositories.CurrentStockOrdersRepository csdb = new SCSM.Data.Respositories.CurrentStockOrdersRepository();


        //Add a new item to the database
        public void AddToStock(AddStockItem stockItem)
        {
            var doublePrice = Convert.ToDouble(stockItem.itemPrice);
            db.Insert(stockItem.itemCode,stockItem.itemName, doublePrice, stockItem.itemQuantity,stockItem.stockArrivalDate,stockItem.minRequired,stockItem.maxRequired);
        }
        
        //Check if the item exists
        public bool CheckIfItemExists(string itemCode)
        {
            var count = db.CheckIfExists(itemCode);
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get all the data to map to the table
        public StockInventoryObject GetData()
        {
            return db.GetData();
        }

        //Delete a stock item
        public void DeleteStockItem(string itemCode)
        {
            db.Delete(itemCode);
        }

        //Check how much revuenue is generated
        public double GenerateRevenue(string itemCode)
        {
            return sdb.GenerateRevenue(itemCode);
        }


        //cancel all the current orders
        public void CancelOrders(string itemCode)
        {
            csdb.Delete(itemCode);
        }


        //calculate how much money we've spent this month so far
        public double CalculateMoneySpent()
        {
            return csdb.GetMoneySpent();
        }

        //Check if the item order vlaue meet the minimum and maximum orders
        public string CheckMinAndMaxRequired(OrderItem itemOrder)
        {
            var maxRequired = db.GetMaximumRequired(itemOrder.itemCode);
            var minRequired = db.GetMinimumRequired(itemOrder.itemCode);

            if (itemOrder.itemQuantity > maxRequired)
            {
                return "too many";
            }

            else if (itemOrder.itemQuantity < minRequired)
            {
                return "too little";
            }

            else
            {
                return "";
            }
        }


        //order more stocks
        public void OrderStocks(OrderItem itemOrder)
        {
            double price = db.GetItemPrice(itemOrder.itemCode);
            csdb.Insert(itemOrder.itemCode, itemOrder.itemQuantity, price, itemOrder.stockArrivalDate);
        }

        //calculate the cost of of ordering an item
        public double CalculateCost(OrderItem itemOrder)
        {
            double totalCost = itemOrder.itemQuantity *  db.GetItemPrice(itemOrder.itemCode);
            return totalCost;
        }
        
       
    }
}
