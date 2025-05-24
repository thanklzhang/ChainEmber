using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle_Client
{
    //实体身上的属性值和状态值等
    public partial class BattleEntity_Client
    {
        //attr
        public BattleEntityAttr attr = new BattleEntityAttr();

        public void InitAttr()
        {
            attr.Init();
        }

        internal void SyncAttr(List<BattleClientMsg_BattleAttr> attrs)
        {
            foreach (var item in attrs)
            {
                var type = (EntityAttrType)item.type;
                this.attr.SetAttr(type,item.value);
                
                if (type == EntityAttrType.MoveSpeed)
                {
                    //ani
                    var aniScale = this.attr.GetValue(EntityAttrType.MoveSpeed) / normalAnimationMoveSpeed;
                    SetAnimationSpeed(aniScale);
                }
                //Logx.Log("sync entity attr : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);
            }
            
            EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, 0);
        }

    }
}