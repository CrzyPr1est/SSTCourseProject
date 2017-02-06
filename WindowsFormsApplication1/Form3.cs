using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.IO;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace SSTProject
{
    public partial class Form3 : Form
    {
        private login l1;
        public Form3(Main m)
        {
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            adduser();
            listy();
        }
        private ConnectionCreator ConCre = new ConnectionCreator();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection();
                connection.ConnectionString = ConCre.buildStr();
                using (connection)
                {
                    int sas;
                    double sas1;
                    connection.Open();
                    string CommandText = "";
                    CommandText = "UPDATE Users SET Name = @Name, Grup = @Grup, Phone = @Phone, Mail = @Mail, Date = @Date, Misc1 = @Misc1, Misc2 = @Misc2, Picture =  @Picture WHERE Login = @Login";
                    MySqlCommand cmd = new MySqlCommand(CommandText, connection);
                    //Параметры
                    cmd.Parameters.Add(new MySqlParameter("@Login", MySqlDbType.VarChar, 20, "Login"));
                    cmd.Parameters["@Login"].Value = listBox1.SelectedItem.ToString();
                    cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar, 50, "Name"));
                    cmd.Parameters["@Name"].Value = textBox1.Text.ToString();
                    if (cmd.Parameters["@Name"].Value.ToString() == "")
                        cmd.Parameters["@Name"].Value = "NOTDEFINED!";
                    cmd.Parameters.Add(new MySqlParameter("@Grup", MySqlDbType.VarChar, 50, "Group"));
                    cmd.Parameters["@Grup"].Value = textBox2.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Phone", MySqlDbType.VarChar, 20, "Phone"));
                    cmd.Parameters["@Phone"].Value = maskedTextBox1.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Mail", MySqlDbType.VarChar, 20, "Mail"));
                    cmd.Parameters["@Mail"].Value = textBox4.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Date", MySqlDbType.Date, 10, "Date"));
                    cmd.Parameters["@Date"].Value = Convert.ToDateTime(monthCalendar1.SelectionRange.Start);
                    cmd.Parameters.Add(new MySqlParameter("@Misc1", MySqlDbType.VarChar, 2000, "Misc1"));
                    cmd.Parameters["@Misc1"].Value = textBox5.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Picture", MySqlDbType.VarChar, 60, "Picture"));
                    cmd.Parameters["@Picture"].Value = textBox3.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Misc2", MySqlDbType.VarChar, 40, "Misc2"));
                    if (radioButton1.Checked)
                        cmd.Parameters["@Misc2"].Value = radioButton1.Text.ToString();
                    if (radioButton2.Checked)
                        cmd.Parameters["@Misc2"].Value = radioButton2.Text.ToString();
                    if (radioButton3.Checked)
                        cmd.Parameters["@Misc2"].Value = radioButton3.Text.ToString();
                    //Параметры
                    int rows = cmd.ExecuteNonQuery();
                    Form3_Load(button1, null);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            using (connection)
            {
                try
                {
                    string basepass = "";
                    string textpass = "";
                    bool isit1 = true;
                    bool isit2 = true;
                    connection.Open();
                    string CommandText1 = "";
                    string CommandText2 = "";
                    CommandText1 = "SELECT Password FROM Users where Login = @Login";
                    MySqlCommand cmd1 = new MySqlCommand(CommandText1, connection);
                    cmd1.Parameters.Add(new MySqlParameter("@Login", MySqlDbType.VarChar, 20, "Login"));
                    cmd1.Parameters["@Login"].Value = login.log;
                    MySqlDataReader rdr = cmd1.ExecuteReader();
                    if (rdr.Read())
                        basepass = rdr["Password"].ToString();
                    connection.Close();
                    textpass = textBox6.Text.ToString();
                    isit1 = (basepass == textpass);
                    isit2 = (textBox7.Text.ToString() == textBox8.Text.ToString());
                    if (isit1 && isit2)
                    {
                        connection.Open();
                        CommandText2 = "UPDATE Users SET Password = @Password WHERE Login = @Login";
                        MySqlCommand cmd2 = new MySqlCommand(CommandText2, connection);
                        cmd2.Parameters.Add(new MySqlParameter("@Login", MySqlDbType.VarChar, 20, "Login"));
                        cmd2.Parameters["@Login"].Value = login.log;
                        cmd2.Parameters.Add(new MySqlParameter("@Password", MySqlDbType.VarChar, 20, "Password"));
                        cmd2.Parameters["@Password"].Value = textBox8.Text;
                        int rows = cmd2.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Успешно!");
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                    }
                    else
                        MessageBox.Show("Несоответствие!", "Error!");
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        
    }
        private void adduser()
        {
            string CommandText = "";
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            CommandText = "SELECT * FROM Users";
            MySqlCommand cmd = new MySqlCommand(CommandText, connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                listBox1.Items.Add(rdr["Login"].ToString());
            }
            rdr.Close();
            connection.Close();
            int index = listBox1.FindString(login.log);
            listBox1.SetSelected(index, true);
        }
        private void listy()
        {
            if (checkBox1.Checked)
            {
                textBox6.UseSystemPasswordChar = true;
                textBox7.UseSystemPasswordChar = true;
                textBox8.UseSystemPasswordChar = true;
            }
            string CommandText = "";
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            CommandText = "SELECT * FROM Users WHERE Login = @Login";
            MySqlCommand cmd = new MySqlCommand(CommandText, connection);
            cmd.Parameters.Add(new MySqlParameter("@Login", MySqlDbType.VarChar, 20, "Login"));
            cmd.Parameters["@Login"].Value = listBox1.SelectedItem.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                try
                {
                    pictureBox1.Load(rdr["Picture"].ToString());
                }
                catch
                {
                    pictureBox1.Image = pictureBox1.ErrorImage;
                }
                textBox1.Text = rdr["Name"].ToString();
                textBox2.Text = rdr["Grup"].ToString();
                maskedTextBox1.Text = rdr["Phone"].ToString();
                textBox4.Text = rdr["Mail"].ToString();
                try
                {
                    monthCalendar1.SetDate(Convert.ToDateTime(rdr["Date"]));
                }
                catch
                {
                    monthCalendar1.SetDate(DateTime.Now);
                }
                textBox5.Text = rdr["Misc1"].ToString();
                textBox3.Text = rdr["Picture"].ToString();
                switch (login.rights) //rdr["Misc2"].ToString()
                {
                    case "administrator":
                        break;
                    case "studsovet":
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        radioButton3.Enabled = false;
                        listBox1.Enabled = false;
                        break;
                    case "student":
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        radioButton3.Enabled = false;
                        listBox1.Enabled = false;
                        break;
                }
                switch (rdr["Misc2"].ToString())
                {
                    case "administrator":
                        radioButton1.Checked = true;
                        break;
                    case "studsovet":
                        radioButton2.Checked = true;
                        break;
                    case "student":
                        radioButton3.Checked = true;
                        break;
                }
            }
            rdr.Close();
            connection.Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox6.UseSystemPasswordChar = true;
                textBox7.UseSystemPasswordChar = true;
                textBox8.UseSystemPasswordChar = true;
            }
            else
            {
                textBox6.UseSystemPasswordChar = false;
                textBox7.UseSystemPasswordChar = false;
                textBox8.UseSystemPasswordChar = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listy();
        }
    }
}
