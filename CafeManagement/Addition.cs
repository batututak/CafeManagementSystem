using System;
using System.Collections.Generic;
using CafeManagement;
using Npgsql;

namespace MyProject.Controller
{
    public class Addition
    {
        private DatabaseConnection dbConnection;

        public Addition()
        {
            dbConnection = new DatabaseConnection();
        }

        public void AddProductToAddition(int tableId, int productId, int totalAmount)
        {
            if (tableId <= 0 || productId <= 0) throw new ArgumentException("tableId ve productId pozitif bir değer olmalıdır.");
            if (totalAmount < 0) throw new ArgumentException("totalAmount pozitif bir değer olmalıdır.");

            int? additionId = GetActiveAdditionId(tableId);

            if (additionId == null) additionId = CreateNewAddition(tableId);

            if (additionId.HasValue)
            {
                if (IsProductInAddition(additionId.Value, productId))
                {
                    UpdateProductInDetail(additionId.Value, productId, totalAmount);
                }
                else
                {
                    AddProductToDetail(additionId.Value, productId, totalAmount);
                }
            }
            else
            {
                throw new InvalidOperationException("Addition ID bulunamadı veya oluşturulamadı.");
            }
        }

        private int? GetActiveAdditionId(int tableId)
        {
            int? additionId = null;
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT addition_id FROM addition WHERE table_id = @TableId AND addition_status = true", conn))
                    {
                        cmd.Parameters.AddWithValue("TableId", tableId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                additionId = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            return additionId;
        }

        private int CreateNewAddition(int tableId)
        {
            int additionId = 0;  // Başlangıç değeri atandı
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO addition (table_id, addition_status) VALUES (@TableId, true) RETURNING addition_id", conn))
                    {
                        cmd.Parameters.AddWithValue("TableId", tableId);
                        additionId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            return additionId;
        }

        private bool IsProductInAddition(int additionId, int productId)
        {
            bool exists = false;
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM addition_detail WHERE addition_id = @AdditionId AND product_id = @ProductId", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            return exists;
        }

        private void AddProductToDetail(int additionId, int productId, int totalAmount)
        {
            decimal unitPrice = GetProductUnitPrice(productId);
            decimal totalPrice = unitPrice * totalAmount;

            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO addition_detail (addition_id, product_id, unit_price, total_price, total_amount) VALUES (@AdditionId, @ProductId, @UnitPrice, @TotalPrice, @TotalAmount)", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        cmd.Parameters.AddWithValue("UnitPrice", unitPrice);
                        cmd.Parameters.AddWithValue("TotalPrice", totalPrice);
                        cmd.Parameters.AddWithValue("TotalAmount", totalAmount);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            UpdateAdditionTotalPrice(additionId);
        }

        public void UpdateProductInDetail(int additionId, int productId, int totalAmount)
        {
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("UPDATE addition_detail SET total_amount = total_amount + @TotalAmount WHERE addition_id = @AdditionId AND product_id = @ProductId RETURNING total_amount", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        cmd.Parameters.AddWithValue("TotalAmount", totalAmount);

                        var result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            throw new Exception("Product not found or update failed.");
                        }

                        int newTotalAmount = Convert.ToInt32(result);

                        if (newTotalAmount <= 0)
                        {
                            RemoveProductFromAddition(additionId, productId);
                        }
                        else
                        {
                            decimal unitPrice = GetProductUnitPrice(productId);
                            decimal totalPrice = unitPrice * newTotalAmount;

                            using (var updateCmd = new NpgsqlCommand("UPDATE addition_detail SET total_price = @TotalPrice WHERE addition_id = @AdditionId AND product_id = @ProductId", conn))
                            {
                                updateCmd.Parameters.AddWithValue("AdditionId", additionId);
                                updateCmd.Parameters.AddWithValue("ProductId", productId);
                                updateCmd.Parameters.AddWithValue("TotalPrice", totalPrice);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Database connection is null.");
                }
            }
            UpdateAdditionTotalPrice(additionId);
        }

        public void RemoveProductFromAddition(int additionId, int productId)
        {
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM addition_detail WHERE addition_id = @AdditionId AND product_id = @ProductId", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            UpdateAdditionTotalPrice(additionId);
        }

        public void ClearAddition(int additionId)
        {
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM addition_detail WHERE addition_id = @AdditionId", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand("UPDATE addition SET addition_status = false WHERE addition_id = @AdditionId", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private decimal GetProductUnitPrice(int productId)
        {
            decimal unitPrice = 0;
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT product_price FROM product WHERE product_id = @ProductId", conn))
                    {
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        var result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            throw new Exception("Ürün bulunamadı.");
                        }
                        unitPrice = (decimal)result;
                    }
                }
            }
            return unitPrice;
        }

        private void UpdateAdditionTotalPrice(int additionId)
        {
            decimal totalPrice = 0;
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT SUM(total_price) FROM addition_detail WHERE addition_id = @AdditionId", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalPrice = (decimal)result;
                        }
                    }
                }
            }

            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("UPDATE addition SET total_price = @TotalPrice WHERE addition_id = @AdditionId", conn))
                    {
                        cmd.Parameters.AddWithValue("AdditionId", additionId);
                        cmd.Parameters.AddWithValue("TotalPrice", totalPrice);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public (int?, bool, decimal?, decimal?, string, DateTime?, List<(int, decimal, decimal, int, DateTime, DateTime)>) GetTableDetail(int tableId)
        {
            int? additionId = null;
            bool additionStatus = false;
            decimal? total_price = null;
            decimal? payed_amount = null;
            string payment_method = null;
            DateTime? payed_at = null;
            List<(int, decimal, decimal, int, DateTime, DateTime)> additionDetails = new List<(int, decimal, decimal, int, DateTime, DateTime)>();

            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT addition_id, addition_status, total_price, payed_amount, payment_method, payed_at FROM addition WHERE table_id = @TableId AND addition_status = true", conn))
                    {
                        cmd.Parameters.AddWithValue("TableId", tableId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                additionId = reader.GetInt32(0);
                                additionStatus = reader.GetBoolean(1);
                                total_price = reader.IsDBNull(2) ? null : reader.GetDecimal(2);
                                payed_amount = reader.IsDBNull(3) ? null : reader.GetDecimal(3);
                                payment_method = reader.IsDBNull(4) ? null : reader.GetString(4);
                                payed_at = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                            }
                        }
                    }

                    if (additionId != null)
                    {
                        using (var cmd = new NpgsqlCommand("SELECT product_id, unit_price, total_price, total_amount, created_at, updated_at FROM addition_detail WHERE addition_id = @AdditionId", conn))
                        {
                            cmd.Parameters.AddWithValue("AdditionId", additionId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int productId = reader.GetInt32(0);
                                    decimal unitPrice = reader.GetDecimal(1);
                                    decimal totalPrice = reader.GetDecimal(2);
                                    int totalAmount = reader.GetInt32(3);
                                    DateTime createdAt = reader.GetDateTime(4);
                                    DateTime updatedAt = reader.GetDateTime(5);
                                    additionDetails.Add((productId, unitPrice, totalPrice, totalAmount, createdAt, updatedAt));
                                }
                            }
                        }
                    }
                }
            }

            return (additionId, additionStatus, total_price, payed_amount, payment_method, payed_at, additionDetails);
        }

        public bool HasActiveAddition(int tableId)
        {
            bool hasActiveAddition = false;

            using (var connection = dbConnection.GetConnection())
            {
                if (connection != null)
                {
                    var query = @"SELECT 1 
                          FROM addition a
                          JOIN addition_detail ad ON a.addition_id = ad.addition_id
                          WHERE a.table_id = @TableId AND a.addition_status = true";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("TableId", tableId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            hasActiveAddition = reader.Read();
                        }
                    }
                }
            }

            return hasActiveAddition;
        }

    }
}
