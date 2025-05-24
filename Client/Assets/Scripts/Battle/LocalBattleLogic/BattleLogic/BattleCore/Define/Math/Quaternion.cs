using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{


    public class Quaternion
    {
        public static Quaternion identity = new Quaternion(0, 0, 0, 1);
        public float x;
        public float y;
        public float z;
        public float w;



        public Quaternion(float _x, float _y, float _z, float _w)
        {
            float mag = _x * _x + _y * _y + _z * _z + _w * _w;
            x = _x / mag;
            y = _y / mag;
            z = _z / mag;
            w = _w / mag;
        }

        public Quaternion(float yaw, float pitch, float roll)
        {

            SetEulerAngle(yaw, pitch, roll);
        }




        //Cos theta of two quaternion
        public static float Dot(Quaternion lhs, Quaternion rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;
        }

        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            float cos_theta = Dot(a, b);

            // if B is on opposite hemisphere from A, use -B instead
            float sign;
            if (cos_theta < 0.0f)
            {
                cos_theta = -cos_theta;
                sign = -1.0f;
            }
            else sign = 1.0f;

            float c1, c2;


            if (cos_theta > 1.0f - float.Epsilon)
            {
                // if q2 is (within precision limits) the same as q1,
                // just linear interpolate between A and B.

                c2 = t;
                c1 = 1.0f - t;
            }
            else
            {
                //float theta = gFloat::ArcCosTable(cos_theta);
                // faster than table-based :
                //const float theta = myacos(cos_theta);
                float theta = (float)Math.Acos(cos_theta);
                float sin_theta = (float)Math.Sin(theta);
                float t_theta = t * theta;
                float inv_sin_theta = 1.0f / sin_theta;
                c2 = (float)Math.Sin(t_theta) * inv_sin_theta;
                c1 = (float)Math.Sin(theta - t_theta) * inv_sin_theta;
            }

            c2 *= sign; // or c1 *= sign
                        // just affects the overrall sign of the output

            // interpolate
            return new Quaternion(a.x * c1 + b.x * c2, a.y * c1 + b.y * c2, a.z * c1 + b.z * c2, a.w * c1 + b.w * c2);
        }

        public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
        {
            return new Quaternion((1 - t) * a.x + t * b.x,
                (1 - t) * a.y + t * b.y,
                (1 - t) * a.z + t * b.z,
                (1 - t) * a.w + t * b.w);
        }

        public static float Angle(Quaternion lhs, Quaternion rhs)
        {
            float cos_theta = Dot(lhs, rhs);

            // if B is on opposite hemisphere from A, use -B instead
            if (cos_theta < 0.0f)
            {
                cos_theta = -cos_theta;
            }
            float theta = (float)Math.Acos(cos_theta);
            return 2 * MathTool.Rad2Deg * theta;
        }



        public void Set(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public void SetEulerAngle(float yaw, float pitch, float roll)
        {
            float angle;
            float sinRoll, sinPitch, sinYaw, cosRoll, cosPitch, cosYaw;

            angle = yaw * 0.5f;
            sinYaw = (float)Math.Sin(angle);
            cosYaw = (float)Math.Cos(angle);

            angle = pitch * 0.5f;
            sinPitch = (float)Math.Sin(angle);
            cosPitch = (float)Math.Cos(angle);

            angle = roll * 0.5f;
            sinRoll = (float)Math.Sin(angle);
            cosRoll = (float)Math.Cos(angle);

            float _y = cosRoll * sinPitch * cosYaw + sinRoll * cosPitch * sinYaw;
            float _x = cosRoll * cosPitch * sinYaw - sinRoll * sinPitch * cosYaw;
            float _z = sinRoll * cosPitch * cosYaw - cosRoll * sinPitch * sinYaw;
            float _w = cosRoll * cosPitch * cosYaw + sinRoll * sinPitch * sinYaw;

            float mag = _x * _x + _y * _y + _z * _z + _w * _w;
            x = _x / mag;
            y = _y / mag;
            z = _z / mag;
            w = _w / mag;
        }


        public static Quaternion operator +(Quaternion q0, Quaternion q1)
        {

            Quaternion result = new Quaternion(0, 0, 0, 0);

            result.x = q0.x + q1.x;
            result.y = q0.y + q1.y;
            result.z = q0.z + q1.z;
            result.w = q0.w + q1.w;
            //x += q.x;
            //y += q.y;
            //z += q.z;
            //w += q.w;
            return result;
        }

        public static Quaternion operator -(Quaternion q0, Quaternion q1)
        {
            Quaternion result = new Quaternion(0, 0, 0, 0);
            result.x = q0.x - q1.x;
            result.y = q0.y - q1.y;
            result.z = q0.z - q1.z;
            result.w = q0.w - q1.w;

            return result;
        }

        public static Quaternion operator *(Quaternion q0, float s)
        {
            Quaternion result = new Quaternion(0, 0, 0, 0);
            result.x = q0.x * s;
            result.y = q0.y * s;
            result.z = q0.z * s;
            result.w = q0.w * s;

            return result;
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(-x, -y, -z, w);
        }

        public Quaternion Inverse()
        {
            return new Quaternion(-x, -y, -z, w);
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            float w1 = lhs.w;
            float w2 = rhs.w;
            Vector3 v1 = new Vector3(lhs.x, lhs.y, lhs.z);
            Vector3 v2 = new Vector3(rhs.x, rhs.y, rhs.z);
            float w3 = w1 * w2 - Vector3.Dot(v1, v2);
            Vector3 v3 = Vector3.Cross(v1, v2) + w1 * v2 + w2 * v1;
            return new Quaternion(v3.x, v3.y, v3.z, w3);
        }

        public static Vector3 operator *(Quaternion q, Vector3 v)
        {

            /*
                Quaternion tmp(v.x, v.y, v.z, 0); //This will normalise the quaternion. this will case error.
                Quaternion result = q * tmp * q.Conjugate();
                return Vector3(result.x, result.y, result.z);*/

            // Extract the vector part of the quaternion
            Vector3 u = new Vector3(q.x, q.y, q.z);

            // Extract the scalar part of the quaternion
            float s = q.w;

            // Do the math
            return 2.0f * Vector3.Dot(u, v) * u
                + (s * s - Vector3.Dot(u, u)) * v
                + 2.0f * s * Vector3.Cross(u, v);
        }

        private Vector3 EulerAngle()
        {

            float yaw = (float)Math.Atan2(2 * (w * x + z * y), 1 - 2 * (x * x + y * y));
            float pitch = (float)Math.Asin(MathTool.Clamp(2 * (w * y - x * z), -1.0f, 1.0f));
            float roll = (float)Math.Atan2(2 * (w * z + x * y), 1 - 2 * (z * z + y * y));
            //Console.WriteLine($"{w} {x} {y} {z}");
            return new Vector3(MathTool.Rad2Deg * yaw, MathTool.Rad2Deg * pitch, MathTool.Rad2Deg * roll);
            // return new Vector3(yaw, pitch, roll);
        }
        /// <summary>
        /// 目前只是一个平面 XZ 的之后可以扩充
        /// </summary>
        /// <param name="dirA"></param>
        /// <param name="dirB"></param>
        /// <returns></returns>
        public static Quaternion FromToRotation(Vector3 dirA, Vector3 dirB)
        {
            dirA = dirA - Vector3.Project(dirA, Vector3.up);
            dirB = dirB - Vector3.Project(dirB, Vector3.up);

            float angle = Vector3.Angle(dirA, dirB);

            float dir = (Vector3.Dot(Vector3.up, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
            angle *= dir;

            Quaternion q = Quaternion.Euler(0, angle, 0);
            //Console.WriteLine(q.EulerAngle().y);
            return q;
        }

        /// <summary>
        /// 欧拉角 -> 四元数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Quaternion Euler(float x, float y, float z)
        {
            return new Quaternion(MathTool.Deg2Rad * x, MathTool.Deg2Rad * y, MathTool.Deg2Rad * z);
        }

        private Vector3 eulerAngles;
        public Vector3 EulerAngles
        {
            get
            {
                var forward = this * Vector3.forward;

                var angle = EulerAngle();//function
                if (forward.z > 0)
                {
                    angle = new Vector3(angle.x, (360 + angle.y) % 360, angle.y);
                }
                else if (forward.z < 0)
                {
                    angle = new Vector3(angle.x, (180 - angle.y) % 360, angle.y);
                }

                return angle;
            }
            set
            {
                eulerAngles = value;
            }
        }
    }

}