using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiCommon
{
    public class AdoTools
    {
        /// <summary>
        /// 执行指定的sql语句，返回执行结果
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="conn">要查询的数据库的连接字符串</param>
        public SqlDataReader adoExecuteSQL(string sql,string conn)
        {
            List<string> dptNames = new List<string>();
            List<int> stfCount = new List<int>();
            try {
                SqlConnection myConn = new SqlConnection(conn);
                SqlCommand myCommand = new SqlCommand(sql, myConn);
                myConn.Open();
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.NextResult())
                {
                    dptNames.Add(reader.GetString(0));
                    stfCount.Add(reader.GetInt32(1));
                }
            }
            catch (Exception e)
            {
                var tmp = e;
            }

            return null;
        }
    }
}
