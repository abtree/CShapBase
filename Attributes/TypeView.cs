using System;
using System.Text;
using System.Reflection;

namespace TypeView
{
    class TypeView
    {
        static StringBuilder _outputText = new StringBuilder();

        public static void AddToOutPut(string text)
        {
            _outputText.Append("\n" + text);
        }
        public static void AnalyzeType(Type t)
        {
            if (t == null)
                return;

            _outputText.Clear();
            
            AddToOutPut("Type Name: " + t.Name);
            AddToOutPut("Full Name: " + t.FullName);
            AddToOutPut("NameSpace: " + t.Namespace);

            Type tbase = t.BaseType;
            if (tbase != null)
            {
                AddToOutPut("Base Type: " + tbase.Name);
            }

            Type tUnderlySystem = t.UnderlyingSystemType;  //类型 在.Net运行库中映射的类型
            if (tUnderlySystem != null)
            {
                AddToOutPut("UnderlyingSystem Type: " + tUnderlySystem.Name);
            }

            AddToOutPut("\n PUBLIC Members: ");
            MemberInfo[] members = t.GetMembers();

            foreach(MemberInfo nextMember in members)
            {
                AddToOutPut(nextMember.DeclaringType + " " + nextMember.MemberType + " " + nextMember.Name);
            }
        }

        public static void Main()
        {
            Type t = typeof(double);
            AnalyzeType(t);
            Console.Write(_outputText);
            Console.WriteLine();
            Console.WriteLine("Analysis of type {0}", t.Name);
        }
    }
}
