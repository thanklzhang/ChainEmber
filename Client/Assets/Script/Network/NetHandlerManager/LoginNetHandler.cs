using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
using GameData;

public class LoginNetHandler : NetHandler
{
    public Action<scCheckLogin> loginResultAction;
    public Action<scEnterGame> enterGameResultAction;
    public Action<scRegistAccount> registAccountAction;
    public override void Init()
    {

        AddListener((int)ProtoIDs.CheckLogin, OnCheckLogin);
        AddListener((int)ProtoIDs.EnterGame, OnEnterGame);
        AddListener((int)ProtoIDs.RegistAccount, OnRegistAccount);

    }

    //check login

    public void SendCheckLogin(string account, string password, Action<scCheckLogin> action)
    {
        loginResultAction = action;
        csCheckLogin login = new csCheckLogin()
        {
            Account = account,
            Password = password
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.CheckLogin, login.ToByteArray());

    }

    public void OnCheckLogin(MsgPack msgPack)
    {
        scCheckLogin check = scCheckLogin.Parser.ParseFrom(msgPack.data);
        GameDataManager.Instance.UserData.Uid = (ulong)check.Uid;
        loginResultAction?.Invoke(check);
        loginResultAction = null;

    }

    //regist account
    public void SendRegistAccount(string account, string password, Action<scRegistAccount> action)
    {
        registAccountAction = action;
        csRegistAccount regist = new csRegistAccount()
        {
            Account = account,
            Password = password
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.RegistAccount, regist.ToByteArray());

    }

    public void OnRegistAccount(MsgPack msgPack)
    {
        scRegistAccount regist = scRegistAccount.Parser.ParseFrom(msgPack.data);
        registAccountAction?.Invoke(regist);
        registAccountAction = null;

    }

    //enter game

    public void SendEnterGame(int uid, string token, Action<scEnterGame> action)
    {
        csEnterGame enter = new csEnterGame()
        {
            Uid = uid,
            Token = token
        };
        enterGameResultAction = action;
        NetworkManager.Instance.SendMsg(ProtoIDs.EnterGame, enter.ToByteArray());
    }

    public void OnEnterGame(MsgPack msgPack)
    {
        scEnterGame enterGame = scEnterGame.Parser.ParseFrom(msgPack.data);

        var userDataStore = GameDataManager.Instance.UserData;
        userDataStore.PlayerInfo = PlayerConvert.ToPlayerInfo(enterGame.PlayerInfo);

        enterGameResultAction?.Invoke(enterGame);
        enterGameResultAction = null;
    }

}

