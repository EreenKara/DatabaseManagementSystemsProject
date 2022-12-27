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
    public partial class FArac : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");

        public FArac()
        {
            InitializeComponent();
        }

        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select * ,(select get_data_kisi((select\"Arac\".\"KisiNo\" from \"Kisi\" where \"Kisi\".\"KisiNo\"=\"Arac\".\"KisiNo\"))) as \"Musteri\" from \"Arac\"";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

        }
        public void CBoxListele()
        {
            baglanti.Open();

            string sorgu = "select \"KisiNo\",(select get_data_kisi(\"KisiNo\")) as \"Musteri\" from \"Musteri\"";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "Musteri";
            comboBox1.ValueMember = "KisiNo";
            comboBox1.DataSource = dt;

            baglanti.Close();
        }

        private void FArac_Load(object sender, EventArgs e)
        {
            Listele();
            CBoxListele();
        }

        private void button1_Click(object sender, EventArgs e) // ekle
        {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) // sil
        {
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e) // ara
        {
            button3.Enabled = false;
            button2.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e) // güncelle
        {
            button6.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e) // arama sonuçlarını sıfırla
        {
            Listele();
        }

        private void button5_Click(object sender, EventArgs e) // uygula
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            

            if (button1.Enabled == false) // ekle
            {
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "insert into \"Arac\" (\"Plaka\",\"Kilometre\",\"Model\",\"KisiNo\")" +
                                    "Values(@p1,@p2,@p3,@p4);";
                string plaka = textBox1.Text;
                int kilometre = int.Parse(textBox2.Text);
                string model = textBox3.Text;
                int kisino = int.Parse(comboBox1.SelectedValue.ToString());
                string kisino2 = comboBox1.SelectedValue.ToString();

                komut.Parameters.AddWithValue("@p1", plaka);
                komut.Parameters.AddWithValue("@p2", kilometre);
                komut.Parameters.AddWithValue("@p3", model);
                komut.Parameters.AddWithValue("@p4", kisino);
                MessageBox.Show(kisino2);
                komut.ExecuteNonQuery();
                
                string mesaj = "Ekleme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if (button2.Enabled == false) // sil
            {
                baglanti.Open();
                int aracno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "delete from \"Arac\" where \"AracNo\"=" + aracno + ";";
                komut.ExecuteNonQuery();
                string mesaj = "Silme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if (button3.Enabled == false) // ara 
            {
                baglanti.Open();
                string plaka = textBox1.Text;
                string sorgu = "select * ,(select get_data_kisi((select\"Arac\".\"KisiNo\" from \"Kisi\" where \"Kisi\".\"KisiNo\"=\"Arac\".\"KisiNo\"))) as \"Müşteri Ad Soyad\" from \"Arac\" where \"Plaka\"=\'"+plaka+"\';";
                
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            else if (button6.Enabled == false) // güncelle 
            {
                baglanti.Open();
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string plaka = textBox1.Text;
                    int kilometre = int.Parse(textBox2.Text);
                    string model = textBox3.Text;
                    int kisino = int.Parse(comboBox1.SelectedValue.ToString());
                    int aracno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    NpgsqlCommand komut = new NpgsqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update \"Arac\" set \"Plaka\"=\'" + plaka + "\',\"Kilometre\"=" + kilometre + ",\"Model\"=\'" + model + "\',\"KisiNo\"=" + kisino + " where \"AracNo\"=" + aracno + ";";


                    komut.ExecuteNonQuery();
                    string mesaj = "Güncelleme işlemi gerçekleşti";
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
