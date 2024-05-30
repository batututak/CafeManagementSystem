using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyProject.Controller;
using Npgsql;

namespace CafeManagement
{
    public partial class UrunEkleForm : Form
    {
        private int _tableId;
        private Category _categoryController;
        private Product _productController;
        private Addition _additionController;

        public UrunEkleForm(int tableId)
        {
            InitializeComponent();
            _tableId = tableId;
            _categoryController = new Category();
            _productController = new Product();
            _additionController = new Addition();
            this.BackColor = Color.FromArgb(34, 40, 49);

        }

        private void UrunEkleForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _categoryController.ListCategories();
            flowLayoutPanelCategories.Controls.Clear();

            foreach (var category in categories)
            {
                var button = new Button
                {
                    Text = category.Item2,
                    Tag = category.Item1,
                    AutoSize = true,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                   // ForeColor = Color.White,
                    BackColor = Color.LightSkyBlue,
                    Margin = new Padding(10)
                };
                button.Click += CategoryButton_Click;
                flowLayoutPanelCategories.Controls.Add(button);
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
            var products = _productController.ListProductsByCategory(categoryId);
            flowLayoutPanelProducts.Controls.Clear();

            foreach (var product in products)
            {
                var productPanel = new FlowLayoutPanel
                {
                    Width = 550,
                    Height = 50,
                    Margin = new Padding(10)
                };

                var productLabel = new Label
                {
                    Text = $"Ürün: {product.Item2}",
                    Width = 200,
                    Font = new Font("Arial", 14),
                    ForeColor = Color.White,
                   
                };

                var stockLabel = new Label
                {
                    Text = $"Stok: {product.Item4}",
                    Width = 100,
                    Font = new Font("Arial", 14),
                    ForeColor = Color.White,
                };

                var addButton = new Button
                {
                    Text = "Ekle",
                    Width = 100,
                    Height = 50,
                    Tag = new { productId = product.Item1, stockLabel = stockLabel },
                    ForeColor = Color.White,
                };
                addButton.Click += AddButton_Click;

                productPanel.Controls.Add(productLabel);
                productPanel.Controls.Add(stockLabel);
                productPanel.Controls.Add(addButton);

                flowLayoutPanelProducts.Controls.Add(productPanel);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            dynamic tag = button.Tag;

            int productId = tag.productId;
            Label stockLabel = tag.stockLabel;

            _additionController.AddProductToAddition(_tableId, productId, 1);
            UpdateProductStock(productId, -1);
            int currentStock = int.Parse(stockLabel.Text.Split(':').Last().Trim());
            stockLabel.Text = $"Stok: {currentStock - 1}";
        }

        private void UpdateProductStock(int productId, int change)
        {
            using (var conn = new DatabaseConnection().GetConnection())
            {
                if (conn != null)
                {
                    var query = "UPDATE product SET stock_amount = stock_amount + @Change WHERE product_id = @ProductId";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Change", change);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
