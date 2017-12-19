using System;

/************************************************************************/
/* 这里讲述C#的特殊的注释方法                                           */
/* 该方法的注释 在编译时加上 /doc:docs.xml 会自动生成帮助文档（XML）格式 */
/* 详细用法需要参考文档                                                 */
/************************************************************************/

namespace SpecialAnnotation
{
    // summary  提供类型和成员的简短小结
    /// <summary>
    /// SpecialAnnotation.SpecialAnnotation class
    /// just determination how to use this kind of annotation
    /// </summary>
    class SpecialAnnotation
    {
        /// <summary>
        /// This Add method allows us to add two integers
        /// </summary>
        /// <param name="x">the first number to add</param>
        /// <param name="y">the second number to add</param>
        /// <returns>Result to addition (int32)</returns>
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
