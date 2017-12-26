using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace TAP
{
    class TAP
    {
        /// <summary>
        /// 用WebClient类已经实现的方法来看Task的基本使用
        /// 注意 要把该方法声明为异步方法
        /// </summary>
        private async void Base()
        {
            var client = new WebClient();
            string resp = await client.DownloadStringTaskAsync("Url");
            Console.WriteLine(resp);

            //下面是Http的用法
            var client1 = new HttpClient(null);
            var response = await client1.GetAsync("Url");
            resp = await response.Content.ReadAsStringAsync();
            //这里再起一个异步
            await Task.Run(() => {
                //注意 这里是在其它线程执行的
                Console.WriteLine(resp);
            });
        }

        #region  这里列出一个Task的简单实例
        /// <summary>
        /// 该方法用于异步调用
        /// </summary>
        /// <param name="name">输出字符串的名称</param>
        /// <returns>输出字符串</returns>
        static string Greeting(string name)
        {
            Thread.Sleep(3000);
            return string.Format("Hello, {0}", name);
        }

        /// <summary>
        /// 该方法异步调用了Greeting
        /// </summary>
        /// <param name="name">传给Greeting</param>
        /// <returns>Greeting的返回</returns>
        static Task<string> GreetingAsync(string name)
        {
            return Task.Run<string>(() => {
                return Greeting(name);
            });
        }

        /// <summary>
        /// 第一种调用方法 CallerWithAsync1在完成GreetingAsync前不会执行后面的代码
        /// 但CallerWithAsync1线程会继续执行
        /// </summary>
        private async static void CallerWithAsync1()
        {
            string result = await GreetingAsync("Stephanie");
            Console.WriteLine(result);
        }

        /// <summary>
        /// 也可以像下面一样书写
        /// async 只能用于返回Task或void的方法（不能用于Main方法）
        /// await只能用于返回Task的方法
        /// </summary>
        private async static void CallerWithAsync2()
        {
            Console.WriteLine(await GreetingAsync("Stephanie"));
        }

        private static void CallerWithContinuationTask()
        {
            Task<string> t1 = GreetingAsync("Stephanie");
            //这里注册回调函数(注意 在主线程执行的 和 GreetingAsync不在一个线程执行)
            t1.ContinueWith(t => {
                string result = t.Result;  //获取返回值
                Console.WriteLine(result);
            });
        }

        /// <summary>
        /// 如果调用多个互相独立的异步方法 使用异步组合器能提高效率
        /// </summary>
        private async static void MultipleAsyncWithCombinator()
        {
            Task<string> t1 = GreetingAsync("Stephanie");
            Task<string> t2 = GreetingAsync("Mathias");
            string[] result = await Task.WhenAll(t1, t2);  //异步组合器
            Console.WriteLine(t1.Result, result[1]);  //这里 result 数组 和 task.Result 结果一样
        }
        #endregion

        #region 使用委托封装的异步方法
        private static Func<string, string> greetInvoker = Greeting;

        static IAsyncResult BeginGreeting(string name, AsyncCallback callback, object state)
        {
            return greetInvoker.BeginInvoke(name, callback, state);
        }

        static string EndGreeting(IAsyncResult ar)
        {
            return greetInvoker.EndInvoke(ar);
        }

        //定义了Begin 和End方法后 就可以用BeginEnd方式了

        /// <summary>
        /// 这个方法实现了Begin End模式到Task模式的转换
        /// </summary>
        private async static void ConvertingAsyncPattern()
        {
            string s = await Task<string>.Factory.FromAsync<string>(BeginGreeting, EndGreeting, "Angela", null);
            Console.WriteLine(s);
        }

        /// <summary>
        /// 定义取消操纵
        /// </summary>
        private async static void CancleAsync()
        {
            var cts = new CancellationTokenSource(1);  //传入timeout时间
            await Task.Run(() =>
            {
                cts.Token.ThrowIfCancellationRequested();
                string ret = Greeting("Cancle");
                Console.WriteLine(ret);
            }, cts.Token);
            cts.Cancel();  //取消任务
        }
#endregion

        public static void Main()
        {
            TAP tap = new TAP();
            tap.Base();
        }
    }
}
