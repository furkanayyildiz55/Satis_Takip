using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Satıs_Takip
{
    public partial class frmFis : Form
    {
        public frmFis()
        {
            InitializeComponent();
        }
        public bool giris;   //giriş yapan kullanıcının yetki seviyesini tutar
        string Ürün = "", Müşteri = "", Tarih = "", Adet = "", Fiyat = "", GenelToplam = ""; 
        SqlConnection baglantı = new SqlConnection(@"Data Source=DESKTOP-14R0RVU\SQLEXPRESS;Initial Catalog=Db_Satis_Takip;Integrated Security=True");//SQL bağlantısı


        //BURADA DGW VEYA DGV DİYE KISALTILMIŞ YERLER DataGRidView İ TEMSİL ETMEKDEDİR


        //LOOOAADDD
        private void frmFis_Load(object sender, EventArgs e)
        {
            
            if (giris == false)   //giris değişkeni ilk formdan false değer getirmisse yetkisiz biri giriş yaptı demektir ve ilgili sekmeler kapatılır
            {
                TcSatıslarr.TabPages.Remove(TbUrun);   //gerekli tabpages ler kapatılır
                TcSatıslarr.TabPages.Remove(TbUrunİslem);
                TcSatıslarr.TabPages.Remove(TbMusteri);
                TcSatıslarr.TabPages.Remove(TbMusteriİslem);
                TcSatıslarr.TabPages.Remove(TbYetkiliİslem);
                TcSatıslarr.TabPages.Remove(TpSatıslar);

                ürünlerToolStripMenuItem.Visible = false;  //Yetkisiz girişte ilgili sekmeleri kapatır
                ürünİşlemlerToolStripMenuItem.Visible = false;
                müşterilerToolStripMenuItem.Visible = false;
                müşteriİşlemToolStripMenuItem.Visible = false;
                yetkiliİşlemToolStripMenuItem.Visible = false;
                satışlarToolStripMenuItem.Visible = false;

            }
             ////111111/////
            listView1.Columns.Add("Müşteri", 150);  //Listview e Sütunlar eklendi  listView1.Columns.Add("SÜTUN ADI",SÜTUN BOYUTU)
            listView1.Columns.Add("Tarih", 155);
            listView1.Columns.Add("Ürün", 150);
            listView1.Columns.Add("Adet", 50);
            listView1.Columns.Add("Fiyat", 50);
            listView1.Columns.Add("G.Toplam", 100);
            ////1111111////
            
            DGW3();  //3. 5. ve 6. sekmedeki  DataGridView ler dolddurulur
            DGW5();
            DGW6();
          

            //DateTİmePicker ın Mindate özelliği Sistem tarihi olarak ayarlandı (Böylelikle geçmiş tarihler seçilemeyecek)
            dateTimePicker1.MinDate = DateTime.Now;



            //TabControl sekmelerini gizler
            TcSatıslarr.Appearance = TabAppearance.FlatButtons;
            TcSatıslarr.ItemSize = new Size(0, 1);
            TcSatıslarr.SizeMode = TabSizeMode.Fixed;

            foreach (TabPage tab in TcSatıslarr.TabPages)
            {
                tab.Text = "";
            }


        }

        //BU PROJEDE FORMDA BULUNAN HER BİR SEKMENİN İŞLERİNİ YAPAN KOD BLOKLARI AYNI YERLERE TOPLANMIŞ YORUM SATIRLARI İLE BELİRTİLMİŞTİR  

        //SOL TARAFATA BULUNAN KOD BLOKLARINI KÜÇÜLTME/BÜYÜLTME İŞİNE YARAYAN ARAÇTAN BÜTÜN BLOKLAR KÜÇÜLTÜLÜRSE PROJE YAPISI VE KODLARI DAHA İYİ ANLAŞILABİLİR KARMAŞADAN KURTULUNABİLİR

        //FURKAN AYYILDIZ 


        //1111111111-//
        private void Btn1FisOlustur_Click(object sender, EventArgs e)
        {

            try
            {

                for (int i = 0; i < listView1.Items.Count; i++) //listiew in satır sayısı kadar işlem yapar
                {

                    string Musteri = listView1.Items[i].SubItems[0].Text; //listview deki i. satırın sütun değerleri tek tek değişkenlere aktarılır
                    string Tarih = listView1.Items[i].SubItems[1].Text;
                    string Urun = listView1.Items[i].SubItems[2].Text;
                    string Adet = listView1.Items[i].SubItems[3].Text;
                    string Fiyat = listView1.Items[i].SubItems[4].Text;
                    string Gtoplam = listView1.Items[i].SubItems[5].Text;

                    SqlCommand FisOlustur = new SqlCommand("insert into tbl_satıs (Musteri,Tarih,Urun,Adet,Fiyat,Gtoplam) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglantı); //tbl_satıs tablosuna ekleme yapar
                    FisOlustur.Parameters.AddWithValue("@p1", Musteri); //parametreler ilgli değişkenden alınır 
                    FisOlustur.Parameters.AddWithValue("@p2", Tarih);
                    FisOlustur.Parameters.AddWithValue("@p3", Urun);
                    FisOlustur.Parameters.AddWithValue("@p4", Adet);
                    FisOlustur.Parameters.AddWithValue("@p5", Fiyat);
                    FisOlustur.Parameters.AddWithValue("@p6", Gtoplam);
                    baglantı.Open();
                    FisOlustur.ExecuteNonQuery();
                    baglantı.Close();
                   

                }
                MessageBox.Show("Fiş Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listView1.Items.Clear();   //listView1 temizlenir
            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Gerçekleştirilemedi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (baglantı.State == ConnectionState.Open)//sql bağlantısı hata sırasında açık kalmışsa kapatır
                {
                    baglantı.Close();
                }

            } 
        }

        private void Btn1SeciliyiSil_Click(object sender, EventArgs e)//listviewdeki seçilen satırı siler
        {
            try
            {
                listView1.CheckedItems[0].Remove(); //listviewdeki seçilen satırı siler
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen Bir Satır Seçiniz");
               
            }
           
        }

        private void frmFis_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();  //uygulamayı kapatır
        }

        private void BtnGeri_Click(object sender, EventArgs e)//form1 açılır bulunduğumuz form gizlenir
        {
            Form1 fr = new Form1();   
            fr.Show();  
            this.Hide();

        }
  
        private void BtnListeyeEkle_Click(object sender, EventArgs e)//ListView e item aktarılır
        {
            Müşteri = cmb1Musteri.Text;  //toollardaki text verileri ilgili değişkenlere aktarılır
            Ürün = cmb1Ürün.Text;
            Adet = numericUpDown1.Value.ToString();
            Tarih = dateTimePicker1.Text.ToString() ;
            Fiyat = Txt1ÜrünFiyat.Text;
            try
            {
                GenelToplam = Convert.ToString(Convert.ToInt64(Adet) * Convert.ToInt64(Fiyat));
            }
            catch (Exception)
            {

                MessageBox.Show("Lütfen bütün alanları doldurunuz");
            }
           
            

           
            if (Müşteri!="" && Ürün!="" && Adet!="" && Tarih!="")//değişkenleriç içi dolu ise ilk kod bloğunu çalıştırır
            {

                string[] Bilgiler = { Müşteri, Tarih, Ürün, Adet,Fiyat,GenelToplam  };  //değişkenler ile dizi oluşturulur
                ListViewItem Lst = new ListViewItem(Bilgiler);  //dizi Lst nesnesine aktarılır
                listView1.Items.Add(Lst);   //Lst item olarak listView1 e aktarılır

                numericUpDown1.Value = 1;
                richTextBox1.Text = "";
             //   cmb1Ürün.Text = "";
            //    cmb1Ürün.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Lütfen Bütün Alanları Doldurunuz");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)//dateTimePicker1 in değeri değiştiğinde çalışır
        {
            string Tarih1 = DateTime.Now.ToString("d");  //değişkene (gg.aa.yyyy) formatındaki sistem tarihi aktarılır

            if (Tarih1 != dateTimePicker1.Value.ToString("d"))  //Tarih1 değişkeni ile dateTimePicker1 deki formatlanmış tarihler farklı ise Lbl1TarihUyarı görünür olur
            {
                Lbl1TarihUyarı.Visible = true;//Visible özelliği true ise label görünür false ise label görünmez
            }
            else
            {
                Lbl1TarihUyarı.Visible = false;//Lbl1TarihUyarı görünmez
            }
        }
        
        private void cmb1Musteri_Click(object sender, EventArgs e)//cmb1Musteri ye tıklandığı anda çalışacak kodlar
        {
            try
            {
                cmb1Musteri.Items.Clear(); //her tıklanmada itemler silinir
                SqlCommand CMBmusteriEkle = new SqlCommand("select MusteriAd from tbl_musteri ", baglantı); //tbl_musteri tablosundan  MusteriAd çekilir
                baglantı.Open();
                SqlDataReader dr33 = CMBmusteriEkle.ExecuteReader();
                while (dr33.Read())
                {
                    cmb1Musteri.Items.Add(dr33[0].ToString());//SQL den okuduğu her veriyi add komutu ile combobax a item olarak ekler
                }
                baglantı.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("1. Sayfada Musteri Eklemede Hata");

            }
        }

        private void cmb1Ürün_Click(object sender, EventArgs e)
        {
            try
            {
                if (rd1Koltuk.Checked!=false  || rd1Mobilya.Checked!=false) 
                {
                    cmb1Ürün.Items.Clear();
                    SqlCommand CMBurunEkle = new SqlCommand("select urunad from tbl_urun where uruntur=@p1 and UrunAdet>0  ", baglantı);
                    if (rd1Koltuk.Checked == true)//radıoButton un cheked özelliği seçilip seçilmediğini kontrol eder (True ise seçili demektir)
                    {
                        CMBurunEkle.Parameters.AddWithValue("@p1", "False"); //Seçili olan radioButton a göre parametre gönderimi
                    }
                    else
                    {
                        CMBurunEkle.Parameters.AddWithValue("@p1", "True");
                    }

                    baglantı.Open();
                    SqlDataReader dr11 = CMBurunEkle.ExecuteReader();
                    while (dr11.Read())
                    {
                        cmb1Ürün.Items.Add(dr11[0].ToString());
                    }
                    baglantı.Close();
                }
                else
                {
                    errorProvider1.BlinkStyle = 0;//Erorro provider in yanıp sönme hızı  (0 ise yanıp sönmez)
                    errorProvider1.SetError(rd1Koltuk, "Bu Alan Seçilmelidir");   
                    errorProvider1.SetError(rd1Mobilya, "Bu Alan Seçilmelidir");
                    
                }     
            }
            catch (Exception)
            {
                MessageBox.Show("Ürün Eklemede Hata!", "HATA!");
            }
        }

        private void cmb1Ürün_TextChanged(object sender, EventArgs e)//Cmb1Ürün ün text i değiştiğinde çalıçacak kodlar
        {
            try
            {
                if (cmb1Ürün.Text!="")
                {
                    SqlCommand UrunFiyatCek = new SqlCommand("select urunfiyat from Tbl_Urun where UrunAd=@p1 ", baglantı);  
                    UrunFiyatCek.Parameters.AddWithValue("@p1", cmb1Ürün.Text);
                    baglantı.Open();
                    SqlDataReader FiyatOku = UrunFiyatCek.ExecuteReader();
                    while (FiyatOku.Read())
                    {
                        Txt1ÜrünFiyat.Text = FiyatOku[0].ToString();
                    }
                    baglantı.Close();
                }
                else
                {
                    MessageBox.Show("Ürün Seçiniz");
                    Txt1ÜrünFiyat.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ürün Fiyat Çekmede Hata");
                
            }






        }

        private void rd1Mobilya_CheckedChanged(object sender, EventArgs e)//RadioButton un check özelliği değiştiğinde çalışacak kondlar
        {
            errorProvider1.Clear(); //Uyarı mesajını ekrandan kaldırmak için
        }

        private void rd1Koltuk_CheckedChanged(object sender, EventArgs e)//RadioButton un check özelliği değiştiğinde çalışacak kondlar
        {
            errorProvider1.Clear();//Errror provider Uyarı mesajını ekrandan kaldırmak için
        }
        //111111111//

        
        //222222-//
        private void btn2Goster_Click(object sender, EventArgs e)//Butona tıklandığı anda çalışacak kodlar
        {
            try
            {
                if (chk2Koltuk2.Checked == true && chk2Mobilya2.Checked == true)//checkBox ların ın seçili olup olmadığını kontrol ediyoruz 
                {
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter da2 = new SqlDataAdapter("select * from tbl_Urun", baglantı); //iki CheckBox da seçili olduğundan bütün ürünler DGV ye çekiliypr
                    da2.Fill(dt2);
                    dataGridView2.DataSource = dt2;
                }
                else if (chk2Mobilya2.Checked == true && chk2Koltuk2.Checked == false)
                {
                    DataTable dt22 = new DataTable();
                    SqlDataAdapter da22 = new SqlDataAdapter("select * from tbl_Urun where uruntur='True'", baglantı); //satede chk2Mobilya2 seçildiği için Mobilya kategorisindeki ürünler çekiliyor
                    da22.Fill(dt22);
                    dataGridView2.DataSource = dt22;

                }
                else if (chk2Koltuk2.Checked == true && chk2Mobilya2.Checked == false)
                {
                    DataTable dt222 = new DataTable();
                    SqlDataAdapter da222 = new SqlDataAdapter("select * from tbl_Urun where uruntur='False'", baglantı); //satede chk2Koltuk2  seçildiği için koltuk kategorisindeki ürünler çekiliyor
                    da222.Fill(dt222);
                    dataGridView2.DataSource = dt222;
                }
                else
                {                                               //Hiçbir checkBox seçilmediği için hata mesajları çıkıyor
                    errorProvider1.BlinkRate = 0;   
                    errorProvider1.SetError(chk2Koltuk2, "En az biri seçilmelidir");  
                    errorProvider1.SetError(chk2Mobilya2, "En az biri seçilmelidir");
                    errorProvider1.BlinkRate = 400;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("2. Sayfada Ürün Çekmede Hata");
            }
        }

        private void chk2Koltuk2_CheckedChanged(object sender, EventArgs e)//ChechkBox değiştiğinde çalışacak kodlar
        {
            errorProvider1.Clear();   //checkbox seçilirse hatalar kalkıyor   
        }

        private void chk2Mobilya2_CheckedChanged(object sender, EventArgs e)//ChechkBox değiştiğinde çalışacak kodlar
        {
            errorProvider1.Clear();      //checkbox seçilirse hatalar kalkıyor   
        }
        //222222//


        //3333333-//
        private void Btn3UrunOlustur_Click(object sender, EventArgs e)//Btn3UrunOlustur butonuna tıklandığında çalışacak kodlar
        {
            try
            {
                if (Txt3ürünAd.Text != "" && Txt3ÜrünAdet.Text != "" && Txt3ÜrünFiyat.Text != "" && rd3Koltuk.Checked != false || rd3Mobilya.Checked != false)
                {
                    SqlCommand UrunOlustur = new SqlCommand("insert into Tbl_Urun (UrunAd,UrunAdet,UrunFiyat,UrunTur) values (@p1,@p2,@p3,@p4)", baglantı);  //Tbl_Urun tablosuna ekleme yapar
                    UrunOlustur.Parameters.AddWithValue("@p1", Txt3ürünAd.Text);  //parametrelere toollardan textler aktarılır
                    UrunOlustur.Parameters.AddWithValue("@p2", Txt3ÜrünAdet.Text);
                    UrunOlustur.Parameters.AddWithValue("@p3", Txt3ÜrünFiyat.Text);
                    if (rd3Koltuk.Checked == true)
                    {
                        UrunOlustur.Parameters.AddWithValue("@p4", "False");
                    }
                    if (rd3Mobilya.Checked == true)
                    {
                        UrunOlustur.Parameters.AddWithValue("@p4", "True");
                    }

                    baglantı.Open();
                    UrunOlustur.ExecuteNonQuery();
                    baglantı.Close();
                    MessageBox.Show("Ürün Kaydı Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DGW3();
                    Temizle3();
                }
                else
                {
                    errorProvider1.BlinkRate = 0;
                    errorProvider1.SetError(Txt3ürünAd, "Bu Alan Doldurulmalıdır");   
                    errorProvider1.SetError(Txt3ÜrünAdet, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(Txt3ÜrünFiyat, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(rd3Mobilya, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(rd3Koltuk, "Bu Alan Doldurulmalıdır");
                    errorProvider1.BlinkRate = 400;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Gerçekleştirilemedi", "HATA");
            }
        }

        private void Btn3UrunSil_Click(object sender, EventArgs e)//Btn3UrunSil tıklandığında çalışacak kodlar
        {
            try
            {
                if (txt3ÜrünId.Text != "")
                {
                    SqlCommand UrunSil = new SqlCommand("Delete from Tbl_Urun where  UrunID=@p1", baglantı);
                    UrunSil.Parameters.AddWithValue("@p1", txt3ÜrünId.Text);

                    baglantı.Open();
                    UrunSil.ExecuteNonQuery();
                    baglantı.Close();
                    MessageBox.Show("Ürün Kaydı Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DGW3();
                    Temizle3();

                }
                else
                {
                    MessageBox.Show("Lütfen silinecek ürünü seçiniz");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Gerçekleştirilemedi", "HATA");

            }






        }

        private void btn3UrunGuncelle_Click(object sender, EventArgs e)//btn3UrunGuncelle tıklandığında çalışacak kodlar
        {
            try
            {
                if (Txt3ürünAd.Text != "" && Txt3ÜrünAdet.Text != "" && Txt3ÜrünFiyat.Text != "" && (rd3Koltuk.Checked != false || rd3Mobilya.Checked != false))
                {
                    SqlCommand UrunGuncelle = new SqlCommand("update Tbl_Urun set Urunad=@p1,UrunAdet=@p2 ,UrunFiyat=@p3, uruntur=@p4 where urunID=@p5", baglantı); //şarta göre tablodaki veriyi parametreden aldığı degerlerle günceller
                    UrunGuncelle.Parameters.AddWithValue("@p5", txt3ÜrünId.Text);
                    UrunGuncelle.Parameters.AddWithValue("@p1", Txt3ürünAd.Text);
                    UrunGuncelle.Parameters.AddWithValue("@p2", Txt3ÜrünAdet.Text);
                    UrunGuncelle.Parameters.AddWithValue("@p3", Txt3ÜrünFiyat.Text);
                    if (rd3Koltuk.Checked == true)
                    {
                        UrunGuncelle.Parameters.AddWithValue("@p4", "False");
                    }
                    if (rd3Mobilya.Checked == true)
                    {
                        UrunGuncelle.Parameters.AddWithValue("@p4", "True");
                    }

                    baglantı.Open();
                    UrunGuncelle.ExecuteNonQuery();
                    baglantı.Close();
                    MessageBox.Show("Ürün Kaydı Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DGW3();
                    Temizle3();

                }
                else
                {
                    errorProvider1.BlinkRate = 0;
                    errorProvider1.SetError(Txt3ürünAd, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(Txt3ÜrünAdet, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(Txt3ÜrünFiyat, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(rd3Mobilya, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(rd3Koltuk, "Bu Alan Doldurulmalıdır");
                    errorProvider1.BlinkRate = 400;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Gerçekleştirilemedi", "HATA");
            }


        }

        void DGW3()//DataGridView içini doldurmak için oluşturulmuş method
        {
            ///DGW e veritabanından veri cekiyoruz
            DataTable dt3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter("select * from tbl_Urun", baglantı);
            da3.Fill(dt3);
            dataGridView3.DataSource = dt3;
        }

        void Temizle3()//Belirli Toolların textlerini temizlemek için method 
        {
            Txt3ürünAd.Text = "";
            Txt3ÜrünAdet.Text = "";
            Txt3ÜrünFiyat.Text = "";
            txt3ÜrünId.Text = "";
            rd3Koltuk.Checked = false;  //radioButon seçili olsun veya olmasın kod son durumunu "False" olarak ayarlıyor
            rd3Mobilya.Checked = false;
        }

        void DepoDolulukoranı()
        {
            try
            {
                SqlCommand DepoDolulukOranı = new SqlCommand("select sum(UrunAdet) from Tbl_Urun ", baglantı); //sum ile Tbl_Urun tablosundaki UrunAdet verilerinin hepsini topla
                baglantı.Open();
                SqlDataReader dr3 = DepoDolulukOranı.ExecuteReader();

                while (dr3.Read())
                {
                    progressBar1.Value = Convert.ToInt32(dr3[0].ToString());  //toplama işleminden sonra SqlDataReader ile okunan veri progressBar1 e değer olarak atanır
                    Lbl3DepoDolulukOranı.Text = dr3[0].ToString() + "/" + progressBar1.Maximum.ToString();
                }
                baglantı.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Depo İle İlgili Sorun Var", "Hata");
            }


        }

        private void btn3DepoHacim_Click(object sender, EventArgs e)
        {
            if (Txt3DepoHacmi.Text != "" &&  (progressBar1.Value <= Convert.ToInt32(Txt3DepoHacmi.Text)))//Txt3DepoHacmi texti boş değil ve içindeki veri progressBar1 in şimdiki değerinden küçük değilse ilk kod bloğu çalışır
            {
               
                 progressBar1.Maximum = Convert.ToInt32(Txt3DepoHacmi.Text);  //Txt3DepoHacmi text i progressBar1 in en büyük alabileceği değer olarak atanır
                DepoDolulukoranı();
              
            }
            else
            {
                if (Txt3DepoHacmi.Text == "")
                {
                    MessageBox.Show("Lütfen Değer Giriniz");

                }
                else
                {
                    MessageBox.Show("Lütfen Geçerli Bir Değer Giriniz !");

                }
            }

        }
        
        private void button1_Click(object sender, EventArgs e)//DepoHacimGoruntule Butonu tıklandığında çalışacak kodlar
        {
            DepoDolulukoranı();
        }

        private void Txt3ürünAd_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear(); //errorProvider1 temizlenir /hata mesajları kalkar
        }
        private void Txt3ÜrünAdet_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();//errorProvider1 temizlenir /hata mesajları kalkar
        }
        private void Txt3ÜrünFiyat_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear(); //errorProvider1 temizlenir /hata mesajları kalkar
        }
        private void rd3Koltuk_CheckedChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();  //errorProvider1 temizlenir /hata mesajları kalkar

        }
        private void dataGridView3_Click(object sender, EventArgs e)//DGV de tıklanan satırdaki veriler ilgili toolların text lerine yazılır
        {
            int secilen3 = dataGridView3.SelectedCells[0].RowIndex;
            txt3ÜrünId.Text = dataGridView3.Rows[secilen3].Cells[0].Value.ToString();
            Txt3ürünAd.Text = dataGridView3.Rows[secilen3].Cells[1].Value.ToString();
            Txt3ÜrünAdet.Text = dataGridView3.Rows[secilen3].Cells[2].Value.ToString();
            Txt3ÜrünFiyat.Text = dataGridView3.Rows[secilen3].Cells[3].Value.ToString();
            if (dataGridView3.Rows[secilen3].Cells[4].Value.ToString() == "False")
            {
                rd3Koltuk.Checked = true;
            }
            if (dataGridView3.Rows[secilen3].Cells[4].Value.ToString() == "True")
            {
                rd3Mobilya.Checked = true;

            }


        }
        //3333333//


        //444444-//
        private void cmb4Musteri_Click(object sender, EventArgs e)//cmb4Musteri ye tıklandığı anda içerisi veritabanında kayıtlı olan müşteriler ile dolacak
        {
            try
            {
                cmb4Musteri.Items.Clear();
                SqlCommand CMBmusteriEkle = new SqlCommand("select MusteriAd from tbl_musteri ", baglantı);  //müşterileri çeken sql sorgusu
                baglantı.Open();
                SqlDataReader dr33 = CMBmusteriEkle.ExecuteReader();
                while (dr33.Read())
                {
                    cmb4Musteri.Items.Add(dr33[0].ToString());   //her müşteri combobax a item olarak ekleniyor
                }
                baglantı.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("4. Sayfada Musteri Eklemede Hata");

            }

        }

        private void cmb4Musteri_TextChanged(object sender, EventArgs e)//cmb4Musteri nin text i değiştiğinde çalışacak kodlar
        {
            DataTable dt4 = new DataTable();
            SqlDataAdapter da4 = new SqlDataAdapter("select * from Tbl_Satıs where musteri=@p1  ", baglantı); //alınan parametre ile şart koşarak veritabanından ilgili müsterinin yaptığı alışveriş GDV ye dolacak
            da4.SelectCommand.Parameters.AddWithValue("@p1", cmb4Musteri.Text);//parametre combobax ın text ini tutar yani müşterinin adını
            da4.Fill(dt4);
            dataGridView4.DataSource = dt4;

        }
        //44444//


        //55555555-//
        private void Btn5MüşteriOluştur_Click(object sender, EventArgs e)
        {
            if (Txt5MusteriAd.Text!="" && Txt5MusteriTC.Text!="")
            {
                SqlCommand MusteriOlustur = new SqlCommand("insert into tbl_musteri (musteriAd,musteriTC) values (@p1,@p2)", baglantı);
                MusteriOlustur.Parameters.AddWithValue("@p1", Txt5MusteriAd.Text);
                MusteriOlustur.Parameters.AddWithValue("@p2", Txt5MusteriTC.Text);
                baglantı.Open();
                MusteriOlustur.ExecuteNonQuery();
                baglantı.Close();
                MessageBox.Show("Müşteri Kaydı Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle5();
                DGW5();
            }
            else
            {
                errorProvider1.SetError(Txt5MusteriAd, "Bu Alan Doldurulmalıdır");
                errorProvider1.SetError(Txt5MusteriTC, "Bu Alan Doldurulmalıdır");
            }
        }

        private void Btn5MüşteriSil_Click(object sender, EventArgs e)
        {
            if (txt5MusteriID.Text!="")
            {
                SqlCommand MusteriSil = new SqlCommand("delete from tbl_musteri where MusteriID=@p1", baglantı);
                MusteriSil.Parameters.AddWithValue("@p1", txt5MusteriID.Text);
                baglantı.Open();
                MusteriSil.ExecuteNonQuery();
                baglantı.Close();
                MessageBox.Show("Müşteri Kaydı Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle5();
                DGW5();

            }
            else
            {
                MessageBox.Show("Silinecek Yetkiliyi Seçiniz");
            }


        }

        private void dataGridView5_Click(object sender, EventArgs e)//DGV de tıklanan satırdaki veriler ilgili toolların text lerine yazılır
        {
            int secilen5 = dataGridView5.SelectedCells[0].RowIndex;

            txt5MusteriID.Text = dataGridView5.Rows[secilen5].Cells[0].Value.ToString();
            Txt5MusteriAd.Text = dataGridView5.Rows[secilen5].Cells[1].Value.ToString();
            Txt5MusteriTC.Text = dataGridView5.Rows[secilen5].Cells[2].Value.ToString();

        }

        private void Btn5MüşteriGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (Txt5MusteriTC.Text != "" && Txt5MusteriAd.Text != "" && txt5MusteriID.Text!="")
                {
                    SqlCommand MusteriGuncelle = new SqlCommand("update tbl_musteri set musteriad=@p1,musteriTC=@p2 where musteriID=@p3", baglantı);
                    MusteriGuncelle.Parameters.AddWithValue("@p1", Txt5MusteriAd.Text);
                    MusteriGuncelle.Parameters.AddWithValue("@p3", txt5MusteriID.Text);
                    MusteriGuncelle.Parameters.AddWithValue("@p2", Txt5MusteriTC.Text);

                    baglantı.Open();
                    MusteriGuncelle.ExecuteNonQuery();
                    baglantı.Close();
                    MessageBox.Show("Müşteri Kaydı Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DGW5();
                    Temizle5();
                }
                else
                {
                    errorProvider1.SetError(Txt5MusteriAd, "Bu Alan Doldurulmalıdır");
                    errorProvider1.SetError(Txt5MusteriTC, "Bu Alan Doldurulmalıdır");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("İşlem Gerçekleştirilemedi","HATA!");
            }
           
            

        }
      
        private void Txt5MusteriAd_TextChanged(object sender, EventArgs e)//Txt5MusteriAd text değiştiğinde çalışacak kodlar
        {
            errorProvider1.Clear();//errorProvider1 temizlenir /hata mesajları kalkar
        }
        private void Txt5MusteriTC_TextChanged(object sender, EventArgs e)//Txt5MusteriTC text değiştiğinde çalışacak kodlar
        {
            errorProvider1.Clear();//errorProvider1 temizlenir /hata mesajları kalkar
        }

        void DGW5()////DataGridView içini doldurmak için oluşturulmuş method
        {
            DataTable dt5 = new DataTable();
            SqlDataAdapter da5 = new SqlDataAdapter("select * from tbl_Musteri ", baglantı);
            da5.Fill(dt5);
            dataGridView5.DataSource = dt5;
        }

        void Temizle5()
        {
            Txt5MusteriAd.Text = "";
            txt5MusteriID.Text = "";
            Txt5MusteriTC.Text = "";
        }
        //55555555//


        //66666666-//
        void DGW6()//DGW6 ya veritabanından veri cekiyoruz 
        {
            DataTable dt6 = new DataTable();
            SqlDataAdapter da6 = new SqlDataAdapter("select * from tbl_yetkili ", baglantı);
            da6.Fill(dt6);
            dataGridView6.DataSource = dt6;
        }

        private void Btn5YetkiliOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                if (Txt6YetkiliKA.Text != "" && Txt6YetkiliSifre.Text != "")
                {
                    SqlCommand YetkiliOlustur = new SqlCommand("insert into Tbl_yetkili (YetkiliKA,YetkiliSifre) values (@p1,@p2)", baglantı);
                    YetkiliOlustur.Parameters.AddWithValue("@p1", Txt6YetkiliKA.Text);
                    YetkiliOlustur.Parameters.AddWithValue("@p2", Txt6YetkiliSifre.Text);
                    baglantı.Open();
                    YetkiliOlustur.ExecuteNonQuery();
                    baglantı.Close();
                    MessageBox.Show("Yetkili Kaydı Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Temizle6();
                    DGW6();
                }
                else
                {
                    errorProvider1.SetError(Txt6YetkiliKA, "Bu alan boş bırakılamaz");
                    errorProvider1.SetError(Txt6YetkiliSifre, "Bu alan boş bırakılamaz");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("İşlem Gerçekleştirilemedi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void Txt6YetkiliKA_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear(); //errorProvider1 temizlenir /hata mesajları kalkar
        }

        private void Txt6YetkiliSifre_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();//errorProvider1 temizlenir /hata mesajları kalkar

        }

        private void Btn6YetkiliSil_Click(object sender, EventArgs e)
        {
            if (Txt6YetkiliID.Text != "")
            {
                SqlCommand YetkilSil = new SqlCommand("delete from tbl_Yetkili where yetkiliID=@p1", baglantı);
                YetkilSil.Parameters.AddWithValue("@p1", Txt6YetkiliID.Text);
                baglantı.Open();
                YetkilSil.ExecuteNonQuery();
                baglantı.Close();
                MessageBox.Show("Yetkili Kaydı Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle6();
                DGW6();
            }
            else
            {
                MessageBox.Show("Silinecek Yetkiliyi Seçiniz");
            }



        }

        private void dataGridView6_Click(object sender, EventArgs e) //DGW de tek tıklanan satırdaki verileri ilgili toollara aktarıyoruz
        {
            int secilen6 = dataGridView6.SelectedCells[0].RowIndex;
            Txt6YetkiliID.Text = dataGridView6.Rows[secilen6].Cells[0].Value.ToString();
            Txt6YetkiliKA.Text = dataGridView6.Rows[secilen6].Cells[1].Value.ToString();
            Txt6YetkiliSifre.Text = dataGridView6.Rows[secilen6].Cells[2].Value.ToString();

        }

        private void Btn6YetkiliGüncelle_Click(object sender, EventArgs e)
        {
            if (Txt6YetkiliID.Text != "" && Txt6YetkiliKA.Text != "" && Txt6YetkiliSifre.Text != "")
            {
                SqlCommand YetkiliGuncelle = new SqlCommand("update tbl_yetkili set yetkiliKA=@p1 ,YetkiliSifre=@p2 where YetkiliID=@p3", baglantı);
                YetkiliGuncelle.Parameters.AddWithValue("@p1", Txt6YetkiliKA.Text);
                YetkiliGuncelle.Parameters.AddWithValue("@p2", Txt6YetkiliSifre.Text);
                YetkiliGuncelle.Parameters.AddWithValue("@p3", Txt6YetkiliID.Text);
                baglantı.Open();
                YetkiliGuncelle.ExecuteNonQuery();
                baglantı.Close();
                MessageBox.Show("Yetkili Bilgileri Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle6();
                DGW6();
            }
            else
            {
                errorProvider1.SetError(Txt6YetkiliKA, "Bu alan boş bırakılamaz");
                errorProvider1.SetError(Txt6YetkiliSifre, "Bu alan boş bırakılamaz");
            }

        }

        private void Temizle6()
        {
            Txt6YetkiliID.Text = "";
            Txt6YetkiliKA.Text = "";
            Txt6YetkiliSifre.Text = "";
        }
        //6666666666//


        //77777777-//
        private void Btn7LstGoster_Click(object sender, EventArgs e)//7. sekmedeki checkedlistbox ın içini dolduran method u çalıştırır 
        {
            ChkListBxDoldur7();
        }

        void ChkListBxDoldur7 ()//7. sekmedeki checkedlistbox ın içini dolduran method 
        {
            try
            {
                checkedListBox1.Items.Clear();//Her dolduma işleminden önce listedeki veriler temizleniyor(AYNI VERİNİN BİRİKİMİNİ ÖNLEMEK İÇİN)
                SqlCommand ListeDoldur7 = new SqlCommand(" select  * from Tbl_Satıs order by (SatısID) DESC ", baglantı); //veritabanından yapılan bütün satışlar çekilior
                baglantı.Open();
                SqlDataReader oku7 = ListeDoldur7.ExecuteReader();
                while (oku7.Read())
                {
                    checkedListBox1.Items.Add(oku7[0] + "  " + oku7[1] + "  " + oku7[2] + "  " + oku7[3] + "  " + oku7[4]);//item olarak eklenecek veri oku7 adlı SqlDataReader nesnesinden gelen verilerle doluyor
                }
                baglantı.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("7. Sekmede Satış Çekmede Hata ", "HATA!");
                if (baglantı.State == ConnectionState.Open)//sql bağlantısı hata sırasında açık kalmışsa kapatır
                {
                    baglantı.Close();
                }

            }
        }

        private void Btn7SeciliSil_Click(object sender, EventArgs e)//7. sekmedeki checkedlistbox da seçili olan itemler silinecek
        {
            try
            {
                baglantı.Open();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)//seçili item sayısı kadar döngü çalışacak ve 
                {
                    string item7 = checkedListBox1.CheckedItems[i].ToString();//seçili olan veri değişkene aktarılıyor
                    int ilkbosluk = item7.IndexOf(" ");   //ilkbosluk değişkenine ise item7 adlı değişkendeki ilk " "(BOŞLUK) karakterinin indexi gönderiliyor
                    string ıd7 = item7.Substring(0, ilkbosluk);//substring özelliği ile item7 deki veriden veri çekiliyor ve ıd7 değişkenine aktarılıyor (Bu işlem yapılan satışdaki ID numarasını almamızı sağlıyor)  

                    SqlCommand SatısSil = new SqlCommand("delete from tbl_satıs where SatısID=@p1 ", baglantı);
                    SatısSil.Parameters.AddWithValue("@p1", ıd7);
                    SatısSil.ExecuteNonQuery();
                    
                   
                }
                baglantı.Close();
                MessageBox.Show("Satış silme işlemi başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChkListBxDoldur7();
                lbl7SecimSayısı.Visible = false;
                ch7TümünüSec.Checked = false;
            }
            catch (Exception)
            {
                MessageBox.Show("7. Sekmede Silme işleminde Hata", "HATA!");
                if (baglantı.State == ConnectionState.Open)//sql bağlantısı hata sırasında açık kalmışsa kapatır
                {
                    baglantı.Close();
                }
            }
            
           
        }

        private void ch7TümünüSec_CheckedChanged(object sender, EventArgs e)//checkbox ın seçilip seçilmemesi değiştiğinde
        {
            if (ch7TümünüSec.Checked==true)//ch7TümünüSec checkBox ı seçilmişe if komutunu seçilmemişşe else komutunu
            {
                int toplam = checkedListBox1.Items.Count;//Dögü CheckedListBox1 deki item sayısı kadar döner
                for (int i = 0; i < toplam; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);//Her i iteminin checked özelliğini ture yapar. yani her i itemini seçer
                }
                lbl7SecimSayısı.Visible = true;
                 lbl7SecimSayısı.Text= toplam + " Adet secim yapıldı.";
            }
            else
            {
                int toplam = checkedListBox1.Items.Count;//Dögü CheckedListBox1 deki item sayısı kadar döner
                for (int i = 0; i < toplam; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);//Her i iteminin checked özelliğini false yapar
                }
                lbl7SecimSayısı.Visible = false;
            }

                
        }
        //777777777//

        //Menuİtem-//
        //Tıklanılan yerde calışmasını istediğimiz kodları ekledik     (TabControl de bulunan ilgili TabPage yi açıyoruz)
        private void fişOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TbFis;   
        }
        private void satışlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TpSatıslar;
        }
        private void ürünlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TbUrun;
        }
        private void ürünİşlemlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TbUrunİslem;
        }
        private void müşterilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TbMusteri;
        }
        private void müşteriİşlemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TbMusteriİslem;
        }
        private void yetkiliİşlemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TcSatıslarr.SelectedTab = TbYetkiliİslem;
        }
        //Menuİtem//



    }






}
