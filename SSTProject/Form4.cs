using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace SSTProject
{
    public partial class Form4 : Form
    {
        private Form1 f1;
        private ConnectionCreator ConCre = new ConnectionCreator();
        public double actualvote ;
        public int actualcount ;
        public Form4(Form1 f)
        {
            f1 = f;
            InitializeComponent();
            radioButton1.Checked = true;
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            organizer();
            string CommandText = "";
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            CommandText = "SELECT * FROM Events WHERE Name = @Name";
            MySqlCommand cmd = new MySqlCommand(CommandText, connection);
            cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar, 40, "Name"));
            cmd.Parameters["@Name"].Value = f1.imya;
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            this.Text = rdr["Name"].ToString();
            label1.Text = rdr["Name"].ToString() + ";   Рейтинг:" + rdr["Rating"].ToString() + " ";
            label2.Text = "Дата и время: " + rdr["Date"].ToString();
            label3.Text = "Ответственный: " + rdr["Org"].ToString();
            textBox1.Text = rdr["Misc"].ToString();
            actualvote = Convert.ToDouble(rdr["Rating"]);
            actualcount = Int32.Parse(rdr["Marks"].ToString());
            rdr.Close();
            connection.Close();
        }
        private void organizer()
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            String CommandText = "SELECT * FROM Users WHERE Name = @Name";
            MySqlCommand cmd = new MySqlCommand(CommandText, connection);
            cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar, 40, "Name"));
            cmd.Parameters["@Name"].Value = f1.organizer;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
                try
                {
                    pictureBox1.Load(rdr["Picture"].ToString());
                }
                catch
                {
                    pictureBox1.Image = pictureBox1.ErrorImage;
                }
                label4.Text = rdr["Name"].ToString();
                label5.Text = rdr["Grup"].ToString();
                string date = rdr["Date"].ToString();
                date = date.Remove(date.Length - 8);
                label6.Text = date;
                label7.Text = "+7" + rdr["Phone"].ToString();
                label8.Text = rdr["Mail"].ToString();
                textBox2.Text = rdr["Misc1"].ToString();
            rdr.Close();
            connection.Close();
        }
        private void vote()
        {
            int newvote = 0;
            if (radioButton1.Checked)
                newvote = newvote + 1;
            if (radioButton2.Checked)
                newvote = newvote + 2;
            if (radioButton3.Checked)
                newvote = newvote + 3;
            if (radioButton4.Checked)
                newvote = newvote + 4;
            if (radioButton5.Checked)
                newvote = newvote + 5;
            actualcount = actualcount+1;
            string CommandText = "";
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            CommandText = "UPDATE Events SET Rating = @Rating, Marks = @Marks WHERE Name = @Name";
            MySqlCommand cmd = new MySqlCommand(CommandText, connection);
            cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar, 40, "Name"));
            cmd.Parameters["@Name"].Value = f1.imya;
            cmd.Parameters.Add(new MySqlParameter("@Rating", MySqlDbType.Double, 10, "Rating"));
            cmd.Parameters["@Rating"].Value = ((actualvote + newvote)/2);
            cmd.Parameters.Add(new MySqlParameter("@Marks", MySqlDbType.Int32, 30, "Marks"));
            cmd.Parameters["@Marks"].Value = actualcount;
            int rows = cmd.ExecuteNonQuery();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vote();
            Form4_Load(button1, null);
        }
    }
}
