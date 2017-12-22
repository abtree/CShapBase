using System;

namespace Delegate_
{
    //委托 是一个特殊的对象 他保存函数的地址（可以解决C#里面函数不能独立与对象存在的问题）

    internal class MathOperator
    {
        public static double MultiplyByTwo(double value)
        {
            Console.WriteLine(value * 2);
            return value * 2;
        }

        public static double Square(double value)
        {
            Console.WriteLine(value * value);
            return value * value;
        }

        public static void MultiplyByTwoV(double value)
        {
            Console.WriteLine(value * 2);
        }

        public static void SquareV(double value)
        {
            Console.WriteLine(value * value);
        }
    }
    class Delegate_
    {
        //定义委托就是声明 该委托接受什么类型的函数指针
        public delegate double DoubleOp(double db);//DoubleOp 为委托名

        public static void Main(String[] args)
        {
            //使用委托 (这里虽然只用了静态方法 但委托可以用实例方法)
            DoubleOp gs = new DoubleOp(MathOperator.MultiplyByTwo);   //依次执行委托中的方法 但顺序是不确定的
            gs += MathOperator.Square;       //这是上面形式的简写形式
            gs(2.3);  // 等价于 gs.Invoke() 
            Console.WriteLine("xxxxxxxxxxxxx");
            //用Action模版代替委托(注意 action 要求 函数返回值为void)
            Action<double> actionOp = MathOperator.MultiplyByTwoV; //其中一个函数抛出异常 整个委托调用就停止
            actionOp += MathOperator.SquareV;
            //自己迭代委托中的函数
            Delegate[] delegates = actionOp.GetInvocationList();
            foreach(Action<double> d in delegates)
            {
                d(2.3);
            }
            Console.WriteLine("xxxxxxxxxxxxx");

            //委托的数组
            DoubleOp[] operArray =
            {
                MathOperator.MultiplyByTwo, MathOperator.Square
            };
            for(int i = 0; i < 2; ++i)
            {
                ProcessAndDisplayNumber(operArray[i], 2.2);
            }
            Console.WriteLine("xxxxxxxxxxxxx");
            //改成委托模版
            Func<double, double>[] operT = {
                MathOperator.MultiplyByTwo, MathOperator.Square
            };
            for (int i = 0; i < 2; ++i)
            {
                ProcessAndDisplayNumber(operT[i], 3.2);
            }
            Console.WriteLine("xxxxxxxxxxxxx");
        }

        //委托作为函数参数
        static void ProcessAndDisplayNumber(DoubleOp action, double db)
        {
            double result = action(db);
            Console.WriteLine(result);
        }

        static void ProcessAndDisplayNumber(Func<double, double> action, double db)
        {
            double result = action(db);
            Console.WriteLine(result);
        }
    }
}
