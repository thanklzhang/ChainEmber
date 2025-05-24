using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Battle;
using Google.Protobuf.Collections;
// 
using System.IO;

//申请战斗所用的工具类
public class ApplyBattleUtil
{
    //申请战斗通用参数 转化为 后台战斗逻辑所需战斗初始化参数(前后端通用)
    public static Battle.BattleCreateArg ToBattleArg(NetProto.ApplyBattleArg args, MapInitArg mapInitData)
    {
        var battleRoomId = args.BattleRoomId;
        var battleConfigId = args.BattleTableId;
        var funcId = args.FunctionId;

        Battle.BattleEntityInitArg entityInitArg = new Battle.BattleEntityInitArg();
        entityInitArg.entityInitList = new List<Battle.EntityInit>();
        Battle.BattlePlayerInitArg playerInitArg = new Battle.BattlePlayerInitArg();
        playerInitArg.battlePlayerInitList = new List<Battle.BattlePlayerInit>();

        var battleId = args.BattleTableId;
        var battleTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleId);
        // var playerPosStr = battleTb.InitPos;

        foreach (var playerInfo in args.PlayerInfoList)
        {
            //填充玩家信息
            Battle.BattlePlayerInit battlePlayerInit = new Battle.BattlePlayerInit();
            battlePlayerInit.uid = playerInfo.Uid;
            battlePlayerInit.playerIndex = playerInfo.PlayerIndex;
            battlePlayerInit.team = playerInfo.Team;
            battlePlayerInit.isPlayerCtrl = playerInfo.EntityInitInfo.IsPlayerCtrl;
            playerInitArg.battlePlayerInitList.Add(battlePlayerInit);

            //填充控制的实体信息 (目前是按照 一个人只有一个英雄实体)
            var srcEntity = playerInfo.EntityInitInfo;
            Battle.EntityInit desEntityInit = new Battle.EntityInit();

            //注意这里 要保证玩家索引从 0 开始并且连续

            if (playerInfo.PlayerIndex >= playerInitArg.battlePlayerInitList.Count)
            {
                BattleLog.LogWarningZxy($"playerIndex is error : {playerInfo.PlayerIndex}");
                continue;
            }

            var initPos = mapInitData.playerInitPosList[playerInfo.PlayerIndex];

            // var posStr = playerPosStr[playerInfo.PlayerIndex];
            // float configX = posStr[0];
            // float configY = posStr[1];
            // float configZ = posStr[2];
            desEntityInit.position = initPos; //new Battle.Vector3(configX, configY, configZ);
            desEntityInit.configId = srcEntity.ConfigId;
            desEntityInit.playerIndex = srcEntity.PlayerIndex;
            desEntityInit.isPlayerCtrl = srcEntity.IsPlayerCtrl;

            if (!srcEntity.IsHeroUseConfig)
            {
                //玩家拥有的英雄实体数据
                desEntityInit.level = srcEntity.Level;

                desEntityInit.skillInitList = new List<Battle.SkillInit>();

                //填充控制实体的技能信息
                foreach (var skillItem in srcEntity.SkillList)
                {
                    Battle.SkillInit skillInit = new Battle.SkillInit();
                    skillInit.configId = skillItem.ConfigId;
                    skillInit.level = skillItem.Level;
                    desEntityInit.skillInitList.Add(skillInit);
                }
            }
            else
            {
                //从表里读取英雄配置(TODO:之后会用 EntityInstance 配置代替)
                var entityTb = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(srcEntity.ConfigId);
                // desEntityInit.level = entityTb.Level;
                desEntityInit.level = 1;

                //配置英雄技能
                //普通攻击和技小招
                var skillIds = entityTb.SkillIds;
                //TODO:之后会用 EntityInstance 配置代替
                // var skillLvs = entityTb.SkillLevels;
                var skillLvs = new List<int>();
                for (int i = 0; i < skillIds.Count; i++)
                {
                    skillLvs.Add(1);
                }

                //大招
                if (entityTb.UltimateSkillId > 0)
                {
                    skillIds.Add(entityTb.UltimateSkillId);
                    skillLvs.Add(1);
                }


                for (int i = 0; i < skillIds.Count; i++)
                {
                    var skillId = skillIds[i];
                    var skillLv = 1;
                    if (i < skillLvs.Count)
                    {
                        skillLv = skillLvs[i];
                    }

                    Battle.SkillInit skillInit = new Battle.SkillInit()
                    {
                        configId = skillId,
                        level = skillLv
                    };
                    desEntityInit.skillInitList.Add(skillInit);
                }
            }

            entityInitArg.entityInitList.Add(desEntityInit);
        }

        ////资源
        ////trigger
        //TriggerSourceData sourceData = new TriggerSourceData();

        //var triggerId = battleTb.TriggerId;
        //Config.BattleTrigger striggerTb = Config.ConfigManager.Instance.GetById<Config.BattleTrigger>(triggerId);
        //var jsonFileRootPath = striggerTb.ScriptPath;

