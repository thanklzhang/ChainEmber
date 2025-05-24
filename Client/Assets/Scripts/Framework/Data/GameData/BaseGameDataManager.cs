using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameData
{
    public class BaseGameDataManager
    {
        public Dictionary<Type, BaseGameData> gameDataDic = new Dictionary<Type, BaseGameData>();

        internal virtual void Init()
        {
            
        }

        protected T AddGameData<T>() where T : BaseGameData, new()
        {
            T t = new T();
            t.Init();

            gameDataDic.Add(typeof(T), t);

            return t;
        }
    }
}