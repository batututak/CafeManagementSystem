using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyProject.Controller;
using Npgsql;

namespace CafeManagement
{
    public partial class MasaDetayForm : Form
    {
        private int tableId; // Masa ID'sini saklamak için private bir değişken
        private Addition additionController;

        public MasaDetayForm(string masaAdi, int tableId)
        {
            InitializeComponent();
            this.Text = masaAdi;
            this.tableId = tableId; // tableId'yi constructor'da alıp saklıyoruz
            this.additionController = new Addition();
            this.BackColor = Color.FromArgb(34, 40, 49);

            LoadProducts();
        }

        private void LoadProducts()
        {
            flowLayoutPanelProducts.Controls.Clear();
            var result = additionController.GetTableDetail(tableId);
            int? additionId = result.Item1;
            bool additionStatus = result.Item2;
            decimal? total_price = result.Item3;
            decimal? payed_amount = result.Item4;
            string payment_method = result.Item5;
            DateTime? payed_at = result.Item6;
            List<(int, decimal, decimal, int, DateTime, DateTime)> additionDetails = result.Item7;

            foreach (var detail in additionDetails)
            {
                var productPanel = new FlowLayoutPanel
                {
                    Width = 600,
                    Height = 50,
                    Margin = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var productLabel = new Label
                {
                    Text = $"Ürün: {GetProductName(detail.Item1)}",
                    Width = 200,
                    Font = new Font("Arial", 12),
                    ForeColor = Color.White
                };

                var quantityLabel = new Label
                {
                    Text = detail.Item4.ToString(),
                    Width = 50,
                    Font = new Font("Arial", 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.White
                };

                var priceLabel = new Label
                {
                    Text = $"{(detail.Item4 * detail.Item2):C}", // Birim fiyatı ile çarpılan toplam fiyatı gösteriyor
                    Width = 100,
                    Font = new Font("Arial", 12),
                    ForeColor = Color.White
                };

                var minusButton = new Button
                {
                    Text = "-",
                    Width = 30,
                    Tag = new { ProductId = detail.Item1, QuantityLabel = quantityLabel, PriceLabel = priceLabel, UnitPrice = detail.Item2, ProductPanel = productPanel },
                    ForeColor = Color.White
                };
                minusButton.Click += MinusButton_Click;

                var plusButton = new Button
                {
                    Text = "+",
                    Width = 30,
                    Tag = new { ProductId = detail.Item1, QuantityLabel = quantityLabel, PriceLabel = priceLabel, UnitPrice = detail.Item2 },
                    ForeColor = Color.White
                };
                plusButton.Click += PlusButton_Click;

                productPanel.Controls.Add(productLabel);
                productPanel.Controls.Add(minusButton);
                productPanel.Controls.Add(quantityLabel);
                productPanel.Controls.Add(plusButton);
                productPanel.Controls.Add(priceLabel);

                flowLayoutPanelProducts.Controls.Add(productPanel);
            }

            UpdateTotalPrice();
        }

        private string GetProductName(int productId)
        {
            string productName = string.Empty;
            using (var conn = new DatabaseConnection().GetConnection())
            {
                if (conn != null)
                {
                    using (var cmd = new NpgsqlCommand("SELECT product_title FROM product WHERE product_id = @ProductId", conn))
                    {
                        cmd.Parameters.AddWithValue("ProductId", productId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productName = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            return productName;
        }

        private void MinusButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            dynamic tag = button.Tag;

            int productId = tag.ProductId;
            Label quantityLabel = tag.QuantityLabel;
            Label priceLabel = tag.PriceLabel;
            decimal unitPrice = tag.UnitPrice;
            FlowLayoutPanel productPanel = tag.ProductPanel;

            int quantity = int.Parse(quantityLabel.Text);
            if (quantity > 1)
            {
                quantity--;
                quantityLabel.Text = quantity.ToString();
                priceLabel.Text = $"{(quantity * unitPrice):C}";

                var result = additionController.GetTableDetail(tableId);
                int? additionId = result.Item1;
                if (additionId.HasValue)
                {
                    additionController.UpdateProductInDetail(additionId.Value, productId, -1);
                }
                UpdateProductStock(productId, 1); // Stok miktarını güncelle
            }
            else if (quantity == 1)
            {
                flowLayoutPanelProducts.Controls.Remove(productPanel);

                var result = additionController.GetTableDetail(tableId);
                int? additionId = result.Item1;
                if (additionId.HasValue)
                {
                    additionController.RemoveProductFromAddition(additionId.Value, productId);
                }
                UpdateProductStock(productId, 1); // Stok miktarını güncelle
            }

            UpdateTotalPrice();
            LoadProducts(); // Ürün silindiğinde listeyi yeniden yükle
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            dynamic tag = button.Tag;

            int productId = tag.ProductId;
            Label quantityLabel = tag.QuantityLabel;
            Label priceLabel = tag.PriceLabel;
            decimal unitPrice = tag.UnitPrice;

            int quantity = int.Parse(quantityLabel.Text);
            quantity++;
            quantityLabel.Text = quantity.ToString();
            priceLabel.Text = $"{(quantity * unitPrice):C}";

            var result = additionController.GetTableDetail(tableId);
            int? additionId = result.Item1;
            if (additionId.HasValue)
            {
                additionController.UpdateProductInDetail(additionId.Value, productId, 1);
            }
            UpdateProductStock(productId, -1); // Stok miktarını güncelle

            UpdateTotalPrice();
            LoadProducts(); // Ürün eklendiğinde listeyi yeniden yükle
        }

        private void UpdateTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (FlowLayoutPanel panel in flowLayoutPanelProducts.Controls)
            {
                var priceLabel = panel.Controls.OfType<Label>().FirstOrDefault(label => label.Text.Contains("₺"));
                if (priceLabel != null)
                {
                    if (decimal.TryParse(priceLabel.Text, System.Globalization.NumberStyles.Currency, null, out decimal price))
                    {
                        totalPrice += price;
                    }
                }
            }
            lblTotalPrice.Text = totalPrice.ToString("C");
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

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            UrunEkleForm urunEkleForm = new UrunEkleForm(tableId);
            urunEkleForm.FormClosed += (s, args) => LoadProducts(); // Ürün eklendiğinde listeyi güncelle
            urunEkleForm.Show();
        }

        private void btnHesap_Click(object sender, EventArgs e)
        {
            var result = additionController.GetTableDetail(tableId);
            int? additionId = result.Item1;
            if (additionId.HasValue && result.Item7.Count > 0)
            {
                var paymentPopup = new Form
                {
                    Width = 300,
                    Height = 200,
                    Text = "Ödeme Yöntemi",
                    BackColor = Color.DarkSlateGray
                };

                var label = new Label
                {
                    Text = "Ödeme Yöntemi Seçin",
                    AutoSize = true,
                    Location = new Point(90, 20),
                    Font = new Font("Arial", 12),
                    ForeColor = Color.White
                };

                var btnNakit = new Button
                {
                    Text = "Nakit",
                    Width = 120,
                    Height = 50,
                    Location = new Point(30, 70),
                    BackColor = Color.MediumSeaGreen,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    ForeColor = Color.White
                };

                var btnKrediKarti = new Button
                {
                    Text = "Kredi Kartı",
                    Width = 120,
                    Height = 50,
                    Location = new Point(150, 70),
                    BackColor = Color.CornflowerBlue,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    ForeColor = Color.White
                };

                var btnTik = new Button
                {
                    Text = "✓",
                    Width = 50,
                    Location = new Point(90, 120),
                    BackColor = Color.MediumSeaGreen,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    Visible = false
                };

                var btnX = new Button
                {
                    Text = "✗",
                    Width = 50,
                    Location = new Point(160, 120),
                    BackColor = Color.Crimson,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    Visible = false
                };

                btnNakit.Click += (s, args) =>
                {
                    btnTik.Visible = true;
                    btnX.Visible = true;
                };

                btnKrediKarti.Click += (s, args) =>
                {
                    btnTik.Visible = true;
                    btnX.Visible = true;
                };

                btnTik.Click += (s, args) =>
                {
                    // Ürünleri temizle
                    if (additionId.HasValue)
                    {
                        additionController.ClearAddition(additionId.Value);
                    }
                    LoadProducts();
                    paymentPopup.Close();
                };

                btnX.Click += (s, args) =>
                {
                    paymentPopup.Close();
                };

                paymentPopup.Controls.Add(label);
                paymentPopup.Controls.Add(btnNakit);
                paymentPopup.Controls.Add(btnKrediKarti);
                paymentPopup.Controls.Add(btnTik);
                paymentPopup.Controls.Add(btnX);

                paymentPopup.ShowDialog();
            }
            else
            {
                MessageBox.Show("Masada ürün bulunmamaktadır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void ShowTransactionCompletionPopup()
        {
            using (var transactionForm = new Form())
            {
                transactionForm.Text = "İşlem Tamamlandı mı?";
                transactionForm.Size = new Size(300, 150);
                transactionForm.StartPosition = FormStartPosition.CenterParent;

                var tikButton = new Button()
                {
                    Text = "✔",
                    DialogResult = DialogResult.OK,
                    Dock = DockStyle.Left,
                    Width = 150
                };

                var cancelButton = new Button()
                {
                    Text = "✘",
                    DialogResult = DialogResult.Cancel,
                    Dock = DockStyle.Right,
                    Width = 150
                };

                tikButton.Click += (s, args) => CompleteTransaction();
                cancelButton.Click += (s, args) => transactionForm.Close();

                transactionForm.Controls.Add(tikButton);
                transactionForm.Controls.Add(cancelButton);

                transactionForm.ShowDialog();
            }
        }

        private void CompleteTransaction()
        {
            var result = additionController.GetTableDetail(tableId);
            int? additionId = result.Item1;
            List<(int, decimal, decimal, int, DateTime, DateTime)> additionDetails = result.Item7;

            foreach (var detail in additionDetails)
            {
                int productId = detail.Item1;
                int quantity = detail.Item4;
                UpdateProductStock(productId, -quantity);
            }

            if (additionId.HasValue)
            {
                additionController.ClearAddition(additionId.Value);
            }

            flowLayoutPanelProducts.Controls.Clear();
            UpdateTotalPrice();
        }
    }
}
