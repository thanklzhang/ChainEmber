using System.Linq;
using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class HeroStateStar
{
    GameObject gameObject;
    Transform transform;

    private Transform starRoot;

    private TextMeshProUGUI expText;

    private Slider expSlider;

    public void Init(GameObject gameObject, BattleEntity_Client entity, HeroStateUIMgr hpUIMgr)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        starRoot = this.transform.Find("starRoot");
        expText = this.transform.Find("expText").GetComponent<TextMeshProUGUI>();
        expSlider = this.transform.Find("starSlider").GetComponent<Slider>();
    }

    public void Refresh(BattleEntity_Client entity, int fromEntityGuid)
    {
        // RefreshData(entity,fromEntityGuid);

        RefreshShow(entity);
    }

    //刷新数据
    public void RefreshData(BattleEntity_Client entity, int fromEntityGuid)
    {
    }

    //刷新显示
    public void RefreshShow(BattleEntity_Client entity)
    {
        int star = entity.starLv;

        //星级显示
        var maxStar = 5;
        for (int i = 0; i < maxStar; i++)
        {
            var child = starRoot.GetChild(i);
            var starTran = child.Find("starBg/star");
            starTran.gameObject.SetActive(star > i);
        }

        //经验显示
        int starExp = entity.starExp;
        var entityUpdateConfig = Config.ConfigManager.Instance.GetById<EntityUpgradeParam>(1);
        var maxExp = entityUpdateConfig.UpgradeExpPerStarLevel.Sum() + 1;
        expText.text = $"{starExp}/{maxExp}";

        expSlider.value = starExp / (float)maxExp;
    }

    public void Update(float timeDelta)
    {
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    internal void SetShowState(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}