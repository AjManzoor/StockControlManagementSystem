using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace SCSM.Data.Respositories
{
    public class StockRepository : BaseRespository
    {

        //insert a new record into current stock inventory
        public void Insert(string itemCode, string itemName, double price, int itemQuantity, DateTime stockArrivalDate, int minRequired, int maxRequired)
        {
            //open connection
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("INSERT INTO stock_inventory (item_code, item_name, price, item_quantity, stock_arrival_date, min_required, max_required) VALUES(@item_code, @item_name, @price, @item_quantity, @stock_arrival_date, @min_required, @max_required)",conn))
                {
                    //create command and assign the query and connection from the constructor
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    cmd.Parameters.AddWithValue("@item_name", itemName);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@item_quantity", itemQuantity);
                    cmd.Parameters.AddWithValue("@stock_arrival_date", stockArrivalDate);
                    cmd.Parameters.AddWithValue("@min_required", minRequired);
                    cmd.Parameters.AddWithValue("@max_required", maxRequired);
                    cmd.ExecuteNonQuery();
                }
               
            }
        }

        //delete a value from current stock inventory
        public void Delete(string itemCode)
        {
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM stock_inventory where item_code = @item_code", conn))
                {
                    cmd.Parameters.AddWithValue("@item_code",itemCode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //checks if the item exists by counting how many copies. As we're passing primary key will either return 1 or 0
        public int CheckIfExists(string itemCode)
        {
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM stock_inventory where item_code = @item_code", conn))
                {
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count;
                }
            }
        }

        //Get all the values from the database and map them to an object containing lists. We will later iterate through these lists to show the data on the datagrid
        public StockInventoryObject GetData()
        {
            StockInventoryObject val = new StockInventoryObject();
            val.itemCode = new List<string>();
            val.itemName = new List<string>();
            val.price = new List<double>();
            val.itemQuantity = new List<int>();
            val.stockArrivalDate = new List<DateTime>();
            val.minRequired = new List<int>();
            val.maxRequired = new List<int>(); 
            

            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM stock_inventory",conn))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        val.itemCode.Add((string)rdr["item_code"]);
                        val.itemName.Add((string)rdr["item_name"]);
                        val.price.Add((double)rdr["price"]);
                        val.stockArrivalDate.Add((DateTime)rdr["stock_arrival_date"]);
                        val.itemQuantity.Add((int)rdr["item_quantity"]);
                        val.minRequired.Add((int)rdr["min_required"]);
                        val.maxRequired.Add((int)rdr["max_required"]);
                    }
                    return val;
                }
            }

        }

        //Get the value set for maximum required for an order
        public int GetMaximumRequired(string itemCode)
        {
            int val = 0;
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM stock_inventory where item_code = @item_code", conn))
                {
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        val = (int)rdr["max_required"];
                        return val;
                    }
                    return val;
                }

            }
        }

        //get the value for the minimum required for an order
        public int GetMinimumRequired(string itemCode)
        {
            int val = 0;
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM stock_inventory where item_code = @item_code", conn))
                {
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        val = (int)rdr["min_required"];
                        return val;
                    }
                    return val;
                }

            }
        }

        //Get the item price for value for an item
        public double GetItemPrice(string itemCode)
        {
            double val = 0;
            using (var conn = new MySqlConnection(GenerateConnectionString()))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM stock_inventory where item_code = @item_code", conn))
                {
                    cmd.Parameters.AddWithValue("@item_code", itemCode);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        val = (double)rdr["price"];
                        return val;
                    }
                    return val;
                }

            }

        }

    }
}

