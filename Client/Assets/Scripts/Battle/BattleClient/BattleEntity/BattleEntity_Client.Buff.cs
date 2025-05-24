using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle_Client
{
    //储存 buff 缓存数据 ， 方便查找 
    public partial class BattleEntity_Client
    {
        private List<BuffEffectInfo_Client> buffList;

        public void InitBuffs()
        {
            buffList = new List<BuffEffectInfo_Client>();
        }

        public void AddBuff(BuffEffectInfo_Client buffInfo)
        {
            if (!buffList.Exists(b=>b.guid == buffInfo.guid))
            {
                buffList.Add(buffInfo);
            }
            else
            {
                Logx.LogWarning(LogxType.Zxy, "AddBuff : the guid is exist : guid : " + buffInfo.guid);
            }
        }

        public void RemoveBuff(int guid)
        {
            var buff = buffList.Find(b=>b.guid == guid);
            if (buff != null)
            {
                buffList.Remove(buff);
            }
            else
            {
                Logx.LogWarning(LogxType.Zxy, "AddBuff : the guid is not exist : guid : " + guid);

            }
        }

        public List<BuffEffectInfo_Client> GetBuffList()
        {
            return this.buffList;
        }


    }
}