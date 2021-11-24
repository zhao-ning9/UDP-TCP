using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace helloeord
{
    class Program
    {
        static void Main(string[] args)
        {
            int recv;
            byte[] data = new byte[1024];

            //得到本机IP，设置TCP端口号         
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 8080);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //绑定网络地址
            newsock.Bind(ip);

            Console.WriteLine("This is a Server, host name is {0}", Dns.GetHostName());

            //等待客户机连接
            Console.WriteLine("Waiting for a client");

            //得到客户机IP
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            recv = newsock.ReceiveFrom(data, ref Remote);
            Console.WriteLine("Message received from {0}: ", Remote.ToString());
            Console.WriteLine(Encoding.UTF8.GetString(data, 0, recv));

            //客户机连接成功后，发送信息
            string welcome = "你好 ! ";

            //字符串与字节数组相互转换
            data = Encoding.UTF8.GetBytes(welcome);

            //发送信息
            newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
            while (true)
            {
                data = new byte[1024];
                //接收信息
                recv = newsock.ReceiveFrom(data, ref Remote);
                Console.WriteLine(Encoding.UTF8.GetString(data, 0, recv));
                //newsock.SendTo(data, recv, SocketFlags.None, Remote);
            }
        }

    }
}
