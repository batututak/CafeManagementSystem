namespace CafeManagement
{
    partial class ProductDetailForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.NumericUpDown numericUpDownStock;
        private System.Windows.Forms.NumericUpDown numericUpDownPrice;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelStock;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Panel panel;

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
            this.panel = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.labelStock = new System.Windows.Forms.Label();
            this.numericUpDownStock = new System.Windows.Forms.NumericUpDown();
            this.labelPrice = new System.Windows.Forms.Label();
            this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.buttonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.labelTitle);
            this.panel.Controls.Add(this.textBoxTitle);
            this.panel.Controls.Add(this.labelStock);
            this.panel.Controls.Add(this.numericUpDownStock);
            this.panel.Controls.Add(this.labelPrice);
            this.panel.Controls.Add(this.numericUpDownPrice);
            this.panel.Controls.Add(this.buttonSave);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(300, 200);
            this.panel.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(20, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(87, 19);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Ürün Adı:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Font = new System.Drawing.Font("Arial", 12F);
            this.textBoxTitle.Location = new System.Drawing.Point(120, 17);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(150, 26);
            this.textBoxTitle.TabIndex = 1;
            // 
            // labelStock
            // 
            this.labelStock.AutoSize = true;
            this.labelStock.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStock.Location = new System.Drawing.Point(20, 60);
            this.labelStock.Name = "labelStock";
            this.labelStock.Size = new System.Drawing.Size(53, 19);
            this.labelStock.TabIndex = 2;
            this.labelStock.Text = "Stok:";
            // 
            // numericUpDownStock
            // 
            this.numericUpDownStock.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDownStock.Location = new System.Drawing.Point(120, 58);
            this.numericUpDownStock.Name = "numericUpDownStock";
            this.numericUpDownStock.Size = new System.Drawing.Size(150, 26);
            this.numericUpDownStock.TabIndex = 3;
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrice.Location = new System.Drawing.Point(20, 100);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(54, 19);
            this.labelPrice.TabIndex = 4;
            this.labelPrice.Text = "Fiyat:";
            // 
            // numericUpDownPrice
            // 
            this.numericUpDownPrice.DecimalPlaces = 2;
            this.numericUpDownPrice.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDownPrice.Location = new System.Drawing.Point(120, 98);
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.Size = new System.Drawing.Size(150, 26);
            this.numericUpDownPrice.TabIndex = 5;
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.buttonSave.Location = new System.Drawing.Point(120, 140);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(150, 30);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Kaydet";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // ProductDetailForm
            // 
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.panel);
            this.Name = "ProductDetailForm";
            this.Text = "Ürün Detayı";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
