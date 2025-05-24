using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
using GameData;

public class BagNetHandler : NetHandler
{
    public override void Init()
    {
        AddListener((int)ProtoIDs.SyncBag, OnSyncBag);
    }

    //同步英雄列表所有数据
    public void SendSyncBag()
    {
        csSyncBag syncBag = new csSyncBag()
        {

        };
        NetworkManager.Instance.SendMsg(ProtoIDs.SyncBag, syncBag.ToByteArray());

    }

    public void OnSyncBag(MsgPack msgPack)
    {
        scSyncBag sync = scSyncBag.Parser.ParseFrom(msgPack.data);

        var dataStore = GameDataManager.Instance.BagData;
        dataStore.SetBagItemList(sync.Items);

    }


}

