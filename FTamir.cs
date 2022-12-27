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
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace VeriTabaniUygulamasi
{
    public partial class FTamir : Form
    {

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");
        public FTamir()
        {
            InitializeComponent();
        }
        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select \"Tamir\".\"GelirNo\",\"Plaka\",\"AracGirisTarihi\",\"AracCikisTarihi\",\"Tutar\", \"NeYapildigi\" from \"Tamir\"  inner join \"Gelir\" on \"Tamir\".\"GelirNo\"=\"Gelir\".\"GelirNo\" inner join \"Arac\" on \"Arac\".\"AracNo\"=\"Tamir\".\"AracNo\" order by \"Tamir\".\"GelirNo\" desc";                   
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();   
        }
        
        public void CBoxListele()
        {
            baglanti.Open();

            string sorgu = "select \"AracNo\",\"Plaka\" from \"Arac\";";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "Plaka";
            comboBox1.ValueMember = "AracNo";
            comboBox1.DataSource = dt;

            baglanti.Close();
        }
        
        public void CheckedListBoxListele()
        {
            checkedListBox1.Items.Clear();
            baglanti.Open();
            string sorgu = "select \"KisiNo\",\"Isim\",\"Soyisim\" from \"Kisi\" where \"KisiTipi\"=\'P\' order by \"KisiNo\" desc;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();

            da.Fill(dt);
            string satir="";
            for (int i = 0; i< dt.Rows.Count; i++)
            {
                satir = "";
                if (dt.Rows[i][0].ToString() != "" || dt.Rows[i][1].ToString() != "")
                {
                    satir = dt.Rows[i][0].ToString()+". "+dt.Rows[i][1].ToString() + " " + dt.Rows[i][2].ToString();
                    checkedListBox1.Items.Add(satir);
                }  
            }
            baglanti.Close();
        }
        public void datecekme()
        {
            string tarih=dateTimePicker1.Value.ToShortDateString();
            DateTime date = dateTimePicker1.Value;
            
        }
        private void FTamir_Load(object sender, EventArgs e)
        {
            button9.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;
            button1.Enabled = true;
            CBoxListele();
            Listele();
            CheckedListBoxListele();
            datecekme();
        }
        private void button9_Click(object sender, EventArgs e) // ekle
        {
            button9.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;
            button1.Enabled = true;


            richTextBox1.Text = "";
            richTextBox1.Enabled=true;
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled= true;
            dateTimePicker2.Enabled= true;
            checkedListBox1.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e) // sil
        {
            button8.Enabled = false;
            button7.Enabled = true;
            button9.Enabled = true;
            button1.Enabled = true;

            richTextBox1.Text = "";
            richTextBox1.Enabled = false;
            comboBox1.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            checkedListBox1.Enabled = false;
        }
        private void button7_Click(object sender, EventArgs e) // ara
        {
            button7.Enabled = false;
            button9.Enabled = true;
            button8.Enabled = true;
            button1.Enabled = true;

            richTextBox1.Text = "";
            richTextBox1.Enabled = false;
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            checkedListBox1.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)  // güncelle
        {
            button1.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;

            richTextBox1.Text = "";
            richTextBox1.Enabled = true;
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            checkedListBox1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e) // uygula
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            if (button9.Enabled == false) // ekle
            {
                baglanti.Open();

                int uzunluk = checkedListBox1.CheckedItems.Count;
                int[] kisinolar = new int[uzunluk];
                for (int i = 0; i < uzunluk; i++)
                {
                    string kisi = checkedListBox1.CheckedItems[i].ToString();
                    kisinolar[i] = int.Parse(kisi.Substring(0,kisi.IndexOf('.')));
                }



                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                DateTime giris = dateTimePicker1.Value;
                DateTime cikis = dateTimePicker2.Value;
                komut.CommandText = "insert into \"Gelir\" (\"Tutar\",\"Tarih\",\"GelirTipi\")"  +
                                    "Values (@p5,\'"+cikis+"\','T');"+
                                    "insert into \"Tamir\" (\"AracNo\",\"AracGirisTarihi\",\"AracCikisTarihi\",\"NeYapildigi\")" +
                                    "Values(@p1,\'"+giris+"\',\'"+cikis+"\',@p4);";
                
                
                int aracno = int.Parse(comboBox1.SelectedValue.ToString());
                string neyapildigi = richTextBox1.Text;
                int tutar = int.Parse(numericUpDown1.Value.ToString());
                
                
                komut.Parameters.AddWithValue("@p1", aracno);
                komut.Parameters.AddWithValue("@p4", neyapildigi);
                komut.Parameters.AddWithValue("@p5", tutar);

                komut.ExecuteNonQuery();

                for (int i = 0; i < uzunluk; i++)
                {
                    komut.CommandText = "insert into \"Tamir-Personel\" (\"KisiNo\") values (" + kisinolar[i] + ") ";
                    komut.ExecuteNonQuery();
                }


                string mesaj = "Ekleme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if(button8.Enabled==false) // sil
            {
                baglanti.Open();
                int tamirno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "delete from \"Gelir\" where \"GelirNo\"=" + tamirno + ";";
                komut.ExecuteNonQuery();
                string mesaj = "Silme islemi tamamlandi";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();
            }
            else if(button7.Enabled==false) // ara
            {
                baglanti.Open();
                int aracno = int.Parse(comboBox1.SelectedValue.ToString());
                string sorgu= "select \"Tamir\".\"GelirNo\",\"Plaka\",\"AracGirisTarihi\",\"AracCikisTarihi\",\"Tutar\",\"NeYapildigi\" from \"Tamir\"  inner join \"Gelir\" on \"Tamir\".\"GelirNo\"=\"Gelir\".\"GelirNo\" inner join \"Arac\" on \"Arac\".\"AracNo\"=\"Tamir\".\"AracNo\" where \"Tamir\".\"AracNo\"=\'"+aracno+"\' order by \"Tamir\".\"GelirNo\" desc; ";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                baglanti.Close();

            }
            else if(button1.Enabled==false) // güncelle
            {
                baglanti.Open();
                int tamirno = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                int aracno = int.Parse(comboBox1.SelectedValue.ToString());
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                DateTime giris = dateTimePicker1.Value;
                DateTime cikis = dateTimePicker2.Value;
                string neyapildigi = richTextBox1.Text;
                int tutar = int.Parse(numericUpDown1.Value.ToString());

                komut.CommandText = "update \"Tamir\" set   \"AracGirisTarihi\"= \'"+giris+"\' ,\"AracCikisTarihi\"=\'"+cikis+"\',\"AracNo\" ="+aracno+", \"NeYapildigi\"=\'"+neyapildigi+"\' where \"GelirNo\"=" + tamirno + ";";

                komut.ExecuteNonQuery();
                komut.CommandText = "Update \"Gelir\" set \"Tutar\"="+tutar+" where \"GelirNo\"="+tamirno+"; ";

                komut.ExecuteNonQuery();

                int uzunluk = checkedListBox1.CheckedItems.Count;
                int[] kisinolar = new int[uzunluk];
                for (int i = 0; i < uzunluk; i++)
                {
                    string kisi = checkedListBox1.CheckedItems[i].ToString();
                    kisinolar[i] = int.Parse(kisi.Substring(0, kisi.IndexOf('.')));
                }

                komut.CommandText = "delete from \"Tamir-Personel\" where \"GelirNo\"=" + tamirno + ";";
                komut.ExecuteNonQuery();
                for (int i = 0; i < uzunluk; i++)
                {

                    komut.CommandText = "insert into \"Tamir-Personel\" (\"GelirNo\",\"KisiNo\") values(@p1,@p2);   ";
                    komut.Parameters.AddWithValue("@p1", tamirno);
                    komut.Parameters.AddWithValue("@p2", kisinolar[i]);
                    komut.ExecuteNonQuery();
                }

                string mesaj = "Güncelleme işlemi gerçekleşti";
                MessageBox.Show(mesaj);
                baglanti.Close();
                Listele();

            }

        }

        private void button6_Click(object sender, EventArgs e) // arama sonuclarini sifirla
        {
            Listele();
        }

        
    }
}
