using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class HeroStateShowObj
{
    GameObject gameObject;
    Transform transform;

    HeroStateUIMgr heroStateUIMgr;

    public HeroStateHp hpPart;
    public HeroStateStar starPart;
    public HeroStateName namePart;

    private GameObject entityObj;
    RectTransform parentTranRect;

    public void Init(GameObject gameObject, BattleEntity_Client entity, HeroStateUIMgr heroStateUIMgr)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.heroStateUIMgr = heroStateUIMgr;

        parentTranRect = heroStateUIMgr.transform.GetComponent<RectTransform>();

        //hp部分
        var hpRoot = this.transform.Find("hpMain");
        hpPart = new HeroStateHp();
        hpPart.Init(hpRoot.gameObject, entity, this.heroStateUIMgr);

        //star部分
        var starRoot = this.transform.Find("starMain");
        starPart = new HeroStateStar();
        starPart.Init(starRoot.gameObject, entity, this.heroStateUIMgr);
        
        //name部分
        var nameRoot = this.transform.Find("nameMain");
        namePart = new HeroStateName();
        namePart.Init(nameRoot.gameObject, entity, this.heroStateUIMgr);
    }

    public void Refresh(BattleEntity_Client entity, int fromEntityGuid)
    {
        entityObj = entity.gameObject;
        hpPart.Refresh(entity, fromEntityGuid);
        starPart.Refresh(entity, fromEntityGuid);
        namePart.Refresh(entity, fromEntityGuid);
    }

    public void Update(float deltaTime)
    {
        UpdatePos();

        hpPart.Update(deltaTime);
        starPart.Update(deltaTime);
        namePart.Update(deltaTime);
    }

    public void UpdatePos()
    {
        //随实体
        var entityObj = this.entityObj;
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        var screenPos = RectTransformUtility.WorldToScreenPoint(camera3D.camera, entityObj.transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTranRect, screenPos, cameraUI.camera, out uiPos);

        //这里可以换成实体上的血条挂点
        this.transform.localPosition = uiPos + Vector2.up * 100;
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