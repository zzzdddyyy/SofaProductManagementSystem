using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonProtocol;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Models
{
    public class ClientManage
    {
        private Socket client;
        public ClientManage()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect(string ip, int port)
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));//脸上之后开启新的线程，接受用户信息
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
            client.Send(msg);
        }
        public void Recieve()
        {
            try
            {
                byte[] msg = new byte[1024 * 1024];
                int msgLen = client.Receive(msg);
                string masStr = Encoding.UTF8.GetString(msg, 0, msgLen);
                Console.WriteLine(masStr);
                Recieve();
            }
            catch (Exception)
            {
                Console.WriteLine("Recieve Error!");
            }
        }
    }
}
