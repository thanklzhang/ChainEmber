//using Config;

using FixedPointy;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;
namespace Config
{
    public class ConfigInfo
    {
        public string name;
        public string json;
    }
    public class ConfigDataLoader : Singleton<ConfigDataLoader>
    {
        //-----------------------------以下是用动态泛型读取文件 在 ios 可能会出问题


        //IEnumerator LoadFromAB(Action<List<TableInfo>> finishCallback)
        //{
        //    List<TableInfo> infoList = new List<TableInfo>();

        //    var tablePath = "Table";
        //    var loadPath = Const.AssetBundlePath + "/" + Const.buildPath + "/" + tablePath;

        //    var files = System.IO.Directory.GetFiles(loadPath);
        //    foreach (var filePath in files)
        //    {
        //        bool isLoadFinish = false;
        //        string loadText = "";
        //        var ext = Path.GetExtension(filePath);
        //        if (ext == ".ab")
        //        {
        //            var strIndex = filePath.IndexOf(Const.buildPath + "/" + tablePath);
        //            var resultPath = filePath.Substring(strIndex).Replace("\\", "/").Replace(".ab", ".json").ToLower();
        //            //Logx.Log("start load : resultPath :  " + resultPath);
        //            ResourceManager.Instance.GetObject<TextAsset>(resultPath, (textAsset) =>
        //            {
        //                //Logx.Log("load text finish: " + textAsset.text);
        //                loadText = textAsset.text;
        //                isLoadFinish = true;
        //            });

        //            while (true)
        //            {
        //                yield return null;

        //                if (isLoadFinish)
        //                {
        //                    TableInfo info = new TableInfo();
        //                    info.name = Path.GetFileNameWithoutExtension(filePath);
        //                    info.json = loadText;
        //                    infoList.Add(info);
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    finishCallback?.Invoke(infoList);
        //}

        /// <summary>
        /// 根据泛型来一次性读取 Config 
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadFromFile(Action<Dictionary<Type, IList>> finishCallback)
        {
            List<ConfigInfo> infoList = new List<ConfigInfo>();

            // var files = TablePathDefine.GetTablePathList();
            //string[] files = System.IO.Directory.GetFiles(loadPath, "*.json", SearchOption.AllDirectories);

            var files = new String[] { };
            if (GlobalConfig.isUseAB)
            {
                //TODO
            }
            else
            {
                
                var loadPath = Path.Combine(GlobalConfig.buildPath, GlobalConfig.configPath);
                files = System.IO.Directory.GetFiles(loadPath, "*.json", SearchOption.AllDirectories);
                
                List<string> fileList = new List<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    var name = Path.GetFileName(file);
                    fileList.Add(name);
                }

                files = fileList.ToArray();
            }

          
            
            foreach (var filePath in files)
            {
                bool isLoadFinish = false;
                string loadText = "";

                var loadPath = Path.Combine(GlobalConfig.buildPath, GlobalConfig.configPath, filePath);
                ResourceManager.Instance.GetObject<TextAsset>(loadPath, (textAsset) =>
                {
                    //Logx.Log("table manager : LoadFromFile : load text finish: " + textAsset.text);
                    loadText = textAsset.text;
                    isLoadFinish = true;
                });

                while (true)
                {
                    yield return null;

                    if (isLoadFinish)
                    {
                        break;
                    }
                }
                string jsonStr = loadText;
                ConfigInfo info = new ConfigInfo();
                info.name = Path.GetFileNameWithoutExtension(filePath);
                info.json = jsonStr;
                infoList.Add(info);

            }

            var dic = LoadAllDataByField(infoList);
            finishCallback?.Invoke(dic);
        }

        public Dictionary<Type, IList> LoadFromFileByEditor()
        {
            //非 AB 加载 之后可能改成 AssetDatabase 的加载
            var loadPath = Application.dataPath + "/BuildRes/Table";
            string[] files = System.IO.Directory.GetFiles(loadPath);
            List<ConfigInfo> infoList = new List<ConfigInfo>();
            files.ToList().ForEach(file =>
            {
                string jsonStr = FileOperate.GetTextFromFile(file);

                ConfigInfo info = new ConfigInfo();
                var ext = Path.GetExtension(file);
                if (ext == ".json")
                {
                    info.name = Path.GetFileNameWithoutExtension(file);
                    info.json = jsonStr;
                    infoList.Add(info);
                }

            });

            //return LoadAllData(infoList);
            var dic = LoadAllDataByField(infoList);

            return dic;
        }


