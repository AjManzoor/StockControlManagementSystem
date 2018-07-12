using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SCSM.Data.Respositories
{
    public class CurrentStockOrdersRepository : BaseRespository
    {
        //add a new value to current stock orders
        public void Insert(string itemCode, int itemQuantity, double price, DateTime stockArrivalDate)
        {
            //open connection
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("INSERT INTO current_orders (item_code, units_ordered, price, stock_arrival_date) VALUES(@item_code, @item_quantity, @price, @stock_arrival_date)", conn))
                {
                    //create command and assign the query and connection from the constructor
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    cmd.Parameters.AddWithValue("@item_quantity", itemQuantity);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock_arrival_date", stockArrivalDate);
                    
                    cmd.ExecuteNonQuery();
                }

            }
        }

        //delete a value from current stock orders
        public void Delete(string itemCode)
        {
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM current_orders where item_code = @item_code", conn))
                {
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public double GetMoneySpent()
        {
            int totalUnits = 0;
            double totalCost = 0;
            double totalSpent = 0;

            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM current_orders", conn))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        totalUnits += (int)rdr["units_ordered"];
                        totalCost += (double)rdr["price"];
                        totalSpent = totalCost * totalUnits;
                        return totalSpent;
                    }

                }
                return totalSpent;
            }
        }

    }
}
