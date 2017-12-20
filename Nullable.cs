using System;

namespace Nullable
{
    class Nullable
    {
        public static void Main(String[] args)
        {
            int x = 0;
            int? y = null;  //在值类型后面加 ？ 可以定义可空值类型（引用类型无意义）
            y = x;   //值类型 可 隐式转换为可空类型
            x = y ?? 0;  //可空类型转换值类型 需要确保值 不为空 用 合并运算符(??)
        }
    }
}
