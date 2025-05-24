using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;
using UnityEngine.UI;
using BattleBox = Config.BattleBox;


//战斗选择战斗奖励界面
public class BattleSelectRewardUI
{
    public GameObject gameObject;
    public Transform transform;

    private Button closeBtn;
    private BattleUI battleUI;

    private Transform selectionRoot;

    private List<BattleSelectRewardCell> boxCellShowList;

    private Text boxCountText;


    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        selectionRoot = this.transform.Find("root/select/rewardScroll/mask/content");

        boxCountText = this.transform.Find("root/select/count").GetComponent<Text>();

        closeBtn = transform.Find("root/select/closeBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);

        EventDispatcher.AddListener(EventIDs.OnBoxOpen, OnOpenBox);

        boxCellShowList = new List<BattleSelectRewardCell>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void CheckBoxState()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        if (null == player)
        {
            return;
        }

        player.TryOpenBox(RewardQuality.Blue);
    }

    void OnOpenBox()
    {
        this.Show();

        this.RefreshSelectionListShow();
    }

    public void RefreshAllUI()
    {
        RefreshSelectionListShow();
    }


    public void RefreshSelectionListShow()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        if (null == player)
        {
            return;
        }

        if (player.currOpenBox != null)
        {
            var selections = player.currOpenBox.selections;

            boxCellShowList.Clear();

            for (int i = 0; i < selections.Count; i++)
            {
                var data = selections[i];
                GameObject go = null;
                if (i < selectionRoot.childCount)
                {
                    go = selectionRoot.GetChild(i).gameObject;
                }
                else
                {
                    go = GameObject.Instantiate(selectionRoot.GetChild(0).gameObject,
                        selectionRoot, false);
                }

                BattleSelectRewardCell cell = new BattleSelectRewardCell();
                cell.Init(go, this);
                cell.Show();
                cell.RefreshUI(data, i);

                boxCellShowList.Add(cell);
            }

            for (int i = selections.Count; i < selectionRoot.childCount; i++)
            {
                selectionRoot.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    void OnClickCloseBtn()
    {
        this.Hide();
    }

    public void OnSelectSelection(BattleSelectRewardCell boxCell)
    {
        this.Hide();

        var player = BattleManager.Instance.GetLocalPlayer();
        if (null == player)
        {
            return;
        }

        var box = ConfigManager.Instance.GetById<BattleBox>(player.currOpenBox.configId);

        var selectIndex = boxCell.index;
        var quality = (RewardQuality)box.Quality;
        BattleManager.Instance.MsgSender.Send_SelectBoxReward(quality, selectIndex);
    }

    public void OnRefreshBoxInfo()
    {
        this.boxCountText.text = "";
    }

    public void Update(float deltaTime)
    {
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
        EventDispatcher.RemoveListener(EventIDs.OnBoxOpen, OnOpenBox);

        // for (int i = 0; i < boxCellShowList.Count; i++)
        // {
        //     var cell = boxCellShowList[i];
        //     cell.Release();
        // }
    }
}