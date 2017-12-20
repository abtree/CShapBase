using System;

/// <summary>
/// 泛型（模板）的协变与抗辩
/// 为了解决泛型 在基类 和 派生类之间的赋值操作
/// 协变（out）限定返回值必须是T类型 为了实现 返回基类赋值给派生类
/// 抗变（in）限定输入值必须为T类型 为了实现 派生类赋值给基类
/// </summary>
namespace TemplateInOut
{
    public class Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public override string ToString()
        {
            return string.Format("Width:{0}, Height:{1}", Width, Height);
        }
    }

    public class Rectangle : Shape
    {

    }
    #region 协变(out)
    //这是一个含泛型的接口
    public interface IIndex<out T>
    {
        T this[int index] { get; }
        int Count { get; }
    }

    //这个类已经不含泛型了
    public class RectangleCollection : IIndex<Rectangle>
    {
        private Rectangle[] data = new Rectangle[3]
        {
            new Rectangle { Height = 2, Width = 5},
            new Rectangle { Height = 3, Width = 7},
            new Rectangle {Height = 4.5, Width = 2.9 }
        };

        private static RectangleCollection coll;

        public static RectangleCollection GetRectangles()
        {
            return coll ?? (coll = new RectangleCollection());
        }

        public Rectangle this[int index]
        {
            get
            {
                if (index < 0 || index > data.Length)
                    throw new ArgumentOutOfRangeException("index");
                return data[index];
            }
        }

        public int Count
        {
            get
            {
                return data.Length;
            }
        }
    }
    #endregion
    #region 抗变（in）
    //有抗变限制 就不能用于返回值
    public interface IDisplay<in T>
    {
        void Show(T item);
    }

    public class ShapeDisplay : IDisplay<Shape>
    {
        public void Show(Shape s)
        {
            Console.WriteLine("{0} Width: {1}, Height: {2}", s.GetType().Name, s.Width, s.Height);
        }
    }
    #endregion
    class TemplateInOut
    {
        #region 协变(out)
        public void OutMethod()
        {
            //IIndex<Rectangle> rectangles = RectangleCollection.GetRectangles();
            //IIndex<Shape> shapes = rectangles;  //因为 限定了返回为Rectangle类型 所以赋值给Shape类型没有问题(简写如下)
            IIndex<Shape> shapes = RectangleCollection.GetRectangles();
            for(int i = 0; i < shapes.Count; ++i)
            {
                Console.WriteLine(shapes[i]);
            }
        }
        #endregion
        #region 抗变（in）
        public void InMethod()
        {
            IDisplay<Shape> shapeDisplay = new ShapeDisplay();
            IDisplay<Rectangle> rectangleDisplay = shapeDisplay;
            rectangleDisplay.Show(new Rectangle { Height = 2, Width = 4 });
        }
        #endregion
    }
}
