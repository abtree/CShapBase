using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientParam
{
    public class MessageHandler : HttpClientHandler
    {
        string displayMessage = "";
        public MessageHandler(string message)
        {
            displayMessage = message;
        }

        /// <summary>
        /// 在获取数据时 根据displayMessage的值 进行不同的操纵
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine("In DisplayMessageHandler " + displayMessage);
            if(displayMessage == "error")
            {
                //这里返回坏请求
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
    class HttpClientParam
    {
        private static void GetData()
        {
            //这里用 MessageHandler作为 HttpClient的参数
            HttpClient httpClient = new HttpClient(new MessageHandler("error"));
            HttpResponseMessage response = null;
            Console.WriteLine();
            response = httpClient.GetAsync("http://cn.bing.com/").Result; //这里会调用MessageHandler的SendAsync
            Console.WriteLine(response.StatusCode);
        }

        public static void Main()
        {
            GetData();
            Console.ReadKey();
        }
    }
}
