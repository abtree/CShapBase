using System;

namespace StaticConstruct
{
    class StaticConstruct
    {
        //readonly 与 const 字段的不同之处在于
        // readonly 可以在构造函数中 根据不同的情况设置不同的值
        // const 字段不能在构造函数中赋值
        public static readonly ulong BackColor;
        public const ulong ForeColor = 0x0000FF;
        //静态构造函数 在类第一次被引用前由系统调用一次 一般用于一些静态成员的初始化
        static StaticConstruct()
        {
            DateTime now = DateTime.Now;
            if (now.DayOfWeek == DayOfWeek.Sunday ||
                now.DayOfWeek == DayOfWeek.Saturday)
                BackColor = 0xFF0000;
            else
                BackColor = 0x00FF00;
        }
        //普通构造函数 创建类实例时 调用
        public StaticConstruct()
        {

        }
    }
}
