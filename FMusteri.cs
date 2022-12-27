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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VeriTabaniUygulamasi
{
    public partial class FMusteri : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");

        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select \"Musteri\".\"KisiNo\",\"TCNo\",\"Isim\",\"Soyisim\",\"Telefon\",\"Adres\",\"VergiAdresi\" from \"Kisi\" inner join \"Musteri\" on \"Musteri\".\"KisiNo\"=\"Kisi\".\"KisiNo\"";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource= dt;
            baglanti.Close();

        }
        public FMusteri()
        {
            InitializeComponent();
        }

        private void FMusteri_Load(object sender, EventArgs e)
        {
            Listele();
        }

        

        private void button1_Click(object sender, EventArgs e) // ekle 
        {
            button1.Enabled= false;
            button2.Enabled= true;
            button3.Enabled= true;
            button6.Enabled= true;
        }

        private void button2_Click(object sender, EventArgs e) // sil
        {
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e) // ara
        {
            button3.Enabled = false;
            button2.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
        }
        private void button4_Click(object sender, EventArgs e)// ARamasonuçlarını sifirla
        {
            Listele();
        }
        private void button6_Click(object sender, EventArgs e) // güncelle
        {
            button6.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = true;
        }
    
        private void button5_Click(object sender, EventArgs e) //uygula
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            string mesaj="";

            if (button1.Enabled==false) // ekle
            {
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText= "insert into \"Kisi\" (\"Isim\",\"Soyisim\",\"TCNo\",\"Telefon\",\"Adres\",\"KisiTipi\")" +
                                    "Values(@p1,@p2,@p3,@p4,@p5,\'M\');" +
                                    "insert into \"Musteri\" (\"VergiAdresi\")" +
                                    " Values(@p6);";
                string isim = textBox1.Text;
                string soyisim = textBox3.Text;
                string TCNo = textBox2.Text;
                string telefon = textBox4.Text;
                string adres = textBox5.Text;
                string vergiadresi = textBox6.Text;
                komut.Parameters.AddWithValue("@p1", isim);
                komut.Parameters.AddWithValue("@p2", soyisim);
                komut.Parameters.AddWithValue("@p3", TCNo);
                komut.Parameters.AddWithValue("@p4", telefon);
                komut.Parameters.AddWithValue("@p5", adres);
                komut.Parameters.AddWithValue("@p6", vergiadresi);
                komut.ExecuteNonQuery();

                mesaj = "Ekleme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if(button2.Enabled==false) // sil
            {
                baglanti.Open();
                int kisino = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "delete from \"Musteri\" where \"KisiNo\"=" + kisino+";"+
                    "delete from \"Kisi\" where \"KisiNo\"="+kisino;
                komut.ExecuteNonQuery();
                mesaj = "Silme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if(button3.Enabled==false)// ara 
            {
                baglanti.Open();
                string sorgu = "select \"Musteri\".\"KisiNo\",\"VergiAdresi\",\"TCNo\",\"Isim\",\"Soyisim\",\"Telefon\",\"Adres\" from \"Kisi\" inner join \"Musteri\" on \"Musteri\".\"KisiNo\"=\"Kisi\".\"KisiNo\" where \"TCNo\"=\'"+textBox2.Text+"\';";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            else if(button6.Enabled==false)// güncelle 
            {
                baglanti.Open();
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int kisino = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    NpgsqlCommand komut = new NpgsqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update \"Kisi\" set \"TCNo\"=\'" + textBox2.Text + "\',\"Isim\"=\'" + textBox1.Text + "\',\"Soyisim\"=\'" + textBox3.Text + "\',\"Telefon\"=\'" + textBox4.Text + "\',\"Adres\"=\'" + textBox5.Text + "\' where \"KisiNo\"=" + kisino +  ";"+
                        "update \"Musteri\" set \"VergiAdresi\"=\'"+textBox6.Text+"\' where \"KisiNo\"="+kisino;


                    komut.ExecuteNonQuery();
                    mesaj = "Güncelleme işlemi gerçekleşti";
                    MessageBox.Show(mesaj);
                    baglanti.Close();
                    Listele();

                }
                else
                {
                    MessageBox.Show("Lütfen bir tane satır seçiniz");
                }
            }



        }

    }
}