        ////这里文件应该是一开始都加载好 这里先这么读取
        //string Battle_Config_Path = "Resource/Battle";
        //string triggerPath = Battle_Config_Path + "/" + jsonFileRootPath;
        //string[] files = System.IO.Directory.GetFiles(triggerPath, "*.json", SearchOption.AllDirectories);

        //List<string> triggerJsonStrs = new List<string>();
        //files.ToList().ForEach(file =>
        //{
        //    //string jsonStr = battle.FileReader.GetTextFromFile(file);
        //    string jsonStr = FileOperate.GetTextFromFile(file);
        //    triggerJsonStrs.Add(jsonStr);
        //});

        //sourceData.dataStrList = triggerJsonStrs;

        //----

        BattleCreateArg battleArg = new BattleCreateArg();
        battleArg.roomId = battleRoomId;
        //地图数据
        battleArg.mapInitArg = mapInitData;
        battleArg.entityInitArg = entityInitArg;
        battleArg.battlePlayerInitArg = playerInitArg;
        battleArg.configId = battleConfigId;
        battleArg.funcId = funcId;
        // battleArg.triggerSrcData = sourceData;

        return battleArg;
    }

    //根据 战斗配置和玩家索引 得到相应的控制实体配置 id
    public static int GetEntityConfigId(int battleConfigId, int playerIndex)
    {
        //效率低下 待优化 之后可以将玩家索引对应的配置存起来快速读取
        var battleTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);

        var useHeroConfigId = 0;
        //if (!string.IsNullOrEmpty(battleTb.ForceUseHeroList))
        {
            var forceUseHeroListStr = battleTb.ForceUseHeroList;
            for (int i = 0; i < forceUseHeroListStr.Count; i++)
            {
                var str = forceUseHeroListStr[i];
                var group = str;
                int pIndex = group[0];
                int configId = group[1];

                if (pIndex == playerIndex)
                {
                    //找到该玩家 强制使用该英雄配置而不是拥有的英雄数据
                    useHeroConfigId = configId;
                    break;
                }
            }
        }

        return useHeroConfigId;
    }

    //获得申请战斗参数 （给客户端纯本地战斗使用）
    public static ApplyBattleArg MakePureLocalApplyBattleArg(int battleConfigId,
        int uid, MapSaveData mapSaveData)
    {
        var applyBattleArg = new ApplyBattleArg();
        applyBattleArg.BattleRoomId = 0;

        applyBattleArg.BattleTableId = battleConfigId;
        applyBattleArg.FunctionId = -1;

        var battleTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);

        var forceUseHeroList = battleTb.ForceUseHeroList;
        if (null == forceUseHeroList)
        {
            forceUseHeroList = new List<List<int>>();
            forceUseHeroList.Add(new List<int>()
                { 0, 0 });
        }

        var initPosList = mapSaveData.playerInitPosList;
        var initPosStrs = initPosList;
        for (int i = 0; i < initPosStrs.Count; i++)
        {
            if (i < forceUseHeroList.Count)
            {
                var paramList = forceUseHeroList[i];
                // if ("0" == paramList)
                // {  //由 配表简洁 到 程序通用
                //     paramList = "0,0";
                // }
                var group = paramList;
                int pIndex = group[0];
                int configId = group[1];

                NetProto.PlayerInitInfo playerInfo = new NetProto.PlayerInitInfo();
                //目前都是一个队伍
                playerInfo.Team = 0;
                playerInfo.PlayerIndex = pIndex;

                playerInfo.EntityInitInfo = new EntityInitInfo();

                var entityInfo = playerInfo.EntityInitInfo;
                entityInfo.PlayerIndex = pIndex;
                if (0 == configId)
                {
                    //理论上不会走这里 因为这里是纯本地战斗 没有玩家英雄数据-----
                    //------------------------------
                    //玩家使用自己的英雄 
                    playerInfo.Level = 1;
                    playerInfo.Uid = 0; //传进来的玩家 id

                    //实体
                    entityInfo.ConfigId = configId; // GetEntityConfigId(battleConfigId, pIndex);
                    entityInfo.IsPlayerCtrl = true;
                    entityInfo.IsHeroUseConfig = true;
                }
                else
                {
                    //玩家强迫使用配置英雄 
                    playerInfo.Level = 1;
                    playerInfo.Uid = 1000 + i;
                    playerInfo.PlayerIndex = pIndex;

                    //实体
                    entityInfo.ConfigId = configId;
                    //entityInfo.ConfigId = GetEntityConfigId(battleConfigId, pIndex);
                    entityInfo.IsPlayerCtrl = false;
                    entityInfo.IsHeroUseConfig = true;
                }

                if (0 == pIndex)
                {
                    //单机自己玩家索引固定为 0
                    playerInfo.Uid = uid;
                    entityInfo.IsPlayerCtrl = true;
                }

                applyBattleArg.PlayerInfoList.Add(playerInfo);
            }
        }

        return applyBattleArg;
    }
}