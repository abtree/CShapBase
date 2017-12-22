using System;
using System.Collections.Generic;

namespace List_
{
    class List_
    {
        List<int> intList = new List<int>();
        //初始化
        void init()
        {
            List<int> list1 = new List<int>(10);  //设置初始Capacity为10
            list1.Capacity = 20;  //扩大Capacity
            intList.TrimExcess();  //清除剩余的Capacity
            var list2 = new List<int>() { 1, 2 };  //创建并初始化
        }

        void insert()
        {
            intList.Add(1);  //直接添加
            intList.Insert(0, 2); //在指定位置插入
            //下标访问
            int v = intList[0];
            //遍历
            foreach (int i in intList)
            {
                Console.WriteLine(i);
            }

            //用自己的foreach方法
            intList.ForEach((int x) => Console.WriteLine(x));
        }

        void find()
        {
            bool exist = intList.Exists(x => x == 3);  //是否有值为3的元素
            exist = intList.Contains(3);  //确定元素3是否在集合
            int ind = intList.IndexOf(3);  //获取3在列表的位置
            var rets = intList.FindAll(x => x % 2 == 1);  //找出所有奇数
            int ret = intList.FindIndex(x => x % 2 == 0);  //找到第一个偶数
        }

        void sort()
        {
            intList.Sort((x1, x2) => {
                if (x1 > x2)
                    return 1;
                else if (x1 == x2)
                    return 0;
                else return -1;
            });  //根据条件排序
            intList.Reverse(); //反转
        }

        void convert()
        {
            //将int list 转换为 float list （实际可以实现对象之间的转换）
            List<float> list = intList.ConvertAll<float>(x => Convert.ToSingle(x));
        }

        void _readonly()
        {
            //在做配置文件时 可通过这个 列表变成只读的
            var ro = intList.AsReadOnly();
        }

        void remove()
        {
            intList.RemoveAt(0);  //根据索引删除（较快）
            intList.Remove(2);  //先在list搜索相应元素 再根据索引删除
            intList.RemoveAll(x => x % 2 == 0);  //删除所有偶数元素
            intList.Clear();  //清空
        }
    }
}
