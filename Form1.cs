//////////////////////////////////////////////////////
//    			     								//
//                Coded by Faruk OKSUZ				//
//                        2019      				//
//////////////////////////////////////////////////////	
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace Proje_2_MayinTarlasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string seviye;//seviye belirleyen değişkenler 
        string kazandi = " KAZANDI ";
        string kaybetti = " KAYBETTİ ";
        string sonuc = "";

        //ZAMANLAYICI DEĞİŞKENLERİ
        int saniye1 = 0;
        int dakika1 = 0;
        int saniye2 = 0;
        int dakika2 = 0;

        bool[,] mayinlar = new bool[21, 21]; //MAYINLARI KONTROL EDEN MATRİS
        int nesne_x = 10;//nesne x koordinatı
        int nesne_y = 20;//nesne y koordinatı
        private void Seviye_Belirle()
        {
            if (rbKolay.Checked == true)//radiobutton kolay seçili ise 
            {
                Mayin_Yerlerini_Rastgele_Ata(40);//PANEL e 40 mayın ata.
                seviye = "Kolay"; 
            }
            else if (rbOrta.Checked == true)//radiobutton orta seçili ise 
            {
                Mayin_Yerlerini_Rastgele_Ata(50);//PANEL e 50 mayın ata.
                seviye = "Orta";
            }
            else//değilse zaten zor seçelidir.//PANEL e 80 mayın ata.
            {
                Mayin_Yerlerini_Rastgele_Ata(80);//radiobutton zor seçili ise 
                seviye = "Zor";
            }
        }

        private void Oyun_Tekrar_Etmek_Ister_Misiniz()
        {
            nesne_x = 10; //oyuna başa alındığında nesne x koordinatı en başa geri döndü
            nesne_y = 20;//oyuna başa alındığında nesne y koordinatı en başa geri döndü
            DialogResult karar = new DialogResult(); //oyunu yeniden başlatmak için
            karar = MessageBox.Show("YENİ OYUN ??", "KAYBETTİNİZ", MessageBoxButtons.YesNo);
            if (karar == DialogResult.Yes)
            {
                Application.Restart();//oyunu yeniden başlat.
                /*
                panel1.BackColor = Color.Aquamarine;
                Koordinal_Ile_Label_Temsil_Et(nesne_x, nesne_y);
                Nesnenin_Konumunu_Belirle(nesne_x, nesne_y);//başlangıçta nesne konumunu belirle

                foreach (object item in this.Controls)
                {
                    if (item is Button)
                    {
                        Button btn = (Button)item;
                        btn.Enabled = true;
                    }
                }
                */
            }
            else
            {
                Application.Exit(); //değilse çıkış 
            }
            using (StreamWriter file = new StreamWriter("skorlar.txt", true)) //dosyaya yazma yapmak için kullanılan STREAM WRİTER NESNESİ
            {
                file.WriteLine(textBox1.Text + " --- > " + lblDakika.Text + ":" + lblSaniye.Text + " " + seviye + " Seviyede" + sonuc); //dosyaya istenilen verileri yazma
            }
        }
        private void btnKisiyiKaydet_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBox1.Text + " kaydedildi "); //ilk girişte çıktı mesajı
        }
        private void Nesnenin_Konumunu_Belirle(int x, int y)
        {
            Image i_2 = Image.FromFile(@"pacman.png"); //nesne icon
            Label lbl = Koordinal_Ile_Label_Temsil_Et(nesne_x, nesne_y); //hangi label üzerinden gittiği 
            if (lbl != null) //yani nesne oradaysa eğer 
            {
                lbl.BackColor = Color.White; //background color beyaz
                lbl.Image = i_2;//background image yolda belirtilen image.
            }
        }
        private void Nesne_Yok_Et(int x, int y) //NESNE HER KONTROLDE TAŞIMA İŞLEMİ
                                //SİL , DAHA SONRA İLERİ GERİ ,SAĞA VEYA SOLA TAŞI İSTEĞE GÖRE
        {
            //nesne hangi koordinatta ise onu yok et 
            Label lbl = Koordinal_Ile_Label_Temsil_Et(nesne_x, nesne_y); //geriye döndürdüğünün label in image i temizle
            lbl.Image = null; // o label image ini boş ata yani temizle nesne hareket ettirilince
        }
        private Label Koordinal_Ile_Label_Temsil_Et(int x, int y) //label geriye ne döndürdüğü 
        {
            // verilen x y koordinatına göre label temsili alınıyor .
            int kacinci_label = x + (y - 1) * 20;  //koordinat temsili label ismi 
            string label_ismi = "label" + kacinci_label.ToString();//panelde kontrol edilecek olan label 

            foreach (Control nesne in panel1.Controls) //panel deki kontroller 
            {
                if (nesne.GetType() == typeof(Label)) //bu kontroller eğer label ise 
                {
                    if (nesne.Name == label_ismi) //eğer bu kontrolün ismi panelde varsa 
                    {
                        return (Label)nesne;  //geriye label döndür
                    }
                }
            }
            return null;  //eğer öyle bir koordinat yoksa geriye boş değer döndürür. 
        }
        private void Mayin_Yerlerini_Rastgele_Ata(int hedef)// hedef parametresi //mayin konumları 
        {
            //Mayin yerlerini rastgele ata.
            Random r = new Random(); //rastgele atamak için değişken
            int x, y; //x y koordinatı
            Array.Clear(mayinlar, 0, mayinlar.Length); //dizinin tümüne 0 ata yani                                      
            do
            {
                x = r.Next(1, 20); //rastgele x koordinatı belirle 
                y = r.Next(1, 20); //rastgele y koordinatı belirle
                if (! mayinlar[x, y]) //x y koordinatında mayin yoksa 
                {
                    mayinlar[x, y] = true; //ekle 
                    hedef--; //kaç adet mayin gelecekse belirlediğimiz seviyede 0 olana kadar azalt 
                                // ve mayınları tek tek ata .
                }
            } while (hedef > 0);//mayın sayısı negatif değer olamayacağı için şartımız 0 dan büyük olması 
        }
        private void Mayinlari_Kontrol_Et(int x, int y) //mayınları kontrol et 
        {
            if (mayinlar[x, y]) //Eğer nesnenin bulunduğu koordinatta mayın varsa
            {
                //yani mayına basılmışssa 
                //BUTONLARI PASİF YAP.
                btnAsagi.Enabled = false;
                btnYukari.Enabled = false;
                btnSaga.Enabled = false;
                btnSola.Enabled = false;
                Mayinlari_Goster();//mayınları PANELDE göster
                timer1.Stop(); //TİMER YANİ ZAMAN DURDUR .
                //YENİ OYUN YAPILACAK .Çünkü kaybetti
                sonuc = kaybetti; //SONUC DEĞİŞKENİNE KAYBETTİ AKTARILIR .
                Oyun_Tekrar_Etmek_Ister_Misiniz(); //OYUN TEKRAR EDİLSİN Mİ _
            }
            else
            {
                //eğer mayına basılmamışsa yakınlık saymaya devam et 
                Mayinlari_Say(x, y); //mayınları say 
            }
        }

        private void Mayinlari_Say(int x, int y)//YAKINLIK SAY 
        {
            int yakinlik_adet = 0;
            //BOMBALARA YAKINLIK BELİRLER             
            //HER NESNENİN YENİ HAREKETİNDE SAĞINA SOLUNA YUKARISINA VE AŞAĞISINA BAKILIR         
            if (x > 0)  //solunda mayın var mı ?
            {
                if (mayinlar[x - 1, y])//eğer o koordinatda mayın varsa 
                    yakinlik_adet++;//yakınlık 1 arttırılır 
            }
            if (x < 19)  //sağında mayın var mı ?
            {
                if (mayinlar[x + 1, y])//eğer o koordinatta mayın varsa
                    yakinlik_adet++;//yakınlık 1 arttırılır 
            }
            if (y > 0) //arkasında mayın var mı ?
            {
                if (mayinlar[x, y - 1]) //eğer o koordinatta mayın varsa
                    yakinlik_adet++;//yakınlık 1 arttırılır 
            }
            if (y < 19) //önünde mayın var mı ?
            {
                if (mayinlar[x, y + 1])//eğer o koordinatta mayın varsa
                    yakinlik_adet++;  //yakınlık 1 arttırılır 
            }
            label401.Text = yakinlik_adet.ToString(); //son olarak her defasında yakın olan mayınlar label a aktarılır .
        }

        private void Mayinlari_Goster() //MAYINA BASILDI 
        {
            Label lbl; //tüm label ları kontrol edecek değişken 
            Image i_1 = Image.FromFile(@"mayin.png"); //image nesnesi CANAVAR için IMAGE
            for (int i = 1; i < 21; i++)
            {
                for (int j = 1; j < 21; j++)
                {
                    lbl = Koordinal_Ile_Label_Temsil_Et(i, j);//Koordinat ile temsil edilen label 
                                            //geriye ne döndürdü ? boş ise şayet arka plan beyaz
                                            //değilse mayın vardır 
                    if (mayinlar[i, j]) //o koordinatta mayın var oyun sona erdi .
                    {
                        lbl.Image = i_1; //o label a "mayin.png" adlı image ata.Yani mayın var.
                        this.BackColor = Color.OrangeRed; //formun arka plan rengini değiş
                    }
                    else
                    {
                        lbl.BackColor = Color.Blue;//nesnenin basmamış olduğu label arka planını beyaz yap
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Nesnenin_Konumunu_Belirle(nesne_x, nesne_y);//başlangıçta nesne konumunu belirle
            Mayinlari_Kontrol_Et(nesne_x, nesne_y);//daha sonra mayinlari kontrol et .
            lblDakika.Text = dakika1.ToString() + dakika2.ToString();
            lblSaniye.Text = saniye1.ToString() + saniye2.ToString();
        }

        private void btnSkorlariGoruntule_Click(object sender, EventArgs e)
        {
            string dosya_Ac;
            dosya_Ac = @"skorlar.txt";
            Process.Start(dosya_Ac);//belirtilen yoldaki dosyayı aç
        }

        private void btnYardim_Click(object sender, EventArgs e)
        {
            string dosya_Ac2;
            dosya_Ac2 = @"yardım.txt";
            Process.Start(dosya_Ac2);//belirtilen yoldaki dosyayı aç
        }

        private void btnYukari_Click(object sender, EventArgs e)
        {
            timer1.Start(); //butonların herhangi birine basıldığı zamanda timer başlar .
            //ileri geri tuşlarında y koordinatı kontrol edilcek .
            //çünkü y koordinatı değişir 
            //label temizlenir ve nesne yeni koordinata aktarılır .

            Seviye_Belirle();
            //yeni konum güncellenince label dan label a silerek taşıma işlemi yap .

            for (int i = 1; i < 21; i++) // x koordinatı herhangi birinde olduğu zamanda ve y 1 olduğunda 
            {                           
                if (nesne_x <= i && nesne_y == 1) //kullanıcı kazanmış
                {
                    MessageBox.Show("Tebrikler kazandınız ", "KAZANDINIZ");
                    sonuc = kazandi;
                    //YENİ OYUN YAPILACAK .  !!!!
                    Oyun_Tekrar_Etmek_Ister_Misiniz();
                    break;
                }
            }
            //label daki nesneyi sil hareket edince yukarı taşı 
            if (nesne_y > 1) //y koordinat 1 den büyükse
            {
                Nesne_Yok_Et(nesne_x, nesne_y); //label temsili koordinattaki nesneye NULL ata
                nesne_y--;//nesneyi hareket ettirmek ve konumunu değişmek için y koordinatı 1 azalt
                Nesnenin_Konumunu_Belirle(nesne_x, nesne_y); //nesnenin yeni konumunu belirle hareket edice           
                Mayinlari_Kontrol_Et(nesne_x, nesne_y);//o koordinattaki mayını kontrol etme
            }
        }

        private void btnSaga_Click(object sender, EventArgs e)
        {
            timer1.Start();//butonların herhangi birine basıldığı zamanda timer başlar .
            //yeniden x koordinatı kontrol ediliyor . çünkü 
            //sağ sol yapınca x koordinatında değişiklik olur .
            Seviye_Belirle();
            for (int i = 1; i < 21; i++)
            {
                if (nesne_x == 20 && nesne_y <= i)
                {
                    MessageBox.Show("Sağa çok gittin");
                    break;
                }
            }
            if (nesne_x < 20) //x koordinat 20 den küçükse
            {
                Nesne_Yok_Et(nesne_x, nesne_y);//label temsili koordinattaki nesneye NULL ata
                nesne_x++;//nesneyi hareket ettirmek ve konumunu değişmek için x koordinatı 1 azalt
                Nesnenin_Konumunu_Belirle(nesne_x, nesne_y);//nesnenin yeni konumunu belirle hareket edice           
                Mayinlari_Kontrol_Et(nesne_x, nesne_y);
            }
        }

        private void btnAsagi_Click(object sender, EventArgs e)
        {
            timer1.Start();//butonların herhangi birine basıldığı zamanda timer başlar .
            //ileri geri tuşlarında y koordinatı kontrol edilcek .
            Seviye_Belirle();
            if (nesne_y < 20)
            {
                //label i temizle yok et 
                Nesne_Yok_Et(nesne_x, nesne_y);
                nesne_y++; // y koordinatını 1 arttır 
                Nesnenin_Konumunu_Belirle(nesne_x, nesne_y); //yeni konum belirle 
                Mayinlari_Kontrol_Et(nesne_x, nesne_y); //bomba var mi kontrol et .
            }
        }

        private void btnSola_Click(object sender, EventArgs e)
        {
            timer1.Start(); //butonların herhangi birine basıldığı zamanda timer başlar .
            //yeniden x koordinatı kontrol ediliyor . çünkü 
            //sağ sol yapınca x koordinatında değişiklik olur .
            Seviye_Belirle();//seviye belirlenir .
            for (int i = 1; i < 21; i++)
            {
                if (nesne_x == 1 && nesne_y <= i) //bu şartta sola çok gidildi
                {
                    MessageBox.Show("Sola çok gittin"); //Sınırlar aşıldı demek
                    break;
                }
            }
            if (nesne_x > 1)
            {
                Nesne_Yok_Et(nesne_x, nesne_y);
                nesne_x--;
                Nesnenin_Konumunu_Belirle(nesne_x, nesne_y);
                Mayinlari_Kontrol_Et(nesne_x, nesne_y);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //ZAMANLAMA KODLARI
            saniye1++;//yön tuşlarından herhangi birine basınca saniye başlar
            lblSaniye.Text = saniye2.ToString() + saniye1.ToString();//LABEL SANİYE
                            

            if (saniye1 == 10) //0-10 arasıdır saniye 
            {
                saniye2++; //saniye 
                saniye1 = 0; 
                lblSaniye.Text = saniye2.ToString() + saniye1.ToString();

                if (saniye2 == 6) //1 dakika 60 saniye olduğu için 
                {
                    dakika1++; //dakika 1 arttırılır 
                    saniye2 = 0; //saniyeler sıfırlanır 
                    saniye1 = 0;
                    lblDakika.Text = dakika2.ToString() + dakika1.ToString();//yazdırılır
                    if (dakika1 == 10) //dakika 0-10 arası olduğu için 
                    {
                        dakika2++; //dakika 1 i arttır 
                        dakika1 = 0; //demek ki 10 dakika ve üzerinde
                        lblDakika.Text = dakika2.ToString() + dakika1.ToString();//label a yazdır
                    }
                }
            }
        }
    }
}
