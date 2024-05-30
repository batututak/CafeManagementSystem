using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace CafeManagement
{
    public partial class StokKontrolForm : Form
    {
        private DatabaseConnection dbConnection;

        public StokKontrolForm()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadCategories();
            this.BackColor = Color.FromArgb(34, 40, 49); // Koyu arka plan rengi
        }

        private void LoadCategories()
        {
            using (var connection = dbConnection.GetConnection())
            {
                if (connection != null)
                {
                    var query = "SELECT * FROM category WHERE category_status = true";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var button = new Button
                                {
                                    Text = reader["category_title"].ToString(),
                                    Tag = reader["category_id"],
                                    AutoSize = true,
                                    Height = 60,
                                    Width = 180,
                                    Font = new Font("Arial", 14, FontStyle.Bold),
                                    BackColor = Color.LightSkyBlue,
                                    Margin = new Padding(5)
                                };
                                button.Click += CategoryButton_Click;
                                flowLayoutPanelCategories.Controls.Add(button);
                            }
                        }
                    }
                }
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int categoryId = (int)button.Tag;
                LoadProducts(categoryId);
            }
        }

        private void LoadProducts(int categoryId)
        {
            flowLayoutPanelProducts.Controls.Clear();
            using (var connection = dbConnection.GetConnection())
            {
                if (connection != null)
                {
                    var query = "SELECT * FROM product WHERE category_id = @CategoryId AND product_status = true";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("CategoryId", categoryId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FlowLayoutPanel panel = new FlowLayoutPanel
                                {
                                    AutoSize = true,
                                    FlowDirection = FlowDirection.LeftToRight,
                                    Margin = new Padding(10),
                                    Width = flowLayoutPanelProducts.Width - 30 // Scroll bar için yer bırak
                                };

                                var decreaseButton = new Button
                                {
                                    Text = "-",
                                    Tag = reader["product_id"],
                                    Height = 60,
                                    Width = 60,
                                    Font = new Font("Arial", 14, FontStyle.Bold),
                                    ForeColor = Color.White,
                                    Margin = new Padding(5)
                                };
                                decreaseButton.Click += DecreaseButton_Click;

                                var productButton = new Button
                                {
                                    Text = $"{reader["product_title"]} - {reader["product_price"]} TL",
                                    Tag = reader["product_id"],
                                    Height = 80,
                                    Width = 300,
                                    Font = new Font("Arial", 12, FontStyle.Regular),
                                    BackColor = Color.LightGreen,
                                    Margin = new Padding(5)
                                };
                                productButton.Click += ProductButton_Click;

                                var stockLabel = new Label
                                {
                                    Text = $"Stok: {reader["stock_amount"]}",
                                    AutoSize = true,
                                    Font = new Font("Arial", 14, FontStyle.Bold),
                                    ForeColor = Color.White,
                                    Margin = new Padding(5, 20, 5, 5)
                                };
                                stockLabel.Tag = reader["product_id"];

                                var increaseButton = new Button
                                {
                                    Text = "+",
                                    Tag = reader["product_id"],
                                    Height = 60,
                                    Width = 60,
                                    Font = new Font("Arial", 14, FontStyle.Bold),
                                    ForeColor = Color.White,
                                    Margin = new Padding(5)
                                };
                                increaseButton.Click += IncreaseButton_Click;

                                panel.Controls.Add(decreaseButton);
                                panel.Controls.Add(productButton);
                                panel.Controls.Add(stockLabel);
                                panel.Controls.Add(increaseButton);

                                flowLayoutPanelProducts.Controls.Add(panel);
                            }
                        }
                    }
                }
            }
        }

        private void DecreaseButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int productId = (int)button.Tag;
                UpdateProductStock(productId, -1);
            }
        }

        private void IncreaseButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int productId = (int)button.Tag;
                UpdateProductStock(productId, 1);
            }
        }

        private void UpdateProductStock(int productId, int change)
        {
            using (var connection = dbConnection.GetConnection())
            {
                if (connection != null)
                {
                    var query = "UPDATE product SET stock_amount = stock_amount + @Change WHERE product_id = @ProductId";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("Change", change);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            var stockLabel = flowLayoutPanelProducts.Controls
                .OfType<FlowLayoutPanel>()
                .SelectMany(p => p.Controls.OfType<Label>())
                .FirstOrDefault(l => (int)l.Tag == productId);
            if (stockLabel != null)
            {
                int currentStock = int.Parse(stockLabel.Text.Replace("Stok: ", "")) + change;
                stockLabel.Text = $"Stok: {currentStock}";
            }

            AddSaveButton();
        }

        private void AddSaveButton()
        {
            var saveButton = flowLayoutPanelProducts.Controls
                .OfType<Button>()
                .FirstOrDefault(b => b.Text == "Kaydet");

            if (saveButton == null)
            {
                saveButton = new Button
                {
                    Text = "Kaydet",
                    AutoSize = true,
                    Height = 50,
                    Width = 100,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    Margin = new Padding(5)
                };
                saveButton.Click += SaveButton_Click;
                flowLayoutPanelProducts.Controls.Add(saveButton);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var selectedCategoryButton = flowLayoutPanelCategories.Controls.OfType<Button>().FirstOrDefault(b => b.Focused);
            if (selectedCategoryButton != null)
            {
                int categoryId = (int)selectedCategoryButton.Tag;
                LoadProducts(categoryId);
            }
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int productId = (int)button.Tag;
                var productDetailForm = new ProductDetailForm(productId);
                productDetailForm.ShowDialog();
                var selectedCategoryButton = flowLayoutPanelCategories.Controls.OfType<Button>().FirstOrDefault(b => b.Focused);
                if (selectedCategoryButton != null)
                {
                    int categoryId = (int)selectedCategoryButton.Tag;
                    LoadProducts(categoryId);
                }
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
