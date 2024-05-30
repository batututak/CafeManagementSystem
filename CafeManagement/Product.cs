using System;
using Npgsql;
using System.Collections.Generic;
using CafeManagement;

namespace MyProject.Controller
{
    public class Product
    {
        private DatabaseConnection dbConnection;

        public Product()
        {
            dbConnection = new DatabaseConnection();
        }

        public List<(int, string, bool, int, decimal)> ListProductsByCategory(int categoryId)
        {
            List<(int, string, bool, int, decimal)> products = new List<(int, string, bool, int, decimal)>();
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM product WHERE category_id = @CategoryId AND product_status = true", conn))
                    {
                        cmd.Parameters.AddWithValue("CategoryId", categoryId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add((reader.GetInt32(0), reader.GetString(2), reader.GetBoolean(3), reader.GetInt32(4), reader.GetDecimal(5)));
                            }
                        }
                    }
                }
            }
            return products;
        }
    }
}
