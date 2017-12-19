using System;
using alias = UsingAlias.Own.Test;

namespace UsingAlias
{
    class UsingAlias
    {
        static void Main(string[] args)
        {
            alias::Test test = new Own.Test.Test();   //注意 使用别名一定要用“::”符号
            Console.WriteLine(test.getNameSpace());
        }
    }

    namespace Own.Test
    {
        class Test
        {
            public string getNameSpace()
            {
                return this.GetType().Namespace;   // -> UsingAlias.Own.Test
            }
        }
    }
}
