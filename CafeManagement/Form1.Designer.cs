namespace CafeManagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStokKontrol;
        private System.Windows.Forms.Button btnMasalar;

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
            this.btnStokKontrol = new System.Windows.Forms.Button();
            this.btnMasalar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStokKontrol
            // 
            this.btnStokKontrol.Location = new System.Drawing.Point(100, 150);
            this.btnStokKontrol.Name = "btnStokKontrol";
            this.btnStokKontrol.Size = new System.Drawing.Size(200, 100);
            this.btnStokKontrol.TabIndex = 0;
            this.btnStokKontrol.UseVisualStyleBackColor = true;
            // 
            // btnMasalar
            // 
            this.btnMasalar.Location = new System.Drawing.Point(350, 150);
            this.btnMasalar.Name = "btnMasalar";
            this.btnMasalar.Size = new System.Drawing.Size(200, 100);
            this.btnMasalar.TabIndex = 1;
            this.btnMasalar.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 400);
            this.Controls.Add(this.btnStokKontrol);
            this.Controls.Add(this.btnMasalar);
            this.Name = "Form1";
            this.Text = "Cafe Management";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }
    }
}
