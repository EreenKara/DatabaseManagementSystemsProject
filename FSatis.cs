using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeriTabaniUygulamasi
{
    public partial class FSatis : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Dukkan; userID=postgres;" +
            " password=123321a;");
        public FSatis()
        {
            InitializeComponent();
        }
        public void Listele()
        {
            baglanti.Open();
            string sorgu = "select \"Satis\".\"GelirNo\",\"Tutar\",\"Tarih\" from \"Satis\" inner join \"Gelir\" on \"Gelir\".\"GelirNo\"=\"Satis\".\"GelirNo\"";
            string sorgu2 = "select \"Satis\".\"GelirNo\",\"Tutar\",string_agg(\"UrunModel\", ', ' ORDER BY \"UrunModel\")as \"Urunler\" ,\"Tarih\"\r\nfrom \"Satis-Urun\" inner join \"Satis\" on \"Satis\".\"GelirNo\"=\"Satis-Urun\".\"GelirNo\" inner join \"Urun\" on \"Urun\".\"UrunNo\"=\"Satis-Urun\".\"UrunNo\"inner join \"Gelir\" on \"Gelir\".\"GelirNo\"=\"Satis-Urun\".\"GelirNo\"  \r\nGROUP BY \"Satis-Urun\".\"GelirNo\" ,\"Tutar\",\"Satis\".\"GelirNo\",\"Tarih\";";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        public void CBoxListele()
        {
            baglanti.Open();

            string sorgu = "select \"DukkanNo\",\"Ad\" from \"Dukkan\";";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "Ad";
            comboBox1.ValueMember = "DukkanNo";
            comboBox1.DataSource = dt;

            string sorgu2 = "select \"KisiNo\",(select get_data_kisi(\"KisiNo\")) as \"Musteri\" from \"Musteri\";";
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            comboBox2.DisplayMember = "Musteri";
            comboBox2.ValueMember = "KisiNo";
            comboBox2.DataSource = dt2;
            baglanti.Close();
        }

        public void CheckedListBoxListele()
        {
            checkedListBox1.Items.Clear();
            baglanti.Open();
            string sorgu = "select \"UrunNo\",\"UrunModel\" from \"Urun\" order by \"UrunNo\" asc;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();

            da.Fill(dt);
            string satir = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                satir = "";
                if (dt.Rows[i][0].ToString() != "" || dt.Rows[i][1].ToString() != "")
                {
                    satir = dt.Rows[i][0].ToString() + ". " + dt.Rows[i][1].ToString();
                    checkedListBox1.Items.Add(satir);
                }
            }
            baglanti.Close();
        }
        private void FSatis_Load(object sender, EventArgs e)
        {
            button9.Enabled = false;
            Listele();
            CBoxListele();
            CheckedListBoxListele();
        }

        private void button9_Click(object sender, EventArgs e) //ekle
        {
            button9.Enabled = false;
            button8.Enabled = true;
            button7.Enabled = true;
            button1.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e) // sil
        {
            button8.Enabled = false;
            button9.Enabled = true;
            button7.Enabled = true;
            button1.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e) //ara
        {
            button7.Enabled = false;
            button8.Enabled = true;
            button9.Enabled = true;
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)  // güncelle
        {
            button1.Enabled = false;
            button8.Enabled = true;
            button7.Enabled = true;
            button9.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e) // uygula
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            if(button9.Enabled==false) // ekle
            {
                baglanti.Open();

                int uzunluk = checkedListBox1.CheckedItems.Count;
                int[] urunnolar = new int[uzunluk];
                for (int i = 0; i < uzunluk; i++)
                {
                    string urun = checkedListBox1.CheckedItems[i].ToString();
                    urunnolar[i] = int.Parse(urun.Substring(0, urun.IndexOf('.')));
                }


                DateTime tarih = dateTimePicker1.Value;
                NpgsqlCommand komut = new NpgsqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "insert into \"Gelir\" (\"Tarih\",\"GelirTipi\")" +
                                    "Values (\'" + tarih + "\','S');"+ 
                                    "insert into \"Satis\" (\"DukkanNo\",\"KisiNo\")" +
                                    "Values(@p1,@p2);";


                int dukkanno = int.Parse(comboBox1.SelectedValue.ToString());
                int musterino = int.Parse(comboBox2.SelectedValue.ToString());
                int tutar = int.Parse(numericUpDown1.Value.ToString());
                int adet = int.Parse(numericUpDown2.Value.ToString());


                komut.Parameters.AddWithValue("@p1", dukkanno);
                komut.Parameters.AddWithValue("@p2", musterino);


                NpgsqlCommand komut2 = new NpgsqlCommand();
                komut2.Connection = baglanti;
                bool varmi=true;
                for (int i = 0; i < uzunluk; i++)
                {
                    komut2.CommandText = "select stoktavarmi('" + urunnolar[i] +"','"+adet+"')";
                    varmi = Convert.ToBoolean(komut2.ExecuteScalar());
                    if(!varmi)
                    {
                        break;
                    }
                }
                
                    
                if (varmi)
                {
                    komut.ExecuteNonQuery();

                    for (int i = 0; i < uzunluk; i++)
                    {
                        komut.CommandText = "insert into \"Satis-Urun\" (\"UrunNo\" , \"UrunAdedi\",\"BirimFiyat\") values (" + urunnolar[i] + ",@p3,@p4) ";
                        komut.Parameters.AddWithValue("@p3", adet);
                        komut.Parameters.AddWithValue("@p4", tutar);
                        komut.ExecuteNonQuery();
                    }


                    string mesaj = "Ekleme islemi tamamlandi";
                    MessageBox.Show(mesaj);
                    
                }
                else
                {
                    MessageBox.Show("İstenilen ürünler stokta yok");
                }
                baglanti.Close();
                Listele();

            }
            else if(button8.Enabled == false)  // sil
            {

            }
            else if(button7.Enabled == false)  // ara
            {

            }
            else if(button1.Enabled == false) // güncelle
            {

            }



        }

        private void button6_Click(object sender, EventArgs e)  //arama sonuçlarını sıfırtla
        { 
            Listele();
        }
    }
}
