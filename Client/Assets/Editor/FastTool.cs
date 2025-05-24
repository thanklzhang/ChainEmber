using UnityEditor;  
using UnityEditor.SceneManagement;  
using UnityEngine;  
 
public static class CustomSceneSwitcher  
{  
    [MenuItem("Custom/Switch to Start Scene _F3", false, 100)] 
    static void SwitchToStartScene()  
    {  
        EditorSceneManager.OpenScene("Assets/Scenes/Startup.unity"); // 替换为你的场景路径  
    }  
}