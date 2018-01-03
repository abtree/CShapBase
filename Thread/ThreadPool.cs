using System;
using System.Threading;

namespace ThreadPool_
{
    /// <summary>
    /// 所有线程池中的线程都是后台线程 不能设置优先级和名称 只能用于较短的任务
    /// </summary>
    class ThreadPool_
    {
        static void JobForAThread(object state)
        {
            for(int i = 0; i < 3; ++i)
            {
                Console.WriteLine("loop {0}, running inside pooled thread{1}", i, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(50);
            }
        }

        static void Main()
        {
            int nWorkerThreads;            //最大工作线程数
            int nCompletionPortThreads;    //最大I/O线程数
            ThreadPool.GetMaxThreads(out nWorkerThreads, out nCompletionPortThreads);
            Console.WriteLine("Max worker threads: {0}, " + "I/O completion threads: {1}", nWorkerThreads, nCompletionPortThreads);
            
            for(int i = 0; i < 5; ++i)
            {
                ThreadPool.QueueUserWorkItem(JobForAThread);
            }
            Thread.Sleep(3000);
        }
    }
}
