using Config;
using GameData;

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//登录 ctrl
public class LobbyUI : BaseUI
{
    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.LobbyUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
    }

    Button closeBtn;
    Button heroListBtn;
    Button mainTaskBtn;
    Button teamBtn;
    Text playerNameText;
    Text playerLevelText;
    private Image playerIconImg;

    protected override void OnLoadFinish()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        heroListBtn = this.transform.Find("heroListBtn").GetComponent<Button>();
        mainTaskBtn = this.transform.Find("mainTaskBtn").GetComponent<Button>();
        teamBtn = this.transform.Find("teamBtn").GetComponent<Button>();
        playerNameText = this.transform.Find("heroInfo/playerNameRoot/name").GetComponent<Text>();
        playerLevelText = this.transform.Find("heroInfo/level").GetComponent<Text>();
        playerIconImg = this.transform.Find("heroInfo/avatarBg/avatar").GetComponent<Image>();

        // closeBtn.onClick.AddListener(() => { onClickCloseBtn?.Invoke(); });
        // heroListBtn.onClick.AddListener(() => { onClickHeroListBtn?.Invoke(); });
        // mainTaskBtn.onClick.AddListener(() => { onClickMainTaskBtn?.Invoke(); });
        teamBtn.onClick.AddListener(OnClickTeamBtn);
    }

    protected override void OnOpen(UICtrlArgs args)
    {
        Logx.Log(LogxType.Game, "enter game lobby success");

        //play bgm
        //TODO: 配表
        AudioManager.Instance.PlayBGM((int)ResIds.bgm_002);
    }

    protected override void OnActive()
    {
        
        UIManager.Instance.Open<TitleBarUI>(new TitleBarUIArgs()
        {
            titleBarId = 1
        });
    
        RefreshAll();
    }

    public int iconResId;
    private string currentAvatarUrl;

    public void RefreshAll()
    {
        ////title
        //RefreshTitleBarUI();
        //lobby
        var playerInfo = GameDataManager.Instance.UserData.PlayerInfo;

        var playerNameStr = playerInfo.name;
        this.playerNameText.text = playerNameStr;
        this.playerLevelText.text = "" + playerInfo.level;
        
        // 检查是否是网络URL
        if (NetUtil.IsNetworkUrl(playerInfo.avatarURL))
        {
            // 如果是网络URL，使用网络加载逻辑
            Debug.Log($"[LobbyUI] 检测到网络头像URL: {playerInfo.avatarURL}");
            currentAvatarUrl = playerInfo.avatarURL;
            
            // 调用网络加载方法
            LoadAvatarFromNetwork(playerInfo.avatarURL);
        }
        else
        {
            // 使用通用方法获取本地头像资源ID
            iconResId = PlayerConvert.GetPlayerAvatarResId(playerInfo.avatarURL);
            currentAvatarUrl = null;
            
            // 加载本地头像精灵
            ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) => { playerIconImg.sprite = sprite; });
        }
    }
    
    /// <summary>
    /// 从网络加载头像
    /// </summary>
    private void LoadAvatarFromNetwork(string url)
    {
        Debug.Log($"[LobbyUI] 开始从网络加载头像: {url}");
        
        // 显示下载中的占位图或默认头像
        int defaultAvatarId = PlayerConvert.GetPlayerAvatarResId(null); // 使用默认头像
        ResourceManager.Instance.GetObject<Sprite>(defaultAvatarId, (sprite) => 
        { 
            playerIconImg.sprite = sprite;
            Debug.Log($"[LobbyUI] 网络头像加载中，暂时使用默认头像");
        });
        
        // TODO: 实现实际的网络图片下载逻辑
        // 这里是网络下载示例代码，实际需要根据项目实现
        /*
        // 使用Unity的WWW或UnityWebRequest下载图片
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        request.SendWebRequest().completed += operation => {
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (url == currentAvatarUrl) // 确保还是加载同一个URL的头像
                {
                    Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    playerIconImg.sprite = sprite;
                    Debug.Log($"[LobbyUI] 网络头像加载成功: {url}");
                }
            }
            else
            {
                Debug.LogError($"[LobbyUI] 网络头像加载失败: {request.error}");
                // 加载失败时使用默认头像
                int defaultAvatarId = PlayerConvert.GetPlayerAvatarResId(null);
                ResourceManager.Instance.GetObject<Sprite>(defaultAvatarId, (sprite) => { playerIconImg.sprite = sprite; });
            }
        };
        */
        
        Debug.Log($"[LobbyUI] 网络头像预留接口测试完成");
    }

    public void OnClickHeroListBtn()
    {
        //CtrlManager.Instance.Enter<HeroListCtrlPre>();
    }

    public void OnClickMainTaskBtn()
    {
        // CtrlManager.Instance.Enter<MainTaskCtrlPre>();
    }

    public void OnClickTeamBtn()
    {
        UIManager.Instance.Open<TeamRoomListUI>();
    }

    protected override void OnInactive()
    {
        base.OnInactive();
    }

    protected override void OnClose()
    {
        // 清理资源
        if (iconResId > 0 && playerIconImg.sprite != null)
        {
            ResourceManager.Instance.ReturnObject(iconResId, playerIconImg.sprite);
            playerIconImg.sprite = null;
        }
        
        currentAvatarUrl = null;
    }
}