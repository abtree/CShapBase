using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace BitArray_
{
    class BitArray_
    {
        public static void DisplayBits(BitArray bits)
        {
            foreach(bool bit in bits)
            {
                Console.Write(bit ? 1 : 0);
            }
        }

        public static void Main()
        {
            var bits1 = new BitArray(8);
            bits1.SetAll(true);  //设置所有位都为true
            bits1.Set(1, false); //设置第2位为false；
            bits1[5] = false;  //设置第6位为false
            bits1[7] = false;  //设置第8位为false
            Console.Write("initialized: ");
            DisplayBits(bits1);
            bits1.Not(); //所有位取反
            Console.WriteLine(bits1.Get(1));  //获取第2位的值
            Console.WriteLine();
            var bits2 = new BitArray(bits1);  //将bits2设置为和bits1相同的值
            bits2[0] = true;
            bits2[1] = false;
            bits2[4] = true;
            bits1.Or(bits2);  //将bits1 he bits2 求或 
            bits2.And(bits1); //将bits2 和 bits1 求于
            bits1.Xor(bits2); //将bits1 异或 bits2
        }
    }
}
