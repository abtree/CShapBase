using System;

namespace Property
{
    class Property
    {
        private int age;
        //定义一个变量的属性
        //属性可以只设置一个（此时变成只读或只写属性）
        //或者改变一个的访问权限（如 private set）但不能两个都不为public的
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;  //value为传入参数
            }
        }
        //自动实现的属性(属性必须同时有get 和 set方法 但访问权限可以不同)
        public string Name
        {
            get;
            set;
        }
        /*
        * 此时系统会自动添加
        * private string Name;  //属性 和 字段名称 相同
        * 字段
        */
    }
}
