using System;

namespace IncreateFunction
{
    //这一个类动态的添加一个方法
    public class CA
    {
        public int A
        {
            get; set;
        }
    }
    //此处新建一个static class (注意 一定是static class)
    public static class CAEx
    {
        //将这个方法添加给CA (但这个方法不能访问CA的private或protected成员)
        public static int AddToA(this CA ca, int a, int b)
        {
            return ca.A + a + b;
        }
    }

    class IncreateFunction
    {
        public static void Main(string[] args)
        {
            CA ca = new CA();
            ca.A = 10;
            Console.WriteLine(ca.AddToA(10, 20));  //此处调用动态添加的方法 就像调用普通方法一样
            //注意 扩展方法 如果与已有方法同名 将永远调用已有方法 扩展方法将被忽略
        }
    }
}
