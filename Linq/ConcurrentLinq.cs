using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrentLinq
{
    class ConcurrentLinq
    {
        public static IEnumerable<int> SampleData()
        {
            //这里建立一个超大的list 太小的看不出并行Linq的效果
            const int arraySize = 100000000;
            var r = new Random();
            return Enumerable.Range(0, arraySize).Select(x => r.Next(140)).ToList();
        }

        public static void Simple(IEnumerable<int> data) 
        {
            //AsParallel 将Enumerable 转换为 ParallelEnumerable 这样调用的其它方法 如 Where 就是 ParallelEnumerable版本的
            var res = (from x in data.AsParallel()
                       where Math.Log(x) < 4
                       select x).Average();
            Console.WriteLine(res);
        }

        /// <summary>
        /// 分区
        /// </summary>
        public static void Partition(IList<int> data)
        {
            //用分区设置负载均衡
            var result = (from x in Partitioner.Create(data, true).AsParallel()
                          where Math.Log(x) < 4
                          select x).Average();
        }

        //加入超时机制
        public static void TimeOut(IEnumerable<int> data)
        {
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = (from x in data.AsParallel().WithCancellation(cts.Token)
                               where Math.Log(x) < 4
                               select x).Average();
                    Console.WriteLine("Query finishedm sum: {0}", res);
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });

            Console.WriteLine("Query started");
            Console.Write("cancel? ");
            string input = Console.ReadLine();
            if (input.ToLower().Equals("y"))
            {
                cts.Cancel();  //这里手动结束
            }
        }
        public static void Main()
        {
            var data = SampleData();
            Simple(data);
            Partition(data as IList<int>);
            TimeOut(data);
        }
    }
}
