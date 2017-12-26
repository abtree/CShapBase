using System;

namespace DynamicBase
{
    class DynamicBase
    {
        /*
         * 扩展方法不能用于dynamic
         * 匿名函数 lambda表达式不能用于 dynamic(因此 linq不能用于dynamic)
         */
        public static void Main()
        {
            //dyn可在运行时改变其所指的类型 var不行 但dynamic的运行效率要低得多
            dynamic dyn;
            dyn = 100;  // -> int
            Console.WriteLine(dyn.GetType());
            Console.WriteLine(dyn);

            dyn = "This is a string";  // -> string
            Console.WriteLine(dyn.GetType());
            Console.WriteLine(dyn);
        }
    }
}
