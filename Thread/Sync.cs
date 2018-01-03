using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace Sync
{
    /*
     * 实现同步的类很多
     * lock 语句
     * InterLocked 类
     * Monitor 类
     * SpinLock 结构
     * WaitHandle 类
     * Mutex 类
     * Semaphore 类
     * Event 类
     * Barrier 类
     * ReaderWriterLockSlim 类
     */

    #region /* lock */
    public class SharedState
    {
        private int _state = 0;
        private object _syncRoot = new object();

        public int State
        {
            get { return _state; }
        }

        public int IncrementState()
        {
            lock (_syncRoot)
            {
                return ++_state;
            }
        }
    }
    #endregion

    #region /* InterLocked 速度比lock语句快 但只能执行简单的操纵 */
    public class InterSharedState
    {
        private int _state = 0;
        private object _syncRoot = new object();

        public int State
        {
            get { return _state; }
        }

        public int IncrementState()
        {
            //这里执行比lock语句更快
            return Interlocked.Increment(ref _state);
        }
    }
    #endregion

    #region /* Monitor lock语句最终被解析为Monitor */
    public class MonitorSharedState
    {
        private int _state = 0;
        private object _syncRoot = new object();

        public int State
        {
            get { return _state; }
        }

        public int IncrementState()
        {
            //这是上面lock语句的monitor形式
            Monitor.Enter(_syncRoot);
            try
            {
                return ++_state;
            }
            finally
            {
                Monitor.Exit(_syncRoot);
            }
        }

        /// <summary>
        /// monitor的优势是可以设置超时
        /// </summary>
        /// <returns></returns>
        public int IncrementStateTimeOut()
        {
            bool lockToken = false;
            Monitor.TryEnter(_syncRoot, 500, ref lockToken);
            if (lockToken)
            {
                try
                {
                    return ++_state;
                }
                finally
                {
                    Monitor.Exit(_syncRoot);
                }
            }
            else
            {
                return _state;
            }
        }
    }
    #endregion

    #region /* SpinLock 用法和 Monitor一样 但SpinLock是结构 只要是性能方面的考虑 */
    #endregion

    #region /* WaitHandle 基类 用于等待一个信号的出现 */
    class WH
    {
        static void handle()
        {
            Func<string, string> downloadString = (address) =>
            {
                var client = new WebClient();
                return client.DownloadString(address);
            };

            downloadString.BeginInvoke("Url", ar =>
            {
                while (true)
                {
                    //这里阻塞线程 并等待信号的出现
                    if (ar.AsyncWaitHandle.WaitOne(1000))
                    {
                        break;
                    }
                }
                //EndInvoke 获取执行的返回数据
                string resp = downloadString.EndInvoke(ar);
                Console.WriteLine(resp);
            }, null);
        }
    }
    #endregion

    #region /* Mutex 继承了 WaitHandle 可以实现跨进程的互斥（互斥标记必须被命名） */
    public class MutexShareState
    {
        public void handle()
        {
            bool createNew;  //createNew 可以在这里判断是否在其它线程或进程定义了相同名字的Mutex
            Mutex mutex = new Mutex(false, "ProCSharpMutex", out createNew);
            if (mutex.WaitOne())  //加锁
            {
                try
                {
                }
                finally
                {
                    mutex.ReleaseMutex();  //解锁
                }
            }
            else
            {
                //这里不能加锁的处理
            }

        }
    }
    #endregion

    #region /* Semaphore SemaphoreSlim 信号量 SemaphoreSlim为在处理较短时间加锁的轻改版 */
    public class SemaphoreShareState
    {
        static void Handle()
        {
            int taskCount = 6;  //定义任务个数
            int semaphoreCount = 3;  //定义信号量的个数
            var semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount); //同时可以处理3个任务
            var tasks = new Task[taskCount];

            for(int i = 0; i < taskCount; ++i)
            {
                tasks[i] = Task.Run(() => TaskMain(semaphore));
            }

            Task.WaitAll(tasks);
            Console.WriteLine("All tasks finished");
        }

        static void TaskMain(SemaphoreSlim semaphore)
        {
            bool isCompleted = false;
            while (!isCompleted)
            {
                if (semaphore.Wait(600))
                {
                    try
                    {
                        Console.WriteLine("Task {0} locks the semaphore", Task.CurrentId);
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        Console.WriteLine("Task {0} releases the semaphore", Task.CurrentId);
                        semaphore.Release();
                        isCompleted = true;
                    }
                }
                else
                {
                    Console.WriteLine("Timeout for task {0}; wait again", Task.CurrentId);
                }
            }
        }
    }
    #endregion

    #region /* Events 这里的Events 和 event 关键字没有关系 这里是System.Threading里面的事件类*/
    public class ManualShareState
    {
        private ManualResetEventSlim mEvent;
        public int Result { get; private set; }

        public ManualShareState(ManualResetEventSlim ev)
        {
            this.mEvent = ev;
        }

        public void Calculation(int x, int y)
        {
            Console.WriteLine("Task {0} starts calculation", Task.CurrentId);
            Thread.Sleep(new Random().Next(3000));
            Result = x + y;
            Console.WriteLine("Task {0} is ready", Task.CurrentId);
            mEvent.Set(); //这里不同
        }

        static void Handle()
        {
            const int taskCount = 4;
            var events = new ManualResetEventSlim[taskCount];
            var waitHandle = new WaitHandle[taskCount];
            var cals = new ManualShareState[taskCount];

            for(int i = 0; i < taskCount; ++i)
            {
                int li = i;
                events[i] = new ManualResetEventSlim(false);
                waitHandle[i] = events[i].WaitHandle;
                cals[i] = new ManualShareState(events[i]);

                Task.Run(() => cals[li].Calculation(li + 1, li + 3));
            }

            for(int i = 0; i < taskCount; ++i)
            {
                int index = WaitHandle.WaitAny(waitHandle);
                if(index == WaitHandle.WaitTimeout)
                {
                    Console.WriteLine("TimeOut!");
                }
                else
                {
                    events[index].Reset();
                    Console.WriteLine("finished task for {0}, result: {1}", index, cals[index].Result);
                }
            }
        }
    }

    public class CountdownShareState
    {
        private CountdownEvent mEvent;
        public int Result { get; private set; }

        public CountdownShareState(CountdownEvent ev)
        {
            this.mEvent = ev;
        }

        public void Calculation(int x, int y)
        {
            Console.WriteLine("Task {0} starts calculation", Task.CurrentId);
            Thread.Sleep(new Random().Next(3000));
            Result = x + y;
            Console.WriteLine("Task {0} is ready", Task.CurrentId);
            mEvent.Signal();  //这里不同
        }

        static void Handle()
        {
            const int taskCount = 4;
            var events = new CountdownEvent(taskCount);  //这里并没有定义数组
            var cals = new CountdownShareState[taskCount]; //这里不需要定义 waithandle

            for (int i = 0; i < taskCount; ++i)
            {
                int li = i;
                cals[i] = new CountdownShareState(events);

                Task.Run(() => cals[li].Calculation(li + 1, li + 3));
            }

            events.Wait();
            Console.WriteLine("all finished");

            for (int i = 0; i < taskCount; ++i)
            {
                Console.WriteLine("task for {0}, result: {1}", i, cals[i].Result);
            }
        }
    }
    #endregion

    #region /* Barrier 适合于先需要分散任务 后需要将分散任务合并的情况 */
    public class BarrierShareState
    {
        public static IEnumerable<string> FillData(int size)
        {
            var data = new List<string>(size);
            var r = new Random();
            for(int i = 0; i < size; ++i)
            {
                data.Add(GetString(r));
            }
            return data;
        }

        public static string GetString(Random r)
        {
            var sb = new StringBuilder(6);
            for(int i = 0; i < 6; ++i)
            {
                sb.Append((char)(r.Next(26) + 97));
            }
            return sb.ToString();
        }

        public static int[] CalculationInTask(int jobNumber, int partitionSize, Barrier barrier, IList<string> coll)
        {
            List<string> data = new List<string>(coll);

            int start = jobNumber * partitionSize;
            int end = start + partitionSize;
            Console.WriteLine("Task {0}: partition from {1} to {2}", Task.CurrentId, start, end);
            int[] charCount = new int[26];
            for(int j = start; j < end; ++j)
            {
                char c = data[j][0];
                charCount[c - 97]++;
            }
            Console.WriteLine("Calculation completed from task {0}. {1}" + "times a, {2} times z", Task.CurrentId, charCount[0], charCount[25]);
            barrier.RemoveParticipant();
            Console.WriteLine("Task {0} removed from barrier, remaining participants {1}", Task.CurrentId, barrier.ParticipantsRemaining);
            return charCount;
        }

        public static void Handle()
        {
            const int numberTasks = 2;
            const int partitionSize = 1000000;
            var data = new List<string>(FillData(partitionSize * numberTasks));

            var barrier = new Barrier(numberTasks + 1);

            var tasks = new Task<int[]>[numberTasks];
            for(int i = 0; i< numberTasks; ++i)
            {
                int jobNumber = i;
                tasks[i] = Task.Run(() => CalculationInTask(jobNumber, partitionSize, barrier, data));
            }

            barrier.SignalAndWait();
            //这里将返回的结果合并起来
            int[] resultCollection = null;
            for (int i = 0; i < numberTasks; ++i)
            {
                if (i == 0)
                {
                    resultCollection = tasks[i].Result;
                }
                else
                {
                    resultCollection = resultCollection.Zip(tasks[i].Result, (c1, c2) =>  c1 + c2).ToArray();
                }
            }

            char ch = 'a';
            int sum = 0;
            foreach (var x in resultCollection)
            {
                Console.WriteLine("{0}, count:{1}", ch++, x);
                sum += x;
            }

            Console.WriteLine("main finished {0}", sum);
            Console.WriteLine("remaining {0}", barrier.ParticipantsRemaining);
        }
    }
    #endregion

    #region /* ReaderWriterLockSlim 可以只锁写 或 锁定读写 也可以先锁读 再升级为锁写 */
    public class RWLockSlim
    {
        private static List<int> items = new List<int>() { 0, 1, 2, 3, 4, 5 };
        private static ReaderWriterLockSlim rw1 = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        static void ReaderMethod(object reader)
        {
            try
            {
                rw1.EnterReadLock();
                for(int i = 0; i < items.Count; ++i)
                {
                    Console.WriteLine("Reader {0}, loop: {1}, item: {2}", reader, i, items[i]);
                    Thread.Sleep(400);
                }
            }
            finally
            {
                rw1.ExitReadLock();
            }
        }

        static void WriterMethod(object writer)
        {
            try
            {
                while (!rw1.TryEnterWriteLock(50))
                {
                    Console.WriteLine("Writer {0} waiting for the write lock", writer);
                    Console.WriteLine("Current reader count: {0}", rw1.CurrentReadCount);
                }
                Console.WriteLine("Writer {0} acquired the lock", writer);
                for(int i = 0; i< items.Count; ++i)
                {
                    items[i]++;
                    Thread.Sleep(50);
                }
                Console.WriteLine("Writer {0} finished", writer);
            }
            finally
            {
                rw1.ExitWriteLock();
            }
        }

        public static void Handle()
        {
            var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            var tasks = new Task[6];
            tasks[0] = taskFactory.StartNew(WriterMethod, 1);  //后一个参数就是WriterMethod的参数
            tasks[1] = taskFactory.StartNew(ReaderMethod, 1);
            tasks[2] = taskFactory.StartNew(ReaderMethod, 2);
            tasks[3] = taskFactory.StartNew(WriterMethod, 2);
            tasks[4] = taskFactory.StartNew(ReaderMethod, 3);
            tasks[5] = taskFactory.StartNew(ReaderMethod, 4);

            for (int i = 0; i < 6; ++i)
            {
                tasks[i].Wait();
            }
        }
    }
    #endregion
    class Sync
    {
        //public static void Main()
        //{
        //    RWLockSlim.Handle();
        //}
    }
}
