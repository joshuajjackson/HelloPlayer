using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DBConnector
    {
        private static string connectionString =
            @"Data Source=localhost\sqlexpress;Initial Catalog=HelloPlayerDB;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
