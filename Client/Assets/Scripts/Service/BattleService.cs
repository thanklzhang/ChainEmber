using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Battle;
using Battle_Client;
using Config;
using GameData;
using Vector3 = System.Numerics.Vector3;

namespace ServerSimulation
{
    public enum FunctionIds
    {
        MainTask = 1,
        Team = 10,
    }
    
    //后台战斗 目前没有状态 只是模拟创建 真正的逻辑在前端
    public class BattleService : Singleton<BattleService>
    {
        private Battle.Battle battle;
        
        public BattleService()
        {
        }

        private BattleCreateArg createArg;
        public void ApplyCreateBattle(int currSelectHeroGuid)
        {
            CoroutineManager.Instance.StartCoroutine(_ApplyCreateBattle(currSelectHeroGuid));
        }

        //申请战斗
        IEnumerator _ApplyCreateBattle(int currSelectHeroGuid)
        {
            //生成战斗所需的战斗创建参数
            yield return GenBattleCreateArg(currSelectHeroGuid);
            
            //根据 创建参数 创建战斗后台逻辑
            var battle = CreateBattle(createArg);
            
            //根据 战斗后台逻辑 创建客户端创建战斗参数
            var clientBattleCreateArg = BattleManager.Instance.GetBattleClientArgs(battle);
            
            //根据 客户端创建战斗参数 客户端开始战斗
            BattleManager.Instance.StartSimulateRemoteBattle(clientBattleCreateArg);
        }

        public IEnumerator GenBattleCreateArg(int currSelectHeroGuid)
        {
            BattleCreateArg createArg = new BattleCreateArg();
            
            createArg.roomId = 1;
            //测试战斗关卡
            createArg.configId = 5900010;
            createArg.funcId = (int)FunctionIds.MainTask;
            
            //玩家初始化数据
            createArg.battlePlayerInitArg = new BattlePlayerInitArg();
            createArg.battlePlayerInitArg.battlePlayerInitList = new List<BattlePlayerInit>();
            var playerInitList = createArg.battlePlayerInitArg.battlePlayerInitList;
            playerInitList.Add(new BattlePlayerInit()
            {
                playerIndex = 0,
                uid = 1,
                team = 0,
                isPlayerCtrl = true,
            });
            
            //地图初始化
            yield return BattleManager.Instance.LoadMapData(createArg.configId, (map) =>
            {
                BattleManager.Instance.mapSaveData = map;
            });
            
            createArg.mapInitArg = new MapInitArg();
            var mapInit = createArg.mapInitArg;
            var mapSaveData = BattleManager.Instance.mapSaveData;
            mapInit.mapList = mapSaveData.mapList;
            mapInit.enemyInitPosList = VectorConvert.ToVector3(mapSaveData.enemyInitPosList);
            mapInit.playerInitPosList = VectorConvert.ToVector3(mapSaveData.playerInitPosList);
            
            //实体初始数据
            var heroData = HeroService.Instance.GetHero(currSelectHeroGuid);
            if (null == heroData)
            {
                Logx.Log(LogxType.Game, $"hero is null currSelectHeroGuid = {currSelectHeroGuid}");
                yield break;
            }

            createArg.entityInitArg = new BattleEntityInitArg();
            createArg.entityInitArg.entityInitList = new List<EntityInit>();
            var  entityInitList = createArg.entityInitArg.entityInitList;
            var entityInit = new EntityInit()
            {
                configId = heroData.configId,
                level = heroData.level,
                playerIndex = 0,
                position = mapInit.playerInitPosList[0],
                star = heroData.star,
                isPlayerCtrl = true,
                roleType = BattleEntityRoleType.Normal
            };
            
            //实体的技能
            SetEntitySkills(entityInit);
         
            
            entityInitList.Add(entityInit);

            this.createArg = createArg;
        }

        public void SetEntitySkills(EntityInit entityInit)
        {
            var entityConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(entityInit.configId);
            entityInit.skillInitList = new List<SkillInit>();

            foreach (var skillConfigId in entityConfig.SkillIds)
            {
                entityInit.skillInitList.Add(new SkillInit()
                {
                    configId = skillConfigId,
                    level = 1
                });
            }
            if (entityConfig.UltimateSkillId > 0)
            {
                entityInit.skillInitList.Add(new SkillInit()
                {
                    configId = entityConfig.UltimateSkillId,
                    level = 1
                });
            }
          
        }

        public Battle.Battle CreateBattle(BattleCreateArg createArg )
        {
            //初始化本地战斗后台逻辑
            var battle = new Battle.Battle();
            int battleGuid = 1;
            battle.TimeDelta = Time.fixedDeltaTime;
            BattleLog.RegisterLog(new BattleLog_Impl());
            battle.PlayerMsgSender = new LocalBattleLogic_MsgSender();
            //加载战斗数据配置
            var battleConfigManager = BattleConfigManager.Instance;
            if (!battleConfigManager.IsInitFinish)
            {
                battleConfigManager.LoadBattleConfig(new BattleConfig_Impl());
            }
            battle.Init(battleGuid, createArg);
            
            //填充客户端所需组件
            BattleManager.Instance.InitLocalExecute(battle);
            
            return battle;
        }
    }
}