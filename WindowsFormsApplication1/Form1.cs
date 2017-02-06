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
    public partial class Form1 : Form
    {
        Main m1;
        private ConnectionCreator ConCre = new ConnectionCreator();
        public Form1(Main m)
        {
            m1 = m;
            InitializeComponent();
            creature();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open(); //Подключаемся к БД
            string checkingorg = "";
            string checkingdate = "";
            string CommandText = "";
            
            if (!(checkedListBox2.CheckedItems.Count == 0)) //Задаем значения для параметра "страна"
            {
                checkingdate = " AND Date IN (";
                foreach (object itemChecked in checkedListBox2.CheckedItems)
                    checkingdate = checkingdate + "'" + itemChecked.ToString() + "'" + ",";
                checkingdate = checkingdate.Remove(checkingdate.Length - 1, 1);
                checkingdate = checkingdate + ")";
            }
            if (!(checkedListBox1.CheckedItems.Count == 0)) //Задаем значения для параметра "жанр"
            {
                checkingorg = " AND Org IN (";
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                    checkingorg = checkingorg + "'" + itemChecked.ToString() + "'" + ",";
                checkingorg = checkingorg.Remove(checkingorg.Length - 1, 1);
                checkingorg = checkingorg + ")";
            }
            
            //Изменяем строку запроса к БД в зависимости от выбранных параметров "страна" и "жанр"
            if (radioButton1.Checked && checkBox1.Checked)
                CommandText = "SELECT * FROM events WHERE Name LIKE @Name" + checkingorg + checkingdate + " ORDER BY Name DESC";
            if (radioButton1.Checked && !checkBox1.Checked)
                CommandText = "SELECT * FROM events WHERE Name LIKE @Name" + checkingorg + checkingdate + " ORDER BY Name";
            if (radioButton2.Checked && checkBox1.Checked)
                CommandText = "SELECT * FROM events WHERE Name LIKE @Name" + checkingorg + checkingdate + " ORDER BY Rating DESC";
            if (radioButton2.Checked && !checkBox1.Checked)
                CommandText = "SELECT * FROM events WHERE Name LIKE @Name" + checkingorg + checkingdate + " ORDER BY Rating ";
            //Конец изменения
            MySqlCommand cmd = new MySqlCommand(CommandText); //Команда на основе строки
            cmd.Connection = connection;
            //Добавляем параметры строки
            cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar, 40, "Name"));
            cmd.Parameters["@Name"].Value = "%" + textBox1.Text.ToString() + "%";
            MySqlDataReader rdr = cmd.ExecuteReader();
            listView1.Items.Clear(); //Очищаем ListView, чтобы не засорялся мусором
            while (rdr.Read()) //Считываем данные из БД и заносим в ListView
            {
                ListViewItem item = new ListViewItem(new[] { rdr["Name"].ToString(), rdr["Date"].ToString(), rdr["Org"].ToString()});
                listView1.Items.Add(item);
            }
            rdr.Close();
            connection.Close();
        }
        public void creature() //Метод для построения формы
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConCre.buildStr();
            connection.Open();
            string CommandText = "SELECT * FROM Events ORDER BY Name";
            MySqlCommand cmd = new MySqlCommand(CommandText);
            cmd.Connection = connection;
            MySqlDataReader rdr = cmd.ExecuteReader();
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            listView1.Items.Clear();
            radioButton1.Checked = true;
            while (rdr.Read())
            {
                ListViewItem item = new ListViewItem(new[] { rdr["Name"].ToString(), rdr["Date"].ToString(), rdr["Org"].ToString()});
                listView1.Items.Add(item);
                if (!checkedListBox1.Items.Contains(rdr["Org"].ToString()))
                    checkedListBox1.Items.Add(rdr["Org"].ToString());
                if (!checkedListBox2.Items.Contains(rdr["Date"].ToString()))
                    checkedListBox2.Items.Add(rdr["Date"].ToString());
            }
            rdr.Close();
            connection.Close();
        }
        public string imya = "";
        public string organizer = "";
        Form4 f4;
        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices[0] != -1)
            {
                imya = Convert.ToString(listView1.SelectedItems[0].Text);
                organizer = Convert.ToString(listView1.SelectedItems[0].SubItems[2].Text);
                if (f4 == null || f4.IsDisposed)
                {
                    f4 = new Form4(this);
                    f4.Show();
                }
            }
        }
    }
}

