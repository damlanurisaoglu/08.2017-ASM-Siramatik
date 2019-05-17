using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anaekran
{
    public partial class Secim : Form
    {
        public int ID_ { get; set; }
        public int TC_ { get; set; }
        public Secim()
        {
            InitializeComponent();
        }

        private void Secim_Load(object sender, EventArgs e)
        {

        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            
           
            FrmMain frm = new FrmMain();
            //TcGiris tcgiris = new TcGiris();
            frm.ID_ = ID_;
            //tcgiris.ID_ = ID_;
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcGiris grs = new TcGiris();
            grs.ID_ = ID_;
            grs.Show();
            this.Hide();
        }
    }
}
