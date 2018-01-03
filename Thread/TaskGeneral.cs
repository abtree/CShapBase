using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGeneral
{
    /// <summary>
    /// Task 主要是使用线程池中的线程
    /// </summary>
    class TaskGeneral
    {
        static object taskMethodLock = new object(); 
        static void TaskMethod(object title)
        {
            //使用lock 保证并行调用同一个方法的输出不会交叉
            lock (taskMethodLock)
            {
                Console.WriteLine(title);
                Console.WriteLine("Task Id: {0}, thread: {1}", Task.CurrentId == null ? "no task" : Task.CurrentId.ToString(), Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("is pooled thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
                Console.WriteLine("is background thread: {0}", Thread.CurrentThread.IsBackground);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 四种启动task的方法
        /// </summary>
        static void TasksUsingThreadPool()
        {
            var tf = new TaskFactory();
            Task t1 = tf.StartNew(TaskMethod, "using a task factory");

            Task t2 = Task.Factory.StartNew(TaskMethod, "factory via a task");

            var t3 = new Task(TaskMethod, "using a task constructor and start");
            t3.Start();

            Task t4 = Task.Run(() => TaskMethod("using the run method"));
        }

        /// <summary>
        /// 使用同步线程调用 这里任务被主线程调用 但会创建一个taskid
        /// </summary>
        static void RunSynchronousTask()
        {
            TaskMethod("just the main thread");
            var t1 = new Task(TaskMethod, "run sync");
            t1.RunSynchronously();
        }

        /// <summary>
        /// 这里声明一个线程是长时间运行的线程 这样 系统就会为线程分配单独的任务调度器
        /// 对长时间运行的线程 任务调度器等待其运行完成是不明智的 故会单独分配
        /// </summary>
        static void LongRunningTask()
        {
            var t1 = new Task(TaskMethod, "long running", TaskCreationOptions.LongRunning);
            t1.Start();
        }

        static Tuple<int, int> TWR(object division)
        {
            Tuple<int, int> div = (Tuple<int, int>)division;
            int result = div.Item1 / div.Item2;
            int reminder = div.Item1 % div.Item2;
            Console.WriteLine("Task creates a result...");
            return Tuple.Create<int, int>(result, reminder);
        }
        /// <summary>
        /// 带返回的任务
        /// </summary>
        static void TaskWithResult()
        {
            //泛型的Task 就是带返回的Task
            var t1 = new Task<Tuple<int, int>>(TWR, Tuple.Create<int, int>(8, 3));
            t1.Start();
            Console.WriteLine(t1.Result);
            t1.Wait();
            Console.WriteLine("result from task: {0} {1}", t1.Result.Item1, t1.Result.Item2);
        }

        static void DoOnSecond(Task t)
        {
            Console.WriteLine("task {0} finished", t.Id);
            TaskMethod("second");
        }
        /// <summary>
        /// 连续任务 任务一个接一个执行
        /// await的任务自动为连续任务
        /// 虽然是连续任务 并不意味着一定在同一线程执行
        /// </summary>
        static void TaskContinue()
        {
            Task t1 = new Task(TaskMethod, "first");
            Task t2 = t1.ContinueWith(DoOnSecond);
            Task t3 = t2.ContinueWith(DoOnSecond);
            Task t4 = t3.ContinueWith(DoOnSecond);
            Task t5 = t4.ContinueWith(DoOnSecond, TaskContinuationOptions.OnlyOnFaulted);
            t1.Start();
        }
#region 带层级的任务
        static void ChildTask()
        {
            TaskMethod("Child");
        }

        static void ParentTask()
        {
            var child = new Task(ChildTask);
            child.Start();
            TaskMethod("Parent");
        }

        static void ParentAndChild()
        {
            var parent = new Task(ParentTask);
            parent.Start();
            Console.WriteLine(parent.Status); //如果父任务先完成 状态为 TaskStatus.WaitingForChildrenToComplete
        }
#endregion
        static void Main()
        {
            //TasksUsingThreadPool();
            //RunSynchronousTask();
            //TaskContinue();
            ParentAndChild();
            Console.ReadKey();
        }
    }
}
