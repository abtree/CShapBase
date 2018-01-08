using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketSimple
{
    class SocketSimple
    {
        public static void receive()
        {
            Console.WriteLine("Starting: Creating Socket object");
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 8091));
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("Waiting for connection on port 8091");
                Socket socket = listener.Accept();
                string receivedValue = string.Empty;
                while (true)
                {
                    byte[] receiveBytes = new byte[1024];
                    int numBytes = socket.Receive(receiveBytes);
                    Console.WriteLine("receiving .");
                    receivedValue += Encoding.ASCII.GetString(receiveBytes, 0, numBytes);
                    if (receivedValue.IndexOf("[FINAL]") > -1)
                        break;
                }
                Console.WriteLine("received value: {0}", receivedValue);
                string replyValue = "Message successful received.";
                byte[] replyMsg = Encoding.ASCII.GetBytes(replyValue);
                socket.Send(replyMsg);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                break;
            }
            listener.Close();
        }

        public static void send()
        {
            byte[] receiveBytes = new byte[1024];
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8091);
            Console.WriteLine("starting: creating socket object");
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            Console.WriteLine("Successfully connected to {0}", sender.RemoteEndPoint);
            string sendingMsg = "Hello World Socket Test [FINAL]";
            byte[] forwardMsg = Encoding.ASCII.GetBytes(sendingMsg);
            sender.Send(forwardMsg);
            int totalRecv = sender.Receive(receiveBytes);
            Console.WriteLine("Message provided from server: {0}", Encoding.ASCII.GetString(receiveBytes, 0, totalRecv));
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public static void Main()
        {
            Task tr = Task.Run(() => receive());

            send();

            tr.Wait();
            Console.ReadKey();
        }
    }
}
