using System;
using System.Collections;
using System.Collections.Generic;

namespace Enumerator_
{
#region 一个简单迭代器实例
    public class HelloWorld
    {
        string[] names = { "aaa", "bbb", "ccc", "dddd" };

        public IEnumerator<string> GetEnumerator()
        {
           for(int i = 0; i < names.Length; ++i)
            {
                yield return names[i];
            }
            yield break;  //终止迭代
        }

        public IEnumerable<string> Reverse()
        {
            for (int i = names.Length - 1; i >= 0; --i)
            {
                yield return names[i];
            }
        }

        public IEnumerator<string> Subset(int index, int length)
        {
            for (int i = index; i < index + length; ++i)
            {
                yield return names[i];
            }
        }
    }
#endregion
#region 返回Enumerator
    public class GameMoves
    {
        private IEnumerator cross;
        private IEnumerator circle;

        public GameMoves()
        {
            cross = Cross();
            circle = Circle();
        }

        private int move = 0;
        const int MaxMoves = 9;

        public IEnumerator Cross()
        {
            while (true)
            {
                Console.WriteLine("Cross, move {0}", move);
                if (++move >= MaxMoves)
                    yield break;
                yield return circle;
            }
        }

        public IEnumerator Circle()
        {
            while (true)
            {
                Console.WriteLine("Circle, move {0}", move);
                if (++move >= MaxMoves)
                    yield break;
                yield return cross;
            }
        }
    }
#endregion
    class Enumerator_
    {
        public static void Main(String[] args)
        {
            //简单迭代器实例
            var hw = new HelloWorld();
            foreach(var s in hw.Reverse())
            {
                Console.Write(s);
            }
            //返回Enumerator
            var game = new GameMoves();
            IEnumerator enumerator = game.Cross();
            while (enumerator.MoveNext())
            {
                enumerator = enumerator.Current as IEnumerator;
            }
        }
    }
}
