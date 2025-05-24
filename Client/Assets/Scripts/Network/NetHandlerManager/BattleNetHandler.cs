using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using NetProto;
using GameData;
using Battle_Client;
using BattleConvert = Battle_Client.BattleConvert;
using Vector3 = UnityEngine.Vector3;

public class BattleNetHandler : NetHandler
{
    //public Action<scStartBattle> startBattleAction;
    //public Action<scEnterGame> enterGameResultAction;

    public Dictionary<ProtoIDs, Action<byte[]>> battleTransitionMsgDic;

    public override void Init()
    {
        battleTransitionMsgDic = new Dictionary<ProtoIDs, Action<byte[]>>();

        // //战斗流程正常协议
        // AddListener((int)ProtoIDs.NotifyCreateBattle, OnNotifyCreateBattle);
        // AddListener((int)ProtoIDs.TransitionBattle2Player, OnTransitionBattle2Player);
        // AddListener((int)ProtoIDs.NotifyBattleEnd, OnNotifyBattleEnd);
        // AddListener((int)ProtoIDs.ApplyBattleEnd, OnApplyBattleEnd);
        //
        // //AddListener((int)ProtoIDs.EnterGame, OnEnterGame);
        //
        // //战斗转发协议 需要转一下
        // AddBattleMsg(ProtoIDs.PlayerLoadProgress, this.OnPlayerLoadProgress);
        // AddBattleMsg(ProtoIDs.NotifyAllPlayerLoadFinish, this.OnNotifyAllPlayerLoadFinish);
        // AddBattleMsg(ProtoIDs.BattleReadyFinish, this.OnBattleReadyFinish);
        // AddBattleMsg(ProtoIDs.NotifyPlayerReadyState, this.OnNotifyPlayerReadyState);
        // AddBattleMsg(ProtoIDs.NotifyBattleStart, this.OnNotifyBattleStart);
        // AddBattleMsg(ProtoIDs.NotifyCreateEntities, this.OnNotifyCreateEntities);
        // //AddBattleMsg(ProtoIDs.NotifyEntityMove, this.OnNotifyEntityMove);
        // AddBattleMsg(ProtoIDs.NotifyEntityMoveByPath, this.OnNotifyEntityMoveByPath);
        // AddBattleMsg(ProtoIDs.NotifyEntityStopMove, this.OnNotifyEntityStopMove);
        // AddBattleMsg(ProtoIDs.NotifyEntityDir, this.NotifyAll_SyncEntityDir);
        // AddBattleMsg(ProtoIDs.NotifyEntityReleaseSkill, this.OnNotifyEntityUseSkill);
        // AddBattleMsg(ProtoIDs.NotifyCreateSkillEffect, this.OnNotifyCreateSkillEffect);
        // AddBattleMsg(ProtoIDs.NotifySkillEffectStartMove, this.OnNotifySkillEffectStartMove);
        // AddBattleMsg(ProtoIDs.NotifySkillEffectDestroy, this.OnNotifySkillEffectDestroy);
        // AddBattleMsg(ProtoIDs.NotifySyncEntityAttr, this.OnNotifySyncEntityAttr);
        // AddBattleMsg(ProtoIDs.NotifySyncEntityValue, this.OnNotifySyncEntityValue);
        // AddBattleMsg(ProtoIDs.NotifyEntityDead, this.OnNotifyEntityDead);
        // AddBattleMsg(ProtoIDs.NotifyPlayPlot, this.OnNotifyPlayPlot);
        // AddBattleMsg(ProtoIDs.ClientPlotEnd, this.OnClientPlotEnd);
        // AddBattleMsg(ProtoIDs.NotifyPlotEnd, this.OnNotifyPlotEnd);
        // AddBattleMsg(ProtoIDs.NotifySetEntityShowState, this.OnNotifySetEntityShowState);
        //
        // AddBattleMsg(ProtoIDs.NotifySkillInfoUpdate, this.OnNotifySkillInfoUpdate);
        // AddBattleMsg(ProtoIDs.NotifyNotifyUpdateBuffInfo, this.OnNotifyNotifyUpdateBuffInfo);
        // AddBattleMsg(ProtoIDs.NotifySkillTrackStart, this.OnNotifySkillTrackStart);
        // AddBattleMsg(ProtoIDs.NotifySkillTrackEnd, this.OnNotifySkillTrackEnd);
    }

