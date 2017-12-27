using System;

namespace Pointer
{
    /// <summary>
    /// 可以将整个类标记为unsafe
    /// </summary>
    unsafe public class UnsafeClass
    {

    }

    public class UnsafeClass2
    {

        /// <summary>
        /// 也可以将成员 或成员函数标记为unsafe
        /// 注意 指针只能声明值类型 而不能用于数组或类类型 因为会破坏垃圾回收器工作
        /// </summary>
        unsafe int* pX;
        unsafe void* pV;  //C# 是支持void指针的

        unsafe int* GetX()
        {
            return pX;
        }

        void handle()
        {
            //也可以标记代码块unsafe
            unsafe
            {
                //注意 是在stack上分配的内存 和C++不同(分配的内存没有初始化)
                decimal* pDecimals = stackalloc decimal[10];
                *(pDecimals) = 0;
                *(pDecimals + 1) = 1;
                pDecimals[2] = 2;  // 这里等价于 *(pDecimals + 2) = 2;
                *pX += 10;
            }
        } 
    }
    class Pointer
    {
    }
}
