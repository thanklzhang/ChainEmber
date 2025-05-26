using System;
using System.Collections.Generic;
using System.Linq;
using Battle_Client;
using UnityEngine;
using ServerSimulation.Account.Models;
using GameData;

namespace ServerSimulation.Services
{
    /// <summary>
    /// 战斗服务类 目前只提供战斗的数据转换系统
    /// </summary>
    public class BattleService
    {
        private static HeroService instance;

        public static HeroService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HeroService();
                }

                return instance;
            }
        }
        
         
        public static void StartBattle(int selectHeroGuid)//,Action<BattleClient_CreateBattleArgs> finishAction
        {
            var battleSys = ServerSimulationManager.Instance.BattleSystem;

            var args = GenBattleCreateArgs();
            BattleManager.Instance.CreateRemoteBattle(args);
        }

        //生成战斗创建参数
        public static BattleClient_CreateBattleArgs GenBattleCreateArgs(int battleConfig,int selectHeroGuid)
        {
            var hero = HeroService.Instance.GetHero(selectHeroGuid);
            if (hero == null)
            {
                Logx.Log(LogxType.Game, $"hero is null selectHeroGuid = {selectHeroGuid}");
                return null;
            }

            BattleClient_CreateBattleArgs battleArgs = new BattleClient_CreateBattleArgs();
            
            battleArgs.configId = battleConfig;
            battleArgs.guid = 1;
            battleArgs.roomId = 1;
            
            battleArgs.clientPlayers = new List<BattleClient_ClientPlayer>();
            BattleClient_ClientPlayer player = new BattleClient_ClientPlayer()
            {
                uid = 1,
                team = 0,
                // ctrlHeroGuid = netPlayer.CtrlHeroGuid,
                playerIndex = 0
            };
            battleArgs.clientPlayers.Add(player);
            
            
            //entityList
            battleArgs.entityList = new List<BattleClientMsg_Entity>();
            foreach (var netEntity in netBattleArgs.EntityInitArg.BattleEntityInitList)
            {
                BattleClientMsg_Entity entity = new BattleClientMsg_Entity()
                {
                    guid = netEntity.Guid,
                    configId = netEntity.ConfigId,
                    level = netEntity.Level,
                    playerIndex = netEntity.PlayerIndex,
                    position = BattleConvert.ConvertToVector3(netEntity.Position),
                };
    
                //entity skill list
                entity.skills = new List<BattleClientMsg_Skill>();
                foreach (var netSkill in netEntity.SkillInitList)
                {
                    BattleClientMsg_Skill skill = new BattleClientMsg_Skill()
                    {
                        configId = netSkill.ConfigId,
                        level = netSkill.Level,
                        maxCDTime = netSkill.MaxCDTime / 1000.0f
                    };
                    entity.skills.Add(skill);
                }
    
                battleArgs.entityList.Add(entity);
            
            
            return battleArgs;;
        }
            
            
            //根据后台申请建战斗
        //     public void ApplyCreateBattle()
        // {
        //     //所有玩家战斗初始化信息填充
        //
        //     //向战斗服务器发出申请战斗
        //     csApplyCreateBattle2S apply = new csApplyCreateBattle2S();
        //     //填充申请战斗通用结构 ApplyBattleArgs
        //     apply.ApplyBattleArg = new ApplyBattleArg();
        //     apply.ApplyBattleArg.BattleRoomId = this.Id;
        //
        //     Table.TeamStage teamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(TeamStageConfigId);
        //     apply.ApplyBattleArg.BattleTableId = teamStageTb.BattleId;
        //     var battleConfigId = apply.ApplyBattleArg.BattleTableId;
        //     apply.ApplyBattleArg.FunctionId = (int)FunctionIds.Team;
        //     //apply.StageId = teamStageConfigId;
        //
        //     var teamPlayers = this.playerDic;
        //
        //     //int team = 0;//目前一个玩家一队
        //
        //     //var battleId = apply.ApplyBattleArg.BattleTableId;
        //     //var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleId);
        //
        //     var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleConfigId);
        //     var forceUseHeroListStrs = battleTb.ForceUseHeroList.Split('|').ToList();
        //     var initPosStrs = battleTb.InitPos;
        //
        //     //取假 uid 给电脑玩家使用-----
        //     var forceUseHeroListStr = battleTb.ForceUseHeroList.Split('|');
        //
        //     List<int> hasUseUidList = new List<int>();
        //     foreach (var playerItem in teamPlayers)
        //     {
        //         hasUseUidList.Add((int)playerItem.Key);
        //     }
        //
        //
        //     var needUidCount = forceUseHeroListStr.Length - 1;
        //     List<int> canUseUidList = new List<int>();
        //     for (int i = 0; i < needUidCount; i++)
        //     {
        //         for (int j = 1; j < 10; j++)
        //         {
        //             var _uid = j;
        //             var isCantains = hasUseUidList.Contains(_uid);
        //             if (!isCantains)
        //             {
        //                 canUseUidList.Add(_uid);
        //                 hasUseUidList.Add(_uid);
        //                 break;
        //             }
        //         }
        //     }
        //     //-----------------------
        //
        //     //补齐
        //     for (int i = 0; i < teamPlayers.Count - forceUseHeroListStrs.Count; i++)
        //     {
        //         forceUseHeroListStrs.Add("");
        //     }
        //
        //
        //     Dictionary<int, string> playerIndexToConfigDic = new Dictionary<int, string>();
        //     int uidIndex = 0;
        //     for (int i = 0; i < forceUseHeroListStrs.Count; i++)
        //     {
        //         var useStr = forceUseHeroListStrs[i];
        //         if ("0" == useStr || "" == useStr)
        //         {
        //             //由 配表简洁 到 程序通用
        //             useStr = "" + i + ",0";
        //         }
        //
        //         var group = useStr.Split(',');
        //         int pIndex = int.Parse(group[0]);
        //         int configId = int.Parse(group[1]);
        //
        //         //开始寻找该玩家
        //         var isFind = false;
        //         foreach (var teamPlayerItem in teamPlayers)
        //         {
        //             var teamPlayer = teamPlayerItem.Value;
        //
        //             var playerIndex = teamPlayer.Seat;
        //             if (playerIndex == pIndex)
        //             {
        //                 //找到该玩家
        //                 //var playerEnterBattleArg = player.enterBattleArg;
        //                 NetProto.PlayerInitInfo playerInfo = new NetProto.PlayerInitInfo();
        //
        //                 playerInfo.Level = 1;
        //                 playerInfo.Uid = (int)teamPlayer.PlayerInfo.uid;
        //                 playerInfo.PlayerIndex = playerIndex;
        //
        //                 //组队刷副本当然是一队了
        //                 playerInfo.Team = 0;
        //
        //                 playerInfo.EntityInitInfo = new EntityInitInfo();
        //                 var entityInfo = playerInfo.EntityInitInfo;
        //
        //                 var userData = Root.userMgr.FindUserOnLogin((ulong)playerInfo.Uid);
        //                 var selectHeroData = userData.HeroListData.GetHeroByGuid(teamPlayer.SelectHeroGuid);
        //                 //--
        //                 if (0 == configId)
        //                 {
        //                     //玩家使用自己的英雄
        //                     entityInfo.IsPlayerCtrl = true;
        //                     entityInfo.IsHeroUseConfig = false;
        //
        //                     BattleProtoConvert.ConvertPlayerHaveEntityProto(selectHeroData, entityInfo);
        //                 }
        //                 else
        //                 {
        //                     //玩家强迫使用配置英雄 
        //                     entityInfo.ConfigId = configId;
        //                     entityInfo.IsPlayerCtrl = false;
        //                     entityInfo.IsHeroUseConfig = true;
        //                 }
        //
        //                 //--
        //                 playerInfo.EntityInitInfo = BattleProtoConvert.GetHeroEntityProto(
        //                     (int)teamPlayer.PlayerInfo.uid,
        //                     playerIndex, selectHeroData, battleConfigId);
        //
        //                 apply.ApplyBattleArg.PlayerInfoList.Add(playerInfo);
        //                 isFind = true;
        //                 break;
        //             }
        //         }
        //
        //         if (!isFind)
        //         {
        //             //没有找到 那么就是由电脑操控
        //
        //             NetProto.PlayerInitInfo playerInfo = new NetProto.PlayerInitInfo();
        //
        //             playerInfo.Level = 1;
        //             playerInfo.Uid = canUseUidList[uidIndex];
        //             uidIndex = uidIndex + 1;
        //             playerInfo.PlayerIndex = pIndex;
        //
        //             //组队刷副本当然是一队了
        //             playerInfo.Team = 0;
        //
        //             playerInfo.EntityInitInfo = new EntityInitInfo();
        //             var entityInfo = playerInfo.EntityInitInfo;
        //
        //             //玩家强迫使用配置英雄 
        //             entityInfo.ConfigId = configId;
        //             entityInfo.IsPlayerCtrl = false;
        //             entityInfo.IsHeroUseConfig = true;
        //             entityInfo.PlayerIndex = pIndex;
        //
        //             apply.ApplyBattleArg.PlayerInfoList.Add(playerInfo);
        //         }
        //     }
        //
        //     ProxyMgr.BattleProxy.RequestAync((int)ProtoIDs.ApplyCreateBattle2S, apply.ToByteArray());
        // }
            
            


    //       // //协议的创建战斗信息 转化为 战斗所用战斗初始信息
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
    //     

    //  //根据战斗开始后生成的战斗初始信息转换为协议给客户端发过去
    // public static BattleInitArg ConvertToClientBattleProto(Battle.Battle battle)
    // {
    //     NetProto.BattleInitArg netBattleInitArg = new BattleInitArg();
    //     netBattleInitArg.Guid = battle.Guid;
    //     netBattleInitArg.RoomId = battle.RoomId;
    //     netBattleInitArg.TableId = battle.tableId;
    //     netBattleInitArg.FunctionId = battle.funcId;
    //     //netBattleInitArg.MapInitArg
    //
    //     //玩家信息
    //     netBattleInitArg.BattlePlayerInitArg = new NetProto.BattlePlayerInitArg();
    //     var battlePlayers = battle.GetAllPlayers();
    //     foreach (var player in battlePlayers)
    //     {
    //         var netPlayer = new NetProto.BattlePlayerProto();
    //         netPlayer.Uid = (int)player.uid;
    //         netPlayer.PlayerIndex = player.playerIndex;
    //         netPlayer.Team = player.team;
    //         netPlayer.CtrlHeroGuid = player.ctrlHeroGuid;
    //         netBattleInitArg.BattlePlayerInitArg.PlayerList.Add(netPlayer);
    //     }
    //
    //     //实体信息
    //     netBattleInitArg.EntityInitArg = new NetProto.BattleEntityInitArg();
    //     var entities = battle.GetAllEntities();
    //     foreach (var keyV in entities)
    //     {
    //         var entity = keyV.Value;
    //
    //         var netEntity = new NetProto.BattleEntityProto();
    //         netEntity.Guid = entity.guid;
    //         netEntity.ConfigId = entity.configId;
    //         netEntity.PlayerIndex = entity.playerIndex;
    //
    //         netEntity.Position = BattleConvertUtil.ConvertToVector3Proto(entity.position);
    //
    //         netEntity.MaxHp = (int)entity.MaxHealth;
    //         netEntity.CurrHp = netEntity.MaxHp;
    //
    //         //技能
    //         var skills = entity.GetAllSkills();
    //         foreach (var skillKV in skills)
    //         {
    //             var skill = skillKV.Value;
    //
    //             NetProto.BattleSkillProto netSkill = new BattleSkillProto();
    //             netSkill.ConfigId = skill.configId;
    //             netSkill.Level = skill.level;
    //             netSkill.MaxCDTime = (int)(skill.GetCDMaxTime() * 1000);
    //             netEntity.SkillInitList.Add(netSkill);
    //         }
    //         netBattleInitArg.EntityInitArg.BattleEntityInitList.Add(netEntity);
    //     }
    //
    //     return netBattleInitArg;
    // }
    //
    //
    }
   
} 