    void AddBattleMsg(ProtoIDs cmd, Action<byte[]> action)
    {
        battleTransitionMsgDic.Add(cmd, action);
    }


    //转发协议相关//////////////////////////////////////////

    //统一转发战斗消息
    public void TransitionBattleMsg(ProtoIDs cmd, IMessage msg)
    {
        // Logx.Log("TransitionBattleMsg : send true cmd : " + cmd);
        csTransitionBattle tranBattleMsg = new csTransitionBattle();

        var myUid = GameDataManager.Instance.UserData.Uid;
        ClientProtoHead head = new ClientProtoHead()
        {
            cmd = (ushort)cmd,
            uid = myUid
        };

        var clientData = ProtoMsgUtil.MakeClientMsgBytes(msg.ToByteArray(), head);

        tranBattleMsg.Cmd = (int)cmd;
        tranBattleMsg.Data = ByteString.CopyFrom(clientData);
        NetworkManager.Instance.SendMsg(ProtoIDs.TransitionBattle, tranBattleMsg.ToByteArray());
    }

    //统一接受战斗消息并解析为客户端需要的结构
    private void OnTransitionBattle2Player(MsgPack msgPack)
    {
        var trans = scTransitionBattle2Player.Parser.ParseFrom(msgPack.data);

        var clientTrueData = trans.Data;

        var clientMsg = ProtoMsgUtil.GetClientMsg(clientTrueData.ToByteArray());
        var clientCmd = (ProtoIDs)clientMsg.head.cmd;

        //Logx.LogBattle("OnTransitionBattle2Player , receive the battle cmd : " + clientMsg.head.cmd + " : " +
        //              clientCmd);
        //Debug.Log("zxy : OnTransitionBattle2Player , receive the true cmd : " + clientCmd);

        var battleMsgData = clientMsg.data;

        if (battleTransitionMsgDic.ContainsKey(clientCmd))
        {
           
            var action = battleTransitionMsgDic[clientCmd];
            action?.Invoke(battleMsgData);
        }
        else
        {
            Logx.LogWarning("the cmd is not found : " + clientCmd);
        }
    }
    //
    // public void OnNotifyCreateBattle(MsgPack msgPack)
    // {
    //     // Logx.Log("OnNotifyCreateBattle");
    //     var netBattleInit = NetProto.scNotifyCreateBattle.Parser.ParseFrom(msgPack.data);
    //    
    //     bool isLocal = netBattleInit.LocalApplyBattleArg != null;
    //     if (isLocal)
    //     {
    //         //本地战斗的话是取服务端的申请战斗参数
    //         BattleManager.Instance.CreateLocalButRemoteResultBattle(netBattleInit.LocalApplyBattleArg);
    //     }
    //     else
    //     {
    //         //战斗初始数据(net 层) 转换为客户端战斗初始数据
    //         var battleInit = GetBattleInitArgsByProto(netBattleInit);
    //         BattleManager.Instance.CreateRemoteBattle(battleInit);
    //     }
    // }
    //
    // //协议的创建战斗信息 转化为 战斗所用战斗初始信息
    // public BattleClient_CreateBattleArgs GetBattleInitArgsByProto(scNotifyCreateBattle netBattle)
    // {
    //     BattleClient_CreateBattleArgs battleArgs = new BattleClient_CreateBattleArgs();
    //     var netBattleArgs = netBattle.BattleInitArg;
    //
    //     battleArgs.configId = netBattleArgs.TableId;
    //     battleArgs.guid = netBattleArgs.Guid;
    //     battleArgs.roomId = netBattleArgs.RoomId;
    //
    //     //clientPlayers
    //     battleArgs.clientPlayers = new List<BattleClient_ClientPlayer>();
    //     foreach (var netPlayer in netBattleArgs.BattlePlayerInitArg.PlayerList)
    //     {
    //         BattleClient_ClientPlayer player = new BattleClient_ClientPlayer()
    //         {
    //             uid = netPlayer.Uid,
    //             team = netPlayer.Team,
    //             ctrlHeroGuid = netPlayer.CtrlHeroGuid,
    //             playerIndex = netPlayer.PlayerIndex
    //         };
    //         battleArgs.clientPlayers.Add(player);
    //     }
    //
    //     //entityList
    //     battleArgs.entityList = new List<BattleClientMsg_Entity>();
    //     foreach (var netEntity in netBattleArgs.EntityInitArg.BattleEntityInitList)
    //     {
    //         BattleClientMsg_Entity entity = new BattleClientMsg_Entity()
    //         {
    //             guid = netEntity.Guid,
    //             configId = netEntity.ConfigId,
    //             level = netEntity.Level,
    //             playerIndex = netEntity.PlayerIndex,
    //             position = BattleConvert.ConvertToVector3(netEntity.Position),
    //         };
    //
    //         //entity skill list
    //         entity.skills = new List<BattleClientMsg_Skill>();
    //         foreach (var netSkill in netEntity.SkillInitList)
    //         {
    //             BattleClientMsg_Skill skill = new BattleClientMsg_Skill()
    //             {
    //                 configId = netSkill.ConfigId,
    //                 level = netSkill.Level,
    //                 maxCDTime = netSkill.MaxCDTime / 1000.0f
    //             };
    //             entity.skills.Add(skill);
    //         }
    //
    //         battleArgs.entityList.Add(entity);
    //     }
    //
    //     return battleArgs;
    // }
    //
    // //主动战斗结束(一般是本地战斗)
    // public void SendApplyBattleEnd(NetProto.ApplyBattleEndArg arg)
    // {
    //     csApplyBattleEnd applyEnd = new csApplyBattleEnd();
    //     applyEnd.ApplyBattleEndArg = arg;
    //     NetworkManager.Instance.SendMsg(ProtoIDs.ApplyBattleEnd, applyEnd.ToByteArray());
    // }
    //
    // public void OnApplyBattleEnd(MsgPack msgPack)
    // {
    // }
    //
    // private void OnNotifyBattleEnd(MsgPack msgPack)
    // {
    //     scNotifyBattleEnd scBattleEnd = scNotifyBattleEnd.Parser.ParseFrom(msgPack.data);
    //
    //     // Logx.Log("net msg : OnNotifyBattleEnd : " + scBattleEnd.ToString());
    //
    //     BattleResultDataArgs resultData = new BattleResultDataArgs();
    //     resultData.isWin = 1 == scBattleEnd.IsWin;
    //     resultData.rewardDataList = new List<ItemData>();
    //     foreach (var serReward in scBattleEnd.Rewards)
    //     {
    //         ItemData item = new ItemData();
    //         item.configId = serReward.ConfigId;
    //         item.count = serReward.Count;
    //         resultData.rewardDataList.Add(item);
    //     }
    //
    //     BattleManager.Instance.BattleEnd(resultData);
    // }
    //
    // //客户端真正操作的协议/////////////////////////////////////////////////////////////////////////
    //
    //
    // //上报加载进度(目前算是 100 )
    // public void SendPlayerLoadProgress(int progress)
    // {
    //     csPlayerLoadProgress csProgress = new csPlayerLoadProgress();
    //     csProgress.Progress = progress;
    //     TransitionBattleMsg(ProtoIDs.PlayerLoadProgress, csProgress);
    // }
    //
    // public void OnPlayerLoadProgress(byte[] byteData)
    // {
    //     scPlayerLoadProgress progress = scPlayerLoadProgress.Parser.ParseFrom(byteData);
    //     // Logx.Log("receive battle msg : PlayerLoadProgress");
    // }
    //
    // //所有人都加载好了
    // public void SendNotifyAllPlayerLoadFinish(Action action)
    // {
    // }
    //
    // public void OnNotifyAllPlayerLoadFinish(byte[] byteData)
    // {
    //     // Logx.Log("receive battle msg : NotifyAllPlayerLoadFinish");
    //     scNotifyAllPlayerLoadFinish allFinish = scNotifyAllPlayerLoadFinish.Parser.ParseFrom(byteData);
    //
    //     ////先用数据层 之后考虑是否用 battleManager 流程
    //     //var battleData = GameDataManager.Instance.BattleGameDataStore;
    //     //battleData.IsAllPlayerLoadFinish = true;
    //
    //     //EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);
    //
    //     BattleManager.Instance.MsgReceiver.On_AllPlayerLoadFinish();
    // }
    //
    // //战斗准备完成
    // public void SendBattleReadyFinish(Action action)
    // {
    //     csBattleReadyFinish ready = new csBattleReadyFinish();
    //     TransitionBattleMsg(ProtoIDs.BattleReadyFinish, ready);
    // }
    //
    // public void OnBattleReadyFinish(byte[] byteData)
    // {
    //     scBattleReadyFinish readyFinish = scBattleReadyFinish.Parser.ParseFrom(byteData);
    // }
    //
    // public void OnNotifyPlayerReadyState(byte[] byteData)
    // {
    //     scNotifyPlayerReadyState notify = scNotifyPlayerReadyState.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_PlayerReadyState(notify.Uid, notify.IsReady);
    // }
    //
    // //战斗正式开始
    // public void SendNotifyBattleStart(Action action)
    // {
    // }
    //
    // public void OnNotifyBattleStart(byte[] byteData)
    // {
    //     scNotifyBattleStart battleStart = scNotifyBattleStart.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_StartBattle();
    // }
    //
    //
    // private void OnNotifyCreateEntities(byte[] byteData)
    // {
    //     scNotifyCreateEntities create = scNotifyCreateEntities.Parser.ParseFrom(byteData);
    //     //foreach (var item in create.BattleEntities)
    //     //{
    //     //    var serverEntity = item;
    //     //    BattleManager.Instance.MsgReceiver.On_CreateEntity(serverEntity);
    //     //}
    //
    //
    //     List<BattleClientMsg_Entity> entityList = new List<BattleClientMsg_Entity>();
    //     foreach (var netEntity in create.BattleEntities)
    //     {
    //         BattleClientMsg_Entity entity = new BattleClientMsg_Entity()
    //         {
    //             guid = netEntity.Guid,
    //             configId = netEntity.ConfigId,
    //             level = netEntity.Level,
    //             playerIndex = netEntity.PlayerIndex,
    //             position = BattleConvert.ConvertToVector3(netEntity.Position),
    //         };
    //
    //         entity.skills = new List<BattleClientMsg_Skill>();
    //         foreach (var netSkill in netEntity.SkillInitList)
    //         {
    //             var skill = new BattleClientMsg_Skill()
    //             {
    //                 configId = netSkill.ConfigId,
    //                 level = netSkill.Level,
    //                 maxCDTime = netSkill.MaxCDTime / 1000.0f
    //             };
    //             entity.skills.Add(skill);
    //         }
    //
    //         entityList.Add(entity);
    //     }
    //
    //     BattleManager.Instance.MsgReceiver.On_CreateEntities(entityList);
    // }
    //
    // //private void OnNotifyEntityMove(byte[] byteData)
    // //{
    // //    scNotifyEntityMove notifyEntityMove = scNotifyEntityMove.Parser.ParseFrom(byteData);
    //
    // //    var targetPos = notifyEntityMove.EndPos;
    // //    var dir = notifyEntityMove.Dir;
    //
    //
    // //    var pos = BattleConvert.ConverToVector3(targetPos);// new UnityEngine.Vector3(targetPos.X, targetPos.Y, targetPos.Z);
    // //    var uDir = BattleConvert.ConverToVector3(dir);// new UnityEngine.Vector3(dir.X, dir.Y, dir.Z);
    //
    //
    // //    var speed = BattleConvert.GetValue(notifyEntityMove.MoveSpeed);
    // //    BattleManager.Instance.MsgReceiver.On_EntityStartMove(notifyEntityMove.Guid, pos, uDir, speed);
    // //}
    //
    //
    // public void OnNotifyEntityMoveByPath(byte[] byteData)
    // {
    //     scNotifyEntityMoveByPath notify = scNotifyEntityMoveByPath.Parser.ParseFrom(byteData);
    //
    //     List<Vector3> resultPosList = new List<Vector3>();
    //     foreach (var pos in notify.PathList)
    //     {
    //         var unityPos = BattleConvert.ConvertToVector3(pos);
    //         resultPosList.Add(unityPos);
    //     }
    //
    //     var speed = BattleConvert.GetValue(notify.MoveSpeed);
    //     BattleManager.Instance.MsgReceiver.On_EntityStartMoveByPath(notify.Guid, resultPosList, speed);
    // }
    //
    // protected void OnNotifyEntityStopMove(byte[] byteData)
    // {
    //     scNotifyEntityStopMove stop = scNotifyEntityStopMove.Parser.ParseFrom(byteData);
    //
    //     var position = stop.EndPos;
    //     var pos = BattleConvert
    //         .ConvertToVector3(position); // new UnityEngine.Vector3(position.X, position.Y, position.Z);
    //     BattleManager.Instance.MsgReceiver.On_EntityStopMove(stop.Guid, pos);
    // }
    //
    // public void NotifyAll_SyncEntityDir(byte[] byteData)
    // {
    //     scNotifyEntityDir notifyDirProto = scNotifyEntityDir.Parser.ParseFrom(byteData);
    //     var dir = notifyDirProto.Dir;
    //     var resultDir = BattleConvert.ConvertToVector3(dir); // new UnityEngine.Vector3(dir.X, dir.Y, dir.Z);
    //     BattleManager.Instance.MsgReceiver.On_EntitySyncDir(notifyDirProto.Guid, resultDir);
    // }
    //
    // public void SendUseSkill(int skillId, int targetGuid, Vector3 targetPos)
    // {
    //     csUseSkill useSkill = new csUseSkill();
    //     useSkill.Guid = 1; //每个玩家就一个实体 忽略
    //     useSkill.SkillId = skillId;
    //     useSkill.TargetGuid = targetGuid;
    //     useSkill.TargetPos = BattleConvert.ConvertToVector3Proto(targetPos);
    //     TransitionBattleMsg(ProtoIDs.UseSkill, useSkill);
    // }
    //
    // private void OnNotifyEntityUseSkill(byte[] byteData)
    // {
    //     scNotifyEntityUseSkill releaseSkill = scNotifyEntityUseSkill.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_EntityUseSkill(releaseSkill.Guid, releaseSkill.SkillConfigId);
    // }
    //
    // public void SendClientPlotEnd()
    // {
    //     csClientPlotEnd clientPlotEnd = new csClientPlotEnd();
    //     TransitionBattleMsg(ProtoIDs.ClientPlotEnd, clientPlotEnd);
    // }
    //
    // private void OnClientPlotEnd(byte[] byteData)
    // {
    // }
    //
    // public void NotifyAll_AllPlayerPlotEnd(string plotName)
    // {
    //     BattleManager.Instance.MsgReceiver.On_PlotEnd();
    // }
    //
    // int count = 0;
    //
    //
    // private void OnNotifyCreateSkillEffect(byte[] byteData)
    // {
    //     scNotifyCreateSkillEffect skillEffect = scNotifyCreateSkillEffect.Parser.ParseFrom(byteData);
    //
    //     var position = skillEffect.Position;
    //     var pos = BattleConvert
    //         .ConvertToVector3(position); // new UnityEngine.Vector3(position.X, position.Y, position.Z);
    //     //var lastTimeInt = (int)(skillEffect.LastTime * 1000);
    //     Battle.CreateEffectInfo createInfo = new Battle.CreateEffectInfo();
    //     createInfo.guid = skillEffect.Guid;
    //     // Logx.LogNet("create skillEffect.Guid : " + skillEffect.Guid);
    //     createInfo.resId = skillEffect.ResId;
    //     createInfo.createPos = new Battle.Vector3(pos.x, pos.y, pos.z);
    //     createInfo.followEntityGuid = skillEffect.FollowEntityGuid;
    //     createInfo.isAutoDestroy = skillEffect.IsAutoDestroy;
    //     //createInfo.effectPosType = skillEffect.effectPosType;
    //
    //     if (skillEffect.BuffInfo != null)
    //     {
    //         createInfo.buffInfo = BattleConvert.ToBuffInfo(skillEffect.BuffInfo);
    //     }
    //
    //     //BattleManager.Instance.MsgReceiver.On_CreateSkillEffect(skillEffect.Guid, skillEffect.ResId, pos, skillEffect.FollowEntityGuid, skillEffect.IsAutoDestroy);
    //     BattleManager.Instance.MsgReceiver.On_CreateSkillEffect(createInfo);
    //
    //
    //     //BattleManager.Instance.MsgReceiver.On_CreateSkillEffect(skillEffect);
    // }
    //
    // private void OnNotifySkillEffectStartMove(byte[] byteData)
    // {
    //     scNotifySkillEffectStartMove effectStartMove = scNotifySkillEffectStartMove.Parser.ParseFrom(byteData);
    //
    //     var guid = effectStartMove.EffectGuid;
    //     var netTargetPos = effectStartMove.TargetPos;
    //     var targetPos =
    //         BattleConvert
    //             .ConvertToVector3(
    //                 netTargetPos); // new UnityEngine.Vector3(netTargetPos.X, netTargetPos.Y, netTargetPos.Z);
    //     var targetGuid = effectStartMove.TargetGuid;
    //     var moveSpeed = BattleConvert.GetValue(effectStartMove.MoveSpeed);
    //     BattleManager.Instance.MsgReceiver.On_SkillEffectStartMove(guid, targetPos, targetGuid, moveSpeed);
    // }
    //
    // private void OnNotifySkillEffectDestroy(byte[] byteData)
    // {
    //     scNotifySkillEffectDestroy skillEffectDestroy = scNotifySkillEffectDestroy.Parser.ParseFrom(byteData);
    //
    //     //Logx.LogNet("delete skillEffectDestroy.EffectGuid : " + skillEffectDestroy.EffectGuid);
    //
    //     BattleManager.Instance.MsgReceiver.On_DestroySkillEffect(skillEffectDestroy.EffectGuid);
    // }
    //
    // private void OnNotifySyncEntityAttr(byte[] byteData)
    // {
    //     scNotifySyncEntityAttr sync = scNotifySyncEntityAttr.Parser.ParseFrom(byteData);
    //
    //     var netAttrs = sync.Attrs.ToList();
    //
    //     List<BattleClientMsg_BattleAttr> attrs = new List<BattleClientMsg_BattleAttr>();
    //     foreach (var option in netAttrs)
    //     {
    //         BattleClientMsg_BattleAttr attr = new BattleClientMsg_BattleAttr();
    //         attr.type = (Battle.EntityAttrType)(int)option.Type;
    //         if (option.Type == (int)Battle.EntityAttrType.AttackSpeed)
    //         {
    //             attr.value = BattleConvert.GetValue(option.Value);
    //         }
    //         else if (option.Type == (int)Battle.EntityAttrType.MoveSpeed)
    //         {
    //             attr.value = BattleConvert.GetValue(option.Value);
    //         }
    //         else if (option.Type == (int)Battle.EntityAttrType.AttackRange)
    //         {
    //             attr.value = BattleConvert.GetValue(option.Value);
    //         }
    //         else if (option.Type == (int)Battle.EntityAttrType.AttackSpeed)
    //         {
    //             attr.value = BattleConvert.GetValue(option.Value);
    //         }
    //         else if (option.Type == (int)Battle.EntityAttrType.AttackRange)
    //         {
    //             attr.value = BattleConvert.GetValue(option.Value);
    //         }
    //         else
    //         {
    //             attr.value = (int)option.Value;
    //         }
    //
    //         attrs.Add(attr);
    //     }
    //
    //     //Debug.Log("zxy : hh : notify attr");
    //
    //     BattleManager.Instance.MsgReceiver.On_SyncEntityAttr(sync.EntityGuid, attrs);
    // }
    //
    // private void OnNotifySyncEntityValue(byte[] byteData)
    // {
    //     scNotifySyncEntityValue sync = scNotifySyncEntityValue.Parser.ParseFrom(byteData);
    //
    //     List<BattleClientMsg_BattleStateValue> values = new List<BattleClientMsg_BattleStateValue>();
    //     foreach (var item in sync.Values)
    //     {
    //         BattleClientMsg_BattleStateValue battleValue = new BattleClientMsg_BattleStateValue()
    //         {
    //             type = (EntityStateValueType)item.Type,
    //             value = item.Value,
    //             fromEntityGuid = item.FromEntityGuid
    //         };
    //         values.Add(battleValue);
    //     }
    //
    //     BattleManager.Instance.MsgReceiver.On_SyncEntityValue(sync.EntityGuid, values);
    // }
    //
    // private void OnNotifyEntityDead(byte[] byteData)
    // {
    //     scNotifyEntityDead sync = scNotifyEntityDead.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_EntityDead(sync.EntityGuid);
    // }
    //
    // private void OnNotifyPlayPlot(byte[] byteData)
    // {
    //     scNotifyPlayPlot sync = scNotifyPlayPlot.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_PlayPlot(sync.PlotName);
    // }
    //
    //
    // private void OnNotifyNotifyUpdateBuffInfo(byte[] byteData)
    // {
    //     scNotifyUpdateBuffInfo info = scNotifyUpdateBuffInfo.Parser.ParseFrom(byteData);
    //     var buffProto = info.BuffInfoList[0];
    //     //Logx.LogWarning("zxy : net : OnNotifyNotifyUpdateBuffInfo : " + info.ToString());
    //
    //     var buff = BattleConvert.ToBuffInfo(buffProto);
    //     BattleManager.Instance.MsgReceiver.On_BuffInfoUpdate(buff);
    // }
    //
    // private void OnNotifySkillInfoUpdate(byte[] byteData)
    // {
    //     scNotifySkillInfoUpdate info = scNotifySkillInfoUpdate.Parser.ParseFrom(byteData);
    //     var currCDTime = info.SkillInfo.CurrCDTime / 1000.0f;
    //     var maxCDTime = info.SkillInfo.MaxCDTime / 1000.0f;
    //     //Debug.LogError("currCDTime + " + currCDTime + " , " + maxCDTime);
    //     BattleManager.Instance.MsgReceiver.On_SkillInfoUpdate(info.EntityGuid, info.SkillConfigId, currCDTime,
    //         maxCDTime);
    // }
    //
    //
    // public void OnNotifyPlotEnd(byte[] byteData)
    // {
    //     scNotifyPlotEnd sync = scNotifyPlotEnd.Parser.ParseFrom(byteData);
    //     //BattleManager.Instance.MsgReceiver.On_PlotEnd(sync);
    // }
    //
    // public void OnNotifySetEntityShowState(byte[] byteData)
    // {
    //     scNotifySetEntityShowState sync = scNotifySetEntityShowState.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_SetEntitiesShowState(sync.Guids.ToList(), sync.IsShow);
    // }
    //
    // private void OnNotifySkillTrackStart(byte[] byteData)
    // {
    //     scNotifySkillTrackStart sync = scNotifySkillTrackStart.Parser.ParseFrom(byteData);
    //
    //     BattleClientMsg_CreateSkillTrack track = new BattleClientMsg_CreateSkillTrack();
    //     track.releaserEntityGuid = sync.ReleaserGuid;
    //     track.trackConfigId = sync.TrackConfigId;
    //     track.targetEntityGuid = sync.TargetEntityGuid;
    //     track.targetPos = BattleConvert.ConvertToVector3(sync.TargetPos);
    //
    //     BattleManager.Instance.MsgReceiver.On_SkillTrackStart(track);
    // }
    //
    // private void OnNotifySkillTrackEnd(byte[] byteData)
    // {
    //     scNotifySkillTrackEnd sync = scNotifySkillTrackEnd.Parser.ParseFrom(byteData);
    //     BattleManager.Instance.MsgReceiver.On_SkillTrackEnd(sync.EntityGuid, sync.SkillTrackConfigId);
    // }

    #region 战斗中玩家操作

    //移动单位实体
    public void SendMoveEntity(int entityGuid, Vector3 targetPos)
    {
        csMoveEntity move = new csMoveEntity()
        {
            Guid = entityGuid,
            TargetPos = BattleConvert.ConvertToVector3Proto(targetPos)
        };
        this.TransitionBattleMsg(ProtoIDs.MoveEntity, move);
    }

    public void OnMoveEntity()
    {
    }

    #endregion
}