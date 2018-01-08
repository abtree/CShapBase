using System.Globalization;
using System;
using System.Threading;

namespace NumberAndDateTimeLocal
{
    class NumberAndDateTimeLocal
    {
        //注意 数字在G 一般输出模式下 显示是一样的 F 下只有小数点不同
        private static void NumberFormatDemo()
        {
            int val = 1234567890;
            //culture of the current thread
            Console.WriteLine(val.ToString("N"));  //N表示以数字方式输出 其它包括 C 货币 D 小数 E 科学计数法 F 定点输出 G 一般输出 X 16进制
            // -> 1,234,567,890.00

            //use IFormatProvider
            Console.WriteLine(val.ToString("N", new CultureInfo("fr-FR")));
            // -> 1?234?567?890,00 ？表示显示不出来的符号

            //change the culture of the thread
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Console.WriteLine(val.ToString("N"));
            // -> 1.234.567.890,00
        }

        private static void DateFormatDemo()
        {
            DateTime d = DateTime.Now;

            //current culture
            Console.WriteLine(d.ToLongDateString());

            //use IFormatProvider
            Console.WriteLine(d.ToString("D", new CultureInfo("fr-FR")));

            //use culture of thread
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            Console.WriteLine("{0} : {1} {2}", ci.ToString(), d.ToString("D"), d.ToString("T")); //D 长日期 d 短日期 ddd 星期中某一天 dddd 星期中某一天全称 yyyy 年 T 时间全称 t 短时间

            ci = new CultureInfo("es-ES");
            Thread.CurrentThread.CurrentCulture = ci;
            Console.WriteLine("{0} : {1} {2}", ci.ToString(), d.ToString("D"), d.ToString("T"));
        }

        public static void Main()
        {
            DateFormatDemo();
        }
    }
}