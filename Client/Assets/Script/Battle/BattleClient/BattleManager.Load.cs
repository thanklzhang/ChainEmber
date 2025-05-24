using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using GameData;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    //战斗中的加载创建相关
    public partial class BattleManager
    {
        public IEnumerator StartLoad()
        {
            if (this.battleType == BattleType.Remote)
            {
                yield return StartLoad_Remote();
            }
            else if (this.battleType == BattleType.PureLocal)
            {
                yield return StartLoad_PureLocal();
            }

            OnLoadFinish();
        }

        /// <summary>
        /// 远端战斗加载
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_Remote()
        {
            //填充客户端所需组件
            MsgSender = new BattleClient_MsgSender_Remote();
            // msgReceiver = new BattleClient_MsgReceiver_Impl();

            //创建战斗数据
            CreateBattleData(battleClientArgs);

            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }

        private MapSaveData mapSaveData;

        /// <summary>
        /// 纯本地战斗加载
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_PureLocal()
        {
            Logx.Log(LogxType.Game, "StartLoad_PureLocal : start laod");

            GameDataManager.Instance.UserData.Uid = 1;
            var uid = GameDataManager.Instance.UserData.Uid;

            //地图数据由本地加载
            EventSender.SendLoadingProgress(0.0f, "开始加载地图数据");
            mapSaveData = null;
            yield return LoadMapData(battleConfigId, (map) => { mapSaveData = map; });

            EventSender.SendLoadingProgress(0.2f, "开始加载本地战斗所需数据");

            Logx.Log(LogxType.Game, "StartLoad_PureLocal : load map config finish");

            //获得申请战斗参数
            var applyArg = ApplyBattleUtil.MakePureLocalApplyBattleArg(battleConfigId, (int)uid,mapSaveData);

            MapInitArg mapInitData = new MapInitArg();
            mapInitData.mapList = mapSaveData.mapList;
            mapInitData.enemyInitPosList = VectorConvert.ToVector3(mapSaveData.enemyInitPosList);
            mapInitData.playerInitPosList = VectorConvert.ToVector3(mapSaveData.playerInitPosList);

            //设置本地战斗逻辑
            SetLocalBattleLogic(applyArg, mapInitData, true);

            //创建本地战斗数据
            CreateBattleData(battleClientArgs);

            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }


        //加载地图
        public IEnumerator LoadMapData(int battleConfigId, Action<MapSaveData> finishCallback)
        {
            var battleConfigTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
            var mapConfig = Config.ConfigManager.Instance.GetById<Config.BattleMap>(battleConfigTb.MapId);

            var isFinish = false;
            // var mapList = new List<List<int>>();
            var mapSaveData = new MapSaveData();
            var path = GlobalConfig.buildPath + "/" + mapConfig.MapDataPath;
            ResourceManager.Instance.GetObject<TextAsset>(path, (textAsset) =>
            {
                //Logx.Log("local execute : load text finish: " + textAsset.text);
                var json = textAsset.text;
                // mapList = LitJson.JsonMapper.ToObject<List<List<int>>>(json);
                mapSaveData = LitJson.JsonMapper.ToObject<MapSaveData>(json);
                isFinish = true;
            });

            while (!isFinish)
            {
                yield return null;
            }

            // finishCallback?.Invoke(mapList);
            finishCallback?.Invoke(mapSaveData);
        }

        public IEnumerator StartLoad_Common()
        {
            Logx.Log(LogxType.Game, "StartLoad_Common : load start");


            //读取战斗相关配置数据
            var battleTableId = BattleManager.Instance.battleConfigId;
            var battleTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleTableId);
            var mapConfig = Config.ConfigManager.Instance.GetById<Config.BattleMap>(battleTb.MapId);
            // var battleTriggerTb = Config.ConfigManager.Instance.GetById<Config.BattleTrigger>(battleTb.TriggerId);
            var sceneResTb = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(mapConfig.ResId);


            //加载场景
            EventSender.SendLoadingProgress(0.3f, "加载 场景 中");
            yield return SceneLoadManager.Instance.LoadRequest(sceneResTb.Name);
            SetSceneInfo();
            SetCameraInfo();
            
            Logx.Log(LogxType.Game, "StartLoad_Common : scene load finish");

            //加载 UI 并打开
            EventSender.SendLoadingProgress(0.4f, "加载 战斗界面 中");
            yield return UIManager.Instance.EnterRequest<BattleUI>();
            Logx.Log(LogxType.Game, "StartLoad_Common : BattleUICtrl load finish");
            //battle ui
            // objsRequestList.Add(new LoadUIRequest<BattleUIPre>() { selfFinishCallback = OnUILoadFinish });

            //战斗实体资源
            EventSender.SendLoadingProgress(0.5f, "加载 战斗实体 中");
            yield return BattleEntityManager.Instance.LoadInitEntities();
            Logx.Log(LogxType.Game, "StartLoad_Common : entities load finish");

            // ShowMapPosView();
        }

        void SetSceneInfo()
        {
            sceneRoot = GameObject.Find("_scene_root").transform;
        }

        //显示辅助地图坐标
        void ShowMapPosView()
        {
            var temp = Resources.Load("GridPos") as GameObject;
            var list = mapSaveData.mapList;
            for (int i = 0; i < list.Count; i++)
            {
                var row = i;
                var l = list[i];
                for (int j = 0; j < l.Count; j++)
                {
                    var line = j;
                    var ins = GameObject.Instantiate(temp, null, false);
                    ins.transform.position = new UnityEngine.Vector3(row, 0, line);
                    ins.GetComponent<TextMesh>().text = "(" + row + "," + line + ")";
                }
            }
        }

        public void OnLoadFinish()
        {
            this.playerInput.InitPlayerInput();
            // var understudyRoot = sceneRoot.Find("UnderstudyArea");
            // UnderstudyManager_Client.Instance.Init(understudyRoot);
        }

        #region 创建战斗

        //创建远端战斗
        public void CreateRemoteBattle(BattleClient_CreateBattleArgs battleClientArgs)
        {
            battleType = BattleType.Remote;

            this.battleClientArgs = battleClientArgs;

            Logx.Log(LogxType.Game, "start create a remote battle");

            //进入战斗场景
            SceneCtrlManager.Instance.Enter<BattleSceneCtrl>();
        }

        //创建纯本地战斗
        public void CreatePureLocalBattle(int battleConfigId)
        {
            this.battleConfigId = battleConfigId;

            battleType = BattleType.PureLocal;

            //进入战斗场景
            SceneCtrlManager.Instance.Enter<BattleSceneCtrl>();
        }

        //创建远端结算的本地战斗
        public void CreateLocalButRemoteResultBattle(ApplyBattleArg applyArg)
        {
            //TODO 进入战斗状态机 从而进行加载
            battleType = BattleType.LocalButRemoteResult;
            Logx.Log(LogxType.Game, "start create a local battle (result at remote)");

            var battleConfigId = applyArg.BattleTableId;
            // CoroutineManager.Instance.StartCoroutine(LoadTriggerResource(battleConfigId, (sourceData) =>
            // {
            //     //CreateLocalBattle(applyArg, sourceData, false);
            // }));
        }

        #endregion

        //设置本地战斗逻辑 供运行本地战斗使用
        public void SetLocalBattleLogic(NetProto.ApplyBattleArg applyArg,
            MapInitArg mapInitData, bool isPureLocal)
        {
            Logx.Log(LogxType.Game, "start create a local battle");
            //填充数据

            //初始化本地战斗后台逻辑
            localBattleExecuter = new LocalBattleLogic_Executer();
            localBattleExecuter.Init();

            //根据申请战斗参数 获得后台战斗逻辑
            var battleLogic = localBattleExecuter.CreateLocalBattleLogic(applyArg, mapInitData, isPureLocal);

            //通过后台战斗逻辑 获得客户端战斗初始化参数（供客户端初始化，如地图加载 模型加载等）
            battleClientArgs = GetBattleClientArgs(battleLogic);

            //填充客户端所需组件
            MsgSender = new BattleClient_MsgSender_Local(battleLogic);
        }

    }
}