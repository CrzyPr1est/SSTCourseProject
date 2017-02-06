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
    public partial class login : Form
    {
        public static string rights = "";
        public static string name = "";
        public static string log = "";
        public static string pass = "";
        public login()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }
        public void ChangeRequiredProperties()
        {
            this.Visible = true;
        }
        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }
        Main glav;
        private void button1_Click(object sender, EventArgs e)
        {
            string CommandText = "";
            ConnectionCreator ConCre = new ConnectionCreator(); //Используем метод для открытия соединения с БД
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open();
            CommandText = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";
            MySqlCommand cmd = new MySqlCommand(CommandText, connection);
            cmd.Parameters.Add(new MySqlParameter("@Login", MySqlDbType.VarChar, 20, "Login"));
            cmd.Parameters["@Login"].Value = textBox1.Text;
            cmd.Parameters.Add(new MySqlParameter("@Password", MySqlDbType.VarChar, 20, "Password"));
            cmd.Parameters["@Password"].Value = textBox2.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()) //Считываем данные из БД и заносим в ListView
            {
                log = rdr["Login"].ToString();
                name = rdr["Name"].ToString();
                pass = rdr["Password"].ToString();
                rights = rdr["Misc2"].ToString();
            }
            rdr.Close();
            //connection.Close();
            connection.Close(); //Используем метод для закрытия соединения с БД
            if (String.IsNullOrEmpty(log))
                MessageBox.Show("Неправильные данные входа!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                if (glav == null || glav.IsDisposed)
            {
                glav = new Main(this);
                glav.Show();
                this.Visible = false;
            }
        }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button1.Enabled = true;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button2.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            button2.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled =false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string CommandText = "";
            try
            {
                ConnectionCreator ConCre = new ConnectionCreator();
                MySqlConnection connection = new MySqlConnection();
                connection.ConnectionString = ConCre.buildStr();
                connection.Open(); //Подключаемся к БД
                CommandText = "INSERT INTO Users (Login,Password,Misc2) VALUES (@Login, @Password,'student')";
                MySqlCommand cmd = new MySqlCommand(CommandText, connection);
                using (connection)
                {
                    cmd.Parameters.Add(new MySqlParameter("@Login", MySqlDbType.VarChar, 20, "Login"));
                    cmd.Parameters["@Login"].Value = textBox3.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Password", MySqlDbType.VarChar, 20, "Password"));
                    cmd.Parameters["@Password"].Value = textBox4.Text;
                    int rows = cmd.ExecuteNonQuery();
                }
                Application.Restart();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
