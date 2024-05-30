using Npgsql;
using System;

namespace CafeManagement
{
    public class DatabaseConnection
    {
        private string connectionString;

        public DatabaseConnection()
        {
            string host = "localhost";
            string port = "5432";
            string database = "cafemanagement"; // Veritabanı adı
            string username = "postgres"; // PostgreSQL kullanıcı adı
            string password = "root"; // PostgreSQL kullanıcı parolası

            connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
        }

        public NpgsqlConnection GetConnection()
        {
            try
            {
                var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                // Hata durumunu ele almak için uygun kodu buraya ekleyin
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
