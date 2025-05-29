// using System.IO;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
//
// namespace ServerSimulation
// {
//     [Serializable]
//     public class SerializableDictionary<TKey, TValue>
//     {
//         [Serializable]
//         public class KeyValuePair
//         {
//             public TKey Key;
//             public TValue Value;
//         }
//
//         public List<KeyValuePair> Items = new List<KeyValuePair>();
//
//         public Dictionary<TKey, TValue> ToDictionary()
//         {
//             Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
//             foreach (var item in Items)
//             {
//                 dict[item.Key] = item.Value;
//             }
//             return dict;
//         }
//
//         public static SerializableDictionary<TKey, TValue> FromDictionary(Dictionary<TKey, TValue> dict)
//         {
//             SerializableDictionary<TKey, TValue> serDict = new SerializableDictionary<TKey, TValue>();
//             foreach (var kvp in dict)
//             {
//                 serDict.Items.Add(new KeyValuePair { Key = kvp.Key, Value = kvp.Value });
//             }
//             return serDict;
//         }
//     }
//
//     public class DataStorage
//     {
//         private string dataPath;
//
//         public void Init()
//         {
//             // 使用Unity的persistentDataPath确保跨平台兼容性
//             dataPath = Path.Combine(Application.persistentDataPath, "ServerSimulation");
//             
//             if (!Directory.Exists(dataPath))
//             {
//                 Directory.CreateDirectory(dataPath);
//             }
//             
//             Debug.Log($"[DataStorage] 数据存储路径: {dataPath}");
//         }
//
//         public void SaveData<T>(string key, T data)
//         {
//             try
//             {
//                 string filePath = Path.Combine(dataPath, $"{key}.json");
//                 string json = JsonUtility.ToJson(data, true);
//                 File.WriteAllText(filePath, json);
//                 Debug.Log($"[DataStorage] 已保存数据: {key}");
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"[DataStorage] 保存数据失败: {key}, 错误: {ex.Message}");
//             }
//         }
//
//         public T LoadData<T>(string key) where T : class
//         {
//             try
//             {
//                 string filePath = Path.Combine(dataPath, $"{key}.json");
//                 
//                 if (!File.Exists(filePath))
//                 {
//                     Debug.Log($"[DataStorage] 数据文件不存在: {key}");
//                     return null;
//                 }
//
//                 string json = File.ReadAllText(filePath);
//                 T data = JsonUtility.FromJson<T>(json);
//                 Debug.Log($"[DataStorage] 已加载数据: {key}");
//                 return data;
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"[DataStorage] 加载数据失败: {key}, 错误: {ex.Message}");
//                 return null;
//             }
//         }
//         
//         public void SaveAllData()
//         {
//             // 在这里可以添加需要一起保存的所有数据
//             Debug.Log("[DataStorage] 所有数据已保存");
//         }
//
//         public void DeleteData(string key)
//         {
//             try
//             {
//                 string filePath = Path.Combine(dataPath, $"{key}.json");
//                 if (File.Exists(filePath))
//                 {
//                     File.Delete(filePath);
//                     Debug.Log($"[DataStorage] 已删除数据: {key}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"[DataStorage] 删除数据失败: {key}, 错误: {ex.Message}");
//             }
//         }
//     }
// } 