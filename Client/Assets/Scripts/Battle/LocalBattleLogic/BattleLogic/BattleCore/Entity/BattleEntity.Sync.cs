using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Battle
{
    //同步消息
    public partial class BattleEntity
    {
        //同步实体数据信息
        //如果实体在战斗开始前就已经有了 那么就会调用这个函数
        public void SyncBattleData()
        {
            this.SyncDir();
            this.SyncAllAttr();
            this.SyncStateValue();
            this.SyncSkillInfo();
            this.SyncItemInfo();
            // this.SyncBoxInfo();
        }
        
        //同步单一属性
        public void SyncAttr(EntityAttrType type)
        {
            var value = this.attrMgr.GetFinalAttrValue(type);
            Dictionary<EntityAttrType, float> dic = new Dictionary<EntityAttrType, float>();
            dic.Add(type, value);

            battle.OnChangeSyncEntityAttr(this.guid, dic);
        }

        //同步全属性
        public void SyncAllAttr()
        {
            var attrs = this.attrMgr.GetFinalAttrs();
            battle.OnChangeSyncEntityAttr(this.guid, attrs);
        }

        //同步当前实体当前战斗数据

        //同步当前生命
        public void SyncCurrHealth(int fromEntityGuid = 0)
        {
            var hp = this.CurrHealth;
            battle.OnChangeEntityCurrHealth(this.guid, (int)hp, fromEntityGuid);
        }

        //更新状态值 如星级等
        public void SyncStateData()
        {
            EntityStateDataBean stateData = new EntityStateDataBean();
            stateData.starLv = this.starLevel;
            stateData.starExp = this.currStarExp;
            
            battle.OnChangeEntityStateData(this.guid, stateData);
            
        }

        public void NotifySkillTrackEnd(SkillTrackEndTimeType type)
        {
            foreach (var kv in this.skillDic)
            {
                var skill = kv.Value;
                skill.NotifySkillTrackEnd(type);
            }
        }

        //同步状态值
        void SyncStateValue()
        {
            SyncCurrHealth();
            SyncStateData();
        }

        void SyncSkillInfo()
        {
            foreach (var item in this.skillDic)
            {
                var skill = item.Value;
                this.battle.OnSkillInfoUpdate(skill);
            }
        }
        
        //同步道具栏信息
        void SyncItemInfo()
        {
            //Logx.Log(LogxType.BattleItem,"entity OnSyncItemInfo : init item list : count : " + itemList.Count);
            this.battle.OnEntityItemsUpdate(this.guid,this.itemBar.itemBarCellList);
        }
        
       

    }
}