﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CitadelsClient
{
    public class ClientNetControl
    {
        private Socket _socketClient;
        //生成与服务器通信的socket
        public Socket SocketClient
        {
            get
            {
                return _socketClient;
            }

            set
            {
                _socketClient = value;
            }
        }
        //构造函数
        public ClientNetControl()
        {
        }
        public ClientNetControl(string ipAddr,string port)
        {
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipAddr);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(port));
            //获得要连接的远程服务器应用程序的IP地址和端口号
            SocketClient.Connect(point);
            Thread th = new Thread(Receive);
            th.IsBackground = true;
            th.Start();
        }
        //发送消息
        public void Send(string str)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(str);
                SocketClient.Send(buffer);
            }
            catch (Exception ex)
            {
                //抛出异常信息
                Console.WriteLine(ex.Message.ToString());
            }
        }
        //接收消息
        public void Receive()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
                try
                {
                    int r = SocketClient.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    Console.WriteLine(str);
                }//try结束
                catch (Exception ex)
                {
                    //抛出异常信息
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
    }
}
