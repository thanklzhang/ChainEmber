using System.Text.RegularExpressions;
using Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//登录 ctrl

public class LoginUI : BaseUI
{
    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.LoginUI;
        this.uiShowLayer = UIShowLayer.Floor_0; 
        
    }
    
     // public Action<string, string> onLoginBtnClick;
    // public Action onChangeUserBtnClick;
    //
    // public Action<string, string> onLoginOrRegisterBtnClick;
    // public Action onLoginOrRegisterCloseBtnClick;
 
    //登录主界面
    private Text userAccountText;
    private Button btn_login;
    Text stateText;
    private Button changeUserBtn;

    //登录/注册界面
    private GameObject loginRootObj;
    InputField accountInput;

    InputField passwordInput;

    // Text statePopText;
    Button loginRegisterConfirmBtn;
    private Button loginRegisterCloseBtn;

    protected override void OnLoadFinish()
    { 
        //登录相关
        loginRootObj = this.transform.Find("loginRoot").gameObject;

        userAccountText = this.transform.Find("account/txt_name").GetComponent<Text>();
        btn_login = this.transform.Find("btn_login").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();
        changeUserBtn = this.transform.Find("account/btn_change_user").GetComponent<Button>();

        //登录/注册界面
        accountInput = loginRootObj.transform.Find("accountInput").GetComponent<InputField>();
        passwordInput = loginRootObj.transform.Find("passwordInput").GetComponent<InputField>();

        loginRegisterConfirmBtn = loginRootObj.transform.Find("loginBtn").GetComponent<Button>();
        // statePopText = loginRootObj.transform.Find("statePopText").GetComponent<Text>();
        loginRegisterCloseBtn = loginRootObj.transform.Find("btn_close").GetComponent<Button>();
        


        btn_login.onClick.AddListener(() =>
        {
            //主界面点击登录按钮
            //onLoginBtnClick?.Invoke("", "");
            
            this.OnClickLoginBtn("","");
        });
        
        changeUserBtn.onClick.AddListener(() =>
        {
            //onChangeUserBtnClick?.Invoke();
            this.OnChangeUserBtnClick();
        });

        loginRegisterConfirmBtn.onClick.AddListener(() =>
        {
            //登录/注册界面点击登录按钮
            var account = accountInput.text;
            var password = passwordInput.text;
            //onLoginOrRegisterBtnClick?.Invoke(account, password);

            this.OnLoginRegisterBtnClick(account,password);
        });

        loginRegisterCloseBtn.onClick.AddListener(() =>
        {
            //onLoginOrRegisterCloseBtnClick?.Invoke();
            this.OnLoginOrRegisterCloseBtnClick();
        });
        
      
        
        this.RefreshSaveLoginSuccessShow();
        
        this.SetStateText("没有登录");
        this.SetLoginRegisterUIShow(false);

        //目前直接当作局域网进行连接
        // string ip = NetTool.GetHostIp();

        string ip = GlobalConfig.LANServerIP;
        int port = 5556;
        OnclickConnectBtn(ip, port);
        
    }

    public void RefreshSaveLoginSuccessShow()
    {
        var preAccount = LocalData.GetString("currAccount");
        var preAassword = LocalData.GetString("currPassword");
        if (string.IsNullOrEmpty(preAccount))
        {
            preAccount = "----";
        }

        accountInput.text = preAccount;
        passwordInput.text = preAassword;

        userAccountText.text = preAccount;
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

    public void OnclickConnectBtn(string ip, int port)
    {
        var isLocal = GlobalConfig.isLANServer;
        if (isLocal)
        {
            // ui.SetConnectTips("start to connect to login server ...");
            this.SetStateText("开始连接登录服务器 ... " + ip + ":" + port);
            
            Logx.Log(LogxType.Game,"start to connect login server ...");
            
            port = 5556;
            NetworkManager.Instance.ConnectToLoginServer(ip, port, (isSuccess) =>
            {
                if (isSuccess)
                {
                    this.SetStateText("连接登录服务器成功");
                    // ui.SetConnectTips("connect to login server success !!!");
                    Logx.Log(LogxType.Game," connect login server success");
                    // ui.SetConnectUIShow(false);
                }
                else
                {
                    this.SetStateText("连接登录服务器失败");
                    // this.ui.SetConnectTips("connect login server fail");
                    Logx.Log(LogxType.Game," connect login server fail");
                }
            });
        }
        else
        {
            // this.ui.SetConnectTips("暂不支持远程服务器");
        }
    }

    public void OnClickLoginBtn(string account, string password)
    {
        var preAccount = LocalData.GetString("currAccount");
        var prePassword = LocalData.GetString("currPassword");
        bool isHaveAccount = !string.IsNullOrEmpty(preAccount);
        if (isHaveAccount)
        {
            //有账号就直接登录
            SendLoginNet(preAccount, prePassword);
        }
        else
        {
            //没有账号 弹出登录/注册界面
            var str = ("无账号,请注册账号");
            // Logx.Log(str);
            //CtrlManager.Instance.globalCtrl.ShowTips(str);

            this.SetLoginRegisterUIShow(true);
        }
    }

    private void OnChangeUserBtnClick()
    {
        this.SetLoginRegisterUIShow(true);
    }

    private void SendLoginNet(string account, string password)
    {
        //account = "zxy";
        //password = "zhang425";
        //ui.SetStateText("start to connect to login server ...");
        var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();
        this.SetStateText("开始登录 ...");
        
        Logx.Log(LogxType.Game," start login ...");
        
        // Logx.Log("account : " + account);
        // Logx.Log("password : " + password);
        loginHandler.SendCheckLogin(account, password, (result) =>
        {
            if (0 == result.Err)
            {
                Logx.Log(LogxType.Game," login success ...");
                this.SetStateText("登录成功");
                //CtrlManager.Instance.globalCtrl.ShowTips("登录成功");
                this.SaveLoginSuccessInfo(account, password);
                this.StartToEnterGame(result);
            }
            else
            {
                Logx.Log(LogxType.Game," login fail , err : " + result.Err);
                
                var tips = "未知错误";
                if (1 == result.Err)
                {
                    tips = "登录失败,该账号和密码不符合";
                }
                else if (2 == result.Err)
                {
                    tips = "注册失败,请联系管理员";
                }

                //CtrlManager.Instance.globalCtrl.ShowTips(tips + " , 错误码：" + result.Err);
                this.SetStateText("登录失败");
            }
        });
    }

    private void OnLoginRegisterBtnClick(string account, string password)
    {
        if (CheckLogin(account))
        {
            this.SetLoginRegisterUIShow(false);
            SendLoginNet(account, password);
        }
        else
        {
            //CtrlManager.Instance.globalCtrl.ShowTips("账号中不能有特殊符号");
        }
    }

    private bool CheckLogin(string account)
    {
        Regex regExp = new Regex("[ \\[ \\] \\^ \\-_*×――(^)$%~!＠@＃#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;/\'\"{}（）‘’“”-]");
        if (regExp.IsMatch(account))
        {
            return false;
        }

        return true;
    }

    protected void OnLoginOrRegisterCloseBtnClick()
    {
        this.SetLoginRegisterUIShow(false);
    }

    public void SaveLoginSuccessInfo(string account, string password)
    {
        LocalData.SetString("currAccount", account);
        LocalData.SetString("currPassword", password);
    }


    void StartToEnterGame(NetProto.scCheckLogin result)
    {
        var ip = result.Ip;
        var port = result.Port;
        var uid = result.Uid;
        // Logx.Log("StartToEnterGame : get uid : " + uid);

        this.SetStateText("开始连接网关服务器 ...");
        
        Logx.Log(LogxType.Game,"start to connect gate server ...");

        NetworkManager.Instance.ConnectToGateServer(ip, port, (isSuccess) =>
        {
            if (isSuccess)
            {
                this.SetStateText("连接网关服务器成功");
                
                Logx.Log(LogxType.Game,"connect gate server success");
                
                var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();

                this.SetStateText("开始进入游戏 ...");

                Logx.Log(LogxType.Game,"request enter game");
                loginHandler.SendEnterGame(uid, "", (enterResult) =>
                {
                    if (0 == enterResult.Err)
                    {
                        Logx.Log(LogxType.Game,"enter game success");
                        
                        this.SetStateText("进入游戏成功");

                        //CtrlManager.Instance.globalCtrl.ShowTips("进入游戏成功");

                        OnGateConnectResult();
                    }
                    else
                    {
                        this.SetStateText("进入游戏失败");

                        Logx.Log(LogxType.Game,"enter game fail");
                        
                        //CtrlManager.Instance.globalCtrl.ShowTips("进入游戏失败");
                    }
                });
            }
            else
            {
                this.SetStateText("连接网关服务器失败");

                Logx.Log(LogxType.Game,"connect gate server fail");

                //CtrlManager.Instance.globalCtrl.ShowTips("连接 网关服务器 失败");
            }
        });
    }

    public void OnGateConnectResult()
    {
        Logx.Log(LogxType.Game,"start enter game lobby ...");

        //前往大厅
        // UICtrlManager.Instance.Enter<LobbyUICtrl>();
        SceneCtrlManager.Instance.Enter<LobbySceneCtrl>();
    }

    protected override void OnClose()
    {
        // onLoginBtnClick = null;
        // onLoginOrRegisterBtnClick = null;
        // onLoginOrRegisterCloseBtnClick = null;
        // onChangeUserBtnClick = null;
        //
        // btn_login.onClick.RemoveAllListeners();
        // loginRegisterConfirmBtn.onClick.RemoveAllListeners();
        // loginRegisterCloseBtn.onClick.RemoveAllListeners();
        // changeUserBtn.onClick.RemoveAllListeners();

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