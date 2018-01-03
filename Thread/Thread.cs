using System;
using System.Threading;

namespace Thread_
{
    /// <summary>
    /// 定义一个类 作为新线程的参数
    /// </summary>
    public class MyThread
    {
        private string _data;
        public MyThread(string data)
        {
            this._data = data;
        }

        /// <summary>
        /// 线程的启动函数
        /// </summary>
        public void ThreadMain()
        {
            Console.WriteLine("Running in a thread, data {0}", _data);
        }
    }
    /// <summary>
    /// Thread 用于创建前台线程 即非线程池中的线程
    /// </summary>
    class Thread_
    {
        static void ThreadMain()
        {
            Console.WriteLine("Running in a thread");
        }

        /// <summary>
        /// 创建一个线程
        /// </summary>
        static void Start()
        {
            var t1 = new Thread(ThreadMain);
            t1.Start();
            Console.WriteLine("this is the main thread");
        }

        static void ThreadMainWithParams(object o)
        {
            string s = (string)o;
            Console.WriteLine("Running in a thread, received {0}", s);
        }

        /// <summary>
        /// 带参数的thread
        /// </summary>
        static void StartWithParams()
        {
            var t1 = new Thread(ThreadMainWithParams) { Priority = ThreadPriority.BelowNormal };  //设置优先级
            t1.Start("params");
            Console.WriteLine("this is the main thread");
            Thread.CurrentThread.Join();  //将当前线程等待 直到其它线程完成
        }

        static void StartWithParams1()
        {
            var obj = new MyThread("Info");
            var t3 = new Thread(obj.ThreadMain) { Name = "MyNewThread", IsBackground = true };  //命名 的 后台线程
            t3.Start();
            Console.WriteLine("this is the main thread");
        }
    }
}
