using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using Config;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleReviveUI : BaseUI
{
    Button confirmBtn;
    Button giveUpBtn;
    private TextMeshProUGUI countText;

    private RectTransform countRectTran;
    private RectTransform rootRectTran;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleReviveUI;
        this.uiShowLayer = UIShowLayer.Floor_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        this.confirmBtn = this.transform.Find("Panel/reviveBtn").GetComponent<Button>();
        this.giveUpBtn = this.transform.Find("Panel/giveUpBtn").GetComponent<Button>();
        var root = this.transform.Find("Panel/reviveRoot");
        this.countText = root.Find("reviveCoinCount").GetComponent<TextMeshProUGUI>();

        rootRectTran = root.GetComponent<RectTransform>();
        countRectTran = countText.GetComponent<RectTransform>();

        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        giveUpBtn.onClick.AddListener(OnClickGiveUpBtn);
    }

    private void OnClickConfirmBtn()
    {
        var localHeroGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
        BattleManager.Instance.MsgSender.Send_SelectToRevive(localHeroGuid, true);
    }

    protected void OnClickGiveUpBtn()
    {
        var localHeroGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
        BattleManager.Instance.MsgSender.Send_SelectToRevive(localHeroGuid, false);
    }

    protected override void OnOpen(UICtrlArgs args)
    {
        var player = BattleManager.Instance.GetLocalPlayer();

        var reviveCount = player.GetReviveCount();

        this.countText.text = $"x {reviveCount}";

        LayoutRebuilder.ForceRebuildLayoutImmediate(countRectTran);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rootRectTran);
        
    }


    protected override void OnClose()
    {
        this.confirmBtn.onClick.RemoveAllListeners();
        this.giveUpBtn.onClick.RemoveAllListeners();
    }
}

public class BattleReviveUIArgs : UICtrlArgs
{
}