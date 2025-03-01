using System;
using System.Windows.Forms;

namespace MysqlTool
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            //MessageBox.Show("点击右键刷新即可显示实时数据");
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedplayersData3();
        }

        private void RedplayersData3()
        {

            //读取数据库行数           
            using (var cmd = MainCode.conn.CreateCommand())
            {
                /*
                string sql = "select Count(*) from playerlogs ";
                //先清空
                this.dataGridView1.Rows.Clear();
                cmd.CommandText = sql;
                var Addrowsobj = cmd.ExecuteScalar();//增加的行数
                var Addrows = Convert.ToInt32(Addrowsobj);
                //增加行
                this.dataGridView1.Rows.Add(Addrows);                              
                */
                //全部数据太多卡顿，只查询后2000行的
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Rows.Add(2000);
                //读玩家数据
                string sql = "select logID,playerID,logTitle,log,logTimeStamp from playerlogs order by logID desc limit 2000";
                cmd.CommandText = sql;
                var rdr = cmd.ExecuteReader();
                int i = 0;//取循环的索引 i+1为第几次循环
                while (rdr.Read())//开始每行插入数据
                {
                    int[] nums = { 0, 1, 2, 3, 4 };
                    foreach (var num in nums)
                    {
                        dataGridView1.Rows[i].Cells[num].Value = rdr[num];
                    }
                    i++;
                }
                rdr.Close();
            }


        }


    }
}
