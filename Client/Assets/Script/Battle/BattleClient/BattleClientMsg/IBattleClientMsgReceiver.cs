// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Battle;
// using Battle_Client;
// using GameData;
// using NetProto;
// using UnityEngine;
//
// namespace Battle_Client
// {
//     //客户端战斗消息接收
//     public interface IBattleClientMsgReceiver
//     {
//         //void On_EntityStartMove(IBattleClientMsg battleMsg);
//         //void On_CreateBattle(BattleClientMsg_InitArg initArg);
//         void On_PlayerReadyState(int uid, bool isReady);
//         void On_AllPlayerLoadFinish();
//         void On_StartBattle();
//
//         void On_CreateEntities(List<BattleClientMsg_Entity> entity);
//
//         //void On_EntityStartMove(int Guid, UnityEngine.Vector3 EndPos, UnityEngine.Vector3 uDir, float MoveSpeed);
//         void On_EntityStartMoveByPath(int Guid, List<UnityEngine.Vector3> EndPos, float MoveSpeed);
//         void On_EntityStopMove(int Guid, UnityEngine.Vector3 EndPos);
//         void On_EntitySyncDir(int Guid, UnityEngine.Vector3 dir);
//         void On_EntityUseSkill(int entityGuid, int skillConfig);
//         void On_CreateSkillEffect(CreateEffectInfo createEffectInfo);
//         void On_SkillEffectStartMove(int EffectGuid, UnityEngine.Vector3 TargetPos, int TargetGuid, float moveSpeed);
//         void On_DestroySkillEffect(int effectGuid);
//         void On_SyncEntityAttr(int entityGuid, List<BattleClientMsg_BattleAttr> atts);
//         void On_SyncEntityValue(int entityGuid, List<BattleClientMsg_BattleStateValue> values);
//         void On_EntityDead(int entityGuid);
//         void On_PlayPlot(string plotName);
//         void On_PlotEnd();
//         void On_SetEntitiesShowState(List<int> Guids, bool isShow);
//         void On_BattleEnd(bool isWin);
//         void On_SkillInfoUpdate(int entityGuid, int skillConfigId, float currCDTime, float maxCDTime);
//         void On_ItemInfoUpdate(int entityGuid, int index,int configId,int count, float currCDTime, float maxCDTime);
//         void On_SkillItemInfoUpdate(int entityGuid, int index,int configId,int count, float currCDTime, float maxCDTime);
//         void On_BuffInfoUpdate(BuffEffectInfo buffInfo);
//         void On_SkillTrackStart(BattleClientMsg_CreateSkillTrack buffInfo);
//         void On_SkillTrackEnd(int entityGuid, int trackId);
//         void On_BoxInfoUpdate(int entityGuid,List<BattleClientMsg_BattleBox> boxList);
//         void On_OpenBox(BattleClientMsg_BattleBox box);
//         void On_SelectBoxReward(int index);
//     }
// }