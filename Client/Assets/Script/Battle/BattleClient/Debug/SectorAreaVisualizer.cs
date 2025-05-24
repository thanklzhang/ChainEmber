using UnityEngine;
using System.Collections.Generic;
// 不要直接引用Battle命名空间，改为显式指定使用的类型

namespace BattleClient.Debug
{
    /// <summary>
    /// 扇形区域可视化工具
    /// 用于Unity编辑器中可视化显示战斗系统中的扇形区域效果
    /// </summary>
    public class SectorAreaVisualizer : MonoBehaviour
    {
        private static List<SectorVisualData> visualDataList = new List<SectorVisualData>();
        private static bool isEnabled = true;

        public void Init()
        {
            
        }

        /// <summary>
        /// 添加一个扇形区域用于可视化
        /// </summary>
        /// <param name="center">扇形中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="angle">角度</param>
        /// <param name="direction">方向</param>
        /// <param name="color">颜色</param>
        /// <param name="duration">持续时间（秒）</param>
        public static void AddSectorVisual(UnityEngine.Vector3 center, float radius, float angle, UnityEngine.Vector3 direction, UnityEngine.Color color, float duration = 3.0f)
        {
            if (!isEnabled) return;

            Logx.Log("zxy : AddSectorVisual");
            // 添加可视化数据
            visualDataList.Add(new SectorVisualData
            {
                center = center,
                radius = radius,
                angle = angle,
                direction = direction,
                color = color,
                createTime = UnityEngine.Time.time,
                duration = duration
            });
        }
        
        // 在Unity场景视图中绘制扇形
        private void OnDrawGizmos()
        {
            DrawAllSectors();
        }
        
        // 在Unity游戏视图中绘制扇形
        private void Update()
        {
            float currentTime = UnityEngine.Time.time;
            
            // 移除过期的可视化数据
            visualDataList.RemoveAll(data => currentTime - data.createTime > data.duration);
            
            // 绘制所有扇形（使用Debug.DrawLine在游戏视图中显示）
            foreach (var data in visualDataList)
            {
                DrawSectorDebugLines(data);
            }
        }
        
        /// <summary>
        /// Gizmos绘制所有扇形
        /// </summary>
        private static void DrawAllSectors()
        {
            if (!isEnabled) return;
            
            float currentTime = UnityEngine.Time.time;
            
            // 移除过期的可视化数据
            visualDataList.RemoveAll(data => currentTime - data.createTime > data.duration);
            
            // 使用Gizmos绘制所有扇形
            foreach (var data in visualDataList)
            {
                DrawSectorGizmos(data);
            }
        }
        
        /// <summary>
        /// 使用Gizmos绘制单个扇形
        /// </summary>
        private static void DrawSectorGizmos(SectorVisualData data)
        {
            int segments = 20;
            float halfAngle = data.angle * 0.5f;
            
            // 保存当前Gizmos颜色
            UnityEngine.Color originalColor = UnityEngine.Gizmos.color;
            UnityEngine.Gizmos.color = data.color;
            
            // 计算扇形的起始和结束向量
            UnityEngine.Quaternion leftRot = UnityEngine.Quaternion.AngleAxis(-halfAngle, UnityEngine.Vector3.up);
            UnityEngine.Quaternion rightRot = UnityEngine.Quaternion.AngleAxis(halfAngle, UnityEngine.Vector3.up);
            UnityEngine.Vector3 leftDir = leftRot * data.direction;
            UnityEngine.Vector3 rightDir = rightRot * data.direction;
            
            // 绘制扇形边缘
            UnityEngine.Gizmos.DrawLine(data.center, data.center + leftDir * data.radius);
            UnityEngine.Gizmos.DrawLine(data.center, data.center + rightDir * data.radius);
            
            // 绘制扇形弧
            float angleStep = data.angle / segments;
            UnityEngine.Vector3 prevPoint = data.center + leftDir * data.radius;
            
            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -halfAngle + angleStep * i;
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.AngleAxis(currentAngle, UnityEngine.Vector3.up);
                UnityEngine.Vector3 dir = rot * data.direction;
                UnityEngine.Vector3 point = data.center + dir * data.radius;
                
                UnityEngine.Gizmos.DrawLine(prevPoint, point);
                prevPoint = point;
            }
            
            // 恢复Gizmos颜色
            UnityEngine.Gizmos.color = originalColor;
        }
        
        /// <summary>
        /// 使用Debug.DrawLine绘制单个扇形
        /// </summary>
        private static void DrawSectorDebugLines(SectorVisualData data)
        {
            int segments = 20;
            float halfAngle = data.angle * 0.5f;
            
            // 计算扇形的起始和结束向量
            UnityEngine.Quaternion leftRot = UnityEngine.Quaternion.AngleAxis(-halfAngle, UnityEngine.Vector3.up);
            UnityEngine.Quaternion rightRot = UnityEngine.Quaternion.AngleAxis(halfAngle, UnityEngine.Vector3.up);
            UnityEngine.Vector3 leftDir = leftRot * data.direction;
            UnityEngine.Vector3 rightDir = rightRot * data.direction;
            
            // 绘制扇形边缘
            UnityEngine.Debug.DrawLine(data.center, data.center + leftDir * data.radius, data.color);
            UnityEngine.Debug.DrawLine(data.center, data.center + rightDir * data.radius, data.color);
            
            // 绘制扇形弧
            float angleStep = data.angle / segments;
            UnityEngine.Vector3 prevPoint = data.center + leftDir * data.radius;
            
            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -halfAngle + angleStep * i;
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.AngleAxis(currentAngle, UnityEngine.Vector3.up);
                UnityEngine.Vector3 dir = rot * data.direction;
                UnityEngine.Vector3 point = data.center + dir * data.radius;
                
                UnityEngine.Debug.DrawLine(prevPoint, point, data.color);
                prevPoint = point;
            }
        }
        
        /// <summary>
        /// 启用或禁用扇形区域可视化
        /// </summary>
        public static void EnableVisualization(bool enable)
        {
            isEnabled = enable;
            
            if (!enable)
            {
                visualDataList.Clear();
            }
        }
        
        /// <summary>
        /// 提供给战斗系统的接口，用于在战斗中可视化扇形区域
        /// </summary>
        public static void VisualizeBattleSector(UnityEngine.Vector3 center, float radius, float angle, UnityEngine.Vector3 direction)
        {
            AddSectorVisual(center, radius, angle, direction, UnityEngine.Color.red, 1.0f);
        }
        
        /// <summary>
        /// 将Battle命名空间下的Vector3转换为Unity的Vector3
        /// </summary>
        public static UnityEngine.Vector3 ConvertToUnityVector3(Battle.Vector3 battleVector)
        {
            return new UnityEngine.Vector3(battleVector.x, battleVector.y, battleVector.z);
        }

        /// <summary>
        /// 提供给战斗系统的接口，接受Battle命名空间下的Vector3并进行可视化
        /// </summary>
        public static void VisualizeBattleSectorWithBattleVector3(Battle.Vector3 center, float radius, float angle, Battle.Vector3 direction)
        {
            VisualizeBattleSector(
                ConvertToUnityVector3(center),
                radius,
                angle,
                ConvertToUnityVector3(direction)
            );
        }
    }

    /// <summary>
    /// 扇形区域可视化数据
    /// </summary>
    public class SectorVisualData
    {
        public UnityEngine.Vector3 center;
        public float radius;
        public float angle;
        public UnityEngine.Vector3 direction;
        public UnityEngine.Color color;
        public float createTime;
        public float duration;
    }
} 