using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class InputProxy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    static readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    static bool IsSelfOrAncestor(Transform ancestor, Transform child)
    {
        while (child)
        {
            if (child == ancestor)
            {
                return true;
            }
            child = child.parent;
        }

        return false;
    }

    private GameObject downGameObject;
    private GameObject upGameObject;
    private GameObject dragGameObject;
    private bool isCustomDrag;
    private float dragDelta;

    public Func<PointerEventData, bool> onCheckDrag = (data) => false;
    public Action<PointerEventData> onBeginDrag = (data) => { };
    public Action<PointerEventData> onDrag = (data) => { };
    public Action<PointerEventData> onEndDrag = (data) => { };

    GameObject PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function, GameObject selectedGameObject = null) where T : IEventSystemHandler
    {
        EventSystem.current.RaycastAll(data, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            var raycast = raycastResults[i].gameObject;
            if (raycast != gameObject)
            {
                if (selectedGameObject == null || IsSelfOrAncestor(selectedGameObject.transform, raycast.transform))
                {
                    raycast = ExecuteEvents.ExecuteHierarchy(raycast, data, function);
                    return raycast;
                }
            }
        }

        return null;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData data)
    {
        Debug.Log("OnPointerDown : 1");
        if (downGameObject)
        {
            return;
        }
        Debug.Log("OnPointerDown : 2");
        downGameObject = PassEvent(data, ExecuteEvents.pointerDownHandler);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData data)
    {
        Debug.Log("OnPointerUp : 1");
        if (downGameObject)
        {
            ExecuteEvents.ExecuteHierarchy(downGameObject, data, ExecuteEvents.pointerUpHandler);
            Debug.Log("OnPointerUp : 2");
        }
        upGameObject = downGameObject;
        downGameObject = null;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData data)
    {
        if (upGameObject)
        {
            PassEvent(data, ExecuteEvents.pointerClickHandler, upGameObject);
        }
        upGameObject = null;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData data)
    {
        if (dragGameObject || isCustomDrag)
        {
            return;
        }

        isCustomDrag = true;
        dragDelta = 0;
        if (!onCheckDrag(data))
        {
            Debug.Log("onCheckDrag : false");
            dragGameObject = PassEvent(data, ExecuteEvents.beginDragHandler);
            if (dragGameObject)
            {
                Debug.Log("dragGameObject : true");
                isCustomDrag = false;
            }
        }

        if (isCustomDrag)
        {
            onBeginDrag(data);
        }
    }

    void IDragHandler.OnDrag(PointerEventData data)
    {
        if (isCustomDrag)
        {
            onDrag(data);
        }
        else if (dragGameObject)
        {
            ExecuteEvents.ExecuteHierarchy(dragGameObject, data, ExecuteEvents.dragHandler);
        }
        else
        {
            return;
        }

        if (downGameObject)
        {
            dragDelta += data.delta.sqrMagnitude;
            if (dragDelta > 30)
            {
                ExecuteEvents.Execute(gameObject, data, ExecuteEvents.pointerUpHandler);
            }
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData data)
    {
        if (isCustomDrag)
        {
            isCustomDrag = false;
            onEndDrag(data);
        }

        if (dragGameObject)
        {
            ExecuteEvents.ExecuteHierarchy(dragGameObject, data, ExecuteEvents.endDragHandler);
            dragGameObject = null;
        }
    }
}