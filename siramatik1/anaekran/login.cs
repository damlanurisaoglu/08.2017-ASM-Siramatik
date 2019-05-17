using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace anaekran
{
    public partial class login : Form
    {
        public int ID_ { get; set; }
        public login()
        {
            InitializeComponent();
        }
       
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
          

        }

        private void tikla(object sender, EventArgs e)
        {
            textBox2.Text += ((Button)sender).Text;

        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

        }
        string UserName_ = ConfigurationManager.ConnectionStrings["UserName_"].ConnectionString.ToString();

        SqlConnection baglan;//= new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog=SIRAMATIK; Integrated Security=True;");

        //   public static string Conn_ = ConfigurationManager.ConnectionStrings["baglan"].ConnectionString.ToString();
        public static string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["baglan"].ConnectionString.ToString();
        }


        private bool VtBaglan()
        {

            try
            {
                baglan = new SqlConnection();
                //SqlConnection baglan = new SqlConnection(ConfigurationManager.ConnectionStrings["baglan"].ConnectionString);
                baglan.ConnectionString = GetConnString();

                //baglan = new SqlConnection();
                //baglan.ConnectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog=SIRAMATIK; Persist Security Info=False; User ID=damla; Password=damla";
                // SqlConnection baglan = new SqlConnection(ConfigurationManager.ConnectionStrings["baglan"].ConnectionString);
                baglan.Open();

                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

        private void login_Load(object sender, EventArgs e)
        {
            if (VtBaglan())
            {
             
            }
            else
            {
                MessageBox.Show("Veritabanı Bağlantısı Kurulumadığı İçin Program Kapatılacak !!!");
                Application.Exit();
            }

            label2.Text = UserName_;
            label2.ForeColor = Color.Red;
           
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
       
        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length>0)
            {
                textBox2.Text = textBox2.Text.Substring(0, textBox2.Text.Length - 1);

            }
            
        }
       
       
        public string KullaniciAdi;
        public string Sifre;
        private void button3_Click(object sender, EventArgs e)
        {
          
            KullaniciAdi = label2.Text;
            Sifre = textBox2.Text;
           
            


            //SqlCommand komut= new SqlCommand("select ID,UserName, Pass from tblLogin where Type_=1 and UserName='"+KullaniciAdi+"' and Pass='"+Sifre+"'",baglan);
            //SqlDataAdapter da = new SqlDataAdapter(komut);
            //komut.ExecuteNonQuery();

            Service_Eu Service_Eu_ = new Service_Eu();
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.Fill();

            DataTable dt1 = Service_Eu_.Select_Login_DataTable(KullaniciAdi, Sifre, 1);
            if (dt1.Rows.Count == 1)
            {
                Secim scm = new Secim();
             
               
                
                ID_ = int.Parse(dt1.Rows[0][0].ToString());
                scm.ID_ = ID_;
                scm.Show();
                this.Hide();
            }
            else
            {
                label3.Text = "Şifreyi Yanlış Girdiniz!";
                label3.ForeColor = Color.Red;
                textBox2.Clear();

            }
           
            
        }
    }
}
