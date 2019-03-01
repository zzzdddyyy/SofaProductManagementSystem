using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MySql.Data.MySqlClient;
using CommonProtocol;
using System.Windows.Forms;

namespace ServerUser
{
    public class SocketServer
    {
        TextBox txtMsg;
        private Socket server;
        public SocketServer()//构造函数
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           
        }
        //public SocketServer() { }
        public SocketServer(TextBox txt) {
            txtMsg = txt;
        }
        /// <summary>
        /// 启动服务器
        /// </summary>
        public void Start()
        {
            server.Bind(new IPEndPoint(IPAddress.Any, 12345));
            server.Listen(10);
            //因为Accept方法会阻塞线程，直到某个用户连接后，所以需开启新的线程
            Thread threadAccept = new Thread(Accept);
            threadAccept.IsBackground = true;
            threadAccept.Start();
        }
        private void Accept()
        {
            Socket client = server.Accept();
            //某个用户连接后，主线程等待其发送消息，阻塞当前线程，需开启新的线程
            IPEndPoint point = client.RemoteEndPoint as IPEndPoint;
            txtMsg.BeginInvoke(new Action(() =>
            {
                txtMsg.Text += "用户【" + point.Address + "@" + point.Port + "】上线！" + DateTime.Now.ToString() + "\r\n";
            }));
            Thread threadRecieve = new Thread(Recieve);
            threadRecieve.IsBackground = true;
            threadRecieve.Start(client);

            Accept();
        }
        private void Recieve(object obj)
        {
            try
            {
                Socket client = obj as Socket;
                byte[] msg = new byte[1024 * 1024];
                int msgLen = client.Receive(msg);
                //string msgStr = Encoding.UTF8.GetString(msg, 0, msgLen);
                //将对象转成二进制
                MemoryStream memory = new MemoryStream(msg, 0, msgLen);
                BinaryFormatter formatter = new BinaryFormatter();
                var protocolSofa = formatter.Deserialize(memory) as SofaProtocal;//反序列化返回的是对象
                switch (protocolSofa.model)
                {
                    case 1://用户管理模块
                        switch (protocolSofa.operate)
                        {
                            case 1://注册
                                try
                                {
                                    var userInfo = protocolSofa.data as UserInfo;
                                    string sql = "insert into userinfo(uname,jobid,gender,age,upwd,department,email) values " +
                                        "(@name,@jobid,@gender,@age,@pwd,@department,@email)";
                                    int result = CommonProtocol.MySqlHelper.Insert(sql,
                                        new MySqlParameter("@name", userInfo.Name),
                                        new MySqlParameter("@jobid", userInfo.JobID),
                                        new MySqlParameter("@gender", userInfo.Gender),
                                         new MySqlParameter("@age", userInfo.Age),
                                        new MySqlParameter("@pwd", userInfo.Pwd),
                                        new MySqlParameter("@department", userInfo.Department),
                                         new MySqlParameter("@email", userInfo.Email));
                                    if (result == 1)
                                    {
                                        client.Send(Encoding.UTF8.GetBytes("注册成功！"));
                                    }
                                    //连接数据库
                                }
                                catch (Exception ex)
                                {
                                    client.Send(Encoding.UTF8.GetBytes("注册失败！" + ex.Message));
                                    //Console.WriteLine("注册失败！具体原因："+ex.Message);
                                }

                                break;
                            case 2://登录
                                break;
                            case 3://找回密码
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2://聊天
                        break;
                    case 3://数据
                        break;
                    default:
                        break;
                }
                Recieve(obj);
            }
            catch (Exception)
            {
            }
        }
        public void Close()
        {
            if(server != null)
            {
                server.Close();
            }
            server = null;

        }
    }
}
