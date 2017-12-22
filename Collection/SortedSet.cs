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
        }
    }
}
