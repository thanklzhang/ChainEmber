using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    public Transform target;
    private GameObject moveGo;
    private Transform root;
    private RectTransform rootRectTran;

    public Action<PointerEventData> onBeginDragBeforeAction;
    public Action<PointerEventData> onBeginDragAction;
    public Action<PointerEventData> onDragAction;
    public Action<PointerEventData> onEndDragAction;

    public object transferData;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDragBeforeAction?.Invoke(eventData);
        
        var battleUI = UIManager.Instance.FindCtrl<BattleUI>() as BattleUI;
        root = battleUI.tempRoot;

        rootRectTran = root.GetComponent<RectTransform>();
        
        moveGo = GameObject.Instantiate(target.gameObject,
            rootRectTran, false);

        var allImage = moveGo.GetComponentsInChildren<Image>();
        foreach (var img in allImage)
        {
            img.raycastTarget = false;
        }
        
        onBeginDragAction?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var rectTran = rootRectTran;
        var uiCamera = CameraManager.Instance.GetCameraUI().camera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTran, eventData.position, uiCamera, out var outPos);

        moveGo.transform.localPosition = outPos;
        
        onDragAction?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (moveGo != null)
        {
            Destroy(moveGo);
        }
        onEndDragAction?.Invoke(eventData);
    }
}