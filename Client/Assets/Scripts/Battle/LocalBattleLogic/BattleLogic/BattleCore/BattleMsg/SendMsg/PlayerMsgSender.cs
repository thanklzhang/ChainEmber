using System.Collections.Generic;

namespace Battle
{
    public interface IBattleMsgSender
    {
        void NotifyAll_PlayerReadyState(int playerIndex,bool isReady);
        void NotifyAll_AllPlayerLoadFinish();
        void NotifyAll_BattleStart();
        void NotifyAll_PlayPlot(string name);
        //void NotifyAll_EntityStartMove(int guid, Vector3 targetPos, Vector3 dir,float finalMoveSpeed);
        void NotifyAll_EntityStartMoveByPath(int guid, List<Vector3> pathList, float finalMoveSpeed,bool isSkillForce);
        void NotifyAll_EntityStopMove(int guid, Vector3 position);
        void NotifyAll_SyncEntityDir(int guid, Vector3 dir);
        void NotifyAll_OnEntityReleaseSkill(int guid, int skillConfigId);
        void NotifyAll_CreateSkillEffect(CreateEffectInfo createEffectInfo);
        void NotifyAll_NotifyUpdateBuffInfo(BuffEffectInfo buffInfo);
        void NotifyAll_SkillEffectStartMove(EffectMoveArg effectrMoveArg);
        void NotifyAll_SkillEffectDestroy(int guid);
        void NotifyAll_CreateEntities(List<BattleEntity> entitiese);
        void NotifyAll_SyncEntityAttr(int guid, Dictionary<EntityAttrType, float> dic);
        //void NotifyAll_SyncEntitySkills(int guid, Dictionary<int, Skill> dic);
        void NotifyAll_SyncEntityCurrHealth(int guid, int hp, int fromEntityGuid);
        void NotifyAll_SyncEntityStateData(int guid, EntityStateDataBean stateData);
        void NotifyAll_EntityAddBuff(int guid, BuffEffect buff);
        void NotifyAll_EntityDead(BattleEntity battleEntity,bool isTrueDead);
        void NotifyAll_AllPlayerPlotEnd(string plotName);
        void NotifyAll_SetEntitiesShowState(List<int> entityGuids, bool isShow);
        void NotifyAll_NotifySkillInfoUpdate(Skill skill);
        void NotifyAll_NotifyItemInfoUpdate(BattleItem item);
        void NotifyAll_NotifySkillItemInfoUpdate(BattleItem item);
        void NotifyAll_NotifyEntityItemsInfoUpdate(int entityGuid,List<ItemBarCell> item);
        void NotifyAll_NotifySkillTrackStart(Skill skill,int skillTrackId);
        void NotifyAll_NotifySkillTrackEnd(Skill skill, int skillTrackId);
        void NotifyAll_SyncIsUseReviveCoin(int playerIndex);

        void NotifyAll_NotifyBoxInfoUpdate(int playerIndex,Dictionary<RewardQuality,List<BattleBox>> boxDic);
        //void NotifyAll_BattleEnd(int winTeam);
        void NotifyAll_NotifyOpenBox(BattleBox box);
        
        
        void NotifyAll_NotifySelectBoxReward(int index);

        void NotifyAll_EnterProcessState(BattleProcessState state,int currWaveIndex,int surplusTimeMS);
        
        void NotifyAll_UpdateProcessStateInfo(int currProgress,int maxProgress);
        
        void NotifyUpdateBoxShop(int playerIndex,Dictionary<RewardQuality,
            BattleBoxShopItem> shopItemDic);
        
        void NotifyUpdateCurrency(int playerIndex,Dictionary<int,
            BattleCurrency> currencyItemDic);

        void Notify_BattleWavePass(int passTeam, BattleWavePassResult reward);

        void Notify_EntityAbnormalEffect(int guid, AbnormalStateBean stateBean);
        
        void NotifySyncPlayerBattleReward(int playerIndex,BaseBattleReward reward);

        void UpdatePlayerTeamMembersInfo(int playerIndex,List<int> teamMemberList);
        
        void NotifyHeroOperationByArraying(int playerIndex,int opHeroGuid,Vector3 targetPos,
           int toUnderstudyIndex);

        void Notify_ReplaceSkillResult(int playerIndex,ResultCode retCode);

        void Notify_ReplaceTeamMemberResult(int playerIndex,ResultCode retCode);
        
        void SyncPlayerWarehouseItem(int playerIndex,int cellIndex,BattleItem opItem);
        
        void SyncEntityItemBarItem(int entityGuid,int cellIndex,BattleItem opItem,bool isUnlock);
        
        void Notify_NotifyEntityRevive(int entityGuid);
        void Notify_SyncPlayerBuyInfo(PlayerBuyInfo buyInfo);

        void SendMsgToClient(int uid, int cmd, byte[] bytes);
        void NotifyAllPlayerMsg(int cmd, byte[] bytes);
      
    }
}

