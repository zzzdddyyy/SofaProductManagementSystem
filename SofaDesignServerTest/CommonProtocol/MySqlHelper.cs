using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CommonProtocol
{
    public class MySqlHelper
    {
        private static string connStr = "server = localhost;port=3306;database =product ;user= root;password=;pooling = false;charset = utf8;";
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static int Insert(string sql, params MySqlParameter[] parameter)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameter);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
