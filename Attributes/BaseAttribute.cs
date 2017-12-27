using System;

namespace BaseAttribute
{
    [AttributeUsage(AttributeTargets.All)]
    class BaseAttribute:Attribute
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public string Describtion { get; set; }
    }

    //如果自定义Attribute已Attribute结束 可以不写Attribute 系统会自动加上
    //这里Base == BaseAttribute
    [Base(Name="A", Version = 1, Describtion = "Test")]  
    public class MyCode
    {

    }

    public class MainClass
    {
        public static void Main()
        {
            var info = typeof(MyCode);
            var classAttribute = (BaseAttribute)Attribute.GetCustomAttribute(info, typeof(BaseAttribute));
            Console.Write("{0} {1} : {2}", classAttribute.Name, classAttribute.Version, classAttribute.Describtion);
        }
    }
}
