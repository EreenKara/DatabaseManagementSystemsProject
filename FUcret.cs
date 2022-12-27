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
    public partial class FUcret : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");

        public FUcret()
        {
            InitializeComponent();
        }
        private void FUcret_Load(object sender, EventArgs e)
        {
            button9.Enabled = false;
            Listele();
        }
        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select * from \"Ucret\" order by \"Maas\"";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        
        private void button9_Click(object sender, EventArgs e) // ekle
        {
            button9.Enabled = false;  // ekle
            button8.Enabled = true;  //sil
            button1.Enabled = true; // güncelle

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e) // sil
        {
            button9.Enabled = true;  // ekle
            button8.Enabled = false;  //sil
            button1.Enabled = true; // güncelle

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) //güncelle
        {
            button9.Enabled = true;  // ekle
            button8.Enabled = true;  //sil
            button1.Enabled = false; // güncelle

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)  //uygula
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }


            
            NpgsqlCommand komut = new NpgsqlCommand();
            komut.Connection = baglanti;
            string mesaj = "";


            if (button9.Enabled == false) // ekle
            {
                baglanti.Open();
                int maas = int.Parse(textBox1.Text);
                int ssk = int.Parse(textBox2.Text);
                int yol = int.Parse(textBox3.Text);
                int yemek = int.Parse(textBox4.Text);
                komut.CommandText = "insert into \"Ucret\" (\"Maas\",\"SSK\",\"Yol\",\"Yemek\")" +
                                    "Values (@p1,@p2,@p3,@p4);";

                komut.Parameters.AddWithValue("@p1", maas);
                komut.Parameters.AddWithValue("@p2", ssk);
                komut.Parameters.AddWithValue("@p3", yol);
                komut.Parameters.AddWithValue("@p4", yemek);
                komut.ExecuteNonQuery();
                baglanti.Close();
                mesaj = "Ekleme islemi tamamlandi";
                MessageBox.Show(mesaj);
                Listele();

            }
            else if (button8.Enabled == false) // sil
            {
                baglanti.Open();
                int ucretno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                
                komut.CommandText = "delete from \"Ucret\" where \"UcretNo\"=" + ucretno;
                komut.ExecuteNonQuery();
                mesaj = "Silme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if (button1.Enabled == false) // güncelle
            {
                baglanti.Open();
                int maas = int.Parse(textBox1.Text);
                int ssk = int.Parse(textBox2.Text);
                int yol = int.Parse(textBox3.Text);
                int yemek = int.Parse(textBox4.Text);
                int ucretno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                komut.CommandText = "update \"Ucret\" set \"Maas\"=" + maas + ",\"SSK\"=" + ssk + ",\"Yol\"=" + yol + ",\"Yemek\"=" + yemek+"where \"UcretNo\"="+ucretno;
                komut.ExecuteNonQuery();
                baglanti.Close();
                mesaj = "Güncelleme işlemi gerçekleşti";
                MessageBox.Show(mesaj);
                Listele();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Listele();
        }

        
    }
}
