using System.Net;
using System;

namespace NetHelpClass
{
    class NetHelpClass
    {
        public void UriAndUriBuilder()
        {
            //Uri一经构造 就不能修改
            Uri uri1 = new Uri("http://cn.bing.com/");
            Console.WriteLine(uri1.Query); //这些都是只读的
            Console.WriteLine(uri1.Host);

            UriBuilder uri2 = new UriBuilder();
            uri2.Scheme = "http";
            uri2.Host = "cn.bing.com";
            uri2.Port = 80;
            Console.WriteLine(uri2.Uri);  //获取构造的Uri
        }

        public void IP()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");  //通过parse构建
            //另外 127.0.0.1 可以直接回环
            Console.WriteLine(IPAddress.Loopback.ToString());

            IPHostEntry bingHost = Dns.GetHostEntry("cn.bing.com"); //获取ip信息
            foreach(IPAddress ipAddr in bingHost.AddressList)
            {
                Console.WriteLine(ipAddr.ToString());
            }
        }
    }
}
