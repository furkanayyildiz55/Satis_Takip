using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//SQL KOMUTLARI ÖZELLİKLERİ VS. KULLANABİLMEK İÇİN SINIFIMIZ

namespace Satıs_Takip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // İSTENİLEN DATABASE E ULAŞIM İÇİN SQL BAĞLANTISI
        SqlConnection baglantı = new SqlConnection(@"Data Source=DESKTOP-14R0RVU\SQLEXPRESS;Initial Catalog=Db_Satis_Takip;Integrated Security=True");

        
        
        private void PcbFis_Click(object sender, EventArgs e)//PİCTUREBOX IN CİLCK EVENYTİ ÜSTÜNE TIKLANDIĞINDA YAPILACAK KODLARI ÇALIŞTIRIR
        {
           
            frmFis fr = new frmFis();  
            fr.giris = false;  //fr nesnesindeki giris değişkenine false değer atanıyor (burada amaç hangi yetkide kullanıcının giriş yaptığını diğer forma aktarmak)
            this.Hide();  //bu formu gizliyoruz
            fr.Show();   //diğer formu açıoruz
            
        }
        
        private void PcbYetkili_Click(object sender, EventArgs e) //YETKİLİ GİRİŞ PANELİ GÖRÜNÜR OLACAK
        {
           
            groupBox1.Visible = true;   //groupbox1 i görünür yapar
            PcbYetkili.Visible = false; //PcbYetkili nin görünürlüğünü kapatır
            timer1.Start(); //  groupbox1 i belirli zaman sonra kapatmak için timer ı başlatıyoruz
        }
       
        private void timer1_Tick(object sender, EventArgs e)// //TİMER BAŞLADIĞINDA BELİRLİ BİR ZAMAN SONRA YETKİLİ GİRİŞ GRUPBOX I KAPANACAK
        {
            groupBox1.Visible = false;  
            PcbYetkili.Visible = true;
            errorProvider1.Clear();   //ekranda hata uyarısı varsa temizlenecek
            timer1.Stop();   //timer sürekli çalımaması için kapatıyoruz

        }

        private void button1_Click(object sender, EventArgs e)//K.A VE ŞİFREYİ KONTROL EDİP DOĞRU İSE ERİŞİM VERRECEK
        {
            baglantı.Open();
            SqlCommand kasorgu = new SqlCommand("select * from tbl_yetkili where yetkiliKA=@p1 and yetkiliSifre=@p2",baglantı);
            kasorgu.Parameters.AddWithValue("@p1",textBox1.Text);
            kasorgu.Parameters.AddWithValue("@p2", maskedTextBox1.Text);

            SqlDataReader dr = kasorgu.ExecuteReader();
            if (dr.Read())  //okumadoğru gerçekleşirse if bloğu çalışacak
            {
                frmFis fr = new frmFis();
                fr.giris = true;  //fr nesnesindeki giris değişkenine true değer atanıyor (burada amaç hangi yetkide kullanıcının giriş yaptığını diğer forma aktarmak)
                this.Hide();
                fr.Show();
            }
            else
            {
                //YANLIŞ K.A. VEYA ŞİFRE GİRİLDİĞİNDE ERROR PROVİDER ÇALIŞACAK VE ÖZELLİKLERDEN EKLEDİĞİM İCO İLE YANIP SÖNECEK
                //ÜSTÜNE GELİNDİĞİNDE HATA MESAJINI GÖSTERECEK
                errorProvider1.SetError(button1, "Kullanıcı Adı Veya Şifre Yanlış");
                errorProvider1.BlinkRate = 250;
                
            }
            baglantı.Close();

        }

        
        
       

        private void çkışToolStripMenuItem_Click(object sender, EventArgs e)  //Tıklandğında uygulama kapansın
        {
            Application.Exit();    //Uygulamayı kapatır
            notifyIcon1.Text = "Satış Takip";  //NotifyIcon üzerine gelindiğinde tooltip mesajını belirtir.
            notifyIcon1.BalloonTipTitle = "Uyarı!";  //Balonunun başlığını belirtir.
            notifyIcon1.BalloonTipText = "Uygulama Kapatıldı";  // Balonun üzerinde bulunan mesajı belirtir.
            notifyIcon1.ShowBalloonTip(4000);   //Balonun ekranda gösterim süresini milisaniye cinsinden belirtir.
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;    //Balonun tipini belirtir.
         

        }

        private void küçültToolStripMenuItem_Click(object sender, EventArgs e) //Tıklandığında uygulamayı küçültsün
        {
            if (FormWindowState.Minimized!=WindowState)  //formun pencere konumu küçültülmüş bir şekilde değilse ilk kod bloğunu çalıştırır
            {
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;    //notifyIcon un görünürlüğünü açar
                notifyIcon1.Text = "Satış Takip";   //NotifyIcon üzerine gelindiğinde tooltip mesajını belirtir.
                notifyIcon1.BalloonTipTitle = "Uyarı!";    // Balonunun başlığını belirtir.
                notifyIcon1.BalloonTipText = "Program sağ alt köşede konumlandı.";    // Balonun üzerinde bulunan mesajı belirtir.
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;       //Balonun tipini belirtir.
                notifyIcon1.ShowBalloonTip(30000);     //Balonun ekranda gösterim süresini milisaniye cinsinden belirtir.
            }
        } 

        private void gösterToolStripMenuItem_Click(object sender, EventArgs e)  //  eğer uygulama gözükmüyosa ekrana getirilsin
        {
          
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)  //  herhangi bir  BallonTip e tıklandığında uygulama gözükmüyorsa ekrana getirilsin
        {
            
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip1.ToolTipIcon = ToolTipIcon.Info;   //ToolTip ipucunun başlığını belirttiğimiz yer
            toolTip1.ToolTipTitle = "Bilgi"; //TooTip Çerçecesi içersinde çıkacak iconu seçiyoruz
            toolTip1.IsBalloon = true;  //IsBalloon : True yaparsak baloncuk şeklinde uyarı verir. False olduğunde ise çerçeveyi kare şeklinde gösterir

        }
    }
}
