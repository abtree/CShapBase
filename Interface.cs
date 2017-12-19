using System;

// 注意 接口中 不能定义操作符重载接口 也不能有字段 但可以有属性
// 另外 接口可以继承接口
namespace Interface
{
    //注意 接口中没有显示声明public
    public interface IBase
    {
        void PayIn(decimal amount);
        decimal Balance { get; }
    }
    class Interface : IBase
    {
        //实现接口方法
        public void PayIn(decimal amount)
        {
            Balance += amount;
        }

        //属性需要重新定义(此处属性定义了set方法)
        public decimal Balance
        {
            get;set;
        }
    }
}
