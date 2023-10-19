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
    public partial class FrmOgrenciDetay : Form
    {
        public FrmOgrenciDetay()
        {
            InitializeComponent();
        }

        SqlConnection bgl = new SqlConnection("Data Source=DESKTOP-F5HD02D\\SQLEXPRESS;Initial Catalog=DbOgrenciNotKayit;Integrated Security=True");
        public string ogrNo;

        private void FrmOgrenciDetay_Load(object sender, EventArgs e)
        {

            LblOgrNo.Text = ogrNo;

            bgl.Open();
            SqlCommand cmd = new SqlCommand(" SELECT * FROM TblOgrenciler WHERE OgrNo = @p1", bgl);
            cmd.Parameters.AddWithValue("@p1", ogrNo);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[2] + " " + dr[3];
                LblSinav1.Text = dr[4].ToString();
                LblSinav2.Text = dr[5].ToString();
                LblSinav3.Text = dr[6].ToString();
                LblOrt.Text = dr[7].ToString();
                LblDurum.Text = dr[8].ToString();
            }
            if (LblDurum.Text == "True") { LblDurum.Text = "Geçti"; }
            if (LblDurum.Text == "False") { LblDurum.Text = "Kaldı"; }
            bgl.Close();
        }
    }
}
