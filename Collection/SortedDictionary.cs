using System;
using System.Collections.Generic;

namespace SortedDictionary_
{
    class SortedDictionary_
    {
        //有序字典使用的是一个二叉树的存储方式 类似与c++的map  用法同SortedList
        public void op()
        {
            SortedDictionary<string, int> st = new SortedDictionary<string, int>();
            st.Add("aaa", 1);
            st["bbb"] = 2;
            //第一种遍历
            foreach (KeyValuePair<string, int> cell in st)
            {
            }
            //只遍历key
            foreach (string key in st.Keys)
            {

            }
            //只遍历value
            foreach (int value in st.Values) { }

            bool exits = st.ContainsKey("aaa");
            exits = st.ContainsValue(1);
            int value = 0;
            exits = st.TryGetValue("bbb", out value);

            st.Remove("bbb");
            var index = st.IndexOfKey("aaa");  //获取索引
            st.Clear();
        }
    }
}
