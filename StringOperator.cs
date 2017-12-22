using System;
using System.Text;
using System.Text.RegularExpressions;

namespace StringOperator
{
    //string 本身是一个值不可变的对象 每次修改都会重新创建 所以效率低下
    //StringBuilder 专门处理字符串的修改增加或删除操作 非常高效
    //在使用StringBuilder时 应尽量保证StringBuild有足够的Capacity 已防止StringBuild的扩容操作 提高效率
    //StringBuilder 不能强制转换为String 必须调用ToString()
    class StringOperator
    {
        const string myText =
            @"This comprehensive compendium provides a broad and through investigation of all
              aspects of programming with ASP.NET. Entirely revised and updated for the fourth
              release of .NET, this book will give you the information you need to master ASP.NET
              and build a dynamic, successful, enterprise Web application";

        //查询所有包含特定字符串的地方
        public void FindAll()
        {
            //搜索包含ion的字符
            //const string pattern = "ion";
            //搜索已字符'n'开头的单词
            //const string pattern = @"\bn";
            //搜索已ion结尾的字符
            //const string pattern = @"ion\b";
            //搜索已a开始 已ion结束的单词  \S表示任意非空白字符 *表示任意次 包括0次
            const string pattern = @"\ba\S*ion\b";
            MatchCollection myMatches = Regex.Matches(myText, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            foreach (Match nextMatch in myMatches)
            {
                int index = nextMatch.Index;
                string result = nextMatch.ToString();   //这里存储了匹配到的单词
                int charsBefore = (index < 5) ? index : 5;
                int fromEnd = myText.Length - index - result.Length;
                int charsAfter = (fromEnd < 5) ? fromEnd : 5;
                int charsToDisplay = charsBefore + charsAfter + result.Length;
                Console.WriteLine("index:{0}, \t String: {1}, \t{2}", index, result, myText.Substring(index - charsBefore, charsToDisplay));
            }
        }

        /*
         * 正则表达式符号
         * ^ 文本开头 例：^Bin 文本已Bin开头
         * $ 文本结尾 例：ion$ 文本已ion结尾
         * . 除换行符的所有字符（一个）例：a.ion
         * * 可重复0次或n次的前导字符  例：ra*t  a 重复0到n次
         * + 可重复1次或n次的前导字符  例：ra+t  a 重复1到n次
         * ？可重复0次或1次的前导字符  例：ra?t  只能匹配 rt he rat
         * \s 任何空白字符
         * \S 任何非空白字符
         * \b 单词头或尾
         * \B 单词非头尾的任意位置
         * [a-z]+ 表示匹配至少一个a-z之间的字符
         * [a|p]. 表示匹配a或者p
         * (an)+ 将an看作一个单词组 匹配至少一个an组的单词
         */

        public static void Main(string[] args)
        {
            StringOperator so = new StringOperator();
            so.FindAll();
        }
    }
}
