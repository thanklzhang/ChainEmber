using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
using GameData;

public class HeroListNetHandler : NetHandler
{
    public override void Init()
    {

        AddListener((int)ProtoIDs.SyncHeroList, OnSyncHeroList);
        AddListener((int)ProtoIDs.UpgradeHeroLevel, OnUpgradeHeroLevel);

    }

    //同步英雄列表所有数据
    public void SendSyncHeroList()
    {
        csSyncHeroList syncHeroList = new csSyncHeroList()
        {

        };
        NetworkManager.Instance.SendMsg(ProtoIDs.SyncHeroList, syncHeroList.ToByteArray());

    }

    public void OnSyncHeroList(MsgPack msgPack)
    {
        scSyncHeroList sync = scSyncHeroList.Parser.ParseFrom(msgPack.data);

        var dataStore = GameDataManager.Instance.HeroData;
        dataStore.SetHeroDataList(sync.HeroList);

    }

    //英雄升级
    public void SendUpgradeHeroLevel(int guid, int level)
    {
        csUpgradeHeroLevel upgrade = new csUpgradeHeroLevel()
        {
            Guid = guid,
            Level = 1,
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.UpgradeHeroLevel, upgrade.ToByteArray());
    }

    public void OnUpgradeHeroLevel(MsgPack msgPack)
    {
        scUpgradeHeroLevel upgrade = scUpgradeHeroLevel.Parser.ParseFrom(msgPack.data);
        var err = upgrade.Err;
        if (0 == err)
        {
            // CtrlManager.Instance.GlobalCtrlPre.ShowTips("升级成功");
        }
        else
        {
            LogNetErrStr(msgPack.cmdId, err);
        }
    }

    ////更新英雄列表的信息 不覆盖
    //public void SendUpdateHeroList()
    //{

    //}

    //public void OnUpdateHeroList(MsgPack msgPack)
    //{
    //    scUpdateHeroLevel resp = scUpdateHeroLevel.Parser.ParseFrom(msgPack.data);
    //    var err = resp.Err;
    //    if (0 == err)
    //    {
    //        resp.
    //    }
    //    else
    //    {
    //        LogNetErrStr(msgPack.cmdId, err);
    //    }
    //}


}

