using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class CollisionTool
    {
        //检测碰撞
        public static bool CheckCollision(Shape a, Shape b)
        {
            Shape t_a = a;
            Shape t_b = b;

            //这里可以用循环判断类型来进行代码整洁优化

            //判断矩形和圆形的碰撞
            if (b is Rect && a is Circle)
            {
                t_a = b;
                t_b = a;
            }

            if (t_a is Rect && t_b is Circle)
            {
                return CheckBoxAndCircle((Rect)t_a, (Circle)t_b);
            }

            return false;
        }


        // 圆与矩形碰撞检测
        // 圆心(x, y), 半径r, 矩形中心(x0, y0), 矩形上边中心(x1, y1), 矩形右边中心(x2, y2)
        public static bool IsCircleIntersectRectangle(float x, float y, float r, float x0, float y0, float x1, float y1,
            float x2,
            float y2)
        {
            float w1 = MathTool.DistanceBetweenTwoPoints(x0, y0, x2, y2);
            float h1 = MathTool.DistanceBetweenTwoPoints(x0, y0, x1, y1);
            float w2 = MathTool.DistanceFromPointToLine(x, y, x0, y0, x1, y1);
            float h2 = MathTool.DistanceFromPointToLine(x, y, x0, y0, x2, y2);

            if (w2 > w1 + r)
                return false;
            if (h2 > h1 + r)
                return false;

            if (w2 <= w1)
                return true;
            if (h2 <= h1)
                return true;

            return (w2 - w1) * (w2 - w1) + (h2 - h1) * (h2 - h1) <= r * r;
        }


        public static bool CheckBoxAndCircle(Rect rect, Circle circle)
        {
            var c_x = circle.center.x;
            var c_y = circle.center.y;
            var c_r = circle.radius;

            var r_center_x = rect.center.x;
            var r_center_y = rect.center.y;


            var upPos = rect.center + rect.heightDir.normalized * rect.height / 2.0f;
            var r_up_center_x = upPos.x;
            var r_up_center_y = upPos.y;

            var rightPos = rect.center + rect.widthDir.normalized * rect.width / 2.0f;
            var r_right_center_x = rightPos.x;
            var r_right_center_y = rightPos.y;

            return IsCircleIntersectRectangle(c_x, c_y, c_r, r_center_x, r_center_y, r_up_center_x, r_up_center_y,
                r_right_center_x, r_right_center_y);


            return false;
        }
    }

    public class Shape
    {
        public virtual bool CheckCollision(Shape other)
        {
            return CollisionTool.CheckCollision(this, other);
        }
    }

    public class Rect : Shape
    {
        public Vector2 widthDir;
        public Vector2 heightDir;

        public Vector2 center;

        public float width;
        public float height;
    }

    public class Circle : Shape
    {
        //圆心
        // public float x;
        // public float y;
        public Vector2 center;

        public float radius;
    }
}