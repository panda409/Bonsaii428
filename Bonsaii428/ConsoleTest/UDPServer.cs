using Riss.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class UDPServer
    {
        private IPEndPoint ipLocalPoint;
        private EndPoint remotePoint;
        private Socket serverSocket;
        private bool RunningFlag = false;
        public UDPServer()
        {
          

        }

        /// <summary>
        /// 获取本机局域网IP地址
        /// </summary>
        /// <returns></returns>
        private string getIPAddress()
        {
            IPAddress[] AddressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            if (AddressList.Length < 1)
                return "";
            return AddressList[0].ToString();
        }
        /// <summary>
        /// 验证端口号的有效性
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private int getValidPort(string port)
        {
            int tmpPort;
            //测试端口号时候有效
            try
            {
                if (port == "")
                    throw new ArgumentException("端口号无效，不能启动UDP");
                tmpPort = System.Convert.ToInt32(port);
            }
            catch (Exception e)
            {
                Console.WriteLine("无效的端口号: " + e.ToString());
                return -1;
            }
            return tmpPort;
        }

        /// <summary>
        /// 验证IP的有效性
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private IPAddress getValidIP(string ip)
        {
            IPAddress lip = null;
            try
            {
                if (!IPAddress.TryParse(ip, out lip))
                {
                    throw new ArgumentException("IP无效，不能启动UDP");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("无效的IP：" + e.ToString());
                return null;
            }
            return lip;
        }
        /// <summary>
        /// 读取机器硬件数据，并写入文件
        /// </summary>
        public void recvDataFromDeviceUdp()
        {
            int recv;
            byte[] buf = new byte[1024];
            ipLocalPoint = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 0, 32 }), 5055);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(ipLocalPoint);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remote = (EndPoint)(sender);

            recv = serverSocket.ReceiveFrom(buf, ref remote);//接受数据           
            Console.WriteLine("Message received from{0}:", remote.ToString());
          
       
            while (true)
            {
                buf = new byte[1024];
                recv = serverSocket.ReceiveFrom(buf, ref remote);
                string tmp = Encoding.GetEncoding("GBK").GetString(buf);
                Console.WriteLine(tmp);
       //         Console.WriteLine(System.Text.Encoding.Default.GetString(buf));
                Console.WriteLine();
            }

        }

        public void udpData()
        {

        }
                /// <summary>
        /// 读取机器硬件数据，并写入文件
        /// </summary>
        public void recvDataFromDeviceTcp()
        {
            IPAddress ip = IPAddress.Parse("192.168.0.32");
            TcpListener server = new TcpListener(ip, 5505);

            server.Start();
            Console.WriteLine("......The Server is waiting......");

            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine(" 客户端已经连接...");

            //构造NetworkStream类，用于获取和操作网络流
            NetworkStream stream = client.GetStream();
            StreamReader sr = new StreamReader(stream);

            while (true)
            {
                Console.WriteLine("Recv data: " + sr.ReadLine());
            }
            client.Close();
        }
    }
}
