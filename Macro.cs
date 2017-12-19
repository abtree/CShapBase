//#define 和 #undef 必须定义在所有代码之前
#undef DEBUG
#define DEBUG
using System;

namespace Macro
{
// 禁止 “字段未使用”的警告
#pragma warning disable 169
//#region 用于定义代码折叠
#region Macro class
    class Macro
    {
#line 12 "Macro.cs"   //改变文件行数
#if DEBUG && (DEBUG == true)   //#if ... endif 可以嵌套使用
#elif DEBUG
#warning "this is a warning"
#else
#error "this is an error"
#endif
#line default   //恢复文件行数
    }
#endregion
//恢复 “字段未使用”的警告
#pragma warning restore 169  
}
