using System;

//注意 c# 的 internal 表示只能在同一个 命名空间中访问
/* 访问限制
 * public       所有代码可以访问
 * protected    类和派生类可以访问
 * internal     同一个命名空间内可以访问
 * private      类自己可以访问
 * protected internal  用一个命名空间的派生类可以访问
 */

/*  修饰符
 *  new 用相同名称（含参数）的成员隐藏父类方法
 *  static 静态方法
 *  virtual 虚函数 可由派生类重写
 *  abstract 纯虚函数  必须由派生类重写
 *  override 重写父类函数
 *  sealed   不能继承的类或方法
 *  extern  兼容其它语言 外来的方法
 */

namespace Extend
{
    // abstract 定义类为抽象类 不能被实例化
    abstract class Base
    {
        //第一个方法声明为虚方法
        virtual public void method1()
        {

        }
        //第二个声明为普通方法
        public void method2()
        {

        }

        //定义一个抽象函数 相当于c++的纯虚函数
        abstract public void method3();
    }

    // sealed 表示这个类不能被继承 (将 method 声明为 sealed 也可以 防止继承)
    sealed class Final
    {

    }
    class Extend : Base  //继承与Base
    {
        //重写基类方法
        public override void method1()
        {
            //注意 base 可以调用基类的任意方法 不一定是重载的方法
            base.method1();
        }

        //显示隐藏基类方法(用 new 关键字)
        public new void method2()
        {
            //隐藏后依然可以调用
            base.method2();
        }

        //必须实例化基类的抽象函数
        public override void method3()
        {
            throw new NotImplementedException();
        }
    }
}
