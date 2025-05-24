using Config;
using GameData;

using UnityEngine;
using UnityEngine.UI;

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

    public void RefreshAll()
    {
        ////title
        //RefreshTitleBarUI();
        //lobby
        var playerInfo = GameDataManager.Instance.UserData.PlayerInfo;

        var playerNameStr = playerInfo.name;
        this.playerNameText.text = playerNameStr;
        this.playerLevelText.text = "" + playerInfo.level;
        iconResId = int.Parse(playerInfo.avatarURL);

        ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) => { playerIconImg.sprite = sprite; });
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
        //TODO 应该由 manager 去管理

        ResourceManager.Instance.ReturnObject(iconResId, playerIconImg.sprite);
        playerIconImg.sprite = null;
    }
}