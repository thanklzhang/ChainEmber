using Battle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameData;
using UnityEngine;

namespace Battle_Client
{
    public class LocalBattleLogic_Executer
    {
        Battle.Battle battle;

        public void Init()
        {
        }

        bool isPureLocal;

        //创建本地战斗
        public Battle.Battle CreateLocalBattleLogic(NetProto.ApplyBattleArg applyArg, 
            MapInitArg mapInitData, bool isPureLocal)
        {
            this.isPureLocal = isPureLocal;

            Logx.Log(LogxType.Battle, "local execute : CreateLocalBattleLogic");

            //根据申请战斗参数 创建后台战斗初始化参数
            var logicArgs = GetBattleLogicArgs(applyArg, mapInitData);

            battle = new Battle.Battle();
            int battleGuid = 0;
            battle.TimeDelta = Time.fixedDeltaTime;

            BattleLog.RegisterLog(new BattleLog_Impl());

            battle.PlayerMsgSender = new LocalBattleLogic_MsgSender();
            //battle.PlayerMsgReceiver = new LocalBattleLogic_MsgReceiver(battle);
            //battle.TriggerReader = new TriggerReader_Impl(battle);
            // BattleConfigManager.Instance = new ConfigManager_Proxy();

            //加载战斗数据配置
            var battleConfigManager = BattleConfigManager.Instance;
            if (!battleConfigManager.IsInitFinish)
            {
                battleConfigManager.LoadBattleConfig(new BattleConfig_Impl());
            }

           

            battle.Init(battleGuid, logicArgs);
            //加载后台战斗
            //battle.Load(logicArgs);

            currTargetLogicTime = battle.TimeDelta;

            return battle;
        }

        //public IEnumerator LoadTriggerResource(int battleConfigId, Action<TriggerSourceData> finishCallback)
        //{
        //    Logx.Log("local execute : LoadTriggerResource ");

        //    TriggerSourceData source = new TriggerSourceData();
        //    source.dataStrList = new List<string>();

        //    var battleConfigTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
        //    var triggerTb = Config.ConfigManager.Instance.GetById<Config.BattleTrigger>(battleConfigTb.TriggerId);

        //    var loadPath = Const.AssetBundlePath + "/" + Const.buildPath + "/" + triggerTb.ScriptPath;
        //    var files = System.IO.Directory.GetFiles(loadPath, "*.ab", System.IO.SearchOption.AllDirectories);

        //    foreach (var filePath in files)
        //    {
        //        bool isLoadFinish = false;
        //        string loadText = "";
        //        //Logx.Log("local execute : start load : filePath :  " + filePath);
        //        var partPath = filePath.Replace(Const.AssetBundlePath + "/","").Replace(".ab",".json").Replace("\\","/");
        //        ResourceManager.Instance.GetObject<TextAsset>(partPath, (textAsset) =>
        //        {
        //            Logx.Log("local execute : load text finish: " + textAsset.text);
        //            loadText = textAsset.text;
        //            isLoadFinish = true;
        //        });

        //        while (true)
        //        {
        //            yield return null;

        //            if (isLoadFinish)
        //            {
        //                source.dataStrList.Add(loadText);
        //                break;
        //            }
        //        }
        //    }

        //    Logx.Log("local execute : finish all ");
        //    finishCallback?.Invoke(source);
        //}

        public void OnEnterBattle()
        {
            battle.OnBattleEnd += OnBattleLogicEnd;
        }

        public void OnBattleLogicEnd(Battle.Battle battle, int teamIndex)//, BattleEndType endType
        {
            //本地战斗结算是在 center server
            var arg = BattleEndUtil.MakeApplyBattleArgProto(battle, teamIndex);//, endType


            //判断是否是服务端结算
            if (!isPureLocal)
            {
                //本地战斗 在服务端结算
                var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
                //battleNet.SendApplyBattleEnd(arg);
            }
            else
            {
                //纯本地战斗
                Logx.Log(LogxType.Battle, "pure battle : battle result");

                BattleResultDataArgs resultData = new BattleResultDataArgs();
                resultData.rewardDataList = new List<ItemData>();

                var isWin = teamIndex == BattleManager.Instance.GetLocalPlayer().team;
                resultData.isWin = isWin;
                
                //test 这里先测试 ， 之后按照需求给
                ItemData data1 = new ItemData()
                {
                    configId = 22000001,
                    count = 1200
                };
                
                resultData.rewardDataList.Add(data1);
                BattleManager.Instance.BattleEnd(resultData);
            }
        }

        public void OnExitBattle()
        {
            battle.OnBattleEnd -= OnBattleLogicEnd;
            isPureLocal = false;
        }

        public Map GetMap()
        {
            return this.battle.GetMap();
        }

        public void StartBattleLogic()
        {
            //battle.Start();
        }

        float currTimer;
        int currBattleClientFrame;


        float currLogicTimer;
        float currTargetLogicTime;

        public void Update(float timeDelta)
        {
            ////Logx.Log("battleClientTimeDelta : " + timeDelta);
            ////Logx.Log("currTimer : " + currTimer);

            //if (battle.BattleState == Battle.BattleState.Battling)
            //{
            //    //var currBattleFrameNum = GameMain.Instance.currBattleFrameNum;
            //    //Logx.Log("moveTest : currFrame : " + currBattleFrameNum);
            //    //Logx.Log("moveTest : BattleClient");


            //    //currTimer = currTimer + timeDelta;

            //    //if (currTimer >= currTargetLogicTime)
            //    //{
            //    //    //Logx.Log("moveTest : BattleLogic");
            //    //    battle.Update();
            //    //    var fixTime = currTimer - currTargetLogicTime;
            //    //    currTargetLogicTime = battle.timeDelta - fixTime;
            //    //    currTimer = 0;
            //    //}
            //}
        }

        public void FixedUpdate(float fixedTime)
        {
            battle?.Update();
        }

        internal Battle.Battle GetBattle()
        {
            return this.battle;
        }

        //根据申请战斗参数 获得 后台战斗初始化参数
        public Battle.BattleCreateArg GetBattleLogicArgs(NetProto.ApplyBattleArg applyArg,
             MapInitArg mapInitData)
        {
            var battleArg = ApplyBattleUtil.ToBattleArg(applyArg,  mapInitData);
            return battleArg;
        }
    }
}