        /// <summary>
        /// 根据 json 按字段赋值 反射出数据 为了 使 Fix 配合表
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Dictionary<Type, System.Collections.IList> LoadAllDataByField(List<ConfigInfo> infos)
        {
            Dictionary<Type, System.Collections.IList> configDic = new Dictionary<Type, System.Collections.IList>();

            infos.ForEach(info =>
            {
                string typeName = "Config." + info.name;
                try
                {
                    Type type = Type.GetType(typeName, true, true);
                    Type listType = typeof(List<>).MakeGenericType(type);


                    //建造每一个对象
                    object list = Activator.CreateInstance(listType);
                    List<JsonData> jdList = JsonMapper.ToObject<List<JsonData>>(info.json);//{}

                    jdList.ForEach(jd =>
                    {

                        //定义 对象
                        object currObject = Activator.CreateInstance(type);

                        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                        fields.ToList().ForEach(f =>
                        {
                            var jsonValue = jd[f.Name];

                            //object value = jsonValue;
                            //if (null == jsonValue)
                            //{
                            //    return;
                            //}
                            
                            //var generType = f.FieldType.GetGenericArguments();//试着获取List<> 的范型类型
                            
                            if (f.FieldType == typeof(int))
                            {
                                f.SetValue(currObject, (int)jsonValue);
                            }
                            else if (f.FieldType == typeof(float))//f.FieldType.Name.Contains("Fix") && 
                            {
                                f.SetValue(currObject, jsonValue);
                            }
                            else if (f.FieldType == typeof(string))
                            {
                                f.SetValue(currObject, jsonValue.ToString());
                            }
                            else if (f.FieldType == typeof(bool))
                            {
                                f.SetValue(currObject, GetBoolFromStr(jsonValue.ToString()));
                            }
                            else if (f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                //json 数组 => List
                                var generType = f.FieldType.GetGenericArguments()[0];//获取List<> 的范型类型
                                Type listType = typeof(List<>).MakeGenericType(generType); // 创建对应类型的List  
                                IList listInstance = (IList)Activator.CreateInstance(listType); // 创建List的实例  

                                if (jsonValue.IsArray)
                                {
                                    for (int i = 0; i < jsonValue.Count; i++)
                                    {
                                        var v = jsonValue[i];

                                        var genericArgs = generType.GetGenericArguments();
                                         if (genericArgs != null && genericArgs.Length > 0) 
                                        {
                                            //List<List<>> 二维
                                            var innerType = genericArgs[0];
                                            Type innerListType = typeof(List<>).MakeGenericType(innerType); // 创建对应类型的List  
                                            IList innerListInstance = (IList)Activator.CreateInstance(innerListType); // 创建List的实例  
                                            for (int j = 0; j < v.Count; j++)
                                            {
                                                var innerV = v[j];
                                            
                                                object convertItem = Convert.ChangeType(innerV.ToString(), innerType);
                                                innerListInstance.Add(convertItem);
                                            }

                                            listInstance.Add(innerListInstance);
                                        }
                                        else
                                        {
                                            //List<> 一维
                                            object convertItem = Convert.ChangeType(v.ToString(), generType);
                                            listInstance.Add(convertItem);
                                        }
                                    }
                                }

                               
                                
                                f.SetValue(currObject, listInstance);
                                
                            }
                        });

                        var addMethod = listType.GetMethod("Add");
                        addMethod.Invoke(list, new object[] { currObject });
                    });

                    configDic.Add(type, list as IList);
                }
                catch (TypeLoadException e)
                {
                    Logx.LogError("the type is not found : " + typeName);
                }
                finally
                {

                }


            });

            return configDic;
        }

        bool GetBoolFromStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            if (str.ToLower().Equals("true") || str.Equals("1"))
            {
                return true;
            }

            return false;
        }



    }
}