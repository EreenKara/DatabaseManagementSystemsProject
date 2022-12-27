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
using System.Configuration;
namespace VeriTabaniUygulamasi
{
    public partial class FPersonel : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");
        
        public FPersonel()
        {
            InitializeComponent();
        }
        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select \"KisiNo\",\"TCNo\",\"Isim\",\"Soyisim\",\"Telefon\",\"Adres\" from \"Kisi\" where \"KisiTipi\"='P' order by \"KisiNo\" desc";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);


            //DataRow dr = dt.Rows[0];
            //MessageBox.Show(dr.Field<int>("KisiNo").ToString());

            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        public void CBoxListele()
        {
            baglanti.Open();

            string sorgu = "select \"DukkanNo\",\"Ad\" from \"Dukkan\"";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox2.DisplayMember = "Ad"; 
            comboBox2.ValueMember= "DukkanNo"; 
            comboBox2.DataSource = dt;


            string sorgu2= "select \"UcretNo\",\"Maas\" from \"Ucret\"";
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataTable dt2=new DataTable();
            
            da2.Fill(dt2);
            
            comboBox1.DisplayMember = "Maas";
            comboBox1.ValueMember = "UcretNo";
            comboBox1.DataSource = dt2;
            
            baglanti.Close();
        }

        private void FPersonel_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            button1.Enabled = false;
            Listele();
            CBoxListele();
        }

        private void button1_Click(object sender, EventArgs e) //Ekle
        {
            Listele();
            button1.Enabled = false;
            button2.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;


            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e) //sil 
        {
            button2.Enabled = false;
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;

        }
        private void button3_Click(object sender, EventArgs e) //Ara
        {
            
            button3.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button6.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;

            

        }
        private void button6_Click(object sender, EventArgs e) // güncelle
        {
            button6.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
        }
        private void button5_Click(object sender, EventArgs e) //Uygula
        {  
            if(baglanti.State==ConnectionState.Open)
            {
                baglanti.Close();  
            }
            string mesaj="";

            if (button1.Enabled==false)  // ekle
            {
                
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "insert into \"Kisi\" (\"Isim\",\"Soyisim\",\"TCNo\",\"Telefon\",\"Adres\",\"KisiTipi\")" +
                                    "Values(@p1,@p2,@p3,@p4,@p5,\'P\');" +
                                    "insert into \"Personel\" (\"DukkanNo\",\"UcretNo\")" +
                                    " Values(@p6,@p7);";


                string isim=textBox1.Text;
                string soyisim = textBox3.Text;
                string TCNo=textBox2.Text;
                string telefon=textBox4.Text;
                string adres=textBox5.Text;
                int DukkanNo = int.Parse(comboBox2.SelectedValue.ToString());
                int UcretNo = int.Parse(comboBox1.SelectedValue.ToString());
                komut.Parameters.AddWithValue("@p1", isim);
                komut.Parameters.AddWithValue("@p2", soyisim);
                komut.Parameters.AddWithValue("@p3", TCNo);
                komut.Parameters.AddWithValue("@p4", telefon);
                komut.Parameters.AddWithValue("@p5", adres);
                komut.Parameters.AddWithValue("@p6", DukkanNo);
                komut.Parameters.AddWithValue("@p7", UcretNo);
                komut.ExecuteNonQuery();

                mesaj = "Ekleme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if(button2.Enabled == false) //sil
            {
                baglanti.Open();
                NpgsqlCommand komut= new NpgsqlCommand();
                komut.Connection = baglanti;

                int kisino = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

                komut.CommandText = "delete from \"Kisi\" where \"KisiNo\"=" + kisino;


                //komut.CommandText = "delete from \"Personel\" where \"KisiNo\"=" +
                //    "(select \"KisiNo\" from \"Kisi\" where \"TCNo\"=\'" + textBox2.Text + "\');"+
                //    "delete from \"Kisi\" where \"TCNo\"= \'"+textBox2.Text+"\';";

                mesaj = "Silme islemi tamamlandi";
                MessageBox.Show(mesaj);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Listele();
                
            }
            else if(button3.Enabled == false)// ara
            {
                baglanti.Open();
                string sorgu = "select \"KisiNo\",\"TCNo\",\"Isim\",\"Soyisim\",\"Telefon\",\"Adres\" from \"Kisi\" where \"KisiTipi\"='P' and \"TCNo\"= \'" + textBox2.Text+"\'";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                baglanti.Close();
            }
            else if(button6.Enabled ==false)  // güncelle
            {
                baglanti.Open();
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int kisino = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    NpgsqlCommand komut= new NpgsqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update \"Kisi\" set \"TCNo\"=\'" + textBox2.Text + "\',\"Isim\"=\'" + textBox1.Text + "\',\"Soyisim\"=\'" + textBox3.Text + "\',\"Telefon\"=\'" + textBox4.Text + "\',\"Adres\"=\'" + textBox5.Text + "\' where \"KisiNo\"="+kisino.ToString();


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

        private void button4_Click(object sender, EventArgs e)
        {
            Listele();
        }
    }
}
