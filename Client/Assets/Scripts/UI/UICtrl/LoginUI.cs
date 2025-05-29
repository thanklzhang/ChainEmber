using System.Text.RegularExpressions;
using Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ServerSimulation.Services;

//登录 ctrl

public class LoginUI : BaseUI
{
    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.LoginUI;
        this.uiShowLayer = UIShowLayer.Floor_0; 
    }
 
    //登录主界面
    private Text userAccountText;
    private Button btn_login;
    Text stateText;
    private Button changeUserBtn;

    //登录/注册界面 - 不再需要，但保留界面引用防止空引用错误
    private GameObject loginRootObj;

    protected override void OnLoadFinish()
    { 
        //登录相关
        loginRootObj = this.transform.Find("loginRoot").gameObject;

        userAccountText = this.transform.Find("account/txt_name").GetComponent<Text>();
        btn_login = this.transform.Find("btn_login").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();
        changeUserBtn = this.transform.Find("account/btn_change_user").GetComponent<Button>();

        btn_login.onClick.AddListener(() =>
        {
            //主界面点击登录按钮
            this.OnClickLoginBtn();
        });
        
        changeUserBtn.onClick.AddListener(() =>
        {
            //切换用户按钮 - 现在变为重置用户按钮
            this.OnResetUserBtnClick();
        });

        // 隐藏登录/注册界面，显示主界面
        this.SetLoginRegisterUIShow(false);
        this.SetStateText("准备登录...");
        
        // 显示设备ID
        this.RefreshDeviceIdShow(); 
        
        // 自动开始登录流程
        this.SetStateText("正在自动登录...");
        this.AutoLogin();
    }

    // /// <summary>
    // /// 获取设备唯一标识符
    // /// </summary>
    // private string GetDeviceIdentifier()
    // {
    //     // 使用SystemInfo.deviceUniqueIdentifier获取设备唯一标识符
    //     // 实际项目中可能需要更复杂的处理来确保跨设备唯一性
    //     string deviceId = SystemInfo.deviceUniqueIdentifier;
    //     
    //     // 为防止ID过长，可以截取或Hash处理
    //     if (deviceId.Length > 20)
    //     {
    //         deviceId = deviceId.Substring(0, 20);
    //     }
    //     
    //     // 添加前缀区分设备类型
    //     string prefix = Application.platform == RuntimePlatform.Android ? "AD_" : 
    //                      Application.platform == RuntimePlatform.IPhonePlayer ? "IOS_" : "PC_";
    //                      
    //     return prefix + deviceId;
    // }
    
    /// <summary>
    /// 使用设备ID自动登录
    /// </summary>
    private void AutoLogin()
    {
        // string deviceId = GetDeviceIdentifier();
        // Debug.Log($"[LoginUI] 使用设备ID登录: {deviceId}");
        
        // // 使用设备ID作为用户名，空密码（后端会处理）
        // LoginService.Instance.Login(deviceId, "", (success, userId, errorMessage) => {
        //     if (success)
        //     {
        //         this.SetStateText("登录成功");
        //         this.StartToEnterGame();
        //     }
        //     else
        //     {
        //         this.SetStateText($"登录失败: {errorMessage}，请点击登录按钮重试");
        //     }
        // });
        
        UserService.Instance.Login((response) =>
        {
            if (response.err == ErrorCode.Success)
            {
                // 登录成功，回调
                this.SetStateText("登录成功");
                this.StartToEnterGame();
            }
            else
            {
                this.SetStateText($"登录失败: {response.err}，请点击登录按钮重试");
            }
        });
    }

    public void RefreshDeviceIdShow()
    {
        // 显示设备ID（可能会截断）
        string deviceId = UserUtil.GetDeviceIdentifier();
        userAccountText.text = deviceId;
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }

    public void SetLoginRegisterUIShow(bool isShow)
    {
        loginRootObj.SetActive(isShow);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    /// <summary>
    /// 主界面点击登录按钮
    /// </summary>
    public void OnClickLoginBtn()
    {
        this.SetStateText("正在登录...");
        this.AutoLogin();
    }

    private void OnResetUserBtnClick()
    {
        // 清除后台存储的用户（重置用户）
        LoginService.Instance.ClearLocalUser();
        
        this.SetStateText("已重置用户数据，正在重新登录...");
        
        // 重新登录
        this.AutoLogin();
    }

    private void StartToEnterGame()
    {
        Logx.Log(LogxType.Game, "start enter game lobby ...");
        //前往大厅
        SceneCtrlManager.Instance.Enter<LobbySceneCtrl>();
    }

    protected override void OnClose()
    {
        this.gameObject = null;
        this.transform = null;
    }
}


// public class LoginCtrl : BaseCtrl
// {
//     protected override void OnInit()
//     {
//         this.uiResId = (int)ResIds.LoginUI;
//         this.uiShowLayer = UIShowLayer.Floor_0; 
//         
//     }
//     
//     
//
// }