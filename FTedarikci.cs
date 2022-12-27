using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeriTabaniUygulamasi
{
    public partial class FTedarikci : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");
        public FTedarikci()
        {
            InitializeComponent();

        }
        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select \"KisiNo\",\"TCNo\",\"Isim\",\"Soyisim\",\"Telefon\",\"Adres\" from \"Kisi\" where \"KisiTipi\"='T' order by \"KisiNo\" desc";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        public void CBoxListele()
        {
            baglanti.Open();

            string sorgu = "select \"UrunNo\", \"UrunModel\" from \"Urun\"";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "UrunModel";
            comboBox1.ValueMember = "UrunNo";
            comboBox1.DataSource = dt;

            baglanti.Close();
        }

        private void FTedarikci_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Listele();
            CBoxListele();
        }

        private void button1_Click(object sender, EventArgs e)  //ekle
        {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) //sil
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)  // ara
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e) //güncelle
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e) //uygula
        {

        }
    }
}
