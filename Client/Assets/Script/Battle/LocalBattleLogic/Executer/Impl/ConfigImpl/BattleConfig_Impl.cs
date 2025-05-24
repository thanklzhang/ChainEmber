using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;


namespace Battle
{
    //战斗配置实现类
    public class BattleConfig_Impl : IBattleConfig
    {
        Dictionary<Type, IList> typeToListConfigDic = new Dictionary<Type, IList>();

        Dictionary<Type, Dictionary<int, IConfig>>
            typeToDicConfigDic = new Dictionary<Type, Dictionary<int, IConfig>>();

        public static string NameSpaceName = "Battle";

        public void Load()
        {
            var configManager = ConfigManager.Instance;
            //ConfigManager.LoadAllTableData(path);
            typeToListConfigDic = new Dictionary<Type, IList>();
            foreach (var item in configManager.typeToListConfigDic)
            {
                var type = item.Key;
                var list = item.Value;

                //战斗中父类
                var typeNameInBattle = NameSpaceName + ".I" + type.Name;

                //子类
                var typeName = NameSpaceName + "." + type.Name;
                var implementTypeName = typeName + "_Impl";


                Type typeInBattle = Type.GetType(typeNameInBattle);
                Type implType = Type.GetType(implementTypeName);

                if (null == typeInBattle)
                {
                    Logx.LogWarning("the type of typeInBattle is not found : " + typeInBattle);
                    continue;
                }

                if (null == implType)
                {
                    Logx.LogWarning("the type of implementTypeName is not found : " + implementTypeName);
                    continue;
                }

                List<IConfig> newList = new List<IConfig>();
                foreach (var data in list)
                {
                    var srcData = data as Config.BaseConfig;
                    var newObj = Activator.CreateInstance(implType) as IConfig;
                    newObj.Init(srcData.Id);
                    newList.Add(newObj);
                }

                typeToDicConfigDic.Add(typeInBattle, newList.ToDictionary((v) => { return v.Id; }));
                typeToListConfigDic.Add(typeInBattle, newList);
            }
        }

        public T GetById<T>(int id) where T : IConfig
        {
            var type = typeof(T);
            if (typeToDicConfigDic.ContainsKey(type))
            {
                var dataDic = typeToDicConfigDic[type];
                if (dataDic.ContainsKey(id))
                {
                    var data = dataDic[id];
                    return (T)data;
                }
                else
                {
                    Logx.LogWarning("BattleConfig : the id is not found : id : " + id + " , type : " + typeof(T));
                }
            }
            else
            {
                Logx.LogWarning("BattleConfig : the type is not found : type : " + typeof(T));
            }

            return default(T);
        }

        public List<T> GetList<T>() where T : IConfig
        {
            var data = (typeToListConfigDic[typeof(T)]);
            return data.Cast<T>().ToList();//Select(d => (T)d).ToList();
        }
        public Dictionary<int, T> GetDic<T>() where T : IConfig
        {
            var type = typeof(T);
            if (typeToDicConfigDic.ContainsKey(type))
            {
                var configDic = typeToDicConfigDic[type];
                return configDic.ToDictionary(kv =>
                {
                    return kv.Key;
                }, (kv) =>
                {
                    return (T)kv.Value;
                });
            }
            return null;
        }
        
        public void Release()
        {
            
        }
    }
}