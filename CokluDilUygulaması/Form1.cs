using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CeviriUygulamasi;

namespace CeviriUygulamasi
{
    public partial class Form1 : Form
    {
       
        private TextBox txtGiris;
        private TextBox txtCikis;
        private ComboBox cmbDiller;
        private Button btnCevir;
        private Label lblBaslik;

        public Form1()
        {
            
            this.Text = "Basit Çeviri Uygulaması - Öğrenci Projesi";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            
            ArayuzuOlustur();
        }

        private void ArayuzuOlustur()
        {
            // Başlık
            lblBaslik = new Label();
            lblBaslik.Text = "Çoklu Dil Çevirici";
            lblBaslik.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblBaslik.AutoSize = true;
            lblBaslik.Location = new Point(130, 20);
            this.Controls.Add(lblBaslik);

            
            txtGiris = new TextBox();
            txtGiris.Multiline = true;
            txtGiris.ScrollBars = ScrollBars.Vertical;
            txtGiris.Size = new Size(400, 100);
            txtGiris.Location = new Point(40, 60);
            txtGiris.Font = new Font("Consolas", 10);
          
            this.Controls.Add(txtGiris);

            
            cmbDiller = new ComboBox();
            cmbDiller.Items.AddRange(new object[] { "İngilizce -> Türkçe", "Türkçe -> İngilizce", "Türkçe -> Almanca", "Türkçe -> Fransızca" });
            cmbDiller.SelectedIndex = 0; 
            cmbDiller.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDiller.Size = new Size(200, 30);
            cmbDiller.Location = new Point(40, 170);
            this.Controls.Add(cmbDiller);

            
            btnCevir = new Button();
            btnCevir.Text = "ÇEVİR";
            btnCevir.Size = new Size(190, 30);
            btnCevir.Location = new Point(250, 168);
            btnCevir.BackColor = Color.DodgerBlue;
            btnCevir.ForeColor = Color.White;
            btnCevir.FlatStyle = FlatStyle.Flat;
            btnCevir.Click += BtnCevir_Click; 
            this.Controls.Add(btnCevir);

            
            txtCikis = new TextBox();
            txtCikis.Multiline = true;
            txtCikis.ScrollBars = ScrollBars.Vertical;
            txtCikis.Size = new Size(400, 100);
            txtCikis.Location = new Point(40, 210);
            txtCikis.Font = new Font("Consolas", 10);
            txtCikis.ReadOnly = true; 
            this.Controls.Add(txtCikis);
        }

       
        private async void BtnCevir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGiris.Text))
            {
                MessageBox.Show("Lütfen çevrilecek bir metin girin.");
                return;
            }

            btnCevir.Text = "Çevriliyor...";
            btnCevir.Enabled = false;

            try
            {
                
                string kaynakDil = "tr";
                string hedefDil = "en";

                switch (cmbDiller.SelectedIndex)
                {
                    case 0: kaynakDil = "en"; hedefDil = "tr"; break; 
                    case 1: kaynakDil = "tr"; hedefDil = "en"; break; 
                    case 2: kaynakDil = "tr"; hedefDil = "de"; break; 
                    case 3: kaynakDil = "tr"; hedefDil = "fr"; break; 
                }

              
                string sonuc = await CeviriYap(txtGiris.Text, kaynakDil, hedefDil);
                txtCikis.Text = sonuc;
            }
            catch (Exception ex)
            {
                txtCikis.Text = "Hata oluştu: " + ex.Message;
            }
            finally
            {
                btnCevir.Text = "ÇEVİR";
                btnCevir.Enabled = true;
            }
        }

    
        private async Task<string> CeviriYap(string metin, string kaynak, string hedef)
        {
            using (HttpClient client = new HttpClient())
            {
            
                string url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(metin)}&langpair={kaynak}|{hedef}";

              
                string jsonResponse = await client.GetStringAsync(url);

               

                var match = Regex.Match(jsonResponse, "\"translatedText\":\"(.*?)\"");
                if (match.Success)
                {
                    
                    return Regex.Unescape(match.Groups[1].Value);
                }
                else
                {
                    return "Çeviri başarısız.";
                }
            }
        }
    }
}