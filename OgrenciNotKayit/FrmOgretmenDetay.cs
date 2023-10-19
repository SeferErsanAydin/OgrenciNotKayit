using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OgrenciNotKayit
{
    public partial class FrmOgretmenDetay : Form
    {
        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }

        SqlConnection bgl = new SqlConnection("Data Source=DESKTOP-F5HD02D\\SQLEXPRESS;Initial Catalog=DbOgrenciNotKayit;Integrated Security=True");

        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            Listele();
        }

        //data grid viewi doldurur
        private void Listele()
        {
            this.tblOgrencilerTableAdapter.Fill(this.dbOgrenciNotKayitDataSet.TblOgrenciler);
            bgl.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUM(CASE WHEN OgrDurum = 1 THEN 1 ELSE 0 END), SUM(CASE WHEN OgrDurum = 0 THEN 1 ELSE 0 END)FROM TblOgrenciler;",bgl);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                LblGecenOgrSayi.Text = dr[0].ToString();
                LblKalanOgrSayi.Text = dr[1].ToString();
            }
            bgl.Close();
        }
        private void Temizle()
        {
            foreach (GroupBox grp in Controls)
            {

                foreach (TextBox item in grp.Controls.OfType<TextBox>())
                {
                    item.Text = "";
                }
                foreach (MaskedTextBox item in grp.Controls.OfType<MaskedTextBox>())
                {
                    item.Text = "";
                }
            }
        }
        private void BtnOgrKaydet_Click(object sender, EventArgs e)
        {
            bgl.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO TblOgrenciler (OgrNo, OgrAd, OgrSoyad) VALUES (@p1, @p2, @p3)", bgl);
            cmd.Parameters.AddWithValue("@p1", MskOgrNo.Text);
            cmd.Parameters.AddWithValue("@p2", TxtAd.Text);
            cmd.Parameters.AddWithValue("@p3", TxtSoyad.Text);
            cmd.ExecuteNonQuery();
            bgl.Close();
            MessageBox.Show($"{TxtAd.Text} {TxtSoyad.Text} adlı Öğrenci Başarıyla Sisteme Eklenmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Temizle();
            Listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MskOgrNo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            TxtAd.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            TxtSinav1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            TxtSinav2.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            TxtSinav3.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            BtnOgrKaydet.Enabled = false;
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
            BtnOgrKaydet.Enabled = true;
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            double ortalama, s1, s2, s3;
            string durum;
            string drm = "";
            s1 = Convert.ToDouble(TxtSinav1.Text);
            s2 = Convert.ToDouble(TxtSinav2.Text);
            s3 = Convert.ToDouble(TxtSinav3.Text);

            ortalama = (s1 + s2 + s3) / 3;
            ortalama = Math.Round(ortalama, 2); // çıkan decimal rakamdaki noktadan sonra 2 rakamı alıyoruz

            LblOrtalama.Text = Convert.ToString(ortalama);

            if (ortalama >= 50) { durum = "True"; }
            else { durum = "False"; }

            if (durum == "True") { drm = "Geçti"; }
            else if (durum == "False") { drm = "Kaldı"; }

            bgl.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TblOgrenciler SET OgrS1 = @p1, OgrS2 = @p2, OgrS3 = @p3, OgrOrt = @p4, OgrDurum = @p5 WHERE OgrNo = @p6", bgl);
            cmd.Parameters.AddWithValue("@p1", TxtSinav1.Text);
            cmd.Parameters.AddWithValue("@p2", TxtSinav2.Text);
            cmd.Parameters.AddWithValue("@p3", TxtSinav3.Text);
            cmd.Parameters.AddWithValue("@p4", Convert.ToDecimal(LblOrtalama.Text));
            cmd.Parameters.AddWithValue("@p5", durum);
            cmd.Parameters.AddWithValue("@p6", MskOgrNo.Text);
            cmd.ExecuteNonQuery();
            bgl.Close();
            MessageBox.Show($"{TxtAd.Text} {TxtSoyad.Text} adlı ögrencinin;\n1.Sınavı: {TxtSinav1.Text}\n2.Sınavı: {TxtSinav2.Text}\n3.Sınavı: {TxtSinav3.Text},\n Not Ortalaması: {LblOrtalama.Text} ve Sınıf Geçme Durumu: {drm}");
            Temizle();
            Listele();
        }
    }
}
