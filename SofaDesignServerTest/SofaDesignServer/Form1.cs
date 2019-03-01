using ServerUser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SofaDesignServer
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            SocketServer socketServer = new SocketServer();
            socketServer.Start();
            txtMsg.BeginInvoke(new Action(() =>
            {
                txtMsg.Text += "服务器已启动！\r" + DateTime.Now.ToString() + "\r\n";
            }));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SocketServer socketServer = new SocketServer();
            socketServer.Close();
            txtMsg.BeginInvoke(new Action(() =>
            {
                txtMsg.Text += "服务器已关闭！\r" + DateTime.Now.ToString() + "\r\n";
            }));

        }
    }
}
