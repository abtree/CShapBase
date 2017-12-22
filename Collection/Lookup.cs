using System;
using System.Collections.Generic;
using System.Linq;

namespace Lookup_
{
    public class Racer
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Country { get; private set; }
        public int Age { get; private set; }

        public Racer(string fname, string lname, string country, int age)
        {
            FirstName = fname;
            LastName = lname;
            Country = country;
            Age = age;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}({2}): {3}", FirstName, LastName, Age, Country);
        }
    }
    class Lookup_
    {
        public static void Main()
        {
            var racer = new List<Racer>(5);
            racer.Add(new Racer("Jacques", "VVas", "Canada", 21));
            racer.Add(new Racer("Alan", "VVas", "Australia", 20));
            racer.Add(new Racer("Jackie", "VVas", "The UK", 22));
            racer.Add(new Racer("James", "VVas", "USA", 19));
            racer.Add(new Racer("Jack", "VVas", "Canada", 21));

            //Lookup 容器只能通过 ToLookup创建
            var lookupRacer = racer.ToLookup(r => r.Country); //以Racer.Country为key建立lookup表

            //遍历特定key的值
            foreach(Racer r in lookupRacer["Canada"])
            {
                Console.WriteLine(r);
            }
        }
    }
}
