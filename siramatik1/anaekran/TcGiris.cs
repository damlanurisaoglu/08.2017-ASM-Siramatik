using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.IO;

namespace anaekran
{
    public partial class TcGiris : Form
    {
        public int ID_ { get; set; }
        public TcGiris()
        {
            InitializeComponent();
        }
        private void tikla(object sender, EventArgs e)
        {
            textBox2.Text += ((Button)sender).Text;

        }

        private void button14_Click(object sender, EventArgs e)
        {
            Secim scm = new Secim();
            scm.ID_ = ID_;
            scm.Show();
            this.Hide();

        }

        string TcKimlik;
        string DoktorID;
        int DoktorID_;
        int KayitSiraNo;
        string DoktorAd;
        int SiraNo;
        int HastaID;
        private void TcGiris_Load(object sender, EventArgs e)
        {
            try
            {
  UdpClient = new UdpClient(6000);
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 1994);
            }
            catch (Exception)
            {  }
          

           
        }
        private void button3_Click(object sender, EventArgs e)
        {

            TcKimlik = textBox2.Text;
            Service_Eu Service_Eu_ = new Service_Eu();
            DataTable dt1 = Service_Eu_.Select_Hastalar_DataTable(TcKimlik);
            Secim scm = new Secim();
            scm.ID_ = ID_;

           
            
            if (dt1.Rows.Count == 1)
            {
                DoktorID = dt1.Rows[0][2].ToString();
                DoktorID_ = int.Parse(DoktorID);
                DataTable dt2 = Service_Eu_.Select_Kayıtlar_DataTable(DoktorID_);
                KayitSiraNo = int.Parse(dt2.Rows[0][2].ToString());
                SiraNo = int.Parse(dt2.Rows[0][4].ToString());
                DoktorAd = dt2.Rows[0][1].ToString();
                KayitSiraNo++;
                HastaID= int.Parse(dt1.Rows[0][3].ToString());
                Service_Eu_.Kayitlar_insert_DataReader(HastaID,DoktorID_, KayitSiraNo);
                Print_Barcode("Maltepe 1 Nolu Aile Sağlığı Merkezi", KayitSiraNo, SiraNo, DoktorAd);
                DisplayGoster(KayitSiraNo, SiraNo);
                
                
                scm.Show();
                this.Hide();
                
            }
            else
            {
                label3.Text = "TC Numaranızı Yanlış Girdiniz!";
                label3.ForeColor = Color.Red;
                textBox2.Clear();

            }


           

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
        private void Print_Barcode(string birimAadi, int SiraNo, int KapiNo, string doktorAdi)
        {

            //string barcode = textBox1.Text;

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
                graphics.DrawString(KapiNo + " - " + numara, oFont, black_, new PointF(70, 60));
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

            //PrintDocument pd = new PrintDocument();
            //pd.PrintPage += PrintPage;
            //pd.Print();

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = "XP-80C";
            printDoc.PrintPage += PrintPage;

            printPreviewDialog.Document = printDoc;
            //Open the print preview dialog
            //printPreviewDialog.ShowDialog();  

            printDoc.Print();

        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            //System.Drawing.Image img = System.Drawing.Image.FromFile(pictureBox1);
            //Point loc = new Point(100, 100);

            //e.Graphics.DrawImage(pictureBox1.Image, loc);


            Graphics g = e.Graphics;
            System.Drawing.Image documentImg;
            //Here you have to specify the location of the image
            documentImg = pictureBox1.Image;
            //Locate the logo image on the location set on new Point
            g.DrawImage(documentImg, new Point(0, 0));
            //String text = "Text \r\nText1\r\nText2";
            String text = "";// textBoxText_.Text.Replace(Brace_, "\r\n");
            //Set font, size and location
            //Font fontText = new Font("Times New Roman", 12, FontStyle.Regular);
            Font fontText = new Font("Arial", 72, FontStyle.Regular);
            g.DrawString(text, fontText, Brushes.Black, new Point(5, 0));


            //Drawing a horizontal line
            Pen pen = new Pen(Color.Gray);
            //Set the begin and the end of the line.
            //g.DrawLine(pen, new Point(10, 180), new Point(50, 200));
        }
        private void DisplayGoster(int SiraNo, int KapiNo)
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
            SendData += (char)(0x01) + tmpKapiNo + "-  " + numara;
            SendData += (char)(0x03);




            UdpSend("192.168.0.93", SendData);

        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                textBox2.Text = textBox2.Text.Substring(0, textBox2.Text.Length - 1);

            }
        }

        private void TcGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
