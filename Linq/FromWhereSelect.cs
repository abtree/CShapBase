using System;
using System.Collections.Generic;
using System.Linq;

namespace FromWhereSelect
{
    [Serializable]
    public class Racer:IComparable<Racer>, IFormattable
    {
        public Racer(string fname, string lname, string country, int starts, int wins)
            :this(fname, lname, country, starts, wins, null, null)
        {

        }

        public Racer(string fname, string lname, string country, int starts, int wins, IEnumerable<int> years, IEnumerable<string> cars)
        {
            this.FirstName = fname;
            this.LastName = lname;
            this.Country = country;
            this.Starts = starts;
            this.Wins = wins;
            this.Years = new List<int>(years);
            this.Cars = new List<string>(cars);
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Wins { get; set; }
        public string Country { get; set; }
        public int Starts { get; set; }
        public IEnumerable<string> Cars { get; private set; }
        public IEnumerable<int> Years { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }

        public int CompareTo(Racer other)
        {
            if (other == null) return -1;
            return string.Compare(this.LastName, other.LastName);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case null:
                case "N":
                    return ToString();
                case "F":
                    return FirstName;
                case "L":
                    return LastName;
                case "C":
                    return Country;
                case "S":
                    return Starts.ToString();
                case "W":
                    return Wins.ToString();
                case "A":
                    return string.Format("{0} {1}, {2}; starts: {3}, wins: {4}", FirstName, LastName, Country, Starts, Wins);
                default:
                    throw new FormatException(string.Format("Format {0} not supported", format));
            }
        }
    }

    [Serializable]
    public class Team
    {
        //params 关键字 必须放在函数最后 且只能出现一次
        //表示一个不定长参数数组 不能与 ref out 一起使用
        public Team(string name, params int[] years)
        {
            this.Name = name;
            this.Years = new List<int>(years);
        }

        public string Name { get; set; }
        public IEnumerable<int> Years { get; private set; }
    }

    public static class Formula1
    {
        private static List<Racer> racers;

        public static IList<Racer> GetChampions()
        {
            if(racers == null)
            {
                racers = new List<Racer>(40);
                racers.Add(new Racer("Nino", "Farina", "Italy", 33, 5, new int[] { 1950 }, new string[] { "Alfa Romeo" }));
                racers.Add(new Racer("Alberto", "Ascari", "Italy", 32, 10, new int[] { 1952, 1953 }, new string[] { "Ferrari" }));
                racers.Add(new Racer("Juan Manuel", "Fangio", "Argentina", 51, 24, new int[] { 1951, 1954, 1955, 1956, 1957 }, 
                    new string[] { "Alfa Romeo", "Maserati", "Mercedes", "Ferrari" }));
                racers.Add(new Racer("Mike", "Hawthorn", "UK", 45, 3, new int[] { 1958 }, new string[] { "Ferrari" }));
                racers.Add(new Racer("Phil", "Hill", "USA", 48, 3, new int[] { 1961 }, new string[] { "Ferrari" }));
                racers.Add(new Racer("John", "Surtees", "UK", 111, 6, new int[] { 1964 }, new string[] { "Ferrari" }));
                racers.Add(new Racer("Jim", "Clark", "UK", 72, 25, new int[] { 1963, 1965 }, new string[] { "Lotus" }));
                racers.Add(new Racer("Jack", "Brabham", "Australia", 125, 14, new int[] { 1959, 1960, 1966 }, new string[] { "Cooper", "Brabham" }));
                racers.Add(new Racer("Denny", "Hulme", "New Zealand", 112, 8, new int[] { 1967 }, new string[] { "Brabham" }));
                racers.Add(new Racer("Graham", "Hill", "UK", 176, 14, new int[] { 1962, 1968 }, new string[] { "BRM", "Lotus" }));
                racers.Add(new Racer("Jochen", "Rindt", "Austria", 60, 6, new int[] { 1970 }, new string[] { "Lotus" }));
                racers.Add(new Racer("Jackie", "Stewart", "UK", 99, 27, new int[] { 1969, 1971, 1973 }, new string[] { "Matra", "Tyrrell" }));
            }
            return racers;
        }

