using System;
using System.Collections.Generic;

namespace Queue_
{
    class Queue_
    {
        Queue<Int32> queue = new Queue<Int32>();
        void create()
        {
            Queue<Int32> q1 = new Queue<Int32>(10);
            q1.TrimExcess(); //将容量设置为队列中元素个数（配置文件有用）
            int size = q1.Count;  //返回队列中元素个数
        }

        void pop()
        {
            queue.Enqueue(3); //压入一个元素
        }

        void push()
        {
            var i = queue.Peek(); //读取一个元素 不删除
            i = queue.Dequeue(); //读取并在队列中删除一个元素
        }
    }
}
