using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MapDataGenerateTool))]
public class MapDataGenerateToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("生成地图文件"))
        {
            GenMapInfo();
        }

        if (GUILayout.Button("清理地图碰撞文件"))
        {
            var tool = (MapDataGenerateTool)this.target;
            tool.ClearMap();
        }

        base.OnInspectorGUI();
    }

    void GenMapInfo()
    {
        var tool = (MapDataGenerateTool)this.target;
        tool.GenMapInfo();
    }
}


