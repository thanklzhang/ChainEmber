using GameData;
using NetProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GameData
{


    public class BattleGameData : BaseGameData
    {
        //public void SetBattleInitData()
        //{

        //}

        ////-----------------------------------

        ////是否所有人都加载好了
        //private bool isAllPlayerLoadFinish = false;
        //public bool IsAllPlayerLoadFinish { get => isAllPlayerLoadFinish; set => isAllPlayerLoadFinish = value; }

        ////战斗的一些相关信息
        //public BattleInfo battleInfo;

        ////本地玩家
        //private ClientPlayer localPlayer;
        //public ClientPlayer LocalPlayer
        //{
        //    get
        //    {
        //        var userDataStore = GameDataManager.Instance.UserGameDataStore;
        //        var uid = (int)userDataStore.Uid;
        //        if (battleInfo.players.ContainsKey(uid))
        //        {
        //            return battleInfo.players[uid];
        //        }
        //        else
        //        {
        //            Logx.LogError("the uid is not found : " + uid);
        //            return null;
        //        }

        //    }
        //}
        ////本地玩家控制的英雄
        //BattleEntityInfo localCtrlEntity;
        //public BattleEntityInfo LocalCtrlEntity
        //{
        //    get
        //    {
        //        var entity = battleInfo.FindEntityById(LocalPlayer.ctrlHeroGuid);
        //        return entity;
        //    }
        //}

        //public void SetBattleInitData(int guid, int configId, int roomId)
        //{
        //    BattleInfo battleInfo = new BattleInfo();

        //    battleInfo.guid = guid;
        //    battleInfo.configId = configId;
        //    battleInfo.roomId = roomId;

        //    Logx.Log("SetBattleInitData");
        //    //battleInfo = ConvertBattleInfo(battleInitArg);

        //}

        //public BattleInfo ConvertBattleInfo(BattleInitArg arg)
        //{
        //    BattleInfo battleInfo = new BattleInfo();
        //    battleInfo.guid = arg.Guid;
        //    battleInfo.configId = arg.ConfigId;
        //    battleInfo.roomId = arg.RoomId;

        //    //玩家信息
        //    battleInfo.players = new Dictionary<int, ClientPlayer>();
        //    foreach (var serverPlayer in arg.BattlePlayerInitArg.PlayerList)
        //    {
        //        ClientPlayer player = new ClientPlayer()
        //        {
        //            //ctrlHeroGuid = serverPlayer.CtrlHeroGuid,
        //            playerIndex = serverPlayer.PlayerIndex,
        //            team = serverPlayer.Team,
        //            uid = serverPlayer.Uid,
        //            ctrlHeroGuid = serverPlayer.CtrlHeroGuid
        //        };

        //        battleInfo.players.Add(player.uid, player);
        //    }

        //    //实体信息
        //    battleInfo.entities = new Dictionary<int, BattleEntityInfo>();
        //    foreach (var serverEntity in arg.EntityInitArg.BattleEntityInitList)
        //    {
        //        BattleEntityInfo entity = new BattleEntityInfo();
        //        entity.guid = serverEntity.Guid;
        //        entity.configId = serverEntity.ConfigId;
        //        entity.playerIndex = serverEntity.PlayerIndex;
        //        entity.position = BattleConvert.ConverToVector3(serverEntity.Position);
        //        entity.skillInfos = new List<BattleSkillInfo>();
        //        foreach (var serverSkill in serverEntity.SkillInitList)
        //        {
        //            BattleSkillInfo skill = new BattleSkillInfo();
        //            skill.configId = serverSkill.ConfigId;
        //            skill.level = serverSkill.Level;
        //            entity.skillInfos.Add(skill);
        //        }


        //        battleInfo.entities.Add(entity.guid, entity);
        //    }

        //    return battleInfo;
        //}

        //public void OnCreateEntity(BattleEntityInfo entity)
        //{
        //    if (battleInfo.AddEntity(entity))
        //    {
        //        EventDispatcher.Broadcast(EventIDs.OnCreateEntity, entity);
        //    }

        //    //EventDispatcher.Broadcast(EventIDs.OnCreateBattle, entity);
        //}
    }

    //public class BattleInfo
    //{
    //    //guid
    //    public int guid;

    //    //表 id 
    //    public int configId;

    //    public Dictionary<int, ClientPlayer> players;

    //    public Dictionary<int, BattleEntityInfo> entities;

    //    public int roomId;

    //    public BattleEntityInfo FindEntityById(int guid)
    //    {
    //        if (entities.ContainsKey(guid))
    //        {
    //            return entities[guid];
    //        }
    //        else
    //        {
    //            Logx.LogWarning("BattleEntityInfo FindEntityById : the guid is not found : " + guid);
    //            return null;
    //        }
    //    }

    //    public bool AddEntity(BattleEntityInfo entityInfo)
    //    {
    //        var guid = entityInfo.guid;
    //        if (entities.ContainsKey(guid))
    //        {
    //            Logx.LogWarning("battle : AddEntity : the guid is exist : " + guid);
    //            return false;
    //        }

    //        entities.Add(entityInfo.guid, entityInfo);
    //        return true;
    //    }
    //}

    ////public class ClientPlayer
    ////{
    ////    public int playerIndex;
    ////    public int team;
    ////    public int uid;

    ////    public int ctrlHeroGuid;

    ////}

    //public class BattleEntityInfo
    //{
    //    public int guid;
    //    public int configId;
    //    public int playerIndex;

    //    public Vector3 position;

    //    //public int level;
    //    //public float maxHp;
    //    //public float currHp;

    //    public List<BattleSkillInfo> skillInfos;

    //}

    //public class BattleSkillInfo
    //{
    //    public int configId;
    //    public int level;
    //}
}