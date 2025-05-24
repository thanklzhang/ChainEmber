using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle_Client
{
    //Hp
    public partial class BattleEntity_Client
    {
        internal void SyncStateValue(List<BattleClientMsg_BattleStateValue> values)
        {
            foreach (var item in values)
            {
                var type = (EntityStateValueType)item.type;
                var value = item.value;
                if (type == EntityStateValueType.CurrHealth)
                {
                    this.CurrHealth = value;
                }
                else if(type == EntityStateValueType.StarLevel)
                {
                    this.starLv = (int)value;
                }
                else if(type == EntityStateValueType.StarExp)
                {
                    this.starExp = (int)value;
                }
                //Logx.Log("sync entity curr value : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);

                EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, item.fromEntityGuid);
            }
        }
    }
}