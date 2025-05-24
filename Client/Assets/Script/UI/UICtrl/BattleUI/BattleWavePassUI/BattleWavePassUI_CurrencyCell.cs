using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//战斗当前波结算 UI 中的货币
public class BattleWavePassUI_CurrencyCell
{
    private GameObject gameObject;
    private Transform transform;

    private WavePassCurrency_RecvMsg data;

    private BattleWavePassUI wavePassUI;

    private Image iconImg;
    private TextMeshProUGUI countText;
    public void Init(GameObject go, BattleWavePassUI battleWavePassUI)
    {
        this.gameObject = go;
        this.transform = this.gameObject.transform;
        this.wavePassUI = battleWavePassUI;

        iconImg = this.transform.Find("iconBg/icon").GetComponent<Image>();
        countText = this.transform.Find("count_text").GetComponent<TextMeshProUGUI>();
    }

    public void Show()
    {
      this.gameObject.SetActive(true);
    }

    public void RefreshUI(WavePassCurrency_RecvMsg data)
    {
        this.data = data;

        var config = ConfigManager.Instance.GetById<BattleItem>(this.data.itemConfigId); 
        var resId = config.IconResId;
        ResourceManager.Instance.GetObject<Sprite>(resId, (sprite) =>
        {
            this.iconImg.sprite = sprite;
        });

        countText.text = "x" + this.data.count;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
        
    }


}