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
    public partial class FUrun : Form
    {

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");

        public FUrun()
        {
            InitializeComponent();
        }
        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select * from \"Urun\" order by \"UrunNo\" desc";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        private void FUrun_Load(object sender, EventArgs e)
        {
            button10.Enabled = false;
            Listele();
        }

        private void button10_Click(object sender, EventArgs e) // ekle
        {
            button10.Enabled = false;  // ekle
            button9.Enabled = true;  //sil
            button8.Enabled = true; // ara
            button1.Enabled = true; // güncelle

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e) // sil
        {
            button9.Enabled = false;  // sil
            button10.Enabled = true;  //ekle
            button8.Enabled = true; // ara
            button1.Enabled = true; // güncelle


            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            numericUpDown1.Enabled = false;

        }

        private void button8_Click(object sender, EventArgs e) // ara
        {
            button8.Enabled = false;  // ara
            button10.Enabled = true;  //ekle
            button9.Enabled = true; // sil
            button1.Enabled = true; // güncelle

            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            numericUpDown1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) // güncelle
        {
            button1.Enabled = false;  // güncelle
            button10.Enabled = true;  // ekle
            button9.Enabled = true; // sil
            button8.Enabled = true; // ara

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e) // uygula
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }


            string urunkodu = textBox1.Text;
            string model = textBox2.Text;
            string isim = textBox3.Text;
            int stoksayi = int.Parse(numericUpDown1.Value.ToString());
            NpgsqlCommand komut = new NpgsqlCommand();
            komut.Connection = baglanti;
            string mesaj = "";


            if (button10.Enabled==false) // ekle
            {
                baglanti.Open();

                komut.CommandText = "insert into \"Urun\" (\"UrunKodu\",\"UrunModel\",\"Isim\",\"Stok\")" +
                                    "Values (@p1,@p2,@p3,@p4);";

                komut.Parameters.AddWithValue("@p1", urunkodu);
                komut.Parameters.AddWithValue("@p2", model);
                komut.Parameters.AddWithValue("@p3", isim);
                komut.Parameters.AddWithValue("@p4", stoksayi);
                komut.ExecuteNonQuery();
                baglanti.Close();
                mesaj= "Ekleme islemi tamamlandi";
                MessageBox.Show(mesaj);
                Listele();

            }
            else if(button9.Enabled==false) // sil
            {
                baglanti.Open();
                int urunno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                komut.CommandText = "delete from \"Urun\" where \"UrunNo\"="+urunno;
                komut.ExecuteNonQuery();
                mesaj = "Silme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if(button8.Enabled==false) // ara
            {
                baglanti.Open();
                string sorgu = "select * from \"Urun\" where \"UrunKodu\"=\'"+urunkodu+"\'";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglanti.Close();
            }
            else if(button1.Enabled==false) // güncelle
            {
                baglanti.Open();
                int urunno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                komut.CommandText = "update \"Urun\" set \"UrunKodu\"='" + urunkodu + "',\"UrunModel\"='" + model + "',\"Isim\"='" + isim + "',\"Stok\"=" + stoksayi +" where \"UrunNo\"="+urunno ;
                komut.ExecuteNonQuery();
                baglanti.Close();
                mesaj = "Güncelleme işlemi gerçekleşti";
                MessageBox.Show(mesaj);
                Listele();
            }

        }

        private void button7_Click(object sender, EventArgs e) // arama sonuçlarını sıfırla
        {
            Listele();
        }
    }
}
