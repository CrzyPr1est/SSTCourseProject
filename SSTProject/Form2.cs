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

namespace SSTProject
{
    public partial class Form2 : Form
    {
        Main m1;
        private ConnectionCreator ConCre = new ConnectionCreator();
        public Form2(Main m)
        {
            m1 = m;
            InitializeComponent();
            listy();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            string deff = "SELECT * FROM Events WHERE id LIKE " + comboBox2.SelectedItem;
            MySqlCommand cmd1 = new MySqlCommand(deff);
            cmd1.Connection = connection;
            MySqlDataReader rdr = cmd1.ExecuteReader();
            // Задаем текст кнопки в зависимости от действия
            if (comboBox1.SelectedIndex == 0)
                button1.Text = "Изменить";
            if (comboBox1.SelectedIndex == 1)
                button1.Text = "Добавить";
            if (comboBox1.SelectedIndex == 2)
                button1.Text = "Удалить";
            // Задали
            // Если нам надо добавить или удалить запись,
            // то поля с данными должны быть пустыми
            //Очищаем поля
            if (comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                maskedTextBox3.Text = "";
            }
            // Очистили
            // Если нам надо добавить запись, то id определится автоматически, заблокируем поле ввода данных в id
            if (comboBox1.SelectedIndex == 1)
                comboBox2.Enabled = false;
            // Если надо изменить или удалить запись, разблокируем поле ввода данных в id
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 2)
                comboBox2.Enabled = true;
            // Если надо удалить запись, то заблокируем поля ввода всех параметров кроме id
            if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Enabled = false; textBox2.Enabled = false; textBox3.Enabled = false; maskedTextBox3.Enabled = false;
            }
            // Если надо добавить или изменить запись, разблокируем поля ввода данных
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1)
            {
                textBox1.Enabled = true; textBox2.Enabled = true; textBox3.Enabled = true; maskedTextBox3.Enabled = true;
            }
            // Если надо изменить запись, введем в поля ввода данные по-умолчанию
            if (comboBox1.SelectedIndex == 0)
            {
                while (rdr.Read())
                {
                    textBox1.Text = rdr["Name"].ToString();
                    maskedTextBox3.Text = Convert.ToString(rdr["Date"]);
                    textBox3.Text = rdr["Org"].ToString();
                    textBox2.Text = rdr["Misc"].ToString();
                }
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open();
            string deff = "SELECT * FROM Events WHERE Id LIKE " + comboBox2.SelectedItem;
            MySqlCommand cmd1 = new MySqlCommand(deff);
            cmd1.Connection = connection;
            MySqlDataReader rdr = cmd1.ExecuteReader();
            if (comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                maskedTextBox3.Text = "";
            }
            if (comboBox1.SelectedIndex == 0)
            {
                while (rdr.Read())
                {
                    textBox1.Text = rdr["Name"].ToString();
                    maskedTextBox3.Text = Convert.ToString(rdr["Date"]);
                    textBox3.Text = rdr["Org"].ToString();
                    textBox2.Text = rdr["Misc"].ToString();
                }
            }
        }

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
                    if (comboBox1.SelectedIndex == 0) // Формирование строки запроса к БД при изменении записи
                    {
                        CommandText = "UPDATE events SET Name = @Name, Date = @Date, Org = @Org, Misc = @Misc WHERE Id = @Id";
                    }
                    if (comboBox1.SelectedIndex == 1) // Формирование строки запроса к БД при добавлении записи
                    {
                        CommandText = "INSERT INTO events (Id,Name,Date,Org,Misc) VALUES (@Id, @Name, @Date, @Org, @Misc)";
                    }
                    if (comboBox1.SelectedIndex == 2) // Формирование строки запроса к БД при удалении записи
                    {
                        CommandText = "DELETE FROM events WHERE Id = @Id";
                    }
                    MySqlCommand cmd = new MySqlCommand(CommandText, connection);
                    //Параметры
                    cmd.Parameters.Add(new MySqlParameter("@Id", MySqlDbType.Int32, 4, "ID"));
                    if (comboBox1.SelectedIndex == 1)
                        cmd.Parameters["@Id"].Value = comboBox2.Items.Count + 1;
                    else
                        cmd.Parameters["@Id"].Value = Convert.ToInt32(comboBox2.SelectedItem.ToString());
                    cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.Text, 40, "Name"));
                    cmd.Parameters["@Name"].Value = textBox1.Text;
                    if (cmd.Parameters["@Name"].Value.ToString() == "")
                        cmd.Parameters["@Name"].Value = "NOTDEFINED!";
                    cmd.Parameters.Add(new MySqlParameter("@Date", MySqlDbType.VarChar, 20, "Date"));
                    cmd.Parameters["@Date"].Value = maskedTextBox3.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Org", MySqlDbType.VarChar, 40, "Org"));
                    cmd.Parameters["@Org"].Value = textBox3.Text;
                    cmd.Parameters.Add(new MySqlParameter("@Misc", MySqlDbType.VarChar, 3000, "Misc"));
                    cmd.Parameters["@Misc"].Value = textBox2.Text;
                    //Параметры
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            safy(); // Метод, не позволяющий оставить пустую базу данных
            reffy(); // Метод, предотвращающий сбивание id при удалении записи
            listy(); // Метод, наполняющий форму (т.к. данные изменились)
        }
        private void listy()
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); 
            string CommandText = "SELECT * FROM Events ORDER BY Id";
            MySqlCommand cmd = new MySqlCommand(CommandText);
            cmd.Connection = connection;
            MySqlDataReader rdr = cmd.ExecuteReader();
            listView1.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            while (rdr.Read())
            {
                ListViewItem item = new ListViewItem(new[] { rdr["Id"].ToString(), rdr["Name"].ToString(), rdr["Date"].ToString(), rdr["Org"].ToString()});
                listView1.Items.Add(item);
                comboBox2.Items.Add(rdr["Id"].ToString());
            }
            rdr.Close();
            // Наполняем значениями ComboBox
            comboBox2.SelectedIndex = 0;
            comboBox1.Items.Add("Изменить");
            comboBox1.Items.Add("Добавить");
            comboBox1.Items.Add("Удалить");
            comboBox1.SelectedIndex = 0;
            string deff = "SELECT * FROM Events WHERE Id LIKE 1";
            MySqlCommand cmd1 = new MySqlCommand(deff);
            cmd1.Connection = connection;
            rdr = cmd1.ExecuteReader();
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); maskedTextBox3.Clear();
            while (rdr.Read()) // Наплняем значениями TextBox
            {
                textBox1.Text = rdr["Name"].ToString();
                maskedTextBox3.Text = Convert.ToString(rdr["Date"]);
                textBox3.Text = rdr["Org"].ToString();
                textBox2.Text = rdr["Misc"].ToString();
            }
            string[] text = new string[8];
            text[0] = textBox1.Text; text[1] = maskedTextBox3.Text; text[2] = textBox3.Text; text[4] = textBox2.Text;
            connection.Close();
        }
        public void reffy() // Используется при удалении строки
        {
            if (comboBox1.SelectedIndex == 2)
            {
                MySqlConnection connection = new MySqlConnection();
                connection.ConnectionString = ConCre.buildStr();
                connection.Open();
                // Строка запроса к БД, "смещающая" ID, у полей, где ID больше удаленного 
                string CommandText = "UPDATE events SET Id = Id - 1 WHERE id > @id";
                MySqlCommand cmd = new MySqlCommand(CommandText);
                cmd.Connection = connection;
                cmd.Parameters.Add(new MySqlParameter("@Id", MySqlDbType.Int32, 4, "ID"));
                cmd.Parameters["@Id"].Value = Convert.ToInt32(comboBox2.SelectedItem.ToString());
                int rows = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void safy() // Используется при удалении последней записи из БД
        {
            if (comboBox1.SelectedIndex == 2 && comboBox2.Items.Count == 1)
            {
                MySqlConnection connection = new MySqlConnection();
                connection.ConnectionString = ConCre.buildStr();
                connection.Open();
                string CommandText = "INSERT INTO events (Name) VALUES ('UNDEFINED')";
                MySqlCommand cmd = new MySqlCommand(CommandText);
                cmd.Connection = connection;
                int rows = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
