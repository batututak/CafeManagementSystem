using System;
using Npgsql;
using System.Collections.Generic;
using CafeManagement;

namespace MyProject.Controller
{
    public class Category
    {
        private DatabaseConnection dbConnection;

        public Category()
        {
            dbConnection = new DatabaseConnection();
        }

        public void AddCategory(string categoryTitle, bool categoryStatus)
        {
            if (string.IsNullOrEmpty(categoryTitle)) throw new ArgumentException("Kategori başlığı boş olamaz.");

            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO category (category_title, category_status) VALUES (@Title, @Status)";
                        cmd.Parameters.AddWithValue("Title", categoryTitle);
                        cmd.Parameters.AddWithValue("Status", categoryStatus);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpdateCategory(int categoryId, string categoryTitle = null, bool? categoryStatus = null)
        {
            var updateQuery = "UPDATE category SET ";
            var updateFields = new List<string>();

            if (categoryTitle != null) updateFields.Add($"category_title = '{categoryTitle}'");
            if (categoryStatus != null) updateFields.Add($"category_status = {categoryStatus}");

            if (updateFields.Count == 0) throw new ArgumentException("Değiştirilecek bir alan bulunamadı.");

            updateQuery += string.Join(", ", updateFields);
            updateQuery += $" WHERE category_id = {categoryId}";

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

        public void DeleteCategory(int categoryId)
        {
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM category WHERE category_id = @CategoryId";
                        cmd.Parameters.AddWithValue("CategoryId", categoryId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<(int, string, bool)> ListCategories()
        {
            List<(int, string, bool)> categories = new List<(int, string, bool)>();
            using (var conn = dbConnection.GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM category WHERE category_status = true", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add((reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));
                        }
                    }
                }
            }
            return categories;
        }
    }
}
