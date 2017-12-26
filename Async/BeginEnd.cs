using System;
using System.Collections.Generic;
using System.Net;

namespace BeginEnd
{
    class BeginEnd
    {
        /// <summary>
        /// 直接调用函数是同步执行的 但是通过BeginInvoke 和 EndInvoke调用 就变成异步的了
        /// </summary>
        public static void Main()
        {
            Func<string, string> downloadString = (address) =>
            {
                var client = new WebClient();
                return client.DownloadString(address);
            };

            //下面是同步执行
            //string resp = downloadString("Url");
            //这里用BeginInvoke 使downloadString这个函数异步执行
            downloadString.BeginInvoke("Url", ar =>
            {
                //EndInvoke 获取执行的返回数据
                string resp = downloadString.EndInvoke(ar);
                Console.WriteLine(resp);
            }, null);
        }
    }
}
