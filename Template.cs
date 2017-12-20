using System.Collections.Generic;
using System;
using System.Collections;

namespace Template
{
    /*
     * 泛型的约束(可同时约束多个)
     * where T : struct 对于结构的约束，T必须是值类型
     * where T : class 类约束 指定T必须是引用类型
     * where T : IFoo   指定接口必须实现特定接口（IFoo）
     * where T : Foo 指定T必须派生于特定类Foo
     * where T : new()  指定T必须包含默认构造函数(只能是默认构造函数 不能是其它构造函数)
     * where T : T2 指定T必须派生至泛型T2 该约束也叫裸类型约束
     */

    /*
     * 泛型类（或接口）的派生类可以是一个普通的类(非泛型)
     */ 
    public class ListNode<T>
        // where T : Interface  //可以通过where子句限定T类型必须实现了特定的接口(这样做是为了方便某些操作 如 遍历)
    {
        public ListNode(T value)
        {
            this.Value = value;
        }

        public T Value
        {
            get;
            private set;
        }
        public ListNode<T> Next
        {
            get;
            internal set;
        }
        public ListNode<T> Prev
        {
            get;
            internal set;
        }
    }
    public class Template<T>
    {
        //泛型中的静态成员 会根据泛型的实例类型不同而含有不同的值 如
        // Template<int>.x = 4;
        // Template<string>.x = 5;
        public static int x = 0; 
        public ListNode<T> First
        {
            get;
            private set;
        }
        public ListNode<T> Last
        {
            get;
            private set;
        }
        public ListNode<T> AddList(T node)
        {
            var newNode = new ListNode<T>(node);
            if(First == null)
            {
                First = newNode;
                Last = First;
            }
            else
            {
                ListNode<T> previous = Last;
                Last.Next = newNode;
                Last = newNode;
                Last.Prev = previous;
            }
            return newNode;
        }

        public T GetValue(ListNode<T> node)
        {
            T ret = default(T); //将T初始化为默认值
            ret = node.Value;
            return ret;
        }

        // 除了定义泛型类型 还可以定义泛型方法 而且泛型方法可以像普通方法一样调用
        // 因为C#编译器会根据方法参数判断泛型类型
    }
}
