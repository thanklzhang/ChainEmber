using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace Battle
{
    //战斗相关事件点
    public partial class Battle
    {
        //有新实体创建了
        internal void OnCreateEntities(List<BattleEntity> entities)
        {
            this.PlayerMsgSender.NotifyAll_CreateEntities(entities);
        }

        internal void OnEntityBeHurt(BattleEntity beHurtEntity, int resultDamage, Skill damageSrcSkill)
        {
            var attacker = damageSrcSkill.releaser;
            beHurtEntity.SyncCurrHealth(attacker.guid);

            if (resultDamage > 0)
            {
                attacker.OnHurtToOther(new EntityTriggerEventArg()
                {
                    damage = resultDamage,
                    damageSrcSkill = damageSrcSkill,
                    oppositeTriggerEntity = beHurtEntity,
                    triggerType = EffectTriggerTimeType.OnHurtToOther
                });
                if (damageSrcSkill.isNormalAttack)
                {
                    attacker.OnNormalAttackToOtherSuccess(beHurtEntity, resultDamage, damageSrcSkill);
                    beHurtEntity.OnBeNormalAttackFromOtherSuccess(attacker, resultDamage, damageSrcSkill);
                }
                else
                {
                    attacker.OnSkillToOtehrSuccess();
                }


                beHurtEntity.ai.OnBeHurt(resultDamage, damageSrcSkill);
                //this.aiMgr.OnEntityBeHurt(beHurtEntity, resultDamage, damageSrcSkill);
            }
        }

        public void OnEntityStartMoveByOnePos_Skill(int guid, Vector3 pos, float finalMoveSpeed)
        {
            var posList = new List<Vector3>();
            posList.Add(pos);
            this.PlayerMsgSender.NotifyAll_EntityStartMoveByPath(guid, posList, finalMoveSpeed, true);
        }

        public void OnEntityStartMoveByPath(int guid, List<Vector3> posList, float finalMoveSpeed, bool isSkillForce)
        {
            this.PlayerMsgSender.NotifyAll_EntityStartMoveByPath(guid, posList, finalMoveSpeed, isSkillForce);
        }

        //有实体停止了移动
        internal void OnEntityStopMove(int guid, Vector3 position)
        {
            //aiMgr.OnEntityStopMove(guid, position);
            this.PlayerMsgSender.NotifyAll_EntityStopMove(guid, position);
        }

        //当实体符合放技能条件的时候(但是前摇还没开始)
        public void OnEntityReleaseSkill(int entityGuid, Skill skill, int targetGuid, Vector3 targetPos)
        {
            var skillConfigId = skill.configId;
            //aiMgr.OnEntityStartSkillEffect(entityGuid, skill, targetGuid, targetPos);

            this.PlayerMsgSender.NotifyAll_OnEntityReleaseSkill(entityGuid, skillConfigId);
        }

        //实体到达了当前目标点
        public void OnMoveToCurrTargetPosFinish(int entityGuid)
        {
            // //TODO 可以将 ctrl 加一个管理 key是 entityGuid value 是 entityCtrl
            // var player = FindPlayerByEntityGuid(entityGuid);
            // if (player != null)
            // {
            //     player.EntityCtrl.OnMoveToCurrTargetPosFinish();
            // }
            // else
            // {
            //     this.aiMgr.OnMoveToCurrTargetPosFinish(entityGuid);
            // }
        }


        public void OnEntityFinishSkillEffect(int entityGuid, Skill skill)
        {
            // //TODO 可以将 ctrl 加一个管理 key是 entityGuid value 是 entityCtrl
            // var player = FindPlayerByEntityGuid(entityGuid);
            // if (player != null)
            // {
            //     player.EntityCtrl.OnFinishSkillEffect(skill);
            // }
            // else
            // {
            //     this.aiMgr.OnEntityFinishSkillEffect(entityGuid,skill);
            // }
        }

        //有技能效果被创建了
        public void OnCreateSkillEffect(CreateEffectInfo createEffectInfo)
        {
            this.PlayerMsgSender.NotifyAll_CreateSkillEffect(createEffectInfo);
        }

        //更新 buff 信息
        public void OnUpdateBuffInfo(BuffEffectInfo buffInfo)
        {
            this.PlayerMsgSender.NotifyAll_NotifyUpdateBuffInfo(buffInfo);
        }

        //有技能效果开始移动
        public void OnSkillEffectStartMove(EffectMoveArg effectMoveArg)
        {
            this.PlayerMsgSender.NotifyAll_SkillEffectStartMove(effectMoveArg);
        }

        //有技能效果销毁了
        public void OnSkillEffectDestroy(int guid)
        {
            this.PlayerMsgSender.NotifyAll_SkillEffectDestroy(guid);
        }

        //有实体改变的属性需要同步
        internal void
            OnChangeSyncEntityAttr(int guid, Dictionary<EntityAttrType, float> dic) // EntityAttrGroup finalAttrGroup
        {
            this.PlayerMsgSender.NotifyAll_SyncEntityAttr(guid, dic);
        }

        internal void OnEntityAddBuff(int guid, BuffEffect buff)
        {
            this.PlayerMsgSender.NotifyAll_EntityAddBuff(guid, buff);
        }

        public void OnSyncIsUseReviveCoin(int playerIndex)
        {
            this.PlayerMsgSender.NotifyAll_SyncIsUseReviveCoin(playerIndex);
        }

        //有实体死亡了
        internal void OnEntityDead(BattleEntity battleEntity, bool isTrueDead)
        {
            var guid = battleEntity.guid;
            //track
            battleEntity.NotifySkillTrackEnd(SkillTrackEndTimeType.OnEntityDead);

            this.skillEffectMgr.DeleteAllBuffsFromEntity(guid, true);

            this.PlayerMsgSender.NotifyAll_EntityDead(battleEntity, isTrueDead);

            OnEntityDeadAction?.Invoke(battleEntity);
            // //_G.Log("--------------OnEntityDead");
            // var eventArgs = new EventEntityEventArg
            // {
            //     context = new ActionContext()
            //     {
            //         DeadEntity = battleEntity,
            //         battle = this
            //     },
            //     entityEventType = EntityEventType.Dead
            // };
            // //Logx.Log("Battle : entity dead : configId : " + battleEntity.configId);
            // this.OnEntityEventAction?.Invoke(eventArgs);
        }

        //所有玩家的剧情都准备好了
        public void OnAllPlayerPlotEnd(string plotName)
        {
            this.PlayerMsgSender.NotifyAll_AllPlayerPlotEnd(plotName);

            //OnPlotEndAction?.Invoke(null);
        }

        //战斗实体控制显隐
        internal void OnSetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            this.PlayerMsgSender.NotifyAll_SetEntitiesShowState(entityGuids, isShow);
        }


        public void OnSkillInfoUpdate(Skill skill)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillInfoUpdate(skill);
        }

        public void OnItemInfoUpdate(BattleItem item)
        {
            this.PlayerMsgSender.NotifyAll_NotifyItemInfoUpdate(item);
        }

        //单位身上的道具更新
        public void OnEntityItemsUpdate(int entityGuid, List<ItemBarCell> itemCellList)
        {
            this.PlayerMsgSender.NotifyAll_NotifyEntityItemsInfoUpdate(entityGuid, itemCellList);
        }

        public void OnSkillItemInfoUpdate(BattleItem item)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillItemInfoUpdate(item);
        }

        internal void OnNotifySkillTrackStart(Skill skill, int skillTrackId)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillTrackStart(skill, skillTrackId);
        }

        internal void OnNotifySkillTrackEnd(Skill skill, int skillTrackId)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillTrackEnd(skill, skillTrackId);
        }

        public void OnUpdateBoxInfo(int playerIndex, Dictionary<RewardQuality, List<BattleBox>> boxDic)
        {
            this.PlayerMsgSender.NotifyAll_NotifyBoxInfoUpdate(playerIndex, boxDic);
        }

        public void OnNotifyOpenBox(BattleBox box)
        {
            this.PlayerMsgSender.NotifyAll_NotifyOpenBox(box);
        }

        public void OnNotifySyncBattleReward(int playerIndex,
            BaseBattleReward battleReward)
        {
            this.PlayerMsgSender.NotifySyncPlayerBattleReward(playerIndex,
                battleReward);
        }

        public void OnSyncReplaceSkillResult(int playerIndex,
            ResultCode retCode)
        {
            this.PlayerMsgSender.Notify_ReplaceSkillResult(playerIndex,
                retCode);
        }

        public void OnSyncReplaceTeamMemberResult(int playerIndex,
            ResultCode retCode)
        {
            this.PlayerMsgSender.Notify_ReplaceTeamMemberResult(playerIndex,
                retCode);
        }

        //同步某个 entity 的朝向
        public void OnChangeEntityDir(int guid, Vector3 dir)
        {
            this.PlayerMsgSender.NotifyAll_SyncEntityDir(guid, dir);
        }

        //同步当前血量
        public void OnChangeEntityCurrHealth(int guid, int hp, int fromEntityGuid)
        {
            this.PlayerMsgSender.NotifyAll_SyncEntityCurrHealth(guid, hp, fromEntityGuid);
        }

        //同步当前状态信息
        public void OnChangeEntityStateData(int guid, EntityStateDataBean bean)
        {
            this.PlayerMsgSender.NotifyAll_SyncEntityStateData(guid, bean);
        }

        //更新宝箱商店
        public void OnNotifyUpdateBoxShop(int playerIndex, Dictionary<RewardQuality, BattleBoxShopItem> shopItemDic)
        {
            this.PlayerMsgSender.NotifyUpdateBoxShop(playerIndex, shopItemDic);
        }

        //更新玩家货币
        public void OnNotifyUpdateCurrency(int playerIndex, Dictionary<int, BattleCurrency> currencyDic)
        {
            this.PlayerMsgSender.NotifyUpdateCurrency(playerIndex, currencyDic);
        }

        //实体复活的时候
        public void OnEntityRevive(BattleEntity battleEntity)
        {
            this.PlayerMsgSender.Notify_NotifyEntityRevive(battleEntity.guid);
        }
    }
}