        private static List<Team> teams;
        public static IList<Team> GetContructorChampions()
        {
            if(teams == null)
            {
                teams = new List<Team>()
                {
                    new Team("Vanwall", 1958),
                    new Team("Cooper", 1959, 1960),
                    new Team("Ferrari", 1961, 1964, 1975, 1976, 1977, 1979, 1982, 1983, 1999, 2000, 2001, 2002, 2003, 2004, 2007, 2008),
                    new Team("BRM", 1962),
                    new Team("Lotus", 1963, 1965, 1968, 1970, 1972, 1973, 1978),
                    new Team("Brabham", 1966, 1967),
                    new Team("Matra", 1969),
                    new Team("Tyrrell", 1971),
                    new Team("McLaren", 1974, 1984, 1985, 1988, 1989, 1990, 1991, 1998),
                    new Team("Williams", 1980, 1981, 1986, 1987, 1992, 1993, 1994, 1996, 1997),
                    new Team("Benetton", 1995),
                    new Team("Renault", 2005, 2006),
                    new Team("Brawn GP", 2009),
                    new Team("Red Bull Racing", 2010, 2011)
                };
            }
            return teams;
        }
    }
    class FromWhereSelect
    {
        //要使用Linq 其容器必须实现IEnumerable<ISource> 接口
        private static void LinqQuery()
        {
            //创建linq的查询语句
            var query = from r in Formula1.GetChampions() where r.Country == "UK" orderby r.Wins descending select r; //where中可包含逻辑运算符
            /* 上面的linq语句 相当于下面的函数调用 */
            //IEnumerable<Racer> ukChampions = Formula1.GetChampions().Where(r => r.Country == "UK")
            //    .OrderByDescending(r => r.Wins).Select(r => r);

            foreach (Racer r in query)
            {
                Console.WriteLine("{0:A}", r);
            }
            /*
             * 查询语句必须以from开头，elect 或 group 结尾，中间可以用 where orderby join let 等
             */
        }

        private static void LinqString()
        {
            var names = new List<string>() { "Nino", "Alberto", "Juan", "Mike", "Phil" };
            var namesWithJ = from n in names where n.StartsWith("J") orderby n select n;  //linq查询在定义时 不会执行 在foreach时才执行
            Console.WriteLine("First iteration");
            foreach(string name in namesWithJ)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();

            names.Add("John");
            names.Add("Jim");
            names.Add("Jack");
            names.Add("Denny");
            Console.WriteLine("Second iteration");
            foreach (string name in namesWithJ)
            {
                Console.WriteLine(name);
            }
        }

        private static void OfTypeUse()
        {
            //OfType 的使用
            object[] data = { "one", 2, 3, " four", "five", 6 };
            var query = data.OfType<string>();
            foreach(var s in query)
            {
                Console.WriteLine(s);
            }
        }

        private static void MoreFrom()
        {
            //多个from的写法 转换为（SelectMany）的重载版本
            var ferrariDrivers = from r in Formula1.GetChampions()
                                 from c in r.Cars
                                 where c == "Ferrari"
                                 orderby r.LastName
                                 select r.FirstName + " " + r.LastName;
            foreach(string s in ferrariDrivers)
            {
                Console.WriteLine(s);
            }
        }

        private static void Group()
        {
            var countries = from r in Formula1.GetChampions()
                            group r by r.Country into g
                            orderby g.Count() descending, g.Key
                            where g.Count() >= 2
                            select new {
                                Country = g.Key,
                                Count = g.Count(),
                                //这里嵌套一个查询
                                Racers = from r1 in g
                                         orderby r1.LastName
                                         select r1.FirstName + " " + r1.LastName
                            };
            foreach(var item in countries)
            {
                Console.WriteLine("{0, -10} {1}", item.Country, item.Count);
            }
        }

        private static void Join()
        {
            var racers = from r in Formula1.GetChampions()
                         from y in r.Years
                         select new
                         {
                             Year = y,
                             Name = r.FirstName + " " + r.LastName
                         };
            var teams = from t in Formula1.GetContructorChampions()
                        from y in t.Years
                        select new
                        {
                            Year = y,
                            Name = t.Name
                        };
            var racersAndTeams = (from r in racers
                                  join t in teams on r.Year equals t.Year
                                  select new
                                  {
                                      r.Year,
                                      Champion = r.Name,
                                      Constructor = t.Name
                                  }).Take(10);  //取前10项
            //左外连接  teams 项可能为空值
            var racersWithTeams = (from r in racers
                                   join t in teams on r.Year equals t.Year into rt
                                   from t in rt.DefaultIfEmpty()
                                   orderby r.Year
                                   select new
                                   {
                                       Year = r.Year,
                                       Champion = r.Name,
                                       Constructor = t == null ? "no constructor championship" : t.Name
                                   }).Take(10);
            Console.WriteLine("Year World Champion \t Constructor Title");
            foreach (var item in racersAndTeams)
            {
                Console.WriteLine("{0}: {1, -20} {2}", item.Year, item.Champion, item.Constructor);
            }
            foreach (var item in racersWithTeams)
            {
                Console.WriteLine("{0}: {1, -20} {2}", item.Year, item.Champion, item.Constructor);
            }
        }

        private static void delegateSelect()
        {
            //定义一个委托方法保存查询
            Func<string, IEnumerable<Racer>> racersByCar = car =>
            from r in Formula1.GetChampions()
            from c in r.Cars
            where c == car
            orderby r.LastName
            select r;
            
            //查询同时满足这两个条件的集合
            foreach(var racer in racersByCar("Ferrari").Intersect(racersByCar("McLaren")))
            {
                Console.WriteLine(racer);
            }
        }

