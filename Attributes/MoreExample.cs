using System;
using System.Collections;
using System.Reflection;
using System.Text;

//assembly 和 module 可以将特性用于整个程序集或模块 但使用时 必须加上 assembly 和 module 关键字
[assembly: MoreExample.SupportsWhatsNewAttributes]
namespace VectorClass
{
    [MoreExample.LastModifiedAttribute("14 Feb 2017", "IEnumerable interface implemented" + "So Vector can now be treated as a collection")]
    [MoreExample.LastModified("10 Feb 2016", "IFormattable interface implemented" + "So Vector new responds to format specifiers N And VE")]
    public class Vector:IFormattable, IEnumerable
    {
        public double x, y, z;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MoreExample.LastModified("10 Feb 2016", "Method added in order to provide formatting support")]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public IEnumerator GetEnumerator()
        {
            return null;
        }
    }

    [MoreExample.LastModified("14 Feb 2016", "Class created as part of collection support for Vector")]
    public class VectorEnumerator : IEnumerator
    {
        public object Current { get; set; }

        public bool MoveNext() { return false; }
        public void Reset() { }
    }
}

namespace MoreExample
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class LastModifiedAttribute : Attribute
    {
        private readonly DateTime _dateModified;
        private readonly string _changes;

        public LastModifiedAttribute(string dateMpdified, string changes)
        {
            _dateModified = DateTime.Parse(dateMpdified);
            _changes = changes;
        }

        public DateTime DateModified
        {
            get { return _dateModified; }
        }

        public string Changes
        {
            get { return _changes; }
        }

        public string Issues { get; set; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class SupportsWhatsNewAttributes : Attribute
    {

    }

    internal class MoreExample
    {
        private static readonly StringBuilder _outputText = new StringBuilder(1000);
        private static DateTime backDateTo = new DateTime(2017, 12, 27);

        static void AddToMessage(string message)
        {
            _outputText.Append("\n" + message);
        }
        public static void Main()
        {
            Assembly theAssembly = Assembly.Load("VectorClass");
            Attribute supportsAttribute = Attribute.GetCustomAttribute(theAssembly, typeof(SupportsWhatsNewAttributes));
            string name = theAssembly.FullName;
            AddToMessage("Assembly: " + name);

            if (supportsAttribute == null)
            {
                AddToMessage("This assembly does not support WhatsNew attributes");
                return;
            }
            else
            {
                AddToMessage("Defined Types: ");
            }

            Type[] types = theAssembly.GetTypes();
            foreach(Type definedType in types)
            {
                DisplayTypeInfo(definedType);
            }

            Console.Write(_outputText);
            Console.WriteLine();
            Console.WriteLine(@"What's New since {0}", backDateTo.ToLongDateString());
        }

        private static void DisplayTypeInfo(Type type)
        {
            //make sure we only pick out classes
            if (!(type.IsClass))
                return;

            AddToMessage("\nclass " + type.Name);
            Attribute[] attribs = Attribute.GetCustomAttributes(type);

            if(attribs.Length == 0)
            {
                AddToMessage("No changes to this attribs");
            }
            else
            {
                foreach(Attribute attrib in attribs)
                {
                    WriteAttributeInfo(attrib);
                }
            }

            MethodInfo[] methods = type.GetMethods();
            AddToMessage("Changes to Methods of This Class:");

            foreach(MethodInfo nextMethod in methods)
            {
                object[] attribs2 = nextMethod.GetCustomAttributes(typeof(LastModifiedAttribute), false);

                if(attribs2 != null)
                {
                    AddToMessage(nextMethod.ReturnType + " " + nextMethod.Name + "()");
                    foreach(Attribute nextAttrib in attribs2)
                    {
                        WriteAttributeInfo(nextAttrib);
                    }
                }
            }
        }

        private static void WriteAttributeInfo(Attribute attrib)
        {
            LastModifiedAttribute lastModifiedAttribute = attrib as LastModifiedAttribute;
            if(lastModifiedAttribute == null)
            {
                return;
            }

            //check that date is in range
            DateTime modifiedDate = lastModifiedAttribute.DateModified;
            if(modifiedDate < backDateTo)
            {
                return;
            }

            AddToMessage(" Modified: " + modifiedDate.ToLongDateString() + ":");
            AddToMessage(" " + lastModifiedAttribute.Changes);

            if (lastModifiedAttribute.Issues != null)
            {
                AddToMessage(" Outstanding issues:" + lastModifiedAttribute.Issues);
            }
        }
    }
}
