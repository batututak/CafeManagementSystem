namespace CafeManagement
{
    partial class MasalarForm
    {
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTables;

        private void InitializeComponent()
        {
            this.flowLayoutPanelTables = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTables
            // 
            this.flowLayoutPanelTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTables.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTables.Name = "flowLayoutPanelTables";
            this.flowLayoutPanelTables.Size = new System.Drawing.Size(800, 450);
            this.flowLayoutPanelTables.TabIndex = 0;
            // 
            // MasalarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanelTables);
            this.Name = "MasalarForm";
            this.Text = "Masalar";
            this.Load += new System.EventHandler(this.MasalarForm_Load);
            this.ResumeLayout(false);

        }
    }
}
