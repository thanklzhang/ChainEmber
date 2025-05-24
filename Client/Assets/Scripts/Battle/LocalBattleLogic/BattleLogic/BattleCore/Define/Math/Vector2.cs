using System;
using System.Collections.Generic;
using System.Text;

namespace Battle
{

    public struct Vector2
    {

       
        public static Vector2 zero
        {
            get
            {
                return new Vector2(0, 0);
            }
        }

        /// <summary>
        /// 向量的长度
        /// </summary>
        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(sqrMagnitude);
            }
        }

        /// <summary>
        /// 向量的的长度的平方
        /// </summary>
        public float sqrMagnitude
        {
            get
            {
                return x * x + y * y;
            }
        }

        /// <summary>
        /// 规范化
        /// </summary>
        public Vector2 normalized
        {
            get
            {
                if (IsZero)
                {
                    return Vector2.zero;
                }
                else
                {
                    Vector2 v = new Vector2();
                    float length = magnitude;
                    v.x = this.x / length;
                    v.y = this.y / length;
                    return v;
                }
            }
        }

        /// <summary>
        /// 是否是零向量
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.x == 0 && this.y == 0;
            }
        }

        #region 操作符重载

        /// <summary>
        /// 两个向量相加
        /// </summary>
        /// <param name="a">向量a</param>
        /// <param name="b">向量b</param>
        /// <returns>两个向量的和</returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        /// <summary>
        /// 两个向量相减
        /// </summary>
        /// <param name="a">向量a</param>
        /// <param name="b">向量b</param>
        /// <returns>两个向量的差</returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        /// <summary>
        /// 向量取反
        /// </summary>
        /// <param name="a">向量a</param>
        /// <returns>向量a的反向量</returns>
        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }

        /// <summary>
        /// 一个数乘以一个向量
        /// </summary>
        /// <param name="d">一个数</param>
        /// <param name="a">一个向量</param>
        /// <returns>返回数与向量的乘积</returns>
        public static Vector2 operator *(float d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        /// <summary>
        /// 一个向量乘以一个数
        /// </summary>
        /// <param name="a">一个向量</param>
        /// <param name="d">一个数</param>
        /// <returns>返回向量与数的乘积</returns>
        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        /// <summary>
        /// 一个数除一个向量，向量a/数b
        /// </summary>
        /// <param name="a">一个向量</param>
        /// <param name="d">一个数</param>
        /// <returns>返回向量a/d</returns>
        public static Vector2 operator /(Vector2 a, float d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        /// <summary>
        /// 两个向量是否相等
        /// </summary>
        /// <param name="lhs">向量lhs</param>
        /// <param name="rhs">向量rhs</param>
        /// <returns>如果相等，则为true，否则为false</returns>
        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            bool x = Math.Abs(lhs.x - rhs.x) < 0.00001f;
            bool y = Math.Abs(lhs.y - rhs.y) < 0.00001f;
            return x && y;
        }

        /// <summary>
        /// 两个向量是否不等
        /// </summary>
        /// <param name="lhs">向量lhs</param>
        /// <param name="rhs">向量rhs</param>
        /// <returns>如果不等，则为true，否则为false</returns>
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        public float x;
        public float y;
        // public float z;

        private float[] data;// = new float[3];

        #region 构造器
        //public Vector2()
        //{
        //    this.x = 0.0f;
        //    this.y = 0.0f;
        //    this.z = 0.0f;

        //    data[0] = 0.0f;
        //    data[1] = 0.0f;
        //    data[2] = 0.0f;
        //}

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
           
            data = new float[2];
            data[0] = 0.0f;
            data[1] = 0.0f;
            
        }

        #endregion

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    default:
                        throw new Exception("index is out of array");
                }
            }
            set
            {
                data[index] = value;
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(this.x);
            sb.Append(",");
            sb.Append(this.y);
            sb.Append(")");
            return sb.ToString();
        }

        public void Set(float new_x, float new_y)
        {
            this.x = new_x;
            this.y = new_y;
            data[0] = new_x;
            data[1] = new_y;
            
        }

        /// <summary>
        /// 规范化，使向量长度为1
        /// </summary>
        public void Normalize()
        {
            if (!IsZero)
            {
                this.x /= magnitude;
                this.y /= magnitude;
            }
        }

        #region 静态函数

        /// <summary>
        /// 距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>返回a和b之间的距离</returns>
        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }

        /// <summary>
        /// sqrt距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>返回a和b之间的距离</returns>
        public static float SqrtDistance(Vector2 a, Vector2 b)
        {
            return (float)(a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
        }

        // /// <summary>
        // /// 计算两个向量的点乘积
        // /// </summary>
        // /// <param name="lhs"></param>
        // /// <param name="rhs"></param>
        // /// <returns></returns>
        // public static float Dot(Vector2 lhs, Vector2 rhs)
        // {
        //     return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        // }

        // /// <summary>
        // /// 计算两个向量的叉乘
        // /// </summary>
        // /// <param name="v1"></param>
        // /// <param name="v2"></param>
        // /// <returns></returns>
        // public static Vector2 Cross(Vector2 v1, Vector2 v2)
        // {
        //     float x = v1.y * v2.z - v2.y * v1.z;
        //     float y = v1.z * v2.x - v2.z * v1.x;
        //     float z = v1.x * v2.y - v2.x * v1.y;
        //     return new Vector2(x, y, z);
        // }

        /// <summary>
        /// 限制长度
        /// </summary>
        /// <param name="v">向量v</param>
        /// <param name="maxLength">最长长度</param>
        /// <returns>返回限制长度后的向量</returns>
        public static Vector2 ClampMagnitude(Vector2 v, float maxLength)
        {
            if (maxLength >= v.magnitude)
            {
                return v;
            }

            return (maxLength / v.magnitude) * v;
        }

        // /// <summary>
        // /// 计算两个向量的夹角
        // /// </summary>
        // /// <param name="from"></param>
        // /// <param name="to"></param>
        // /// <returns></returns>
        // public static float Angle(Vector2 from, Vector2 to)
        // {
        //     float dot = Dot(from, to);
        //     float mXm = dot / (from.magnitude * to.magnitude);
        //     mXm = Math.Min(1, mXm);
        //     var angle = Math.Acos(mXm);
        //
        //     //Console.WriteLine("Vector2 " + dot + " " + from.magnitude * to.magnitude + " " + mXm + angle);
        //     return (float)(180 * angle) / (float)Math.PI;
        // }

        // /// <summary>
        // /// 投射一个向量到另一个向量
        // /// </summary>
        // /// <param name="vector"></param>
        // /// <param name="onNormal"></param>
        // /// <returns>返回被投射到onNormal的vector</returns>
        // public static Vector2 Project(Vector2 vector, Vector2 onNormal)
        // {
        //     if (vector.IsZero || onNormal == Vector2.zero)
        //     {
        //         return Vector2.zero;
        //     }
        //     return Dot(vector, onNormal) / (onNormal.magnitude * onNormal.magnitude) * onNormal;
        // }

        // /// <summary>
        // /// 反射
        // /// </summary>
        // /// <param name="inDirection"></param>
        // /// <param name="inNormal"></param>
        // /// <returns></returns>
        // public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
        // {
        //     return Vector2.zero;
        // }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector2 scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
            
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 Scale(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        // /// <summary>
        // /// 两个向量是否平行
        // /// </summary>
        // /// <param name="a"></param>
        // /// <param name="b"></param>
        // /// <returns></returns>
        // public static bool Parallel(Vector2 a, Vector2 b)
        // {
        //     return Cross(a, b).IsZero;
        // }

        /// <summary>
        /// 两个向量之间的线性插值
        /// </summary> 
        /// <param name="from">向量from</param>
        /// <param name="to">向量to</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 from, Vector2 to, float t)
        {
            if (t <= 0)
            {
                return from;
            }
            else if (t >= 1)
            {
                return to;
            }
            return t * to + (1 - t) * from;
        }

        ///// <summary>
        ///// 两个向量的球形插值
        ///// </summary>
        ///// <param name="a">向量a</param>
        ///// <param name="b">向量b</param>
        ///// <param name="t">t的值在[0..1]</param>
        ///// <returns></returns>
        //public static Vector2 Slerp(Vector2 a, Vector2 b, float t)
        //{
        //    if (t <= 0)
        //    {
        //        return a;
        //    }
        //    else if (t >= 1)
        //    {
        //        return b;
        //    }

        //    Vector2 v = RotateTo(a, b, Vector2.Angle(a, b) * t);

        //    //向量的长度，跟线性插值一样计算
        //    float length = b.magnitude * t + a.magnitude * (1 - t);
        //    return v.normalized * length;
        //}

        ///// <summary>
        ///// 将向量from向向量to旋转角度angle
        ///// </summary>
        ///// <param name="from"></param>
        ///// <param name="to"></param>
        ///// <param name="angle"></param>
        ///// <returns></returns>
        //public static Vector2 RotateTo(Vector2 from, Vector2 to, float angle)
        //{
        //    //如果两向量角度为0
        //    if (Vector2.Angle(from, to) == 0)
        //    {
        //        return from;
        //    }

        //    //旋转轴
        //    Vector2 n = Vector2.Cross(from, to);
        //    n.Normalize();

        //    //旋转矩阵
        //    Matrix4x4 rotateMatrix = new Matrix4x4();

        //    //旋转的弧度
        //    double radian = angle * Math.PI / 180;
        //    float cosAngle = (float)Math.Cos(radian);
        //    float sinAngle = (float)Math.Sin(radian);
        //    rotateMatrix.SetRow(0, new Vector4(n.x * n.x * (1 - cosAngle) + cosAngle, n.x * n.y * (1 - cosAngle) + n.z * sinAngle, n.x * n.z * (1 - cosAngle) - n.y * sinAngle, 0));
        //    rotateMatrix.SetRow(1, new Vector4(n.x * n.y * (1 - cosAngle) - n.z * sinAngle, n.y * n.y * (1 - cosAngle) + cosAngle, n.y * n.z * (1 - cosAngle) + n.x * sinAngle, 0));
        //    rotateMatrix.SetRow(2, new Vector4(n.x * n.z * (1 - cosAngle) + n.y * sinAngle, n.y * n.z * (1 - cosAngle) - n.x * sinAngle, n.z * n.z * (1 - cosAngle) + cosAngle, 0));
        //    rotateMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        //    Vector4 v = Vector2.ToVector4(from);
        //    Vector2 vector = new Vector2();
        //    for (int i = 0; i < 3; ++i)
        //    {
        //        for (int j = 0; j < 3; j++)
        //        {
        //            vector[i] += v[j] * rotateMatrix[j, i];
        //        }
        //    }
        //    return vector;
        //}

        ///// <summary>
        ///// 将一个Vector2D转换为Vector4
        ///// </summary>
        ///// <param name="v"></param>
        ///// <returns></returns>
        //public static Vector4 ToVector4(Vector2 v)
        //{
        //    return new Vector4(v.x, v.y, v.z, 0);
        //}

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        public override bool Equals(object obj)
        {
            var vector = (Vector2)obj;
            return vector != null &&
                   magnitude == vector.magnitude &&
                   sqrMagnitude == vector.sqrMagnitude &&
                   EqualityComparer<Vector2>.Default.Equals(normalized, vector.normalized) &&
                   IsZero == vector.IsZero &&
                   x == vector.x &&
                   y == vector.y &&
                   EqualityComparer<float[]>.Default.Equals(data, vector.data);
        }

        // /// <summary>
        // /// xz 平面上的距离(未开方之前)
        // /// </summary>
        // /// <returns></returns>
        // public float SqrDistanceOnXZ()
        // {
        //     return x * x + z * z;
        // }


        // /// <summary>
        // /// xz 平面上的距离
        // /// </summary>
        // /// <returns></returns>
        // public float DistanceOnXZ()
        // {
        //     return (float)Math.Sqrt(SqrDistanceOnXZ());
        // }


        public Vector3 ToVector3ByXZ()
        {
            Vector3 v = new Vector3();
            v.x = this.x;
            v.y = 0;
            v.z = this.y;

            return v;
        }

        #endregion
    }
}