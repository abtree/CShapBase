using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObservableCollection_
{
    class ObservableCollection_
    {
        //可观察的集合 是为WPF设计的 可以监测集合中数据的变化 以做出相应的回掉
        public static void DataCollectionChange(Object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("action: {0}", e.Action.ToString());

            if(e.OldItems != null){
                Console.WriteLine("starting index for old item(s): {0}", e.OldStartingIndex);
                Console.WriteLine("old item(s):");
                foreach(var item in e.OldItems)
                {
                    Console.WriteLine(item);
                }
            }
            if(e.NewItems != null){
                Console.WriteLine("starting index for new item(s):{0}", e.NewStartingIndex);
                Console.WriteLine("new item(s):");
                foreach(var item in e.NewItems)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine();
        }

        public static void Main()
        {
            var data = new ObservableCollection<string>();
            data.CollectionChanged += DataCollectionChange;
            data.Add("One");
            data.Add("Two");
            data.Insert(1, "Three");
            data.Remove("one"); 
        }
    }
}
