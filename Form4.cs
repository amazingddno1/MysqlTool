using System;
using System.Windows.Forms;

namespace MysqlTool
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            RedplayersData3();

        }

        private void RedplayersData3()
        {

            //读取数据库行数           
            using (var cmd = MainCode.conn.CreateCommand())
            {
                string sql = "select Count(*) from vip";
                //先清空
                this.dataGridView1.Rows.Clear();
                cmd.CommandText = sql;
                var Addrowsobj = cmd.ExecuteScalar();//增加的行数
                var Addrows = Convert.ToInt32(Addrowsobj);
                //增加行
                this.dataGridView1.Rows.Add(Addrows);
                //读玩家数据
                sql = "select sn,item,used,usedtime,userinfo from vip";
                cmd.CommandText = sql;
                var rdr = cmd.ExecuteReader();
                int i = 0;//取循环的索引 i+1为第几次循环
                while (rdr.Read())//开始每行插入数据
                {
                    int[] nums = { 0, 1, 2, 3, 4 };
                    foreach (var num in nums)
                    {
                        dataGridView1.Rows[i].Cells[num].Value = rdr[num].ToString();
                    }
                    i++;
                }
                rdr.Close();
            }


        }




    }
}
