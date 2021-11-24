using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 做好连接准备
             */
            int i = 0;
            int port = 2000;
            string host = "192.168.55.204";
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket类
            s.Bind(ipe);//绑定2000端口
            /*
             * 循环监听并处理消息
             */
            while (true)
            {
                i++;
                try
                {
                    Console.Write("Perform operations {0} :", i);
                    Console.WriteLine("\t-----------------------------------------------");
                    s.Listen(0);//开始监听
                    Console.WriteLine("1. Wait for connect...");
                    /*
                     * 实例一个新的socket端口
                     */
                    Socket temp = s.Accept();//为新建连接创建新的Socket。
                    Console.WriteLine("2. Connect successful");
                    /*
                     * 接收客户端发的消息并做解码处理
                     */
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = temp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
                    recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
                    Console.WriteLine("3. Server Get Message:{0}", recvStr);//把客户端传来的信息显示出来
                    /*
                     * 返回给客户端连接成功的消息
                     */
                    string sendStr = "Ok!receive message sucessful!";
                    byte[] bs = Encoding.UTF8.GetBytes(sendStr);
                    temp.Send(bs, bs.Length, 0);//返回客户端成功信息
                    /*
                     * 关闭端口
                     */
                    temp.Close();
                    Console.WriteLine("4. Completed...");
                    Console.WriteLine("-----------------------------------------------------------------------");
                    Console.WriteLine("");
                    //s.Close();//关闭socket（由于再死循环中，所以不用写，但如果是单个接收，实例socket并完成任务后需关闭）
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            }
        }
    }
}
