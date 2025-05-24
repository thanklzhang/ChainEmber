using Battle;

namespace BattleClient.Debug
{
    /// <summary>
    /// 扇形区域可视化桥接类
    /// 用于连接战斗逻辑和Unity可视化工具
    /// </summary>
    public static class SectorAreaVisualizerBridge
    {
        /// <summary>
        /// 战斗中调用的可视化方法，将战斗数据传递给可视化工具
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="angle">角度</param>
        /// <param name="direction">方向</param>
        public static void VisualizeBattleSector(Vector3 center, float radius, float angle, Vector3 direction)
        {
            // 如果在Unity环境中，调用可视化工具
            #if !PURE_LOGIC_SERVER
            SectorAreaVisualizer.VisualizeBattleSectorWithBattleVector3(center, radius, angle, direction);
            #endif
        }
        
        /// <summary>
        /// 启用或禁用扇形区域可视化
        /// </summary>
        public static void EnableVisualization(bool enable)
        {
            #if !PURE_LOGIC_SERVER
            SectorAreaVisualizer.EnableVisualization(enable);
            #endif
        }
    }
} 