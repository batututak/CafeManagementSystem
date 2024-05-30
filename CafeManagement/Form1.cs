using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CafeManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(34, 40, 49); // Koyu arka plan rengi
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Butonları oluştur ve stillerini uygula
            CreateButton(btnStokKontrol, "Stok Kontrolü", Color.FromArgb(52, 152, 219), new EventHandler(this.btnStokKontrol_Click));
            CreateButton(btnMasalar, "Masalar", Color.FromArgb(46, 204, 113), new EventHandler(this.btnMasalar_Click));
        }

        private void CreateButton(Button button, string text, Color backColor, EventHandler clickEvent)
        {
            button.Text = text;
            button.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            button.Size = new Size(200, 100);
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Click += clickEvent;
            button.Paint += new PaintEventHandler(this.Button_Paint);
            button.MouseEnter += (s, e) => button.BackColor = ControlPaint.Light(backColor);
            button.MouseLeave += (s, e) => button.BackColor = backColor;
        }

        private void Button_Paint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, 20, 20, 180, 90);
                    path.AddArc(button.Width - 20, 0, 20, 20, 270, 90);
                    path.AddArc(button.Width - 20, button.Height - 20, 20, 20, 0, 90);
                    path.AddArc(0, button.Height - 20, 20, 20, 90, 90);
                    path.CloseAllFigures();
                    button.Region = new Region(path);
                }
            }
        }

        private void btnStokKontrol_Click(object sender, EventArgs e)
        {
            StokKontrolForm stokKontrolForm = new StokKontrolForm();
            stokKontrolForm.Show();
        }

        private void btnMasalar_Click(object sender, EventArgs e)
        {
            MasalarForm masalarForm = new MasalarForm();
            masalarForm.Show();
        }
    }
}
