using System;
using System.Collections.Generic;

namespace SortedSet_
{
    class SortedSet_
    {
        public void op() {
            var companyTeam = new SortedSet<string>() { "aaa", "bbb", "ccc" };
            if (companyTeam.Add("ddd"))
            {
                Console.WriteLine("ddd add");
            }
            if (!companyTeam.Add("aaa"))
            {
                Console.WriteLine("aaa already exist");
            }
            //通过UnionWith可将多个set连成一个
            //companyTeam.UnionWith(companyTeam);
            //调用ExceptWith可以将一个set中包含的另一个set的内容全部删除
            //companyTeam.ExceptWith(companyTeam);
        }
    }
}
