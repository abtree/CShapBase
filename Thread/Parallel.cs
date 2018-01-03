using System;
using System.Threading.Tasks;
using System.Threading;

namespace Parallel_
{
    class Parallel_
    {
        /// <summary>
        /// 在多线程中for循环
        /// </summary>
        static void ThreadFor()
        {
            //循环 0 - 9 由于是多线循环 顺序是不可知的
            ParallelLoopResult result = Parallel.For(0, 10, i => {
                Console.WriteLine("{0}, task: {1}, thread: {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(10);
            });
            Console.WriteLine("Is Completed: {0}", result.IsCompleted);
        }

        static void ThreadForAsync()
        {
            //注意 这里的lambda表达式需要声明为异步 (ParallelLoopState 用于控制for循环的提前停止)
            ParallelLoopResult result = Parallel.For(10, 40, async (int i, ParallelLoopState pls) =>
            {
                Console.WriteLine("{0}, task: {1}, thread: {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                await Task.Delay(10);  //使用delay甚至可以让同一个函数的不同部分在不同线程运行
                Console.WriteLine("{0}, task: {1}, thread: {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                if(i > 15)
                {
                    pls.Break();  //这里的停止并不能确定哪些循环已经被执行了
                }
            });

            Console.WriteLine("Is Completed: {0}", result.IsCompleted);
            Console.WriteLine("Lowest break iteration: {0}", result.LowestBreakIteration);
        } 

        /// <summary>
        /// 带初始化和结束函数的for模版
        /// </summary>
        static void ThreadForTemplate()
        {
            Parallel.For<string>(0, 10, () =>
            {
                //线程的初始化方法 每个线程调用一次
                Console.WriteLine("init thread {0}, task {1}", Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
                return string.Format("t{0}", Thread.CurrentThread.ManagedThreadId);  //这个返回 为body的str1
            }, (i, pls, str1) =>
            {
                //循环中每个元素调用一次
                Console.WriteLine("body i {0} str1 {1} thread {2} task {3}", i, str1, Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
                Thread.Sleep(10);
                return string.Format("i {0}", i); //这个返回为finally的str1
            }, (str1) =>
            {
                //线程结束时调用 每个线程调用一次
                Console.WriteLine("finally {0}", str1);
            });
        }

        /// <summary>
        /// 带取消操纵的for循环
        /// </summary>
        static void ThreadForCancle()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("*** token cancled"));
            cts.CancelAfter(500);  //定义在500ms后取消

            try
            {
                ParallelLoopResult result = Parallel.For(0, 100, new ParallelOptions() {
                    CancellationToken = cts.Token
                }, x => {
                    Console.WriteLine("loop {0} started", x);
                    int sum = 0;
                    for (int i = 0; i < 100; ++i)
                    {
                        Thread.Sleep(2);
                        sum += 1;
                    }
                    Console.WriteLine("loop {0} finished", x);
                });
            }catch(OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 多线程执行的遍历操纵 顺序无法确定
        /// </summary>
        static void ThreadForEach()
        {
            string[] data = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve" };
            ParallelLoopResult result = Parallel.ForEach<string>(data, s => {
                Console.WriteLine(s);
            });
        }

        /// <summary>
        /// 该方法实现函数的多线程调用
        /// </summary>
        static void ThreadInvoke()
        {
            Parallel.Invoke(ThreadFor, ThreadForEach);
        }

        static void Main(string[] args)
        {

            //ThreadFor();
            //ThreadForAsync();
            //ThreadForTemplate();
            //ThreadForEach();
            Console.ReadKey();
        }
    }
}
