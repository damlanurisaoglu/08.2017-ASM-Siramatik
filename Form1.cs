using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace siramatik1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

      

        private void button1_Click(object sender, EventArgs e)
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
            /* SendData="";
             SendData += (char)(0x02);
             SendData += (char)(0x14)+"ALM";
             SendData += (char)(0x01);
             SendData += (char)(0x00);
             SendData += (char)(0x00);
             SendData += (char)(0x01)+"01-  001";
             SendData += (char)(0x03);





             UdpSend("192.168.0.93", SendData);*/
        }
      
        int sayi=0;
        private void button2_Click(object sender, EventArgs e)
        {
            //  timer1.Start();
            sayi++;



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
            if (sayi.ToString().Length == 1)
            {
                numara = "00" + sayi.ToString();
            }
            else
        if (sayi.ToString().Length == 2)
            {
                numara = "0" + sayi.ToString();
            }
            else
                numara = sayi.ToString();

            SendData = "";
            SendData += (char)(0x02);
            SendData += (char)(0x14) + "ALM";
            SendData += (char)(0x01);
            SendData += (char)(0x00);
            SendData += (char)(0x00);
            SendData += (char)(0x01) + "01-  " + numara;
            SendData += (char)(0x03);




            UdpSend("192.168.0.93", SendData);
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
        private void Form1_Load(object sender, EventArgs e)
        {
            UdpClient = new UdpClient(6000);
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 1994);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
