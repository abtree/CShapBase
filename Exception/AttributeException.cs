using System;
using System.Runtime.CompilerServices;

namespace AttributeException
{
    class AttributeException
    {
        //调用这个函数 不需要传递参数 系统自动传递
        public void Log([CallerLineNumber]int line = -1, [CallerFilePath]string path = null, [CallerMemberName]string name = null)
        {
            Console.WriteLine((line < 0) ? "No line" : "Line" + line);
            Console.WriteLine((path == null) ? "No file path" : path);
            Console.WriteLine((name == null) ? "No member name" : name);
            Console.WriteLine();
        }

        private int someProperty;
        public int SomeProperty
        {
            get { return someProperty;}
            set
            {
                this.Log();
                someProperty = value;
            }
        }

        public static void Main()
        {
            var p = new AttributeException();
            p.Log();
            p.SomeProperty = 33;
            Action a1 = () => p.Log();
            a1();
        }
    }
}
