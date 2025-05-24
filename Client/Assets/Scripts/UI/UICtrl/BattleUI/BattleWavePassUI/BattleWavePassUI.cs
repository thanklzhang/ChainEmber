using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle_Client;
using Config;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//战斗当前波结算UI
public partial class BattleWavePassUI
{
    public GameObject gameObject;
    public Transform transform;

    private BattleUI battleUI;

    private Button closeBtn;
    private Button confirmBtn;
    private Text passTotalTimeText;
    private Transform currencyRoot;
    private Transform boxRoot;

    private BattleWavePass_RecvMsg_Arg data;

    private List<BattleWavePassUI_CurrencyCell> currencyShowList;
    private List<BattleWavePassUI_BoxCell> boxShowList;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        var mainTran = transform.Find("root/main");
        closeBtn = mainTran.Find("closeBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnClickCloseBtn);

        confirmBtn = mainTran.Find("confirmBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);

        currencyRoot = mainTran.Find("itemRoot/mask/content");
        boxRoot = mainTran.Find("boxRoot/mask/content");
    }

    public void ShowByData(BattleWavePass_RecvMsg_Arg battleWavePassRecvMsgArg)
    {
        this.data = battleWavePassRecvMsgArg;
        this.Show();
        this.RefreshAllUI();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        if (null == this.data)
        {
            return;
        }

        RefreshCurrencyListShow();
        RefreshBoxListShow();
    }

    void RefreshCurrencyListShow()
    {
        currencyShowList = new List<BattleWavePassUI_CurrencyCell>();
        var dataList = this.data.currencyDic.Values.ToList();
        dataList.Sort((a, b) => a.itemConfigId.CompareTo(b));

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            GameObject go = null;
            if (i < this.currencyRoot.childCount)
            {
                go = this.currencyRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.currencyRoot.GetChild(0).gameObject,
                    this.currencyRoot, false);
            }

            var cell = new BattleWavePassUI_CurrencyCell();
            cell.Init(go, this);
            cell.Show();
            cell.RefreshUI(data);
        }

        for (int i = dataList.Count; i < this.currencyRoot.childCount; i++)
        {
            var go = this.currencyRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    void RefreshBoxListShow()
    {
        boxShowList = new List<BattleWavePassUI_BoxCell>();
        var dataList = new List<WavePassRewardBoxUIGroupData>();
        // List<List<WavePassBox_RecvMsg>> list = new List<List<WavePassBox_RecvMsg>>();
        foreach (var kv in this.data.boxDic)
        {
            var quality = kv.Key;
            var boxList = kv.Value;
            if (boxList != null && boxList.Count > 0)
            {
                var uiData = new WavePassRewardBoxUIGroupData();
                uiData.boxConfigId = boxList[0].boxConfigId;
                uiData.count = boxList.Count;
                uiData.quality = quality;
                dataList.Add(uiData);
            }
        }

        dataList.Sort((a, b) => 
            { return a.quality.CompareTo(b.quality); });

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            GameObject go = null;
            if (i < this.boxRoot.childCount)
            {
                go = this.boxRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.boxRoot.GetChild(0).gameObject,
                    this.boxRoot, false);
            }

            var cell = new BattleWavePassUI_BoxCell();
            cell.Init(go, this);
            cell.Show();
            cell.RefreshUI(data);
        }

        for (int i = dataList.Count; i < this.boxRoot.childCount; i++)
        {
            var go = this.boxRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    void OnClickCloseBtn()
    {
        this.Hide();
    }

    void OnClickConfirmBtn()
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

    public void Release()
    {
        for (int i = 0; i < currencyShowList.Count; i++)
        {
            var currencyShow = currencyShowList[i];
            currencyShow.Hide();
            currencyShow.Release();
        }

        for (int i = 0; i < boxShowList.Count; i++)
        {
            var currencyShow = boxShowList[i];
            currencyShow.Hide();
            currencyShow.Release();
        }
    }
}

public class WavePassRewardBoxUIGroupData
{
    //这里只为了方便
    public RewardQuality quality;

    //这里的 id 只是为了显示宝箱的外观（因为所有一个等级的箱子都一个样）
    public int boxConfigId;
    public int count;
}