using System;

namespace AnonymousObject
{
    class AnonymousObject
    {
        public static void Main(string[] args)
        {
            // var 类似于 c++的auto 会自动推断类型
            // 但 var 和 new 组合 可以创建匿名的对象 
            var AnonymousObj = new { FirstName = "FirstName", LastName = "LastName", Age = 18 };
            //匿名对象的字段都是只读的
            Console.WriteLine(AnonymousObj.FirstName.ToString());
        }
    }
}