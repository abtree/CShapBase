using System.Collections.Generic;
using System;

namespace ConditionAndLoop
{
    class ConditionAndLoop
    {
        static void Main(string[] args)
        {
            //if 语句
            bool condition = false;
            bool otherCondition = true;
            if (condition)
            {
                // todo sth
            }
            else if (otherCondition)
            {
                //todo sth
            }
            else
            {
                // todo sth
            }
            // switch
            string key = "a";
            switch (key)
            {
                case "a":
                    //todo sth
                    break;
                case "b":
                    //todo sth
                    break;
                case "c":
                case "d":
                    //todo sth
                    break;
                default:
                    //todo sth
                    break;
            }
            //for loop
            for(int i = 0; i < 10; ++i)
            {
                //todo sth;
            }
            // while loop
            while (otherCondition)
            {
                //todo sth
            }
            //do ... while loop
            do
            {
                //todo sth
            } while (condition);
            // foreach loop
            List<int> list = new List<int>();
            foreach(int li in list)
            {
                // todo sth
            }
            // goto
            goto NEXT;  //goto只能在函数内使用
            //todo sth
NEXT:
            //todo sth
            // continue
            // break
            // return
        }
    }
}
