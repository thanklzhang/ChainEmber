using System;
using GameData;

public enum ErrorCode
{
    Success = 0,
    Error = 1,
}

public class LoginResponse
{
    public ErrorCode err; 
}

public class UserService : Singleton<UserService>
{
    public UserData userData;

    public void Init(UserData userData)
    {
        this.userData = userData;

        HeroService.Instance.Init(userData.heroData);
        BagService.Instance.Init(userData.bagData);
    }

    public void Login(Action<LoginResponse> action)
    {
        var deviceId = UserUtil.GetDeviceIdentifier();
        
        //根据设备 id 进行读取本地存档

        bool isHaveData = false;
        if (isHaveData)
        {
            LoadUser();
        }
        else
        {
            Register();
        }
        
        action?.Invoke(new LoginResponse()
        {
            err = ErrorCode.Success,
        });
    }

    public void LoadUser()
    {
        //从存档取
        //userData = 
    }

    public void Register()
    {
        //新创建一个初始用户
        //userData =
        SaveData();
    }

    public void SaveData()
    {
        //存档
    }
}