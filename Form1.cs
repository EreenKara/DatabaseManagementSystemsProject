using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace VeriTabaniUygulamasi
{

    
    public partial class Form1 : Form
    {
        FPersonel fpersonel = new FPersonel();
        FMusteri fmusteri= new FMusteri();
        FArac farac=new FArac();
        FTamir ftamir=new FTamir();
        FTedarikci ftedarikci=new FTedarikci(); 
        FUrun furun=new FUrun();
        FSiparis fsiparis = new FSiparis();
        FSatis fsatis= new FSatis();    
        FUcret fucret=new FUcret();
        FKarzarar fkarzarar=new FKarzarar();
        FGelir fgelir= new FGelir();
        FGider fgider=new FGider();
        FFatura ffatura=new FFatura();


        public Form1()
        {
            InitializeComponent();
            
            
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fpersonel.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fmusteri.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            farac.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ftamir.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ftedarikci.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            furun.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            fsiparis.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fsatis.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            fucret.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e) // Karzarar
        {
            fkarzarar.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e) // Gelirler
        {
            fgelir.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            fgider.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ffatura.ShowDialog();
        }
    }
}
