using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpSimple
{
    class UdpSimple
    {
        public void send()
        {
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Loopback, 8090);
            UdpClient udpClient = new UdpClient(localIpep);  //这里需要绑定不同的端口
            string sendMsg = "Hello Echo Server";
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendMsg);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 8089);  //只能发送到绑定的端口上
            udpClient.Send(sendBytes, sendBytes.Length, endPoint);

            IPEndPoint recPoint = new IPEndPoint(IPAddress.Loopback, 8089);  //这里设置Ip 和 端口号没有意义(因为会被覆盖)
            byte[] bytes = udpClient.Receive(ref recPoint);
            string mesg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            Console.WriteLine(mesg);

            udpClient.Close();
        }

        public void receive()
        {
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Loopback, 8089);
            UdpClient udpClient = new UdpClient(localIpep); //这里需要绑定不同的端口

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = udpClient.Receive(ref endPoint);    //这里会返回发送方的ip和端口
            string mesg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            Console.WriteLine(mesg);

            IPEndPoint sendPoint = new IPEndPoint(IPAddress.Loopback, 8090);  //只能发送到绑定的端口上
            udpClient.Send(bytes, bytes.Length, sendPoint);
            udpClient.Close();
        }

        public static void Main()
        {
            UdpSimple uc = new UdpSimple();
            Task tr = Task.Run(() => uc.receive());

            uc.send();

            tr.Wait();
            Console.ReadKey();
        }
    }
}
