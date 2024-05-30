using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace CafeManagement
{
    public partial class MasalarForm : Form
    {
        private DatabaseConnection dbConnection;

        public MasalarForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            this.BackColor = Color.FromArgb(34, 40, 49); // Koyu arka plan rengi
        }

        private void MasalarForm_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void LoadTables()
        {
            flowLayoutPanelTables.Controls.Clear();

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    if (connection != null)
                    {
                        var query = "SELECT * FROM table_info";
                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var button = new Button
                                    {
                                        Text = reader["table_title"].ToString(),
                                        Tag = reader["table_id"],
                                        AutoSize = true,
                                        Height = 100,
                                        Width = 100,
                                        Font = new Font("Arial", 14, FontStyle.Bold),
                                        BackColor = Color.LightGray,
                                        Margin = new Padding(10)
                                    };
                                    button.Click += TableButton_Click;

                                    int tableId = reader.GetInt32(reader.GetOrdinal("table_id"));
                                    if (HasActiveAddition(tableId))
                                    {
                                        button.BackColor = Color.FromArgb(255, 99, 71); // Modern bir kırmızı ton
                                    }

                                    flowLayoutPanelTables.Controls.Add(button);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading tables: {ex.Message}");
            }
        }

        private bool HasActiveAddition(int tableId)
        {
            bool hasActiveAddition = false;

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while checking active additions: {ex.Message}");
            }

            return hasActiveAddition;
        }

        private void TableButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int tableId = (int)button.Tag;
                var masaDetayForm = new MasaDetayForm(button.Text, tableId);
                masaDetayForm.FormClosed += (s, args) => LoadTables(); // Form kapandığında masaları yeniden yükle
                masaDetayForm.Show();
            }
        }
    }
}
