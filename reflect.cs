using System;
using System.Reflection;

namespace Test
{
    //测试反射的用例
    class ReflectTest
    {
        //共用无参数函数反射调用
        public void Print()
        {
            Console.WriteLine("Print");
        }

        //非共用带参数函数反射调用
        private static int Show(string str)
        {
            Console.WriteLine("Show" + str);
            return 0;
        }


        public string mStr = "";
        private Int32 mInt = 0;
        public static string mSStr = "";
        private static UInt32 mSInt = 0;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //反射获取类型
            Type typ = Type.GetType("Test.ReflectTest");
            //用类型生成对象
            Object obj = Activator.CreateInstance(typ);

            //获取共用非静态成员
            FieldInfo info1 = typ.GetField("mStr");
            info1.SetValue(obj, "PublicMstr");
            Console.WriteLine(info1.GetValue(obj) as string);
            //获取私有非静态成员
            FieldInfo info2 = typ.GetField("mInt", BindingFlags.NonPublic | BindingFlags.Instance);
            info2.SetValue(obj, 123);
            Console.WriteLine("{0}", Convert.ToInt32(info2.GetValue(obj)));
            //获取共用静态成员
            FieldInfo info3 = typ.GetField("mSStr", BindingFlags.Public | BindingFlags.Static);
            info3.SetValue(null, "StaticMSStr");
            Console.WriteLine(info3.GetValue(null));
            //获取私有静态成员
            FieldInfo info4 = typ.GetField("mSInt", BindingFlags.NonPublic | BindingFlags.Static);
            info4.SetValue(null, 345u);
            Console.WriteLine("{0}", Convert.ToUInt32(info4.GetValue(obj)));

            //获取方法
            MethodInfo fun1 = typ.GetMethod("Print");
            fun1.Invoke(obj, null);

            MethodInfo fun2 = typ.GetMethod("Show", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
            object ret = fun2.Invoke(null, new object[] { "abcde" });
            Console.WriteLine("{0}", Convert.ToInt32(ret));
        }
    }
}
