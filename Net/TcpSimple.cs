using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TcpSimple
{
    class TcpSimple
    {
        private void send()
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 8088);
            NetworkStream ns = tcpClient.GetStream();

            FileStream fs = File.Open("a.txt", FileMode.Open);
            int data = fs.ReadByte();
            while(data != -1)
            {
                ns.WriteByte((byte)data);
                data = fs.ReadByte();
            }
            fs.Close();
            ns.Close();
            tcpClient.Close();
        }

        private void receive()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 8088);
            tcpListener.Start();

            TcpClient tcpClient = tcpListener.AcceptTcpClient();
            NetworkStream ns = tcpClient.GetStream();
            StreamReader sr = new StreamReader(ns);
            string result = sr.ReadToEnd();

            UpdateDisplay(result);
            //Invoke(new UpdateDisplayDelegate(UpdateDisplay), new object[] { result });
            tcpClient.Close();
            tcpListener.Stop();
        }

        public void UpdateDisplay(string txt)
        {
            Console.WriteLine(txt);
        }

        //protected delegate void UpdateDisplayDelegate(string txt);

        public static void Main()
        {
            TcpSimple ts = new TcpSimple();
            Task taskServer = Task.Run(() => ts.receive());

            ts.send();

            taskServer.Wait();
        }
    }
}
