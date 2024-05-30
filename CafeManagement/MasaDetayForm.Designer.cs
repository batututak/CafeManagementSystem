using System.Windows.Forms;

namespace CafeManagement
{
    partial class MasaDetayForm
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flowLayoutPanelProducts;
        private Label lblTotalPrice;
        private Button btnUrunEkle;
        private Button btnHesap;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotalPrice = new System.Windows.Forms.Label();
            this.btnUrunEkle = new System.Windows.Forms.Button();
            this.btnHesap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.AutoScroll = true;
            this.flowLayoutPanelProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(800, 400);
            this.flowLayoutPanelProducts.TabIndex = 0;
            // 
            // lblTotalPrice
            // 
            this.lblTotalPrice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotalPrice.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPrice.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPrice.Location = new System.Drawing.Point(0, 400);
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Size = new System.Drawing.Size(800, 50);
            this.lblTotalPrice.TabIndex = 1;
            this.lblTotalPrice.Text = "Toplam Fiyat: 0.00";
            this.lblTotalPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUrunEkle
            // 
            this.btnUrunEkle.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUrunEkle.FlatAppearance.BorderSize = 0;
            this.btnUrunEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunEkle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnUrunEkle.ForeColor = System.Drawing.Color.White;
            this.btnUrunEkle.Location = new System.Drawing.Point(650, 410);
            this.btnUrunEkle.Name = "btnUrunEkle";
            this.btnUrunEkle.Size = new System.Drawing.Size(130, 40);
            this.btnUrunEkle.TabIndex = 2;
            this.btnUrunEkle.Text = "Ürün Ekle";
            this.btnUrunEkle.UseVisualStyleBackColor = false;
            this.btnUrunEkle.Click += new System.EventHandler(this.btnUrunEkle_Click);
            // 
            // btnHesap
            // 
            this.btnHesap.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnHesap.FlatAppearance.BorderSize = 0;
            this.btnHesap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHesap.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnHesap.ForeColor = System.Drawing.Color.White;
            this.btnHesap.Location = new System.Drawing.Point(500, 410);
            this.btnHesap.Name = "btnHesap";
            this.btnHesap.Size = new System.Drawing.Size(130, 40);
            this.btnHesap.TabIndex = 3;
            this.btnHesap.Text = "Hesap";
            this.btnHesap.UseVisualStyleBackColor = false;
            this.btnHesap.Click += new System.EventHandler(this.btnHesap_Click);
            // 
            // MasaDetayForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnHesap);
            this.Controls.Add(this.btnUrunEkle);
            this.Controls.Add(this.flowLayoutPanelProducts);
            this.Controls.Add(this.lblTotalPrice);
            this.Name = "MasaDetayForm";
            this.ResumeLayout(false);

        }

    }
}
