using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using FixedPointy;
using System;
using System.IO;

namespace Config
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        public Dictionary<Type, IList> typeToListConfigDic = new Dictionary<Type, IList>();

        public Dictionary<Type, Dictionary<int, Config.BaseConfig>> typeToDicConfigDic =
            new Dictionary<Type, Dictionary<int, Config.BaseConfig>>();

        internal void Init()
        {
            //HeroInfoStore.Init();
        }

        public IEnumerator LoadAllConfigData()
        {
            yield return ConfigDataLoader.Instance.LoadFromFile((dic) =>
            {
                //加载完成
                typeToListConfigDic = dic;
                typeToDicConfigDic = TypeListToDic(typeToListConfigDic);

                //foreach (var configKV in typeToListConfigDic)
                //{
                //    var iDic = new Dictionary<int, Config.BaseTable>();
                //    foreach (var tableData in configKV.Value)
                //    {
                //        Config.BaseTable convertTableData = tableData as Config.BaseTable;
                //        iDic.Add(convertTableData.Id, convertTableData);
                //    }

                //    typeToDicConfigDic.Add(configKV.Key, iDic);
                //}
            });
        }

        public void LoadAllConfigDataByEditor()
        {
            var dic = ConfigDataLoader.Instance.LoadFromFileByEditor();
            //加载完成
            typeToListConfigDic = dic;

            typeToDicConfigDic = TypeListToDic(typeToListConfigDic);


            //foreach (var configKV in typeToListConfigDic)
            //{
            //    var iDic = new Dictionary<int, Config.BaseTable>();
            //    foreach (var tableData in configKV.Value)
            //    {
            //        Config.BaseTable convertTableData = tableData as Config.BaseTable;
            //        iDic.Add(convertTableData.Id, convertTableData);
            //    }

            //    typeToDicConfigDic.Add(configKV.Key, iDic);
            //}
        }


        //type list 转换为 type dic
        public Dictionary<Type, Dictionary<int, Config.BaseConfig>> TypeListToDic(Dictionary<Type, IList> list)
        {
            var dic = new Dictionary<Type, Dictionary<int, Config.BaseConfig>>();


            foreach (var configKV in list)
            {
                var iDic = new Dictionary<int, Config.BaseConfig>();
                foreach (var tableData in configKV.Value)
                {
                    Config.BaseConfig convertTableData = tableData as Config.BaseConfig;
                    if (!iDic.ContainsKey(convertTableData.Id))
                    {
                        iDic.Add(convertTableData.Id, convertTableData);
                    }
                    else
                    {
                        Logx.LogError($"has duplicate id : " + convertTableData.Id + " type : " + convertTableData.GetType());
                    }
                }

                dic.Add(configKV.Key, iDic);
            }

            return dic;
        }

        public T GetById<T>(int id) where T : Config.BaseConfig
        {
            var type = typeof(T);
            if (typeToDicConfigDic.ContainsKey(type))
            {
                var dataDic = typeToDicConfigDic[type];
                if (dataDic.ContainsKey(id))
                {
                    var data = dataDic[id];
                    return data as T;
                }
                else
                {
                    Logx.LogError("Table", "the id is not found : " + id + " type : " + type);
                }
            }
            else
            {
                Logx.LogError("Table", "the type is not found : " + type);
            }

            return null;
        }


        public List<T> GetList<T>() where T : Config.BaseConfig
        {
            var type = typeof(T);
            if (!typeToDicConfigDic.ContainsKey(type))
            {
                Logx.LogError("Table", "the type is not found : " + type);
                return null;
            }

            var data = (typeToListConfigDic[typeof(T)]);
            return data.Cast<T>().ToList(); //Select(d => (T)d).ToList();
        }

        public Dictionary<int, T> GetDic<T>() where T : Config.BaseConfig
        {
            var type = typeof(T);
            if (typeToDicConfigDic.ContainsKey(type))
            {
                var configDic = typeToDicConfigDic[type];
                return configDic.ToDictionary(kv => { return kv.Key; }, (kv) => { return kv.Value as T; });
            }

            return null;
        }

        public void Clear()
        {
            foreach (var item in typeToListConfigDic)
            {
                item.Value.Clear();
            }

            foreach (var item in typeToDicConfigDic)
            {
                item.Value.Clear();
            }

            typeToListConfigDic.Clear();
            typeToDicConfigDic.Clear();
        }
    }
}