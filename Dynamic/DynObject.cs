using System.Dynamic;
using System.Collections.Generic;
using System;

namespace DynObject
{
    /// <summary>
    /// 通过继承的方法创建自己的动态对象
    /// </summary>
    internal class DynObject : DynamicObject
    {
        Dictionary<string, object> _dynamicData = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            bool success = false;
            result = null;
            if (_dynamicData.ContainsKey(binder.Name))
            {
                result = _dynamicData[binder.Name];
                success = true;
            }
            else
            {
                result = "Property Not Found!";
                success = false;
            }
            return success;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dynamicData[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = _dynamicData[binder.Name];
            result = method((DateTime)args[0]);
            return result != null;
        }
    }

    class Handle
    {
        public static void Main()
        {
            dynamic dynObj = new DynObject();
            dynObj.FirstName = "Bugs";  //添加属性
            dynObj.LastName = "Bunny";
            Func<DateTime, string> GetTomorrow = today => today.AddDays(1).ToShortDateString();
            dynObj.GetTomorrowDate = GetTomorrow;  //添加方法
            Console.WriteLine(dynObj.GetType());
            Console.WriteLine("{0} {1}", dynObj.FirstName, dynObj.LastName);
            Console.WriteLine("Tomorrow is: {0}", dynObj.GetTomorrowDate(DateTime.Now));

            //系统自带的对象(行为和上面一样)
            dynamic expObj = new ExpandoObject();
            expObj.FirstName = "Bugs";  //添加属性
            expObj.LastName = "Bunny";
            expObj.GetTomorrowDate = GetTomorrow;  //添加方法
            Console.WriteLine(expObj.GetType());
            Console.WriteLine("{0} {1}", expObj.FirstName, expObj.LastName);
            Console.WriteLine("Tomorrow is: {0}", expObj.GetTomorrowDate(DateTime.Now));
        }
    }
}
