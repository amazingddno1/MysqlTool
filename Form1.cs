using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;



namespace MysqlTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        


        #region 第一窗体操作
        //连接
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "777888999")
            { 
            this.button17.Enabled = true;
            
            }
            try
            {
                MainCode.Myconn(1);
                MessageBox.Show("连接成功");
                
                this.toolStripMenuItem1.Enabled = true;
                this.ToolStripMenuItem2.Enabled = true;
                this.toolStripMenuItem3.Enabled = true;
                this.button6.Enabled = true;
                this.copb.Enabled = true;
                this.medb.Enabled = true;  
                this.civb.Enabled = true;
                this.rebb.Enabled = true;

            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
                
            }
            
            
        }

        //断开
        private void button2_Click(object sender, EventArgs e)
        {
            MainCode.Myconn(0);
            MessageBox.Show("断开连接");
            Application.Exit();
        }

        //查询
        private void button3_Click(object sender, EventArgs e)
        {
            String sql = String.Empty; 
            int IsAffected = 20;//用于判断影响行数 如果要改生成多少个随机码 需要改这里
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请选择ITEM项");
                return;
            }
            try
            {
                sql = $"SELECT Count(*) FROM vip WHERE item = '{comboBox1.SelectedIndex + 1}' AND used='0'";
                MySqlCommand Sqlselect = new MySqlCommand(sql,MainCode.conn);
                
                var Affected = Sqlselect.ExecuteScalar();
                IsAffected = int.Parse(Affected.ToString());
                //ExecuteScalar 执行返回第一行第一列 我用于查询语句
                if (int.Parse(Affected.ToString()) >= 20) //如果要改生成多少个随机码 需要改这里
                { 
                    MessageBox.Show("已经有20个未用过的随机码，请用过后再添加，随机码已经放到框框内部");
                    sql = $"select sn from vip where item = '{comboBox1.SelectedIndex + 1}' And used = '0'";
                    MySqlCommand SqlselectSn = new MySqlCommand(sql, MainCode.conn);
                    var rdr = SqlselectSn.ExecuteReader();
                    //接收到的随机码组合
                    String GetSn = String.Empty;
                    while (rdr.Read())
                    {
                        var ReveiveSn = rdr[0].ToString();
                        GetSn += $"{ReveiveSn}\r\n";
                        //MessageBox.Show(GetSn);                       
                    }
                    richTextBox1.Text = GetSn;
                    
                    rdr.Close();
                    return;
                }
                //MessageBox.Show($"测试:有{int.Parse(Affected.ToString())}行");
                
            }
            catch (InvalidOperationException ex)
            {

                MessageBox.Show($"Mysql连接异常:{ex.Message}");
            }
      


            //将字符串输出为char数组
            var Rletter = "abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLNMOPQRSTUVWXYZ123456789";
            char[] Rlettertochar = Rletter.ToCharArray();
            //指定随机数
            var Rnumber = new Random();
            //防止与上面sql查询行数冲突
            sql = String.Empty;
            //用一个数接收一下随机数
            String Getsnn = String.Empty;

            //指定取随机数字符串并循环 IsAffected为sn有多少
            for (int j = 0; j < (20-IsAffected); j++) //如果要改生成多少个随机码 需要改这里
            {
                string Rnumberstr = string.Empty; //最终获取的16位随机数
                for (int i = 0; i < 16; i++)
                {
                    var Getindex = Rnumber.Next(Rlettertochar.Length);
                    Rnumberstr += Rlettertochar[Getindex];
                    
                }
                Getsnn += Rnumberstr + "\r\n";
                //开始插入数据

                sql += $"INSERT INTO vip (sn, item)VALUES ( '{Rnumberstr}', '{comboBox1.SelectedIndex + 1}');";
            }


                try
            {              
                MySqlCommand cmd = new MySqlCommand(sql, MainCode.conn);
                var IsTrue = cmd.ExecuteNonQuery();
                if (IsTrue > -1 )
                {
                    MessageBox.Show("生成随机码成功");
                    //将随机码放入编辑框
                    richTextBox1.Text = Getsnn;
                }
                else
                {
                    MessageBox.Show("Mysql异常或你没有选择item生成项");
                }

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Mysql连接异常:{ex.Message}");
                
            }

            //MessageBox.Show((comboBox1.SelectedIndex+1).ToString());


        }

        //复制
        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
             MessageBox.Show("编辑框内容是空的");
                return;
            }
            Clipboard.SetText(richTextBox1.Text);
            MessageBox.Show("随机码已经复制到剪贴板");
        }

        //生成TXT文件
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || richTextBox1.Text == String.Empty)
            {
                MessageBox.Show("请选择ITEM项或你没有生成随机码在文本框中");
                return;
            }
            var path = Application.StartupPath;//获取当前程序目录
            var GetDirectoryAllname = Path.Combine(path, "Random");//获取创建文件夹目录
            if (!Directory.Exists(GetDirectoryAllname))//如果不存在则创建目录
            {
               Directory.CreateDirectory(GetDirectoryAllname);
            }
            
            var pathtxt = Path.Combine(GetDirectoryAllname, $@"{DateTime.Now.ToLongDateString()}  ITEM={comboBox1.SelectedIndex+1} .txt");//具体文件目录

            try
            {
                File.WriteAllText(pathtxt,richTextBox1.Text);
                MessageBox.Show("随机码已写入程序目录下文件名:'Random'里，请打开查看");
            }
            catch (System.Security.SecurityException ex)
            {

                MessageBox.Show($"你没有相应权限，请使用管理员运行程序:{ex.Message}");
            }



        }



        /// <summary>
        /// 白名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                String sqlWhite = $"select COUNT(*) from players where playerid = '{this.textBox5.Text}' AND civlevel = '1' ";

                var sqlselect = MainCode.conn.CreateCommand();
                sqlselect.CommandText = sqlWhite;
                var Affected = sqlselect.ExecuteScalar();
                if (int.Parse(Affected.ToString()) != 0)
                {
                    sqlWhite = $"UPDATE players set civlevel = '2' where playerid = '{this.textBox5.Text}' ";
                    sqlselect.CommandText = sqlWhite;
                    sqlselect.ExecuteNonQuery();
                    MessageBox.Show($"uid:{this.textBox5.Text}成功加入白名单");
                }
                else
                {
                    MessageBox.Show($"uid:{this.textBox5.Text}玩家不存在或已加入白名单");

                }
            }
            catch (InvalidOperationException ex )
            {

                MessageBox.Show($"mysql连接异常:{ex.Message}");
            }




        }

        /// <summary>
        /// 充值金额
        /// </summary>
        /// <param name="textBox6"></param> 按钮对应输入框
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {


            try
            {
                if (int.Parse(this.textBox6.Text) < 0 || int.Parse(this.textBox6.Text) > 9999999)
                {
                    MessageBox.Show($"输入金额不能小于0，大于999999");
                    return;

                }

                String sql = $"select bankacc from players where playerid = '{this.textBox5.Text}' ";

                var sqlselect = MainCode.conn.CreateCommand();
                sqlselect.CommandText = sql;
                var Affected = sqlselect.ExecuteScalar();//返回第一行第一列
                if (Affected != null)
                {
                    var money = (int)Affected;
                    
                    var Add = Convert.ToInt32(this.textBox6.Text);//增加金额
                    sql = $"UPDATE players set bankacc = '{money+ Add}' where playerid = '{this.textBox5.Text}' ";
                    sqlselect.CommandText = sql;
                    sqlselect.ExecuteNonQuery();
                    MessageBox.Show($"uid:{this.textBox5.Text}成功增加金额{Add}");
                    //插入数据库查询LOG
                    String log = $"uid:{this.textBox5.Text}成功更改增加金额{Add}";
                    sql = $"Insert into toollog set log = '{log}'";
                    sqlselect.CommandText = sql;
                    sqlselect.ExecuteNonQuery();

                }
                else
                {
                    MessageBox.Show($"uid:{this.textBox5.Text}玩家不存在，请检查输入uid");

                }
            }
            catch (NullReferenceException ex)
            {

                MessageBox.Show($"mysql未连接:{ex.Message}");
            }
            catch ( OverflowException )
            {

                MessageBox.Show($"你输入的值过大");
            }

        }

        /// <summary>
        /// 上平3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {

            try
            {
                String sqlWhite = $"UPDATE players set civlevel = '1' where playerid = '{this.textBox5.Text}'";

                var sqlselect = MainCode.conn.CreateCommand();
                sqlselect.CommandText = sqlWhite;
                var Istrue = sqlselect.ExecuteNonQuery();
                //判断影响行数 没有数据则返回0 成功则返回1 若为select语句则为-1
                if (Istrue == 0)
                {
                    MessageBox.Show("UID输入有错或名称太长，请检查");
                }
                else
                {
                    MessageBox.Show($"uid:{this.textBox5.Text}成功更改平民等级为 1");
                    String log = $"uid:{this.textBox5.Text}成功更改平民等级为 1";
                    sqlWhite = $"Insert into toollog set log = '{log}'";
                    sqlselect.CommandText = sqlWhite;
                    sqlselect.ExecuteNonQuery();
                }


            }
            catch (InvalidOperationException ex)
            {

                MessageBox.Show($"mysql连接异常:{ex.Message}");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"未连接数据库");
            }


        }

        /// <summary>
        /// 上叛3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {

            try
            {
                String sqlWhite = $"UPDATE players set reblevel = '1' where playerid = '{this.textBox5.Text}'";

                var sqlselect = MainCode.conn.CreateCommand();
                sqlselect.CommandText = sqlWhite;
                var Istrue = sqlselect.ExecuteNonQuery();
                //判断影响行数 没有数据则返回0 成功则返回1 若为select语句则为-1
                if (Istrue == 0)
                {
                    MessageBox.Show("UID输入有错或名称太长，请检查");
                }
                else
                {
                    MessageBox.Show($"uid:{this.textBox5.Text}成功更改叛军等级为 1");
                    String log = $"uid:{this.textBox5.Text}成功更改叛军等级为 1";
                    sqlWhite = $"Insert into toollog set log = '{log}'";
                    sqlselect.CommandText = sqlWhite;
                    sqlselect.ExecuteNonQuery();
                }


            }
            catch (InvalidOperationException ex)
            {

                MessageBox.Show($"mysql连接异常:{ex.Message}");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"未连接数据库");
            }

        }

        /// <summary>
        /// 下警察位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            String sqlWhite = $"UPDATE players set coplevel = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "警察等级为 0";
            MainCode.MainSql(sqlWhite,this.textBox5.Text, Text);
        }

        /// <summary>
        /// 上最高警察位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            String sqlWhite = $"UPDATE players set coplevel = '20' where playerid = '{this.textBox5.Text}'";
            String Text = "警察等级为 20";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }

        /// <summary>
        /// 下医生位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            String sqlWhite = $"UPDATE players set mediclevel = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "医生等级为 0";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }

        /// <summary>
        /// 上医生位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            String sqlWhite = $"UPDATE players set mediclevel = '3' where playerid = '{this.textBox5.Text}'";
            String Text = "医生等级为 3";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }

        /// <summary>
        /// 监狱时间清零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {

            String sqlWhite = $"UPDATE players set jail_time = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "监狱时间为 0";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        
        }

        /// <summary>
        /// 发放总统位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 改名
        /// </summary>
        /// <param name="textBox7"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {

            try
            {
                String sqlWhite = $"UPDATE players set name = '{this.textBox7.Text}' where playerid = '{this.textBox5.Text}'";

                var sqlselect = MainCode.conn.CreateCommand();
                sqlselect.CommandText = sqlWhite;
                var Istrue = sqlselect.ExecuteNonQuery();
                //判断影响行数 没有数据则返回0 成功则返回1 若为select语句则为-1
                if (Istrue == 0)
                {
                    MessageBox.Show("UID输入有错或名称太长，请检查");
                }
                else
                {
                    MessageBox.Show($"uid:{this.textBox5.Text}成功修改姓名为:{this.textBox7.Text}");
                    String log = $"uid:{this.textBox5.Text}成功修改姓名为:{this.textBox7.Text}";
                    sqlWhite = $"Insert into toollog set log = '{log}'";
                    sqlselect.CommandText = sqlWhite;
                    sqlselect.ExecuteNonQuery();
                }


            }
            catch (InvalidOperationException ex)
            {

                MessageBox.Show($"mysql连接异常:{ex.Message}");
            }
            catch (NullReferenceException )
            {
                MessageBox.Show($"未连接数据库");
            }
        }

        //充值金额事件
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (!(e.KeyChar == 8 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
            {
                e.Handled = true;
            }

        }

        //输入UID事件
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = true;

        }
        //读取操作记录
        private void button17_Click(object sender, EventArgs e)
        {
            var sql = "select log from toollog";
            try
            {
                var sqlR = MainCode.conn.CreateCommand();
                sqlR.CommandText = sql;
                var rdr = sqlR.ExecuteReader();
                var Receivelog = String.Empty;
                while (rdr.Read())
                {
                    var Receive = rdr[0].ToString();
                    Receivelog = Receivelog + Receive + "\r\n";
                }

                this.richTextBox1.Text = Receivelog;
                rdr.Close();
                this.button17.Enabled = false;
                this.textBox1.Text = "";

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("数据库未连接");
            }


        }

        //下平民位
        private void button18_Click(object sender, EventArgs e)
        {
            String sqlWhite = $"UPDATE players set civlevel = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "平民等级为 0";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }
        
        //下叛军位
        private void button19_Click(object sender, EventArgs e)
        {
            String sqlWhite = $"UPDATE players set reblevel = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "叛军等级为 0";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }
        #endregion
        #region 右键
        /// <summary>
        /// 玩家日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击右键刷新即可显示实时数据");
            Form3 form3 = new Form3();
            form3.ShowDialog();
            
        }

        /// <summary>
        /// 玩家数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击右键刷新即可显示实时数据");
            Form2 form2 = new Form2();
            form2.Show();
        }


        #endregion


        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击右键刷新即可显示实时数据");
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }
        #region 上等级
        //警察
        private void copb_Click(object sender, EventArgs e)
        {
            if (this.textBox5.Text == "") {
                MessageBox.Show("请粘贴UID到框框内"); return; 
            }
            if (this.comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择需要的选项");return;
            }

            if (int.Parse(this.comboBox2.Text) > 28)
            {
                MessageBox.Show("警察等级最高为28级"); return;
            }

            String sqlWhite = $"UPDATE players set coplevel ='{this.comboBox2.Text}',mediclevel ='0',civlevel='0',reblevel='0' where playerid = '{this.textBox5.Text}'";
            String Text = $"警察等级为 {this.comboBox2.Text}";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
            //MessageBox.Show(this.comboBox2.Text);
        }
        #endregion

        private void medb_Click(object sender, EventArgs e)
        {
            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请粘贴UID到框框内"); return;
            }
            if (this.comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择需要的选项"); return;
            }

            if (int.Parse(this.comboBox2.Text) > 7)
            {
                MessageBox.Show("医生等级最高为7级"); return;
            }

            String sqlWhite = $"UPDATE players set coplevel ='0',mediclevel ='{this.comboBox2.Text}',civlevel='0',reblevel='0' where playerid = '{this.textBox5.Text}'";
            String Text = $"医生等级为 {this.comboBox2.Text}";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }

        private void civb_Click(object sender, EventArgs e)
        {
            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请粘贴UID到框框内"); return;
            }
            if (this.comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择需要的选项"); return;
            }

            if (int.Parse(this.comboBox2.Text) > 1)
            {
                MessageBox.Show("只能上普通平民，平3用充值码兑换"); return;
            }

            String sqlWhite = $"UPDATE players set coplevel ='0',mediclevel ='0',civlevel='{this.comboBox2.Text}',reblevel='0' where playerid = '{this.textBox5.Text}'";
            String Text = $"平民等级为 {this.comboBox2.Text}";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }

        private void rebb_Click(object sender, EventArgs e)
        {
            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请粘贴UID到框框内"); return;
            }
            if (this.comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择需要的选项"); return;
            }

            if (int.Parse(this.comboBox2.Text) > 1)
            {
                MessageBox.Show("只能上普通叛军，叛3用充值码兑换"); return;
            }

            String sqlWhite = $"UPDATE players set coplevel ='0',mediclevel ='0',civlevel='0',reblevel='{this.comboBox2.Text}' where playerid = '{this.textBox5.Text}'";
            String Text = $"叛军等级为 {this.comboBox2.Text}";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请粘贴UID到框框内"); return;
            }
            String sqlWhite = $"UPDATE players set jail_time = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "监狱时间为 0";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }
        //上白名单
        private void button7_Click_1(object sender, EventArgs e)
        {

            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请粘贴UID到框框内"); return;
            }
            String sqlWhite = $"UPDATE players set EPoint = '1' where playerid = '{this.textBox5.Text}'";
            String Text = "上了白名单";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);


        }

        //下白名单
        private void button8_Click_1(object sender, EventArgs e)
        {

            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请粘贴UID到框框内"); return;
            }
            String sqlWhite = $"UPDATE players set EPoint = '0' where playerid = '{this.textBox5.Text}'";
            String Text = "下了白名单";
            MainCode.MainSql(sqlWhite, this.textBox5.Text, Text);
        }
    }

}

