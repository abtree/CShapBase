using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Concurrent
{
    class genericUse
    {
        ConcurrentQueue<string> cq = new ConcurrentQueue<string>(); //用一种免锁定的算法实现，使用在内部合并到一个链表中的32项数组
        ConcurrentStack<string> cs = new ConcurrentStack<string>(); //除了访问方法不同 几乎和ConcurrentQueue一样
        ConcurrentBag<string> cb = new ConcurrentBag<string>(); //使用一个把线程映射到内部使用的数组上的概念，以此尝试减少锁定
        ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>(); //线程安全的key value集合
        BlockingCollection<string> bc = new BlockingCollection<string>(); //带锁的集合
    }

    public class Info
    {
        public Info(string word, int count)
        {
            this.Word = word;
            this.Count = count;
        }
        public string Word { get; set; }
        public int Count { get; set; }

        public string Color { get; set; }
    }

    public class ConsoleHelper
    {
        private static object syncOutput = new object();
        public static void WriteLine(string message)
        {
            lock (syncOutput)
            {
                Console.WriteLine(message);
            }
        }

        public static void WriteLine(string message, string color)
        {
            lock (syncOutput)
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }

    public static class PipelineStages
    {
        public static Task ReadFilenamesAsync(string path, BlockingCollection<string> output)
        {
            return Task.Run(()=>
            {
                foreach(string filename in Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories))
                {
                    output.Add(filename);
                    ConsoleHelper.WriteLine(string.Format("stage 1: add {0}", filename));
                }
                output.CompleteAdding();  //多线中调用该方法很重要
            }
                );
        }

        public static async Task LoadContentAsync(BlockingCollection<string> input, BlockingCollection<string> output)
        {
            foreach (var filename in input.GetConsumingEnumerable())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    var reader = new StreamReader(stream);
                    string line = null;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        output.Add(line);
                        ConsoleHelper.WriteLine(string.Format("stage 2: added {0}", line));
                    }
                }
            }
            output.CompleteAdding();
        }

        public static Task ProcessContentAsync(BlockingCollection<string> input, ConcurrentDictionary<string, int> output)
        {
            return Task.Run(()=>
            {
                foreach(var line in input.GetConsumingEnumerable())
                {
                    string[] words = line.Split(' ', ';', '\t', '{', '}', '(', ')', ':', ',', '"');
                    foreach(var word in words.Where(w => !string.IsNullOrEmpty(w)))
                    {
                        output.AddOrIncrementValue(word);
                        ConsoleHelper.WriteLine(string.Format("stage 3: added {0}", word));
                    }
                }
            }
                );
        }

        public static Task TransferContentAsync(ConcurrentDictionary<string, int> input, BlockingCollection<Info> output)
        {
            return Task.Run(()=>
            {
                foreach(var word in input.Keys)
                {
                    int value;
                    if(input.TryGetValue(word, out value))
                    {
                        var info = new Info(word, value);
                        output.Add(info);
                        ConsoleHelper.WriteLine(string.Format("stage 4: added {0}", info));
                    }
                }
                output.CompleteAdding();
            });
        }

        public static Task AddColorAsync(BlockingCollection<Info> input, BlockingCollection<Info> output)
        {
            return Task.Run(()=>
            {
                foreach (var item in input.GetConsumingEnumerable())
                {
                    if (item.Count > 40)
                    {
                        item.Color = "Red";
                    }
                    else if (item.Count > 20)
                    {
                        item.Color = "Yellow";
                    }
                    else
                    {
                        item.Color = "Green";
                    }
                    output.Add(item);
                    ConsoleHelper.WriteLine(string.Format("stage 5: add color {1} to {0}", item, item.Color));
                }
                output.CompleteAdding();
            });
        }

        public static Task ShowContentAsync(BlockingCollection<Info> input)
        {
            return Task.Run(() => {
                foreach(var item in input.GetConsumingEnumerable())
                {
                    ConsoleHelper.WriteLine(string.Format("stage 6: {0}", item), item.Color);
                }
            });
        }
    }

    public static class ConcurrentDictionaryExtension
    {
        public static void AddOrIncrementValue(this ConcurrentDictionary<string, int> dict, string key)
        {
            bool success = false;
            while (!success)
            {
                int value;
                if(dict.TryGetValue(key, out value))
                {
                    if(dict.TryUpdate(key, value + 1, value))
                    {
                        success = true;
                    }
                }
                else
                {
                    if(dict.TryAdd(key, 1))
                    {
                        success = true;
                    }
                }
            }
        }
    }
    class Concurrent
    {
        public static async void StartPipeline()
        {
            var fileNames = new BlockingCollection<string>();
            var lines = new BlockingCollection<string>();
            var words = new ConcurrentDictionary<string, int>();
            var items = new BlockingCollection<Info>();
            var coloredItems = new BlockingCollection<Info>();
            Task t1 = PipelineStages.ReadFilenamesAsync(@"../../", fileNames);
            ConsoleHelper.WriteLine("started stage 1:");
            Task t2 = PipelineStages.LoadContentAsync(fileNames, lines);
            ConsoleHelper.WriteLine("started stage 2:");
            Task t3 = PipelineStages.ProcessContentAsync(lines, words);
            await Task.WhenAll(t1, t2, t3);
            ConsoleHelper.WriteLine("started 1,2,3 completed");
            Task t4 = PipelineStages.TransferContentAsync(words, items);
            Task t5 = PipelineStages.AddColorAsync(items, coloredItems);
            Task t6 = PipelineStages.ShowContentAsync(coloredItems);
            ConsoleHelper.WriteLine("stages 4,5,6 started");
            await Task.WhenAll(t4, t5, t6);
            ConsoleHelper.WriteLine("all stages finished");
        }

        public static void Main()
        {
            //这里用循环保证线程不过早退出
             while (true)
            {
                StartPipeline();
            }
        }
    }
}
