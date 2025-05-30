using System;
using GameData;
using System.IO;
using UnityEngine;
using System.Text;

public enum ErrorCode
{
    Success = 0,
    Error = 1,
    
    LoginFail = 101,
    
    UpgradeHeroMaxLevel = 201,
}

public class ServiceResponse
{
    public ErrorCode err;
    
    public static ServiceResponse New(ErrorCode err)
    {
        return new ServiceResponse()
        {
            err = err,
        };
    }
    public static ServiceResponse Success=> New(ErrorCode.Success);
}


public class LoginResponse : ServiceResponse
{
   
}

public class UserService : Singleton<UserService>
{
    public UserData userData;
    private const string USER_DATA_FILENAME = "user_data.json";

    public void Init(UserData userData)
    {
        this.userData = userData;

        HeroService.Instance.Init(userData.heroListData);
        BagService.Instance.Init(userData.bagData);
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

    
    
    public UserData LoadUser()
    {
        string filePath = Path.Combine(Application.persistentDataPath, USER_DATA_FILENAME);
        
        try
        {
            if (!File.Exists(filePath))
            {
                Logx.Log("UserService", "未找到用户数据文件");
                return null;
            }
            
            // 直接读取整个文件内容，避免逐行读取的性能损失
            string json = File.ReadAllText(filePath, Encoding.UTF8);
            
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
        userData.heroListData = new HeroListData();
        userData.bagData = new BagData();
        userData.name  = $"NewPlayer";
        userData.avatarURL = "1";
        
        SaveData();

        OnUserCreate();
    }

    //当用户创建的时候初始化操作
    public void OnUserCreate()
    {
        
    }
    

    public void SaveData()
    {
        if (userData == null)
        {
            Logx.LogError("UserService", "无法保存空的用户数据");
            return;
        }
        
        string filePath = Path.Combine(Application.persistentDataPath, USER_DATA_FILENAME);
        string directory = Path.GetDirectoryName(filePath);
        
        try
        {
            // 确保目录存在
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            // 先将数据序列化为JSON
            string json = LitJson.JsonMapper.ToJson(userData);
            
            // 直接将整个字符串写入文件，覆盖原文件
            File.WriteAllText(filePath, json, Encoding.UTF8);
            
            Logx.Log("UserService", $"用户数据保存成功，用户ID: {userData.uid}");
        }
        catch (Exception e)
        {
            Logx.LogError("UserService", $"保存用户数据失败: {e.Message}");
        }
    }
}