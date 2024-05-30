using System;
using System.Windows.Forms;
using Npgsql;

namespace CafeManagement
{
    public partial class ProductDetailForm : Form
    {
        private int productId;
        private DatabaseConnection dbConnection;

        public ProductDetailForm(int productId)
        {
            InitializeComponent();
            this.productId = productId;
            dbConnection = new DatabaseConnection();
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            using (var connection = dbConnection.GetConnection())
            {
                if (connection != null)
                {
                    var query = "SELECT product_title, stock_amount, product_price FROM product WHERE product_id = @ProductId";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxTitle.Text = reader["product_title"].ToString();
                                numericUpDownStock.Value = Convert.ToInt32(reader["stock_amount"]);
                                numericUpDownPrice.Value = Convert.ToDecimal(reader["product_price"]);
                            }
                        }
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var connection = dbConnection.GetConnection())
            {
                if (connection != null)
                {
                    var query = "UPDATE product SET product_title = @Title, stock_amount = @Stock, product_price = @Price WHERE product_id = @ProductId";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("Title", textBoxTitle.Text);
                        cmd.Parameters.AddWithValue("Stock", numericUpDownStock.Value);
                        cmd.Parameters.AddWithValue("Price", numericUpDownPrice.Value);
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            this.Close();
        }
    }
}
