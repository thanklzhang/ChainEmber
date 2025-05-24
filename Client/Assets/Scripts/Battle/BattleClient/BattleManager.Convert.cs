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
        /// <summary>
        /// 根据战斗后台逻辑 得到 创建客户端战斗的战斗初始化参数
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        public Battle_Client.BattleClient_CreateBattleArgs GetBattleClientArgs(Battle.Battle battle)
        {
            var _battle = battle;
            Battle_Client.BattleClient_CreateBattleArgs battleClientArgs = new BattleClient_CreateBattleArgs();
            battleClientArgs.guid = _battle.guid;
            battleClientArgs.configId = _battle.battleConfigId;
            battleClientArgs.roomId = _battle.roomId;

            battleClientArgs.clientPlayers = new List<BattleClient_ClientPlayer>();

            var _battlePlayerList = _battle.GetAllPlayers();
            foreach (var _player in _battlePlayerList)
            {
                BattleClient_ClientPlayer player = new BattleClient_ClientPlayer();
                player.playerIndex = _player.playerIndex;
                player.team = _player.team;
                player.uid = _player.uid;
                player.ctrlHeroGuid = _player.ctrlHeroGuid;


                //宝箱商店
                player.boxShop = new BattleClientMsg_BoxShop();
                var shopItems = _player.boxShop.GetShopItems();
                player.boxShop.shopItems = new Dictionary<RewardQuality, BattleClientMsg_BoxShopItem>();

                foreach (var kv in shopItems)
                {
                    var quality = kv.Key;
                    var shopItem = kv.Value;

                    var _shopItem = new BattleClientMsg_BoxShopItem();
                    _shopItem.configId = shopItem.configId;
                    _shopItem.canBuyCount = shopItem.GetCanBuyCount();
                    _shopItem.maxBuyCount = shopItem.GetMaxBuyCount();
                    _shopItem.costItemId = shopItem.costItemId;
                    _shopItem.costCount = shopItem.costCount;
                    player.boxShop.shopItems.Add((RewardQuality)shopItem.config.Quality, _shopItem);
                }

                //玩家宝箱
                player.myBox = new BattleClientMsg_BattleMyBox();
                player.myBox.boxGroupDic = new
                    Dictionary<RewardQuality, BattleClientMsg_MyBoxQualityGroup>();

                foreach (var myBox in _player.myBoxDic)
                {
                    var _quality = myBox.Key;
                    var _boxList = myBox.Value;

                    var boxGroup = new BattleClientMsg_MyBoxQualityGroup();
                    boxGroup.quality = _quality;
                    boxGroup.count = _boxList.Count;

                    player.myBox.boxGroupDic.Add(boxGroup.quality, boxGroup);
                }

                //玩家货币
                player.currency = new BattleClient_Currency();
                player.currency.currencyDic = BattleConvert.ConvertTo(_player.currencyDic);

                battleClientArgs.clientPlayers.Add(player);
            }

            //地图尺寸
            battleClientArgs.mapSizeX = _battle.GetMapSizeX();
            battleClientArgs.mapSizeZ = _battle.GetMapSizeZ();


            battleClientArgs.entityList = new List<BattleClientMsg_Entity>();
            var entities = _battle.GetAllEntities();
            foreach (var keyV in entities)
            {
                var _entity = keyV.Value;

                var entity = new BattleClientMsg_Entity();
                entity.guid = _entity.guid;
                entity.configId = _entity.configId;
                entity.playerIndex = _entity.playerIndex;

                entity.position = new UnityEngine.Vector3(_entity.position.x, _entity.position.y, _entity.position.z);
                ;

                //netEntity.MaxHp = (int)entity.MaxHealth;
                //netEntity.CurrHp = netEntity.MaxHp;

                //技能
                var skills = _entity.GetAllSkills();
                entity.skills = new List<BattleClientMsg_Skill>();
                foreach (var skillKV in skills)
                {
                    var skill = skillKV.Value;

                    BattleClientMsg_Skill skillInfo = new BattleClientMsg_Skill();
                    skillInfo.configId = skill.configId;
                    // skillInfo.level = skill.level;
                    skillInfo.maxCDTime = skill.GetCDTotalTime();
                    entity.skills.Add(skillInfo);
                }

                battleClientArgs.entityList.Add(entity);
            }

            return battleClientArgs;
        }

        /// <summary>
        /// 根据客户端战斗的战斗初始化参数 创建战斗运行时数据
        /// </summary>
        /// <param name="battleInit"></param>
        public void CreateBattleData(BattleClient_CreateBattleArgs battleInit)
        {
            // Logx.Log("battle manager : CreateBattle");
            Logx.Log(LogxType.Game, "create battle");

            //战斗信息
            this.battleGuid = battleInit.guid;
            this.battleConfigId = battleInit.configId;
            this.battleRoomId = battleInit.roomId;

            //玩家信息
            var userDataStore = GameDataManager.Instance.UserData;
            var localPlayerUid = (int)userDataStore.Uid;
            playerDic = new Dictionary<int, ClientPlayer>();
            playerList = new List<ClientPlayer>();
            foreach (var serverPlayer in battleInit.clientPlayers)
            {
                ClientPlayer player = new ClientPlayer()
                {
                    playerIndex = serverPlayer.playerIndex,
                    team = serverPlayer.team,
                    uid = (int)serverPlayer.uid,
                    ctrlHeroGuid = serverPlayer.ctrlHeroGuid
                };

                player.Init();

                //设置本地玩家
                if (player.uid == localPlayerUid)
                {
                    this.localPlayer = player;

                    //本地玩家宝箱商店
                    Dictionary<RewardQuality, BoxShopItem> localBoxDic = new Dictionary<RewardQuality, BoxShopItem>();
                    if (serverPlayer.boxShop.shopItems != null)
                    {
                        foreach (var kv in serverPlayer.boxShop.shopItems)
                        {
                            var quality = kv.Key;
                            var shopItem = kv.Value;
                            var localBoxItem = new BoxShopItem();
                            localBoxItem.configId = shopItem.configId;
                            localBoxItem.costItemId = shopItem.costItemId;
                            localBoxItem.costCount = shopItem.costCount;
                            localBoxItem.canBuyCount = shopItem.canBuyCount;
                            localBoxItem.maxBuyCount = shopItem.maxBuyCount;

                            localBoxDic.Add((RewardQuality)quality, localBoxItem);
                        }
                    }

                    this.localPlayer.SetBoxShopItemsData(localBoxDic);

                    //玩家拥有的宝箱
                    var boxGroupDic = new Dictionary<RewardQuality, MyBoxGroup>();
                    var _boxGroupDic = serverPlayer.myBox.boxGroupDic;
                    foreach (var kv in _boxGroupDic)
                    {
                        var _quality = kv.Key;
                        var _group = kv.Value;

                        var boxGroup = new MyBoxGroup();
                        boxGroup.quality = _quality;
                        boxGroup.count = _group.count;

                        boxGroupDic.Add(boxGroup.quality, boxGroup);
                    }

                    this.localPlayer.SetMyBoxList(boxGroupDic);

                    //玩家战斗货币资源
                    if (serverPlayer.currency != null)
                    {
                        this.localPlayer.SetCurrencyData(
                            serverPlayer.currency.currencyDic);
                    }
                    
                    //玩家偏好
                    // player.SetPreference();
                }


                this.playerDic.Add(player.uid, player);
                this.playerList.Add(player);
            }

            if (null == localPlayer)
            {
                Logx.LogError("the uid of localPlayer is not found : " + localPlayerUid);
            }


            //实体信息
            // Logx.Log(LogxType.Battle,"battle manager : CreateInitEntity");
            foreach (var serverEntity in battleInit.entityList)
            {
                BattleEntityManager.Instance.CreateViewEntityInfo(serverEntity);
            }

            //设置本地玩家控制的英雄
            this.localCtrlEntity = BattleEntityManager.Instance.FindEntity(this.localPlayer.ctrlHeroGuid);
            if (null == this.localCtrlEntity)
            {
                Logx.LogError("the localCtrlEntity is not found : ctrlHeroGuid : " + this.localPlayer.ctrlHeroGuid);
            }
            
            localPlayer.SetPreference();

            BattleState = BattleState.Loading;

            this.OnEnterBattle();

            // //进入战斗状态
            // CtrlManager.Instance.Enter<BattleCtrlPre>();
        }
    }
}