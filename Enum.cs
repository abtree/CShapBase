using System;

namespace Enum_
{
    class Enum_
    {
        public enum TimeOfDay
        {
            Morning = 0,
            AfterNoon = 1,
            Evening = 2
        }
        static void Main(string[] args)
        {
            //可以取枚举的字符串
            TimeOfDay time1 = TimeOfDay.AfterNoon;
            Console.WriteLine(time1.ToString());
            //也可以将string转换为enum
            TimeOfDay time2 = (TimeOfDay)Enum.Parse(typeof(TimeOfDay), "afternoon", true);  //最后的true表示忽略大小写
            Console.WriteLine(Convert.ToInt32(time2));
        }
    }
}
