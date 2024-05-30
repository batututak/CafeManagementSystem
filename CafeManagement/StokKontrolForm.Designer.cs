namespace CafeManagement
{
    partial class StokKontrolForm
    {
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCategories;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelProducts;
        private System.Windows.Forms.Panel panelCategories;
        private System.Windows.Forms.Panel panelProducts;
        private System.Windows.Forms.Button buttonBack;

        private void InitializeComponent()
        {
            this.panelCategories = new System.Windows.Forms.Panel();
            this.flowLayoutPanelCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.panelProducts = new System.Windows.Forms.Panel();
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.panelCategories.SuspendLayout();
            this.panelProducts.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCategories
            // 
            this.panelCategories.AutoScroll = true;
            this.panelCategories.Controls.Add(this.flowLayoutPanelCategories);
            this.panelCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCategories.Location = new System.Drawing.Point(0, 0);
            this.panelCategories.Name = "panelCategories";
            this.panelCategories.Size = new System.Drawing.Size(1920, 100);
            this.panelCategories.TabIndex = 0;
            // 
            // flowLayoutPanelCategories
            // 
            this.flowLayoutPanelCategories.AutoSize = true;
            this.flowLayoutPanelCategories.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelCategories.Name = "flowLayoutPanelCategories";
            this.flowLayoutPanelCategories.Size = new System.Drawing.Size(1900, 60);
            this.flowLayoutPanelCategories.TabIndex = 0;
            this.flowLayoutPanelCategories.WrapContents = false;
            // 
            // panelProducts
            // 
            this.panelProducts.AutoScroll = true;
            this.panelProducts.Controls.Add(this.flowLayoutPanelProducts);
            this.panelProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProducts.Location = new System.Drawing.Point(0, 100);
            this.panelProducts.Name = "panelProducts";
            this.panelProducts.Size = new System.Drawing.Size(1920, 961);
            this.panelProducts.TabIndex = 1;
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.AutoSize = true;
            this.flowLayoutPanelProducts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(1900, 880);
            this.flowLayoutPanelProducts.TabIndex = 1;
            // 
            // buttonBack
            // 
            this.buttonBack.AutoSize = true;
            this.buttonBack.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.buttonBack.Location = new System.Drawing.Point(12, 12);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(150, 50);
            this.buttonBack.TabIndex = 2;
            this.buttonBack.Text = "Geri";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // StokKontrolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1061);
            this.Controls.Add(this.panelProducts);
            this.Controls.Add(this.panelCategories);
            this.Controls.Add(this.buttonBack);
            this.Name = "StokKontrolForm";
            this.Text = "Stok Kontrolü";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelCategories.ResumeLayout(false);
            this.panelCategories.PerformLayout();
            this.panelProducts.ResumeLayout(false);
            this.panelProducts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
