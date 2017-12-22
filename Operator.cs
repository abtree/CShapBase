using System;

namespace Operator
{
    //类class 和 结构struct 是一样的
    //不能重载赋值运算符(=)  重载了+ 也就重载了+=
    public struct Vector
    {
        public double x, y, z;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(Vector rhs)
        {
            x = rhs.x;
            y = rhs.y;
            z = rhs.z;
        }

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }

        //所有运算符重载都必须声明为public static
        public static Vector operator+(Vector lhs, Vector rhs)
        {
            Vector result = new Vector(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            result.z += rhs.z;

            return result;
        }

        //这里定义两个
        public static Vector operator*(double d, Vector rhs)
        {
            return new Vector(rhs.x * d, rhs.y * d, rhs.z * d);
        }

        public static Vector operator*(Vector rhs, double d)
        {
            //调用上一个的重载操作
            return d * rhs;
        }

        public static double operator*(Vector lhs, Vector rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //C# 要求 比较运算符必须成对重载 必须返回 bool 
        // ( == and != )   ( > && < ) ( >= && <= )
        public static bool operator== (Vector lhs, Vector rhs)
        {
            return (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z);
        }
        public static bool operator!=(Vector lhs, Vector rhs)
        {
            if (lhs == rhs)
                return false;
            return true; 
        }

        //强制类型转换(implicit 表示隐式转换 explicit 表示显示转换)
        public static implicit operator Vector(double d)
        {
            //在隐式或显示的强制类型转换中 为了保留数据的精度 和对数据溢出的检查 最好在需要时加上 checked运算服
            checked
            {
                return new Vector(d, d, d);
            }
        }

        public static explicit operator double(Vector rhs)
        {
            return rhs.x;
        }
    }
    class Operator
    {
    }
}
