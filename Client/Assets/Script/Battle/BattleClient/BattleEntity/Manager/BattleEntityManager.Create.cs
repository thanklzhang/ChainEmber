using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleEntityManager
    {
        
        internal BattleEntity_Client CreateEntity(BattleClientMsg_Entity msg_entity)
        {
            var entity = CreateViewEntityInfo(msg_entity);
            entity.StartSelfLoadModel();

            EventDispatcher.Broadcast<BattleEntity_Client>(EventIDs.OnCreateEntity, entity);

            return entity;
        }


        internal BattleEntity_Client CreateViewEntityInfo(BattleClientMsg_Entity msgEntity)
        {
            var guid = msgEntity.guid;
            var configId = msgEntity.configId;

            if (entityDic.ContainsKey(guid))
            {
                Logx.LogWarning("BattleEntityManager : OnCreateEntity : the guid is exist : " + guid);
                return null;
            }

            BattleEntity_Client entity = new BattleEntity_Client();
            entity.Init(guid, configId);
            entity.SetPlayerIndex(msgEntity.playerIndex);
            entity.SetPosition(msgEntity.position);
            entity.SetLocationType(msgEntity.localType);
            entity.SetLevel(msgEntity.level);

            //填充技能
            List<BattleSkillInfo> skills = new List<BattleSkillInfo>();
            msgEntity.skills = null == msgEntity.skills ? new List<BattleClientMsg_Skill>() : msgEntity.skills;
            foreach (var serverSkill in msgEntity.skills)
            {
                BattleSkillInfo skill = new BattleSkillInfo()
                {
                    configId = serverSkill.configId,
                    // level = serverSkill.level,
                    maxCDTime = serverSkill.maxCDTime,
                    releaserGuid = guid
                };
                skills.Add(skill);
            }

            entity.SetSkillList(skills);

            // //填充初始道具
            // List<BattleItemInfo> itemList = new List<BattleItemInfo>();
            // msgEntity.itemList = null == msgEntity.itemList ? new List<BattleClientMsg_Item>() : msgEntity.itemList;
            // foreach (var serverItem in msgEntity.itemList)
            // {
            //     BattleItemInfo item = new BattleItemInfo()
            //     {
            //         configId = serverItem.configId,
            //         count = serverItem.count,
            //     };
            //     itemList.Add(item);
            // }
            // entity.SetItemList(itemList);
            // Logx.Log(LogxType.BattleItem,"BattleEntityManager : init item list : count : " + itemList.Count);

            //
            entityDic.Add(guid, entity);

            //Debug.Log("zxy : hh : add entity guid : " + guid );
            //EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);
            //entity.StartLoadModel(loadFinishCallback);
            return entity;
        }


        /// <summary>
        /// 通知创建当前所有已存在的实体 一般战斗真正开始的时候会调用（所有玩家加载完毕）
        /// </summary>
        public void NotifyCreateAllEntities()
        {
            foreach (var item in this.entityDic)
            {
                var entity = item.Value;
                EventDispatcher.Broadcast<BattleEntity_Client>(EventIDs.OnCreateEntity, entity);
            }
        }
    }
}