        private static void Zip()
        {
            var racerName = from r in Formula1.GetChampions()
                            where r.Country == "Italy"
                            orderby r.Wins descending
                            select new { Name = r.FirstName + " " + r.LastName };
            var racerNamesAndStarts = from r in Formula1.GetChampions()
                                      where r.Country == "Italy"
                                      orderby r.Wins descending
                                      select new { LastName = r.LastName, Starts = r.Starts };
            //此处将两次查询合并
            var racers = racerName.Zip(racerNamesAndStarts, (first, second) => first.Name + " , starts: " + second.Starts);
            foreach(var r in racers)
            {
                Console.WriteLine(r);
            }
        }

        //分页
        private static void PageSize()
        {
            int pageSize = 5;
            int numberPages = (int)Math.Ceiling(Formula1.GetChampions().Count() / (double)pageSize);
            for (int page = 0; page < numberPages; ++page)
            {
                var racers = (from r in Formula1.GetChampions()
                              orderby r.LastName, r.FirstName
                              select r.FirstName + " " + r.LastName).Skip(page * pageSize).Take(pageSize);
                foreach(var name in racers)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();
             }
        }

        private static void Math()
        {
            var query = from r in Formula1.GetChampions()
                        let numberYears = r.Years.Count()
                        where numberYears >= 3
                        orderby numberYears descending, r.LastName
                        select new
                        {
                            Name = r.FirstName + " " + r.LastName,
                            TimesChampion = numberYears
                        };
            foreach(var r in query)
            {
                Console.WriteLine("{0} {1}", r.Name, r.TimesChampion);
            }

            var countrys = (from c in
                                from r in Formula1.GetChampions()
                                group r by r.Country into c
                                select new
                                {
                                    Country = c.Key,
                                    Wins = (from r1 in c select r1.Wins).Sum()
                                }
                            orderby c.Wins descending, c.Country
                            select c).Take(10);
            foreach(var country in countrys)
            {
                Console.WriteLine("{0} {1}", country.Country, country.Wins);
            }
        }

        private static void ToLookup()
        {
            var racers = (from r in Formula1.GetChampions()
                          from c in r.Cars
                          select new
                          {
                              Car = c,
                              Racer = r
                          }).ToLookup(cr => cr.Car, cr => cr.Racer);
            if (racers.Contains("Williams"))
            {
                foreach(var williamsRacer in racers["Williams"])
                {
                    Console.WriteLine(williamsRacer);
                }
            }
        }

        //在非类型化的集合上使用linq查询 需要cast
        private static void Cast()
        {
            var lists = new System.Collections.ArrayList(Formula1.GetChampions() as System.Collections.ICollection);
            var quary = from r in lists.Cast<Racer>()
                        where r.Country == "USA"
                        orderby r.Wins descending
                        select r;
            foreach(var racer in quary)
            {
                Console.WriteLine("{0:A}", racer);
            }
        }

        private static void Range()
        {
            var values = Enumerable.Range(1, 30).Select(n => n * 3);
            foreach(var item in values)
            {
                Console.Write("{0} ", item);
            }
            Console.WriteLine();
        }

        public static void Main()
        {
            LinqQuery();
            LinqString();
            OfTypeUse();
            Group();
            Join();
            delegateSelect();
        }
    }
}

/*
 * Linq 中可用关键字 （值得注意的是 下面这些运算可以被重载 已实现特定的功能）
 * 
 * Where
 * OfType<TResult>  根据类型筛选
 * 
 * Select
 * SelectMany
 * 
 * OrderBy     下面几个都用于排序
 * ThenBy
 * OrderByDescending
 * ThenByDescending
 * Reverse
 * 
 * Join      用于根据条件连接 如sql
 * GroupJoin
 * 
 * GroupBy   分组  如sql
 * ToLookup  转换为lookup表 类似于分组
 * 
 * Any       查询限定符
 * All
 * Contains
 * 
 * Take       提取满足条件的子集
 * Skip
 * TakeWhile
 * SkipWhile
 * 
 * Distinct     去重
 * Union        连接
 * Intersect    返回两个集合都有的元素
 * Except       返回一个有一个没有的元素
 * Zip          合并两个集合
 * 
 * First            下面几个都是返回元素个数的限制
 * FirstOrDefault
 * Last
 * LastOrDefault
 * ElementAt
 * ElementAtOrDefault
 * Single
 * SingleOrDefault
 * 
 * Count       函数计算
 * Sum
 * Min
 * Max
 * Average      平均
 * Aggregate    
 * 
 * ToArray      结果的转换
 * AsEnumerable
 * ToList
 * ToDictionary
 * Cast<TResult>
 * 
 * Empty       返回空
 * Range       返回一系列数字
 * Repeat      返回始终重复的一个值的集合
 */
