using System;

namespace AppDomain
{
    #region 这部分用于生成AppDomain.exe程序集
    [Serializable]   //必须序列化
    public class Demo
    {
        public Demo(int var1, int var2)
        {
            Console.WriteLine("Constructor with the values {0}, {1} in domain " +
                "{2} called", var1, var2, System.AppDomain.CurrentDomain.FriendlyName);
            this.Var1 = var1;
            this.Var2 = var2;
        }

        public int Var1 { get; private set; }
        public int Var2 { get; private set; }
    }
    class AppDomain
    {
        public static void Main()
        {
            Console.WriteLine("Main in domain {0} called", System.AppDomain.CurrentDomain.FriendlyName);
        }
    }
    #endregion

    //注意 要调用其它程序域的方法 必须继承 MarshalByRefObject
    class Program : MarshalByRefObject
    {
        static void Main(string[] args)
        {
            //这里获取当前程序域
            System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
            Console.WriteLine(currentDomain.FriendlyName);

            //这里创建一个程序域 并在这个程序域中 运行AppDomain.exe
            System.AppDomain secondDomain = System.AppDomain.CreateDomain("New AppDomain");
            secondDomain.ExecuteAssembly("AppDomain.exe");

            //这里创建一个其它程序域的对象
            var cls = secondDomain.CreateInstanceFrom("Cs8.exe", "AppDomain.Demo", false, System.Reflection.BindingFlags.Default, null, new object[2] { 1, 2 }, null, null);
            //这里使用动态类型 是因为没有引用Cs8.exe程序集 所以取不到AppDomain.Demo类型(实际使用中是可以引用Cs8.exe程序集的)
            dynamic clsi = cls.Unwrap();
       
            Console.WriteLine(clsi.GetType().ToString());
            Console.WriteLine("{0} {1}", clsi.Var1, clsi.Var2);

            //这里清除掉程序域
            System.AppDomain.Unload(secondDomain);
        }
    }
}
