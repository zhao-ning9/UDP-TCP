using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace helloword
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            string input, stringData;

            //构建TCP 服务器
            Console.WriteLine("This is a Client, host name is {0}", Dns.GetHostName());

            //设置服务IP（这个IP地址是服务器的IP），设置TCP端口号
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.201.236"), 8080);

            //定义网络类型，数据连接类型和网络协议UDP
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            

            string welcome = "你好! ";
            data = Encoding.UTF8.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ip);
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;

            data = new byte[1024];
            //对于不存在的IP地址，加入此行代码后，可以在指定时间内解除阻塞模式限制
            int recv = server.ReceiveFrom(data, ref Remote);
            Console.WriteLine("Message received from {0}: ", Remote.ToString());
            Console.WriteLine(Encoding.UTF8.GetString(data, 0, recv));
            int i = 0;
            while (true)
            {
                string s = "Hello Cqjtu!重交物联2019级" + i;
                Console.WriteLine(s);
                server.SendTo(Encoding.UTF8.GetBytes(s), Remote);
                if (i == 50)
                {
                    break;
                }
                i++;
            }
            Console.WriteLine("Stopping Client.");
            server.Close();
            Console.WriteLine("数据发送完毕，按任意键退出...");
            System.Console.ReadKey();;
        }

    }
}
