using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;


namespace MysqlTool
{
    internal class MainCode
    {
        public   static String connstr = "server=127.0.0.1;port=3306;user=root;password=apparma3; database=altislife;Charset=utf8;";
        public   static MySqlConnection conn ;
        public  static void Myconn(int OpenorClose) 
        {
            MySqlConnection Conn = new MySqlConnection(connstr);
           
            conn = Conn;
            switch (OpenorClose)
            {
               case 1:{ conn.Open(); }
                    break;
                default: { conn.Close(); }
                    break;
            }
            
        }

        /// <summary>
        /// 主SQL过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="textboxText"></param> UID
        /// <param name="text"></param> 更改内容
        public static void MainSql(string sql,string textboxText,string text) 
        {
            var sqlM = conn.CreateCommand();
            sqlM.CommandText = sql;
            //更新，插入，删除
            var Istrue = sqlM.ExecuteNonQuery();
            if (Istrue == 0)
            {
                MessageBox.Show("UID输入有错或名称太长，请检查");
            }
            else
            {
                MessageBox.Show($"uid:{textboxText}成功更改{text}");
                String log = $"uid:{textboxText}成功更改{text}";
                sql = $"Insert into toollog set log = '{log}'";
                sqlM.CommandText = sql;
                sqlM.ExecuteNonQuery();
            }

        }







    }
}
