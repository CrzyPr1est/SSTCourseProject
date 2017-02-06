using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SSTProject
{
    public partial class Main : Form
    {
        private login l1;
        Form3 f3;
        Form2 f2;
        Form1 f1;
        public Main(login l)
        {
            l = l1;
            InitializeComponent();
            label1.Text = login.name;
            switch (login.rights)
            {
                case "administrator":
                    label2.Text = "Администратор";
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    button4.Visible = true;
                    break;
                case "studsovet":
                    label2.Text = "Член ССТ";
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    button4.Visible = true;
                    break;
                case "student":
                    label2.Text = "Студент";
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    button4.Visible = false;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            l1 = new login();
            l1.ChangeRequiredProperties();
            Close();
            login.log = "";
            login.rights = "";
            login.pass = "";
            login.name = "";
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (f3 == null || f3.IsDisposed)
            {
                f3 = new Form3(this);
                f3.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (f2 == null || f2.IsDisposed)
            {
                f2 = new Form2(this);
                f2.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (f1 == null || f1.IsDisposed)
            {
                f1 = new Form1(this);
                f1.Show();
            }
        }
    }
}
