using UnityEngine;
using UnityEditor;

public static class CopyPath
{
    // 为资源文件添加右键菜单项
    [MenuItem("Assets/Copy Node Path")]
    private static void CopyAssetPath()
    {
        // 获取当前选中的资源路径
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path))
        {
            // 复制到剪贴板
            EditorGUIUtility.systemCopyBuffer = path;
            // Debug.Log("Copied Asset Path: " + path);
        }
    }

    // 为场景中的节点添加右键菜单项
    [MenuItem("GameObject/Copy Node Path", true)]
    private static bool ValidateCopyNodePath()
    {
        // 检查是否有选中的GameObject
        return Selection.activeGameObject != null;
    }

    [MenuItem("GameObject/Copy Node Path")]
    private static void CopyGameObjectPath()
    {
        // 获取当前选中的GameObject
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            // 获取GameObject的完整路径
            string path = GetFullPath(selectedObject.transform);
            // 复制到剪贴板
            EditorGUIUtility.systemCopyBuffer = path;
            // Debug.Log("Copied GameObject Path: " + path);
        }
    }

    // 递归获取GameObject的完整路径
    private static string GetFullPath(Transform t)
    {
        if (t.parent == null)
            return t.name;
        return GetFullPath(t.parent) + "/" + t.name;
    }
}