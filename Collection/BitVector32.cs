using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace BitVector32_
{
    class BitVector32_
    {
        public static void Main()
        {
            var bits1 = new BitVector32();
            //bits1[0xabcdef] = true;
            Console.WriteLine(bits1);

            int bit1 = BitVector32.CreateMask();
            int bit2 = BitVector32.CreateMask(bit1);
            int bit3 = BitVector32.CreateMask(bit2);
            int bit4 = BitVector32.CreateMask(bit3);
            BitVector32.Section bTemp = BitVector32.CreateSection(0xf);  //这里用于占位
            BitVector32.Section bit5 = BitVector32.CreateSection(0xf, bTemp);

            bits1[bit1] = true;
            bits1[bit2] = false;
            bits1[bit3] = true;
            bits1[bit4] = true;
            bits1[0xF00] = true;

            Console.WriteLine(bits1);
        }
    }
}
