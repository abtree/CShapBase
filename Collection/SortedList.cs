using System;
using System.Collections.Generic;

namespace SortedList_
{
    //SortedList 是 key 不能重复的列表(数组实现)
    class SortedList_
    {
        public void handle()
        {
            SortedList<string, int> st = new SortedList<string, int>();
            st.Capacity = 10; //设置容积
            st.Add("aaa", 1);  //如果“aaa”存在 会报错
            st["bbb"] = 2;   //用下标添加元素

            st.TrimExcess(); //重新设置容积

            //第一种遍历
            foreach (KeyValuePair<string, int> cell in st)
            {
            }
            //只遍历key
            foreach(string key in st.Keys)
            {

            }
            //只遍历value
            foreach(int value in st.Values) { }

            bool exits = st.ContainsKey("aaa");
            exits = st.ContainsValue(1);
            int value = 0;
            exits = st.TryGetValue("bbb",out value);

            st.Remove("bbb");
            var index = st.IndexOfKey("aaa");  //获取索引
            st.Clear();
        }
    }
}
