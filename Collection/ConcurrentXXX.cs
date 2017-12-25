using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ConcurrentXXX
{
    class ConcurrentXXX
    {
        //并发集合 与普通集合的用法一样
        public static void Main()
        {
            ConcurrentQueue<string> cq = new ConcurrentQueue<string>();
            ConcurrentStack<string> cs = new ConcurrentStack<string>();
            ConcurrentBag<string> cb = new ConcurrentBag<string>();
            ConcurrentDictionary<string, string> cd = new ConcurrentDictionary<string, string>();
            BlockingCollection<string> bc = new BlockingCollection<string>(); //会自动加锁的集合
            //BlockingCollection 可用于创建线程间的管道
        }
    }
}