
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public Button btn;
    void Awake()
    {
        SetInputProxy();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("zxy : click ");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform inputController;
    public Transform rootGridLayout;
    public void SetInputProxy()
    {
        InputProxy inputProxy=  inputController.GetComponent<InputProxy>();
        inputProxy.onCheckDrag = (data) =>
        {
            Debug.Log("zxy : onCheckDrag");
            var delta = data.delta;
            return Mathf.Abs(delta.x) > Mathf.Abs(1.2f * delta.y);
        };
        
        inputProxy.onBeginDrag = (data) =>
        {
            Debug.Log("zxy : onBeginDrag");
            ExecuteEvents.ExecuteHierarchy(rootGridLayout.gameObject, data, ExecuteEvents.beginDragHandler);
        };
        
        inputProxy.onDrag = (data) =>
        {
          Debug.Log("zxy : OnDrag");
            ExecuteEvents.ExecuteHierarchy(rootGridLayout.gameObject, data, ExecuteEvents.dragHandler);
        };
        
        inputProxy.onEndDrag = (data) =>
        {
            Debug.Log("zxy : onEndDrag");
            ExecuteEvents.ExecuteHierarchy(rootGridLayout.gameObject, data, ExecuteEvents.endDragHandler);
        };

        // ScrollRect scrollRect = rootGridLayout.GetComponentInParent<ScrollRect>();
        // if (!scrollRect.GetComponent<EventTrigger>())
        // {
        //     EventTrigger eventTrigger = scrollRect.AddComponent<EventTrigger>();
        //     EventTrigger.Entry entry = new EventTrigger.Entry();
        //     entry.eventID = EventTriggerType.EndDrag;
        //     entry.callback.AddListener((data) => MoveNearIndex());
        //     eventTrigger.triggers.Add(entry);
        // }

    }
}
