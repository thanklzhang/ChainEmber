using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// 编辑器工具：打开持久化数据路径
/// </summary>
public class OpenPersistentDataPathTool
{
    [MenuItem("Tools/Open Persistent Data Folder")]
    public static void OpenPersistentDataFolder()
    {
        string path = Application.persistentDataPath;
        Debug.Log($"Persistent data path: {path}");
        
        // 确保路径存在，如果不存在则创建
        if (!Directory.Exists(path))
        {
            Debug.Log($"Creating persistent data directory: {path}");
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to create directory: {e.Message}");
                // 将路径复制到剪贴板，让用户手动创建
                EditorGUIUtility.systemCopyBuffer = path;
                Debug.Log("Path copied to clipboard. Please create the directory manually.");
                return;
            }
        }

        Debug.Log($"Opening persistent data path: {path}");

        // 将正斜杠转换为反斜杠(Windows路径)
        string winPath = path.Replace("/", "\\");
        
        bool success = false;
        
        // Windows特定方法
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            try
            {
                // 方法1: 使用/select参数，这会打开父文件夹并选中目标文件夹
                System.Diagnostics.Process.Start("explorer.exe", "/select," + winPath);
                success = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Method 1 failed: {e.Message}");
                
                try
                {
                    // 方法2: 直接打开文件夹
                    System.Diagnostics.Process.Start("explorer.exe", winPath);
                    success = true;
                }
                catch (System.Exception e2)
                {
                    Debug.LogWarning($"Method 2 failed: {e2.Message}");
                    
                    try
                    {
                        // 方法3: 使用绝对路径作为参数
                        System.Diagnostics.Process.Start("explorer.exe", "/root," + winPath);
                        success = true;
                    }
                    catch (System.Exception e3)
                    {
                        Debug.LogWarning($"Method 3 failed: {e3.Message}");
                    }
                }
            }
        }
        // MacOS方法
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            try
            {
                System.Diagnostics.Process.Start("open", path);
                success = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Mac open method failed: {e.Message}");
            }
        }
        // Linux方法
        else if (Application.platform == RuntimePlatform.LinuxEditor)
        {
            try
            {
                System.Diagnostics.Process.Start("xdg-open", path);
                success = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Linux open method failed: {e.Message}");
            }
        }

        // 如果所有方法都失败，显示路径并提供一个可点击的按钮
        if (!success)
        {
            EditorGUIUtility.systemCopyBuffer = path;
            Debug.Log("Could not open folder automatically. Path copied to clipboard: " + path);
            
            bool openDialog = EditorUtility.DisplayDialog("无法自动打开文件夹", 
                $"无法自动打开持久化数据文件夹。\n\n路径已复制到剪贴板：\n{path}\n\n需要手动打开Explorer并导航到此路径。", 
                "打开Explorer", "取消");
            
            if (openDialog)
            {
                // 至少打开一个资源管理器窗口
                System.Diagnostics.Process.Start("explorer.exe");
            }
        }
    }
    
    [MenuItem("Tools/Clear Persistent Data (清档)")]
    public static void ClearPersistentDataFolder()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Debug.Log("[清档] 持久化数据目录不存在，无需清理。");
            // 仍然清除PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("[清档] PlayerPrefs 已清空。");
            EditorUtility.DisplayDialog("清档完成", "PlayerPrefs 已清空！", "确定");
            return;
        }
        
        // 弹窗确认
        if (!EditorUtility.DisplayDialog("清档确认", $"确定要清空持久化数据目录并清除PlayerPrefs？\n{path}", "确定清档", "取消"))
        {
            return;
        }
        
        try
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var file in dirInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in dirInfo.GetDirectories())
            {
                dir.Delete(true);
            }
            // 清除PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log($"[清档] 已清空持久化数据目录: {path}");
            Debug.Log("[清档] PlayerPrefs 已清空。");
            EditorUtility.DisplayDialog("清档完成", "持久化数据目录和PlayerPrefs已清空！", "确定");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[清档] 清空持久化数据目录失败: {e.Message}");
            EditorUtility.DisplayDialog("清档失败", $"清空失败：{e.Message}", "确定");
        }
    }
    
    // [MenuItem("Tools/Create Test File in Persistent Data Path")]
    // public static void CreateTestFile()
    // {
    //     string path = Application.persistentDataPath;
    //     string filePath = Path.Combine(path, "test_file.txt");
    //     
    //     try
    //     {
    //         // 确保目录存在
    //         if (!Directory.Exists(path))
    //         {
    //             Directory.CreateDirectory(path);
    //         }
    //         
    //         // 创建测试文件
    //         File.WriteAllText(filePath, "This is a test file created by Unity Editor Tool.\n" + 
    //                          "Time: " + System.DateTime.Now.ToString());
    //         
    //         Debug.Log($"Test file created at: {filePath}");
    //         
    //         bool openFolder = EditorUtility.DisplayDialog("成功", 
    //             $"测试文件已创建：\n{filePath}", 
    //             "打开所在文件夹", "关闭");
    //             
    //         if (openFolder)
    //         {
    //             // 尝试打开文件所在文件夹并选中该文件
    //             if (Application.platform == RuntimePlatform.WindowsEditor)
    //             {
    //                 string winPath = filePath.Replace("/", "\\");
    //                 System.Diagnostics.Process.Start("explorer.exe", "/select," + winPath);
    //             }
    //         }
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogError($"Failed to create test file: {e.Message}");
    //         EditorUtility.DisplayDialog("错误", $"创建测试文件失败：\n{e.Message}", "确定");
    //     }
    // }
    //
    // [MenuItem("Tools/Open Explorer at C Drive")]
    // public static void OpenExplorerAtCDrive()
    // {
    //     // 这是一个简单的测试函数，用于打开C盘根目录
    //     // 如果这个都不工作，那么问题可能在系统设置
    //     try
    //     {
    //         System.Diagnostics.Process.Start("explorer.exe", "C:\\");
    //         Debug.Log("尝试打开C盘根目录");
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogError($"Failed to open C drive: {e.Message}");
    //         EditorUtility.DisplayDialog("错误", $"无法打开C盘：\n{e.Message}", "确定");
    //     }
    // }
}