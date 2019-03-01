using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using Common;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Test0707
{
    public partial class frmLogin : Form
    {
        //零件配置界面
        public static Form1 frmMain = null;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();//关闭窗口
        }
        /// <summary>
        /// 登录btn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //定义数据库原文本命令对象，初始值为null
            MySqlCommand myCmd = null;
            //定义数据库连接对象，初始值为null
            MySqlConnection connection = null;
            //判断输入是否有效
            if (!CheckLoginInput.IsEmployeeNum(txtUser.Text.Trim()))
            {
                MessageBox.Show("请输入有效的9位工号！");
                txtUser.Focus();
            }
            if (string.IsNullOrWhiteSpace(txtUser.Text.Trim())){
                MessageBox.Show("请输入工号！");
                txtUser.Focus();
            }

            if (string.IsNullOrWhiteSpace(txtPwd.Text.Trim()))
            {
                MessageBox.Show("请输入密码！");
                txtPwd.Focus();
            }
            if (string.IsNullOrWhiteSpace(cmbox.Text))
            {
                MessageBox.Show("请选择所属部门！");
                cmbox.Focus();
            }
            //连接数据库
            try
            {
                string connStr = "server='localhost';database='product';user='root';password='';";
                connection = new MySqlConnection(connStr);
                connection.Open();//打开数据库
                                  //对比账号密码合法性
                                  //Create a list to store the result
                List<string>[] queryList = new List<string>[3];
                queryList[0] = new List<string>();
                queryList[1] = new List<string>();
                queryList[2] = new List<string>();
                try
                {
                    //SQl语句
                    string sqlStr = string.Format("SELECT COUNT(*) FROM users WHERE u_id = '{0}' AND u_pwd = '{1}' ", 
                        this.txtUser.Text.Trim(), this.txtPwd.Text.Trim());
                    //创建执行语句
                     myCmd = new MySqlCommand(sqlStr, connection);
                    int queryCount = Convert.ToInt32(myCmd.ExecuteScalar());//执行SQl语句，返回结果集的第一行第一列值，是object类型，通过Convert转换
                    if (queryCount == 1)
                    {
                        myCmd = null;
                        string sqlDepart = string.Format("SELECT u_depart from users WHERE u_id = '{0}' AND u_pwd = '{1}' ", this.txtUser.Text.Trim(), this.txtPwd.Text.Trim());
                        //创建执行语句
                        myCmd = new MySqlCommand(sqlDepart, connection);
                        MySqlDataReader myDR = myCmd.ExecuteReader();
                        while (myDR.Read())
                        {
                            queryList[0].Add(Convert.ToString(myDR["u_depart"]));
                        }
                        myDR.Close();
                        if (queryList[0].Contains(this.cmbox.Text))
                        {
                            Program.currentUser = txtUser.Text.Trim();
                            // MessageBox.Show("登录成功！");
                            this.Hide();
                            //登录成功，打开主界面
                            Program.isLogin = true;
                            //隐藏登录界面。打开部件管理平台
                            frmMain = new Form1();
                            frmMain.Show();

                        }
                        
                        else
                        {
                            MessageBox.Show("请选择正确的部门！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cmbox.Focus();
                            return;
                        };
                    }
                    else
                    {
                        //如果用户密码不正确，则COUNT(*)查询结果为0，此时为登录失败。
                        MessageBox.Show("账户或密码错误，请重新输入。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //全选密码文本框
                        txtPwd.SelectAll();
                        //获得焦点
                        txtPwd.Focus();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //如果发生错误，则提示错误信息。
                    MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    //释放SqlCommand命令对象。释放前先判断 bcommand 不为 null，这样代码更加充实
                    if (myCmd != null)
                    {
                        myCmd.Dispose();
                    }


                    //关闭SqlConneticon 连接对象。释放前先判断 aconneticon 不为 null。
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
               
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to Server!Please contact Administrator.", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username or password,please try again!", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        break;
                }
            }
           
        }
        frmRegister register = null;
        /// <summary>
        /// 注册新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (register == null)
            {
                register = new frmRegister();
                register.Show();//向用户显示窗体
            }
            else register.Activate();
            //隐藏登录窗体
            this.Hide();
        }
    }
}
