using System;

namespace FinalizeAndDispose
{
    class FA
    {
        /// <summary>
        /// Finalize 为C#类的析构函数 但很难确定该析构函数在何时调用 
        /// 因此C#中析构函数使用很少
        /// </summary>
        protected override void Finalize()
        {
            try
            {
            }
            finally
            {
                //如果存在继承 一定确保调用父类的析构函数
                //base.Finalize();
            }
        }
    }

    /// <summary>
    /// 实现IDisposable接口 是C#中处理析构的一般做法
    /// </summary>
    class FB : IDisposable
    {
        public void Dispose()
        {

        }
    }
    class FinalizeAndDispose
    {
        public static void Main()
        {
            //只需要确保FB实例在using中声明 就能确保Dispose函数被调用
            //也可以手动调用Dispose方法
            using (var fb = new FB())
            {
                //to do 
            }
        }

    }
}
