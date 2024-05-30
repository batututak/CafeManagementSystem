using System;
using System.Collections.Generic;
using CafeManagement;
using Npgsql;

namespace MyProject.Controller
{
    public class Table
    {
        private DatabaseConnection dbConnection;

        public Table()
        {
            dbConnection = new DatabaseConnection();
        }

        public void AddTable(string tableTitle, int tableNumber, bool tableStatus)
        {
            if (string.IsNullOrEmpty(tableTitle)) throw new ArgumentException("Masa başlığı boş olamaz.");

            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO table_info (table_title, table_number, table_status) VALUES (@Title, @Number, @Status)";
                        cmd.Parameters.AddWithValue("Title", tableTitle);
                        cmd.Parameters.AddWithValue("Number", tableNumber);
                        cmd.Parameters.AddWithValue("Status", tableStatus);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpdateTable(int tableId, string tableTitle = null, int? tableNumber = null, bool? tableStatus = null)
        {
            var updateQuery = "UPDATE table_info SET ";
            var updateFields = new List<string>();

            if (tableTitle != null) updateFields.Add($"table_title = '{tableTitle}'");
            if (tableNumber != null) updateFields.Add($"table_number = {tableNumber}");
            if (tableStatus != null) updateFields.Add($"table_status = {tableStatus}");

            if (updateFields.Count == 0) throw new ArgumentException("Değiştirilecek bir alan bulunamadı.");

            updateQuery += string.Join(", ", updateFields);
            updateQuery += $" WHERE table_id = {tableId}";

            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand(updateQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteTable(int tableId)
        {
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM table_info WHERE table_id = @TableId";
                        cmd.Parameters.AddWithValue("TableId", tableId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<(int, string, int, bool, int?, bool, decimal?, decimal?, string, DateTime?, DateTime?, DateTime?)> ListTables()
        {
            List<(int, string, int, bool, int?, bool, decimal?, decimal?, string, DateTime?, DateTime?, DateTime?)> tables = new List<(int, string, int, bool, int?, bool, decimal?, decimal?, string, DateTime?, DateTime?, DateTime?)>();
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT t.table_id, t.table_title, t.table_number, t.table_status, a.addition_id, a.addition_status, a.total_price, a.payed_amount, a.payment_method, a.payed_at, a.created_at, a.updated_at FROM table_info t LEFT JOIN addition a ON t.table_id = a.table_id AND a.addition_status = true", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? additionId = reader.IsDBNull(4) ? null : reader.GetInt32(4);
                            bool hasActiveAddition = additionId != null && reader.GetBoolean(5);

                            decimal? total_price = reader.IsDBNull(6) ? null : reader.GetDecimal(6);
                            decimal? payed_amount = reader.IsDBNull(7) ? null : reader.GetDecimal(7);
                            string payment_method = reader.IsDBNull(8) ? null : reader.GetString(8);
                            DateTime? payed_at = reader.IsDBNull(9) ? null : reader.GetDateTime(9);
                            DateTime created_at = reader.GetDateTime(10);
                            DateTime updated_at = reader.GetDateTime(11);

                            tables.Add((reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetBoolean(3), additionId, hasActiveAddition, total_price, payed_amount, payment_method, payed_at, created_at, updated_at));
                        }
                    }
                }
            }
            return tables;
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
    }
}
