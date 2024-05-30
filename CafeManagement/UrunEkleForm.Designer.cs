namespace CafeManagement
{
    partial class UrunEkleForm
    {
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCategories;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelProducts;

        private void InitializeComponent()
        {
            this.flowLayoutPanelCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelCategories
            // 
            this.flowLayoutPanelCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelCategories.AutoScroll = true;
            this.flowLayoutPanelCategories.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowLayoutPanelCategories.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelCategories.Name = "flowLayoutPanelCategories";
            this.flowLayoutPanelCategories.Size = new System.Drawing.Size(800, 100);
            this.flowLayoutPanelCategories.TabIndex = 0;
            this.flowLayoutPanelCategories.WrapContents = false;
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelProducts.AutoScroll = true;
            this.flowLayoutPanelProducts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(0, 100);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(800, 350);
            this.flowLayoutPanelProducts.TabIndex = 1;
            // 
            // UrunEkleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanelProducts);
            this.Controls.Add(this.flowLayoutPanelCategories);
            this.Name = "UrunEkleForm";
            this.Text = "Ürün Ekle";
            this.Load += new System.EventHandler(this.UrunEkleForm_Load);
            this.ResumeLayout(false);
        }
    }
}
