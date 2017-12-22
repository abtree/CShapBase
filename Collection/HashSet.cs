using System;
using System.Collections.Generic;

namespace HashSet_
{
    class HashSet_
    {
        public void op() {
            var companyTeam = new HashSet<string>() { "aaa", "bbb", "ccc" };
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
