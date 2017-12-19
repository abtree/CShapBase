using System;

//弱引用
namespace WeakReference_
{
    class RefClass
    {
        public int A = 100; 
    };
    class WeakReference_
    {
        public static void Main(string[] args)
        {
            //建立弱引用对象
            WeakReference refClass = new WeakReference(new RefClass());
            //使用取前一定要检查对象是否存活
            if (refClass.IsAlive)  //此时还活着
            {
                RefClass clas = refClass.Target as RefClass;
                Console.WriteLine("refclass is alive value is {0}", clas.A.ToString());
            }
            else
            {
                Console.WriteLine("refclass is dead");
            }
            GC.Collect();  //此时调用垃圾回收器清理垃圾
            if (refClass.IsAlive)  //已经被回收
            {
                RefClass clas = refClass.Target as RefClass;
                Console.WriteLine("refclass is alive value is {0}", clas.A.ToString());
            }
            else
            {
                Console.WriteLine("refclass is dead");
            }
        }
    }
}
