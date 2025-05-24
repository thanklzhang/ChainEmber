using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Clipper2Lib;

public class PolygonIntersectionWindow : EditorWindow
{
    private PolygonCollider2D colliderA;
    private PolygonCollider2D colliderB;
    private GameObject goA;
    private GameObject goB;
    private float intersectionArea;
    private float ratioA, ratioB;

    [MenuItem("Tools/Polygon Intersection Calculator (Clipper2)")]
    public static void ShowWindow()
    {
        GetWindow<PolygonIntersectionWindow>("Polygon Intersection (Clipper2)");
    }
    public static class SelectionHelper
    {
        // 判断是否为资源中的预设
        public static bool IsPrefabInProject()
        {
            var obj = Selection.activeObject;
            if (obj == null) return false;

            // 是否为持久化资源（如Assets目录中的文件）
            if (EditorUtility.IsPersistent(obj))
            {
                // 检查是否为GameObject类型的预设
                if (obj is GameObject)
                {
                    return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.NotAPrefab;
                }
            }
            return false;
        }

        // 判断是否为场景中的节点
        public static bool IsObjectInScene()
        {
            var obj = Selection.activeObject;
            if (obj == null) return false;

            if (obj is GameObject go)
            {
                // 非持久化资源，且场景有效
                return !EditorUtility.IsPersistent(go) && go.scene.IsValid();
            }
            return false;
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Polygon Intersection Calculator (Clipper2)", EditorStyles.boldLabel);

        // 拖入两个 PolygonCollider2D
        // colliderA = (PolygonCollider2D)EditorGUILayout.ObjectField("Collider A", colliderA, typeof(PolygonCollider2D), true);
        // colliderB = (PolygonCollider2D)EditorGUILayout.ObjectField("Collider B", colliderB, typeof(PolygonCollider2D), true);

       
        
        
        goA = (GameObject)EditorGUILayout.ObjectField("Collider A", goA, typeof(GameObject), true);
        goB = (GameObject)EditorGUILayout.ObjectField("Collider B", goB, typeof(GameObject), true);

        // 计算按钮
        if (GUILayout.Button("Calculate Intersection"))
        {
            // if (colliderA != null && colliderB != null)
            // {
            //     CalculateIntersection();
            // }
            // else
            // {
            //     Debug.LogError("Please assign both colliders!");
            // }
            var go = Selection.activeObject;
            Debug.Log("IsObjectInScene : " + SelectionHelper.IsObjectInScene());
            Debug.Log("IsPrefabInProject : " + SelectionHelper.IsPrefabInProject());

            if (SelectionHelper.IsPrefabInProject())
            {
                var newInstance = (GameObject)PrefabUtility.InstantiatePrefab(go);
            }
            else
            {
                Instantiate(go);
            }

            
            CalculateIntersection();
        }

        // 显示结果
        GUILayout.Space(10);
        GUILayout.Label($"Intersection Area: {intersectionArea}");
        GUILayout.Label($"Ratio A: {ratioA:P2}");
        GUILayout.Label($"Ratio B: {ratioB:P2}");
    }

    private void CalculateIntersection()
    {
        // 获取两个多边形的顶点（世界坐标）

        colliderA = goA.GetComponent<PolygonCollider2D>();
        if (null == colliderA)
        {
            colliderA = goA.AddComponent<PolygonCollider2D>();
        }

        colliderB = goB.GetComponent<PolygonCollider2D>();
        if (null == colliderB)
        {
            colliderB = goB.AddComponent<PolygonCollider2D>();
        }
        
        List<Vector2> polygonA = GetAllWorldPoints(colliderA);
        List<Vector2> polygonB = GetAllWorldPoints(colliderB);

        // 将顶点转换为 Clipper2 的 Path64 格式（带缩放）
        Path64 pathA = ConvertToPath64(polygonA);
        Path64 pathB = ConvertToPath64(polygonB);

        // 计算相交区域
        Paths64 solution = Clipper.Intersect(new Paths64 { pathA }, new Paths64 { pathB }, FillRule.NonZero);

        // 计算面积
        double areaA = Clipper.Area(pathA); // Clipper.Area 返回 double 类型
        double areaB = Clipper.Area(pathB); // Clipper.Area 返回 double 类型
        double intersectionArea = solution.Count > 0 ? Clipper.Area(solution[0]) : 0; // Clipper.Area 返回 double 类型

        // 将面积转换为 float 类型
        this.intersectionArea = (float)(intersectionArea / (1000.0 * 1000.0)); // 缩放因子平方

        // 计算占比
        ratioA = (float)(intersectionArea / areaA);
        ratioB = (float)(intersectionArea / areaB);
    }

    // 获取 PolygonCollider2D 的所有路径的世界坐标顶点
    private List<Vector2> GetAllWorldPoints(PolygonCollider2D collider)
    {
        List<Vector2> worldPoints = new List<Vector2>();
        for (int i = 0; i < collider.pathCount; i++)
        {
            Vector2[] points = collider.GetPath(i);
            foreach (Vector2 point in points)
            {
                worldPoints.Add(collider.transform.TransformPoint(point));
            }
        }
        return worldPoints;
    }

    // 将 Vector2 列表转换为 Clipper2 的 Path64 格式（带缩放）
    private Path64 ConvertToPath64(List<Vector2> polygon)
    {
        const double scale = 1000.0; // 缩放因子，避免精度丢失
        Path64 path = new Path64();
        foreach (Vector2 point in polygon)
        {
            // 将浮点数坐标放大后转换为 long
            long x = (long)(point.x * scale);
            long y = (long)(point.y * scale);
            path.Add(new Point64(x, y));
        }
        return path;
    }
}