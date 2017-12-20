using System;

namespace Array_
{
    public class CA
    {

    }
    //数组是引用类型
    class Array_
    {
        int[] arrayA = new int[4] { 1, 3, 0, 2 };  //普通数组
        //因为数组是引用类型 所以复制数组需要调用clone函数
        void ArrayClone()
        {
            //Clone会创建新的数组
            int[] arrayE = (int[])arrayA.Clone(); //如果数组元素是值类型 会复制值 否则复制引用
            Array.Sort(arrayA);  //使用 QuickSort 算法对数组排序(对 对象排序 要求对象实现IComparable 接口)
            //如果两个数组都已经创建 也可以拷贝值（拷贝不创建新的数组）
            Array.Copy(arrayA, arrayE, arrayE.Length);
        }

        //数组作为参数
        void ArrayParams(int[] arr)
        {

        }
        
        int[,] arrayC =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };  //3x3的2维数组

        //锯齿数组 数组每一列元素长度不同
        void SawtoothArray()
        {
            int[][] arrayD = new int[3][];
            arrayD[0] = new int[2];
            arrayD[1] = new int[3];
            arrayD[2] = new int[4];

            //遍历
            for(int row = 0; row < arrayD.Length; ++row)
            {
                for(int cul = 0; cul < arrayD[row].Length; ++cul)
                {
                    Console.Write(arrayD[row][cul]);
                }
            }
        }

        CA[] arrayB = new CA[] { new CA(), new CA() };   //如果数组元素是引用类型 必须为每个元素分配内存
        #region Array class
        void ArrayClass()
        {
            //Array类是一个抽象的类 不能实例化
            //它反应了数组在C#底层的实现 
            Array intArray = Array.CreateInstance(typeof(int), 5);  //实际上是创建了 int[5] 数组
            //CreateInstance有许多重载方法 可以实现各种数组
            for (int i = 0; i < 5; ++i)
            {
                intArray.SetValue(0, i);  //赋值
            }
            for(int i = 0; i < 5; ++i)
            {
                Console.Write(intArray.GetValue(i));  //获取值
            }
            int[] intArray2 = (int[])intArray;  //强制转换为普通数组
        }
        #endregion

        #region ArraySegment class 对数组部分进行操作
        public int SumOfSegments(ArraySegment<int>[] segments)
        {
            //对多个数组段值求和
            int sum = 0;
            foreach(var segment in segments)
            {
                 for(int i = segment.Offset; i< segment.Offset + segment.Count; ++i)
                {
                    sum += segment.Array[i];
                }
            }
            return sum;
        }

        public void SegmentArray()
        {
            int[] arrayE = (int[])arrayA.Clone();
            Array.Sort(arrayA);
            var segments = new ArraySegment<int>[]
            {
                new ArraySegment<int>(arrayA, 0, 2),  //只去0-2部分
                new ArraySegment<int>(arrayE, 1, 3)   //取1-3部分
            };
            Console.WriteLine(SumOfSegments(segments));
        }
        #endregion
    }
}
