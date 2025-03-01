using System;
using System.Windows.Forms;


namespace MysqlTool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        #region 刷新
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedplayersData();
        }

        #endregion

        private void RedplayersData()
        {

            //读取数据库行数           
            using (var cmd = MainCode.conn.CreateCommand())
            {
                string sql = "select Count(*) from players";
                //先清空
                this.dataGridView1.Rows.Clear();
                cmd.CommandText = sql;
                var Addrowsobj = cmd.ExecuteScalar();//增加的行数
                var Addrows = Convert.ToInt32(Addrowsobj);
                //增加行
                this.dataGridView1.Rows.Add(Addrows);
                //读玩家数据
                sql = "select name,playerid,cash,bankacc,mediclevel,coplevel,civlevel,reblevel,donorlevel,donat_time,EPoint,cop_licenses,cop4_time,civ_time,reb_time,tiexue_time,tiexue from players";
                cmd.CommandText = sql;
                var rdr = cmd.ExecuteReader();
                int i = 0;//取循环的索引 i+1为第几次循环
                while (rdr.Read())//开始每行插入数据
                {                   
                    int[] nums = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14,15,16};
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
