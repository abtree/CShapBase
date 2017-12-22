using System;
using System.Collections.Generic;

namespace LinkedList_
{
    class LinkedList_
    {
        /*
         * LinkedList 中包含 First 和 Last 指向 链表头和尾
         * LinkedListNode 为LinkedList的节点 包含value next previous 和 list
         */
        public void handle()
        {
            LinkedList<int> ll = new LinkedList<int>();
            LinkedListNode<int> p = ll.AddFirst(1);   // -> 1  
            ll.AddLast(2);      // -> 12
            p = ll.AddAfter(p, 3);  // -> 132
            p = ll.AddBefore(p, 4);  // -> 1432

            bool exit = ll.Contains(4);

            int size = ll.Count; //获取长度

            LinkedListNode<int> q = ll.Find(3);
            q = q.Next; //获取下一个
            ll.Remove(q);  //删除一个
            
            ll.Clear();
        }
    }
}
