using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Battle
{
    public interface IBattleConfig
    {
        void Load();
        void Release();
        
        T GetById<T>(int id) where T : IConfig;
        List<T> GetList<T>() where T : IConfig;
        Dictionary<int, T> GetDic<T>() where T : IConfig;
    }


}