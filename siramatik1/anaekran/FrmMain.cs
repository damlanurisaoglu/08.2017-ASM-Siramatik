using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace anaekran
{
    public partial class FrmMain : Form
    {
        public int ID_ { get; set; }

        public FrmMain()
        {
            InitializeComponent();
        }

        const int MaxSatirSayisi= 6;

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void FrmMain_InputLanguageChanging(object sender, InputLanguageChangingEventArgs e)
        {

        }

        struct doktor
        {
            public Int64 ID;
            public String DoktorAdi;
            public int SiraNo;
            public int KayitSiraNo;
            public Button btnDoktor;
            public Image imgDoktor;
        }
        doktor[] Doktorlar;

        int[] Siralar;
        Button[] butonlar;
        int[] Hastalar;

        SqlConnection baglan;
        public static string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["baglan"].ConnectionString.ToString();
        }


        private bool VtBaglan()
        {
           
            try
            {
                baglan = new SqlConnection();
                 baglan.ConnectionString = GetConnString();
               
                baglan.Open();

                return true;

            }
            catch (Exception)
            {

                return false;
            }
            
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

            if (VtBaglan())
            {
                GenerateTable();
                try
                {
                UdpClient = new UdpClient(6000);
                RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 6000);
                }
                catch (Exception)
                {  }
               
            }
            else
            {
                MessageBox.Show("Veritabanı Bağlantısı Kurulumadığı İçin Program Kapatılacak !!!");
                Application.Exit();
            }





        }

        Service_Eu Service_Eu_ = new Service_Eu();
        DataTable dt = new DataTable();


        private void GenerateTable()
        {
            int columnCount = 0; int rowCount = 0;


            //if (ID_==0 )
            //{
            //    dt = Service_Eu_.Select_Doktorlar_DataTable(2);
            //}
            //else
            //{
            //   dt = Service_Eu_.Select_Doktorlar_DataTable(ID_);
            //}
            dt = Service_Eu_.Select_Doktorlar_DataTable(2);


            if (dt.Rows.Count <= MaxSatirSayisi)
            {
                columnCount = 1;
                rowCount = dt.Rows.Count;

            }
            else if (dt.Rows.Count > MaxSatirSayisi && dt.Rows.Count % 2 == 0)
            {
                columnCount = 2;
                rowCount = MaxSatirSayisi;

            }
            else
            {
                columnCount = 2;
                rowCount = ((dt.Rows.Count + 1) / 2)+1;

            }
            panel.Controls.Clear();
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();

            panel.ColumnCount = columnCount;
            panel.RowCount = rowCount;


            Doktorlar = new doktor[dt.Rows.Count];


                for (int x = 0; x < columnCount; x++)
                {

                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    for (int y = 0; y < rowCount; y++)
                    {

                        if (x == 0)
                        {
                            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                        }
                            int i = (x * MaxSatirSayisi) + y;
                        if (i == dt.Rows.Count)
                        {
                             break;
                        }
                            Doktorlar[i].ID = Convert.ToInt64(dt.Rows[i]["ID"].ToString());
                            Doktorlar[i].DoktorAdi = dt.Rows[i]["AdSoyad"].ToString();
                            Doktorlar[i].KayitSiraNo = int.Parse(dt.Rows[i]["KayitSiraNo"].ToString());
                            Doktorlar[i].SiraNo = int.Parse(dt.Rows[i]["SiraNo"].ToString());
                            try
                            {
                                Doktorlar[i].imgDoktor = (Image)dt.Rows[i]["Resim"];
                            }
                            catch 
                            {

                            }
                            
                            Doktorlar[i].btnDoktor = new Button();


                            Doktorlar[i].btnDoktor.Text= dt.Rows[i]["AdSoyad"].ToString();

                            Doktorlar[i].btnDoktor.Dock = DockStyle.Fill;
                            Doktorlar[i].btnDoktor.AutoSize = false;
                            Doktorlar[i].btnDoktor.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                            Doktorlar[i].btnDoktor.Image = Doktorlar[i].imgDoktor;
                            Doktorlar[i].btnDoktor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                            Doktorlar[i].btnDoktor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            Doktorlar[i].btnDoktor.Name = dt.Rows[i]["ID"].ToString();
                            Doktorlar[i].btnDoktor.TabIndex = 1;
                            Doktorlar[i].btnDoktor.Size = new System.Drawing.Size(this.Size.Width - 100, 132);
                            Doktorlar[i].btnDoktor.Text = dt.Rows[i]["AdSoyad"].ToString();
                            Doktorlar[i].btnDoktor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                            Doktorlar[i].btnDoktor.UseVisualStyleBackColor = true;
                            Doktorlar[i].btnDoktor.Tag = i;
                            
                            Doktorlar[i].btnDoktor.Click += FrmMain_Click;
                            panel.Controls.Add(Doktorlar[i].btnDoktor, x, y);
                    }
                }

            

            




          
           
        }
    

        private void FrmMain_Click(object sender, EventArgs e)
        {
          
            int btnID = int.Parse(((Button)sender).Tag.ToString());

            Doktorlar[btnID].KayitSiraNo++;

            DisplayGoster(Doktorlar[btnID].KayitSiraNo, Doktorlar[btnID].SiraNo);

           Print_Barcode("Maltepe 1 Nolu Aile Sağlığı Merkezi", Doktorlar[btnID].KayitSiraNo, Doktorlar[btnID].SiraNo, Doktorlar[btnID].DoktorAdi);
          

             Service_Eu Service_Eu_ = new Service_Eu();
            

             Service_Eu_.Kayitlar_insert_DataReader(0, int.Parse((Doktorlar[btnID].ID).ToString()), int.Parse(Doktorlar[btnID].KayitSiraNo.ToString()));
            Secim scm = new Secim();
            scm.Show();
            this.Hide();
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {   

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }


        private void Print_Barcode(string birimAadi, int SiraNo, int KapiNo, string doktorAdi)
        {
            
            string numara = "";
            if (SiraNo.ToString().Length == 1)
            {
                numara = "00" + SiraNo.ToString();
            }
            else
            if (SiraNo.ToString().Length == 2)
            {
                numara = "0" + SiraNo.ToString();
            }
            else
                numara = SiraNo.ToString();

            Bitmap bitmap_ = new Bitmap(300, 300);
                using (Graphics graphics = Graphics.FromImage(bitmap_))
                {
                    Font oFont = new System.Drawing.Font("IDAHC39M Code 39 Barcode", 25);
                    Font oFont2 = new System.Drawing.Font("IDAHC39M Code 39 Barcode", 12);
                    SolidBrush black_ = new SolidBrush(Color.Black);
                    SolidBrush white_ = new SolidBrush(Color.White);
                    graphics.FillRectangle(white_, 0, 0, bitmap_.Width, bitmap_.Height);
                    graphics.DrawString(birimAadi, oFont2, black_, new PointF(20, 10));
                    graphics.DrawString(KapiNo+" - "+numara, oFont, black_, new PointF(70, 60));
                    graphics.DrawString(doktorAdi, oFont2, black_, new PointF(50, 110));
                    graphics.DrawString(DateTime.Now.ToString(), oFont2, black_, new PointF(50, 140));
                
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap_.Save(ms, ImageFormat.Png);
                    pictureBox1.Image = bitmap_;
                    pictureBox1.Height = bitmap_.Height;
                    pictureBox1.Width = bitmap_.Width;
                }
                
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrinterSettings.PrinterName = "XP-80C";
                printDoc.PrintPage += PrintPage;

                printPreviewDialog.Document = printDoc;
               
                printDoc.Print();
            
        }
       
        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            System.Drawing.Image documentImg;
            documentImg = pictureBox1.Image;
            g.DrawImage(documentImg, new Point(0, 0));
            String text = "";
            Font fontText = new Font("Arial", 72, FontStyle.Regular);
            g.DrawString(text, fontText, Brushes.Black, new Point(5, 0));
           
            
            Pen pen = new Pen(Color.Gray);
        }

        public static UdpClient UdpClient;
        public static IPEndPoint RemoteIpEndPoint;

        public static void UdpSend(string IPAdres, string Data)
        {
            try
            {
                   Encoding enTr = Encoding.GetEncoding("windows-1254");
                IPAddress ip = IPAddress.Parse(IPAdres);
                IPEndPoint ipEndPoint = new IPEndPoint(ip, 6001);

                byte[] content = enTr.GetBytes(Data);


                UdpClient.Send(content, content.Length, ipEndPoint);
            }
            catch { }
        }


        private void DisplayGoster(int SiraNo,int KapiNo)
        {


            string SendData = "";
            SendData += (char)(0x02);
            SendData += (char)(0x15) + "RST";
            SendData += (char)(0x01);
            SendData += (char)(0x00);
            SendData += (char)(0x00);
            SendData += (char)(0x01);
            SendData += (char)(0x03);
            UdpSend("192.168.0.93", SendData);


            Thread.Sleep(100);

            string numara = "";
            if (SiraNo.ToString().Length == 1)
            {
                numara = "00" + SiraNo.ToString();
            }
            else
        if (SiraNo.ToString().Length == 2)
            {
                numara = "0" + SiraNo.ToString();
            }
            else
                numara = SiraNo.ToString();


            string tmpKapiNo = "";
            if (KapiNo.ToString().Length == 1)
            {
                tmpKapiNo = "0" + KapiNo.ToString();
            }
            else
                tmpKapiNo = KapiNo.ToString();

            SendData = "";
            SendData += (char)(0x02);
            SendData += (char)(0x14) + "ALM";
            SendData += (char)(0x01);
            SendData += (char)(0x00);
            SendData += (char)(0x00);
            SendData += (char)(0x01) + tmpKapiNo+"-  " + numara;
            SendData += (char)(0x03);




            UdpSend("192.168.0.93", SendData);

        }
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
           
        }
        int kapat=0;
       
        private void lblBirimAdi_Click(object sender, EventArgs e)
        { 
                timer1.Stop();

            kapat++;
            if(kapat>=5)
                Application.Exit(); 
           
            timer1.Interval = 6000;
                timer1.Start();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void tikla(object sender, EventArgs e)
        {
            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            kapat = 0;
            timer1.Stop();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Secim scm = new Secim();
            scm.ID_ = ID_;
            scm.Show();
            this.Hide();


        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
