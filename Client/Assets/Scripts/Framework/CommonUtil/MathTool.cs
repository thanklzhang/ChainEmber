using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;



//过后抽象出来
public class MathTool
{
    /// <summary>
    /// 计算直线与平面的交点
    /// </summary>
    /// <param name="point">直线上某一点</param>
    /// <param name="direct">直线的方向</param>
    /// <param name="planeNormal">垂直于平面的的向量</param>
    /// <param name="planePoint">平面上的任意一点</param>
    /// <returns></returns>
    private static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
    {
        float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);

        return d * direct.normalized + point;
    }


    public static Vector3 GetVector3Lerp(Vector3 a, Vector3 b,float delta01)
    {
        var vector = b - a;
        var dir = vector.normalized;
        var len = vector.magnitude;
        var currPos = a + len * delta01 * dir;
        return currPos;
    }
}
