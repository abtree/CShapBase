using System;

namespace Lambda_
{
    class Lambda_
    {
        public static void Main(String[] args)
        {
            string mid = " , middle part,";
            //先看一个匿名方法的例子
            Func<string, string> anonDel = delegate (string param)
           {
               param += mid;
               param += " the last.";
               return param;
           };
            Console.WriteLine(anonDel("The First"));
            Console.WriteLine();
            //将 上面的例子改为Lambda表达式
            //系统会为lambda表达式生成一个匿名的类 并且这个类的构造函数用于传递需要的外部参数
            Func<string, string> lambdaDel = param =>
            {
                param += mid;
                param += " the last.";
                return param;
            };
            Console.WriteLine(lambdaDel("The First"));
            Console.WriteLine();

            //lambda表达式也可以这样写
            Func<double, double, double> func = (double x, double y) => x * y;  //这里省略了 花括号 和 return 语句
            Console.WriteLine(func(1.2, 2.3));
        }
    }
}
