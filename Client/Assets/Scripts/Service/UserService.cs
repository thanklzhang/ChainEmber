using System;
using System.Collections.Generic;
using GameData;
using System.IO;
using UnityEngine;
using System.Text;

public class UserService : Singleton<UserService>
{
    public UserData userData;
    private const string USER_DATA_FILENAME = "user_data.json";
    private IStorageService storageService = new FileStorageService();

    public void Init()
    {
        // this.userData = userData;
        //
        // HeroService.Instance.Init(userData.heroListData);
        // BagService.Instance.Init(userData.bagData);
    }

    public void Login(Action<LoginResponse> action)
    {
        try
        {
            //先尝试读取存档
            userData = LoadUser();


            if (null == userData || string.IsNullOrEmpty(userData.deviceId))
            {
                //第一次登录，创建新用户
                Register();
            }

            //登陆成功
            OnLoginSuccess();
            action?.Invoke(new LoginResponse()
            {
                err = ErrorCode.Success,
            });
        }
        catch (Exception e)
        {
            Logx.LogError("UserService", e);
            action?.Invoke(new LoginResponse()
            {
                err = ErrorCode.LoginFail,
            });
        }
    }

    public void OnLoginSuccess()
    {
        // if (null == userData.bagData) userData.bagData = new BagData();
        // BagService.Instance.SetData(userData.bagData);
        //
        // if (null == userData.heroListData) userData.heroListData = new HeroListData();
        // HeroService.Instance.SetData(userData.heroListData);
    }


    public UserData LoadUser()
    {
        try
        {
            if (!storageService.Exists(USER_DATA_FILENAME))
            {
                Logx.Log("UserService", "未找到用户数据文件");
                return null;
            }

            string json = storageService.Load(USER_DATA_FILENAME);
            if (string.IsNullOrEmpty(json))
            {
                Logx.LogWarning("UserService", "用户数据文件为空");
                return null;
            }

            UserData data = LitJson.JsonMapper.ToObject<UserData>(json);
            if (data != null)
            {
                Logx.Log("UserService", $"用户数据加载成功，用户ID: {data.uid}");
            }

            return data;
        }
        catch (Exception e)
        {
            Logx.LogError("UserService", $"加载用户数据失败: {e.Message}");
            return null;
        }
    }


    public void Register()
    {
        var deviceId = UserUtil.GetDeviceIdentifier();
        int uid = UserUtil.GetRandomUid(deviceId);
        userData = new UserData();
        userData.uid = uid;
        userData.deviceId = deviceId;

        SetUserInitInfo();

        SaveData();
    }


    public List<int> initHeroIds = new List<int>()
    {
        1000001,
        1000002,
        1000003,
        1000007,
    };

    public int initUserCoin = 500;

    //当用户创建的时候初始化操作
    public void SetUserInitInfo()
    {
        userData.heroListData = new HeroListData();
        userData.bagData = new BagData();
        userData.name = $"NewPlayer";
        userData.avatarURL = "1001";
        userData.level = 1;

        //添加初始道具数据
        userData.bagData = new BagData();
        userData.bagData.bagItemList = new List<BagItem>();
        
        BagService.Instance.AddItem(ItemIds.CoinId,initUserCoin);
        // new BagItem()
        // {
        //     //初始金币
        //     configId = ItemIds.CoinId,
        //     count = 500,
        // }

        //添加初始英雄数据
        userData.heroListData = new HeroListData();
        userData.heroListData.heroList = new List<HeroData>();
        var heroList = userData.heroListData.heroList;
        for (int i = 0; i < initHeroIds.Count; i++)
        {
            
            HeroService.Instance.AddHero(initHeroIds[i]);
            
            // heroList.Add(new HeroData()
            // {
            //     configId = initHeroIds[i],
            //     level = 1,
            //     star = 1,
            // });
        }
    }


    public void SaveData()
    {
        if (userData == null)
        {
            Logx.LogError("UserService", "无法保存空的用户数据");
            return;
        }

        try
        {
            string json = LitJson.JsonMapper.ToJson(userData);
            storageService.Save(USER_DATA_FILENAME, json);
            Logx.Log("UserService", $"用户数据保存成功，用户ID: {userData.uid}");
        }
        catch (Exception e)
        {
            Logx.LogError("UserService", $"保存用户数据失败: {e.Message}");
        }
    }
}