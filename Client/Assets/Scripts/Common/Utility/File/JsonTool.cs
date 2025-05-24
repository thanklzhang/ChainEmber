using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//简单的文件操作类
public class JsonTool
{
    public static void SaveObject<T>(string path, T obj)
    {
        var jsonStr = LitJson.JsonMapper.ToJson(obj);
        SaveJson(path, jsonStr);
    }


    public static string LoadJsonFromFile(string path)
    {
        var json = FileOperate.GetTextFromFile(path);
        return json;
    }

    public static T LoadObjectFromFile<T>(string path)
    {
        var json = FileOperate.GetTextFromFile(path);
        return LitJson.JsonMapper.ToObject<T>(json);
    }

    public static void SaveJson(string path, string json)
    {
        FileOperate.SaveToFile(path, json);
    }
}

