using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropScript : MonoBehaviour, IDropHandler

{
    // public Transform target;
    // private GameObject moveGo;
    // private Transform root;
    // private RectTransform rootRectTran;

    public Action<PointerEventData> OnDropAction;

    public void OnDrop(PointerEventData eventData)
    {
        // GameObject dropped = eventData.pointerDrag;
        // var dragScript = dropped.GetComponent<DragScript>();
        //
        //
        // Destroy(dropped);
        
        OnDropAction?.Invoke(eventData);
    }
    
    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     var battleUI = UIManager.Instance.FindCtrl<BattleUI>() as BattleUI;
    //     root = battleUI.tempRoot;
    //
    //     rootRectTran = root.GetComponent<RectTransform>();
    //     
    //     moveGo = GameObject.Instantiate(target.gameObject,
    //         rootRectTran, false);
    //     
    //     onBeginDragAction?.Invoke(eventData);
    // }
    //
    // public void OnDrag(PointerEventData eventData)
    // {
    //     var rectTran = rootRectTran;
    //     var uiCamera = CameraManager.Instance.GetCameraUI().camera;
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTran, eventData.position, uiCamera, out var outPos);
    //
    //     moveGo.transform.localPosition = outPos;
    //     
    //     onDragAction?.Invoke(eventData);
    // }
    //
    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     if (moveGo != null)
    //     {
    //         Destroy(moveGo);
    //     }
    //     onEndDragAction?.Invoke(eventData);
    // }
   
}