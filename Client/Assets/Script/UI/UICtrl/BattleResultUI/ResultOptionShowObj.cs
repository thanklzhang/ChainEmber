using Config;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class ResultOptionShowObj
{
    public GameObject gameObject;
    public Transform transform;

    Text nameText;
    Text countText;
    Image iconImg;
    public ItemData data;

    int currIconResId;
    Sprite currIconSprite;

    private BattleResultUI battleResultUI;

    public void Init(GameObject gameObject, BattleResultUI battleResultUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.battleResultUI = battleResultUI;

        nameText = this.transform.Find("root/name").GetComponent<Text>();
        countText = this.transform.Find("root/count").GetComponent<Text>();
        iconImg = this.transform.Find("root/icon").GetComponent<Image>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(ItemData data)
    {
        this.data = data;

        var configId = this.data.configId;
        var itemTb = ConfigManager.Instance.GetById<Config.Item>(configId);
        nameText.text = itemTb.Name;
        countText.text = "" + this.data.count;

        currIconResId = itemTb.IconResId;
        ResourceManager.Instance.GetObject<Sprite>(currIconResId, (sprite) =>
        {
            //TODO
            //注意 这里界面关闭了还会再次执行
            //这里应该判断是否界面界面关闭了等状态
            currIconSprite = sprite;
            iconImg.sprite = sprite;
        });
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
        if (currIconSprite != null)
        {
            ResourceManager.Instance.ReturnObject<Sprite>(currIconResId, currIconSprite);
        }
    }
}