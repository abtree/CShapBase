using System;

namespace VariableType
{
    class VariableType
    {
        static void Main(string[] args)  //注意 Main 大写
        {
            //变量都在System中定义
            //整形
            sbyte sb = 0;  //8位有符号整数
            short s = 0;   //16位有符号整数
            int i = 0;     //32位有符号整数
            long l = 0l;   //64位有符号整数
             
            byte b = 0;    //8位无符号整数
            ushort us = 0;   //16位无符号整数
            uint ui = 0u;    //32位无符号整数
            ulong ul = 0ul; //64位有符号整数

            Console.WriteLine(sb.GetType().ToString());  // -> System.SByte
            Console.WriteLine(s.GetType().ToString());   // -> System.Int16
            Console.WriteLine(i.GetType().ToString());   // -> System.Int32
            Console.WriteLine(l.GetType().ToString());   // -> System.Int64

            Console.WriteLine(b.GetType().ToString());   // -> System.Byte
            Console.WriteLine(us.GetType().ToString());  // -> System.UInt16
            Console.WriteLine(ui.GetType().ToString());  // -> System.UInt32
            Console.WriteLine(ul.GetType().ToString());  // -> System.UInt64
            //浮点型
            float f = 0.0f;   //32位单精度浮点数
            double d = 0.0d;  //64位双精度浮点数
            Console.WriteLine(f.GetType().ToString());  // -> System.Single
            Console.WriteLine(d.GetType().ToString());  // -> System.Double

            decimal m = 0.0m;  //128位高精度10进制数表示（常用于货币） 
            Console.WriteLine(m.GetType().ToString());  // -> System.Decimal

            bool bo = false;
            Console.WriteLine(bo.GetType().ToString());  // -> System.Boolean

            char c = '\0';
            Console.WriteLine(c.GetType().ToString());  // -> System.Char

            //引用类型
            object obj = c;  //object对象 是 所有对象的 父对象 其类型显示为其引用类型
            string str = "";
            Console.WriteLine(obj.GetType().ToString());  // -> System.Char 
            Console.WriteLine(str.GetType().ToString());  // -> System.String
        }
    }
}
