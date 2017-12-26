using System;
using System.Collections.Generic;
using System.Net;

namespace EventAsync
{
    /// <summary>
    /// 基于事件的异步调用 用起来简单 但要实现他比较麻烦
    /// 可以用BackgroundWorker类实现
    /// 不过 由于C#已经提供了更好的解决方案（Task）这里就不用再讨论了
    /// </summary>
    class EventAsync
    {
        public static void Main()
        {
            var client = new WebClient();
            //这里注册完成时的回调函数
            client.DownloadStringCompleted += (sender, e) =>
            {
                string resp = e.Result;
                Console.WriteLine(resp);
            };
            //这里异步调用
            client.DownloadStringAsync(new Uri("Url"));
        }
    }
}
