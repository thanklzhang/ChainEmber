using System;
using System.Collections.Generic;
using System.Linq;


namespace Battle
{
   
    //战斗专用配置管理
    public class BattleConfigManager
    {
        private IBattleConfig battleConfig;

        public bool IsInitFinish = false;

        public void LoadBattleConfig(IBattleConfig battleConfig)
        {
            this.battleConfig = battleConfig;
            this.battleConfig.Load();
            IsInitFinish = true;
        }


        public T GetById<T>(int id) where T : IConfig
        {
            return battleConfig.GetById<T>(id);
        }
        
        public List<T> GetList<T>() where T : IConfig
        {
            return battleConfig.GetList<T>();
        }
        
        public Dictionary<int, T> GetDic<T>() where T : IConfig
        {
            return battleConfig.GetDic<T>();
        }
        

        private static BattleConfigManager instance;
        public static BattleConfigManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new BattleConfigManager();
                }

                return instance;
            }
        }
    }
}