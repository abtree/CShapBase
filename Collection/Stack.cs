using System;
using System.Collections.Generic;

namespace Stack_
{
    class Stack_
    {
        public void handle()
        {
            var stack = new Stack<int>(3);
            stack.Push(1);  //压入
            stack.Push(2);
            stack.Push(3);

            bool exist = stack.Contains(3);  //是否存在

            foreach(var i in stack)
            {
                Console.Write(i);
            }  //321 先进后出原理

            stack.Peek();  //获取栈顶元素（不删除）
            int size = stack.Count; //获取栈中元素个数
            stack.Pop();  //获取并删除栈顶元素
            stack.TrimExcess(); //重新规划栈的容量
        }
    }
}
