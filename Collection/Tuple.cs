using System;
using System.Collections.Generic;

namespace Tuple_
{
    class Tuple_
    {
        public static void Main(string[] args)
        {
            //元祖的数据个数不能超过8个 但可以元祖包含元祖 这样就能实现无限多个的元祖
            var tup = Tuple.Create<string, string, int, char, float, Tuple<int, string>>(
                "str1", "str2", 1, 'c', 0.1f, Tuple.Create<int, string>(4, "str4")
                );
            var tup1 = tup;
            Console.Write(tup.Item2);
            Console.WriteLine(tup.Item6.Item2);
            Console.WriteLine(tup == tup1);     //比较两个对象
            Console.WriteLine(tup.Equals(tup1)); //比较两个对象中的元素
        }
    }
}
