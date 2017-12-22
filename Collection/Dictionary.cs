using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Dictionary_
{
    //Dictionary 是根据对象的GetHashCode来组织key的
    //相同的对象 返回的GetHashCode必须相同 才能找到相同的value
    //Object 的默认GetHashCode 是根据对象地址获取hashCode的 因此没有问题
    //       但只能是同一个对象的引用能获取到value 如果需要实现两个相等对象可获取相同的value 需要重写GetHashCode和Equals方法
    //string 对象 默认重载了该方法 相等的string可以获取相同的value

    //特别注意 dectionary 会根据hashcode组织数据 如果hashcode重复 会将数据插入附近的空格子里面 所有hashcode如果能均匀分布 将提高效率
    //dectionary中key不能重复
    [Serializable]
    public class EmployeeIdException : Exception
    {
        public EmployeeIdException(string message) : base(message)
        {

        }
    }

    //定义一个id class 用做Dictionary的key
    [Serializable]
    public class EmployeeId : IEquatable<EmployeeId>
    {
        private readonly char prefix;
        private readonly int number;

        public EmployeeId(string id)
        {
            Contract.Assert(id != null);
            //Contract.Requires<ArgumentNullException>(id != null);

            prefix = (id.ToUpper())[0];
            int numLength = id.Length - 1;
            try
            {
                number = int.Parse(id.Substring(1, numLength > 6 ? 6 : numLength));
            }
            catch (FormatException)
            {
                throw new EmployeeIdException("Invalid EmployeeId format");
            }
        }

        public override string ToString()
        {
            return prefix.ToString() + string.Format("{0, 6:000000}", number);
        }

        public override int GetHashCode()
        {
            //hashcode的算法有很多 可在网上找
            // 也可以用 string 的 hashcode 算法
            return (number ^ number << 16) * 0x15051505;
        }

        public bool Equals(EmployeeId other)
        {
            //这里因为重载了Equals 和 == 操作 如果直接用 == 判断 null 会调用重载后的 ==
            //根据下面的定义 会出现死循环 所有将对象转换为 object
            if ((other as object) == null) return false;
            return (prefix == other.prefix && number == other.number);
        }

        public override bool Equals(object obj)
        {
            return Equals((EmployeeId)obj);
        }

        public static bool operator == (EmployeeId left, EmployeeId right)
        {
            return left.Equals(right);
        }

        public static bool operator != (EmployeeId left, EmployeeId right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class Employee
    {
        private string name;
        private decimal salary;
        private readonly EmployeeId id;

        public Employee(EmployeeId id, string name, decimal salary)
        {
            this.id = id;
            this.name = name;
            this.salary = salary;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1, -20} {2:C}", id.ToString(), name, salary);
        }
    }
    class Dictionary_
    {
        public static void Main()
        {
            var employees = new Dictionary<EmployeeId, Employee>(31);

            var idTony = new EmployeeId("C3755");
            var tony = new Employee(idTony, "Tony Stewart", 379025.00m);
            employees.Add(idTony, tony);
            Console.WriteLine(tony);

            var idCarl = new EmployeeId("F3547");
            var carl = new Employee(idCarl, "Carl Edwards", 403466.00m);
            employees.Add(idCarl, carl);
            Console.WriteLine(carl);

            var idKevin = new EmployeeId("C3386");
            var kevin = new Employee(idKevin, "Kevin Harwick", 415261.00m);
            employees.Add(idKevin, kevin);
            Console.WriteLine(kevin);

            var idMatt = new EmployeeId("F3323");
            var matt = new Employee(idMatt, "Matt Kenseth", 1415261.00m);
            employees.Add(idMatt, matt);
            Console.WriteLine(matt);

            var idBrad = new EmployeeId("D3234");
            var brad = new Employee(idBrad, "Brad Kenseth", 315261.00m);
            employees.Add(idBrad, brad);
            Console.WriteLine(brad);

            while (true)
            {
                Console.WriteLine("Enter employee id (X to exit)>");
                var userInput = Console.ReadLine();
                userInput = userInput.ToUpper();
                if (userInput == "X")
                    break;

                EmployeeId id;
                try
                {
                    id = new EmployeeId(userInput);
                    Employee employee;
                    if(!employees.TryGetValue(id, out employee))
                    {
                        Console.WriteLine("Employee with id{0} does not exist", id);
                    }
                    else
                    {
                        Console.WriteLine(employee);
                    }
                }
                catch (EmployeeIdException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            employees.Clear();
        }
    }
}
