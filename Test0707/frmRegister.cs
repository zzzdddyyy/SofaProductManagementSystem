using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Models;
using CommonProtocol;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Test0707
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
            toolTip1.SetToolTip(txtPwd, "有字母和数字组成。");
        }
        private void frmRegister_Load(object sender, EventArgs e)
        {
            rbMale.Checked = true;
        }
        /// <summary>
        /// 提交注册数据到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Socket clientSocket;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 输入校验
            if (string.IsNullOrWhiteSpace(txtname.Text.Trim()))
            {
                MessageBox.Show("请输入姓名", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtname.Focus();
                return;
            }
            if (!CheckLoginInput.IsEmployeeNum(txtJobID.Text.Trim()))
            {
                MessageBox.Show("请输入有效的9位工号！", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtJobID.Focus();
                return;
            }
            if (!CheckLoginInput.IsPwd(txtPwd.Text.Trim()))
            {
                MessageBox.Show("请输入有效的密码！", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwd.Focus();
                return;
            }
            if (txtPwd.Text.Trim() != txtPwd2.Text.Trim())
            {
                MessageBox.Show("两次密码不一致，请重新确认密码！", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwd2.Focus();
                return;
            }
            if (!CheckLoginInput.IsEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("请输入正确的邮箱地址！", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }
            #endregion
            //创建Socket
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //向服务器发送请求*****
            UserInfo userInfo = new UserInfo()
            {
                Name = txtname.Text.Trim(),
                JobID = Int32.Parse(txtJobID.Text.Trim()),
                Gender = rbMale.Checked ? rbMale.Text : rbFemale.Text,
                Age = Int32.Parse(txtAge.Text.Trim()),
                Pwd = txtPwd.Text.Trim(),
                Department = cmbDepatment.Text,
                Email = txtEmail.Text.Trim(),
            };
            //遵守协议
            SofaProtocal sofaProtocal = new SofaProtocal();
            sofaProtocal.data = userInfo;
            sofaProtocal.model = 1;
            sofaProtocal.operate= 1;
            try
            {
                
                Connect("192.168.0.108", 12345);
                Send(sofaProtocal);
                //MessageBox.Show("注册成功！即将返回登录界面。");
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("注册失败！" + ex.Message);
            }
            MessageBox.Show("注册成功，即将返回登录界面！");
            this.Close();
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();

        }
        /// <summary>
        /// 返回登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
        }
        //用户自定义方法
        private  bool CheckInput()
        {
            if (string.IsNullOrWhiteSpace(txtname.Text.Trim()))
            {
                Console.WriteLine("请输入姓名！");
                txtname.Focus();
                return false;
            }
            if (!CheckLoginInput.IsEmployeeNum(txtJobID.Text.Trim()))
            {
                Console.WriteLine("请输入有效的9位工号！");
                txtJobID.Focus();
                return false;
            }
            if (!CheckLoginInput.IsPwd(txtPwd.Text.Trim()))
            {
                Console.WriteLine("请输入有效的密码！");
                txtPwd.Focus();
                return false;
            }
            if(txtPwd.Text.Trim() != txtPwd2.Text.Trim())
            {
                Console.WriteLine("两次密码不一致，请重新确认密码！");
                txtPwd2.Focus();
                return false;
            }
            if (!CheckLoginInput.IsEmail(txtEmail.Text.Trim()))
            {
                Console.WriteLine("请输入正确的邮箱地址！");
                txtEmail.Focus();
                return false;
            }
            return true;
        }

        public void Connect(string ip, int port)
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));//脸上之后开启新的线程，接受用户信息
            Thread threadRecieve = new Thread(Recieve);
            threadRecieve.IsBackground = true;
            threadRecieve.Start();
        }
        public void Send(SofaProtocal protocolDesign)
        {
            MemoryStream memory = new MemoryStream();
            BinaryFormatter binary = new BinaryFormatter();//序列化
            binary.Serialize(memory, protocolDesign);
            byte[] msg = memory.GetBuffer();//返回字节数组
            clientSocket.Send(msg);
        }
        public void Recieve()
        {
            try
            {
                byte[] msg = new byte[1024 * 1024];
                int msgLen = clientSocket.Receive(msg);
                string masStr = Encoding.UTF8.GetString(msg, 0, msgLen);
                MessageBox.Show(masStr);
                Recieve();
            }
            catch (Exception)
            {
                MessageBox.Show("接收出错！");
            }
        }


    }
}
