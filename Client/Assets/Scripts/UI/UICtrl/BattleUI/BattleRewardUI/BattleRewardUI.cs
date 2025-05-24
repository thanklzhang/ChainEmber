using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;
using UnityEngine.UI;
using BattleBox = Config.BattleBox;


//战斗获得的战斗奖励列表界面
public class BattleRewardUI
{
    public GameObject gameObject;
    public Transform transform;

    private Button closeBtn;
    private BattleUI battleUI;

    private Transform rewardRoot;

    private List<BattleRewardCell> cellShowList;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        rewardRoot = this.transform.Find("root/taskScroll/mask/content");

        closeBtn = transform.Find("root/closeBtn").GetComponent<Button>();

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(OnClickCloseBtn);

        EventDispatcher.AddListener(EventIDs.OnUpdateBattleReward, OnUpdateBattleReward);

        cellShowList = new List<BattleRewardCell>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        RefreshListShow();
    }


    public void RefreshListShow()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        if (null == player)
        {
            return;
        }


        var rewards = player.GetAllBattleRewards();

        cellShowList.Clear();

        for (int i = 0; i < rewards.Count; i++)
        {
            var data = rewards[i];
            GameObject go = null;
            if (i < rewardRoot.childCount)
            {
                go = rewardRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(rewardRoot.GetChild(0).gameObject,
                    rewardRoot, false);
            }

            BattleRewardCell cell = new BattleRewardCell();
            cell.Init(go, this);
            cell.Show();
            cell.RefreshUI(data, i);

            cellShowList.Add(cell);
        }

        for (int i = rewards.Count; i < rewardRoot.childCount; i++)
        {
            rewardRoot.GetChild(i).gameObject.SetActive(false);
        }
    }

    void OnClickCloseBtn()
    {
        this.Hide();
    }

    public void Update(float deltaTime)
    {
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnUpdateBattleReward()
    {
        this.RefreshAllUI();
    }

    public void Release()
    {
        EventDispatcher.RemoveListener(EventIDs.OnUpdateBattleReward, OnUpdateBattleReward);

        // for (int i = 0; i < boxCellShowList.Count; i++)
        // {
        //     var cell = boxCellShowList[i];
        //     cell.Release();
        // }
    }
}