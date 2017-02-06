using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace SSTProject
{
    class ConnectionCreator
    {
        private What wh = new What();
        public string buildStr ()
        {
            MySqlConnectionStringBuilder mysqlCSB;
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "db4free.net";
            mysqlCSB.Port = 3307;
            mysqlCSB.Database = "sst_base";
            mysqlCSB.UserID = "student_pp";
            mysqlCSB.Password = wh.DeShifrovka("lvEMt3PRl7ied9cWUFGBOg==", "HESOYAM");
            string connstr = mysqlCSB.ConnectionString;
            return connstr;
        }
        
    }
}
