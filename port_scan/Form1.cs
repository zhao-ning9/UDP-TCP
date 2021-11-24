using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace port_scan
{
    public partial class Form1 : Form
    {
        //多线程
        public Form1()
        {
            InitializeComponent();
            //不进行跨线程检查
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //自定义一些需要的变量
        private int port;                          //记录当前扫描的端口号
        private string Address;                    //记录扫描的系统主机地址
        private bool[] done = new bool[65536];     //记录端口的开放状态
        private int start;                         //记录扫描的起始端口
        private int end;                           //记录扫描的结束端口
        private Thread scanThread;                  //定义端口状态数据（开放则为true，否则为false）
        private bool OK;                           //结束扫描
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //初始化
                textBox4.Clear();
                label4.Text = "0%";
                //获取ip地址和始末端口号
                Address = textBox1.Text;
                start = Int32.Parse(textBox2.Text);
                end = Int32.Parse(textBox3.Text);
                if (decideAddress())
                {
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox3.ReadOnly = true;
                    //创建线程，并创建ThreadStart委托对象
                    Thread process = new Thread(new ThreadStart(PortScan));
                    process.Start();
                    //设置进度条的范围
                    progressBar1.Minimum = start;
                    progressBar1.Maximum = end;
                    //显示框显示
                    textBox4.AppendText("端口扫描器 v1.0.0" + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    //若端口号不合理，弹窗报错
                    MessageBox.Show("输入错误，端口范围为[0-65536]!");
                }
            }
            catch
            {
                //若输入的端口号为非整型，则弹窗报错
                MessageBox.Show("输入错误，端口范围为[0-65536]!");
            }
        }
        ////单线程
        //try
        //{

        //    textBox4.Clear();
        //    label4.Text = "0%";

        //    Address = textBox1.Text;
        //    start = Int32.Parse(textBox2.Text);
        //    end = Int32.Parse(textBox3.Text);
        //    if (decideAddress())
        //    {

        //        textBox1.ReadOnly = true;
        //        textBox2.ReadOnly = true;
        //        textBox3.ReadOnly = true;

        //        progressBar1.Minimum = start;
        //        progressBar1.Maximum = end;

        //        textBox4.AppendText("端口扫描器 v1.0.0" + Environment.NewLine + Environment.NewLine);

        //        PortScan();
        //    }
        //    else
        //    {

        //        MessageBox.Show("输入错误，端口范围为[0-65536]!");
        //    }
        //}
        //catch
        //{

        //    MessageBox.Show("输入错误，端口范围为[0-65536]!");
        //}   
        //}
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        /*
        private bool decideAddress()
        {
            //单线程
            if ((start >= 0 && start <= 65536) && (end >= 0 && end <= 65536) && (start <= end))
                return true;
            else
                return false;
        }

        private void PortScan()
        {
            //单线程
            double x;
            string xian;

            textBox4.AppendText("开始扫描...（可能需要请您等待几分钟）" + Environment.NewLine + Environment.NewLine);

            for (int i = start; i <= end; i++)
            {
                x = (double)(i - start + 1) / (end - start + 1);
                xian = x.ToString("0%");
                port = i;

                Scan();

                label4.Text = xian;
                label4.Refresh();
                progressBar1.Value = i;
            }
            textBox4.AppendText(Environment.NewLine + "扫描结束！" + Environment.NewLine);

            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
        }

        private void Scan()
        {
            //单线程
            int portnow = port;

            TcpClient objTCP = null;
            try
            {

                objTCP = new TcpClient(Address, portnow);

                textBox4.AppendText("端口 " + port + " 开放！" + Environment.NewLine);
            }
            catch
            {

            }
        } */

        private bool decideAddress()
        {
            //判断端口号是否合理
            if ((start >= 0 && start <= 65536) && (end >= 0 && end <= 65536) && (start <= end))
                return true;
            else
                return false;
        }

        private void PortScan()
        {
            double x;
            string xian;
            //显示扫描状态
            textBox4.AppendText("开始扫描...（可能需要请您等待几分钟）" + Environment.NewLine + Environment.NewLine);
            //循环抛出线程扫描端口
            for (int i = start; i <= end; i++)
            {
                x = (double)(i - start + 1) / (end - start + 1);
                xian = x.ToString("0%");
                port = i;
                //使用该端口的扫描线程
                scanThread = new Thread(new ThreadStart(Scan));
                scanThread.Start();
                //使线程睡眠
                System.Threading.Thread.Sleep(100);
                //进度条值改变
                label4.Text = xian;
                progressBar1.Value = i;
            }
            while (!OK)
            {
                OK = true;
                for (int i = start; i <= end; i++)
                {
                    if (!done[i])
                    {
                        OK = false;
                        break;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            textBox4.AppendText(Environment.NewLine + "扫描结束！" + Environment.NewLine);
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
        }

        private void Scan()
        {
            int portnow = port;
            //创建线程变量
            Thread Threadnow = scanThread;
            //扫描端口，成功则写入信息
            done[portnow] = true;
            //创建TcpClient对象，TcpClient用于为TCP网络服务提供客户端连接
            TcpClient objTCP = null;
            try
            {
                //用于TcpClient对象扫描端口
                objTCP = new TcpClient(Address, portnow);
                //扫描到则显示到显示框
                textBox4.AppendText("端口 " + port + " 开放！" + Environment.NewLine);
            }
            catch
            {
                //未扫描到，则会抛出错误
            }
        }
    }
}
