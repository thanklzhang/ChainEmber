using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemShowObj
{
    public GameObject gameObject;
    public float scaleDelta;
    public int index;
}

public class ScrollScale : MonoBehaviour
{
    public Transform root;
    public GameObject prefab;
    public Transform mask;

    public float maxScale = 1.0f;
    public float minScale = 0.70f;
    public float xSpace = 164.0f;
    public float scaleAreaWidth = 237.2f;
    public ScrollRect scroll;

    List<ItemShowObj> showObjList = new List<ItemShowObj>();

    void OnClick(int i)
    {
        var rootRectTran = root.GetComponent<RectTransform>();
        var x = i * xSpace;
        rootRectTran.anchoredPosition = new Vector2(-x, rootRectTran.anchoredPosition.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        var count = 10;
        for (int i = 0; i < count; i++)
        {
            var obj = GameObject.Instantiate(prefab, root, false);
            obj.SetActive(true);
            ItemShowObj showObj = new ItemShowObj()
            {
                gameObject = obj,
                index = i
            };
            showObjList.Add(showObj);

            var btn = showObj.gameObject.transform.Find("root/pic").GetComponent<Button>();
            int a = i;
            btn.onClick.AddListener(() =>
            {
                OnClick(a);
            });
        }

        var rootRectTran = root.GetComponent<RectTransform>();
        var pre = rootRectTran.sizeDelta;
        var maskRect = mask.GetComponent<RectTransform>();
        var halfWidth = maskRect.rect.width / 2;
        var length = halfWidth + (count - 1) * xSpace + halfWidth;
        rootRectTran.sizeDelta = new Vector2(length, pre.y);
    }

    public int currTargetIndex = -1;

    public void OnStartDrag()
    {
        preMousePos = Input.mousePosition;
        isScrollStart = true;
        currTargetIndex = -1;
    }

    public void OnEndDrag()
    {
        isScrollStart = false;

        var len = (Input.mousePosition - preMousePos).sqrMagnitude;
        if (len < 0.2f)
        {
            return;
        }
        var maxScale = 0.0f;
        int maxIndex = 0;
        for (int i = 0; i < showObjList.Count; i++)
        {
            var showObj = showObjList[i];
            if (showObj.scaleDelta >= maxScale)
            {
                maxIndex = i;
                maxScale = showObj.scaleDelta;
            }
        }
        currTargetIndex = maxIndex;
    }

    public Vector3 preMousePos;
    public bool isScrollStart;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnStartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnEndDrag();
        }

        var maskRect = mask.GetComponent<RectTransform>();
        var halfWidth = maskRect.rect.width / 2;

        List<ItemShowObj> scaleList = new List<ItemShowObj>();
        for (int i = 0; i < showObjList.Count; i++)
        {
            var showObj = showObjList[i];
            var go = showObj.gameObject;

            var tran = go.transform;
            tran.localPosition = new Vector3(i * xSpace + halfWidth, 0, 0);

            var scaleDelta = 0.0f;

            var contentAbsX = Mathf.Abs(root.localPosition.x);

            var rootRectAnchored = root.GetComponent<RectTransform>().anchoredPosition;
            var absX = Mathf.Abs(tran.localPosition.x + rootRectAnchored.x - halfWidth);

            if (absX >= scaleAreaWidth)
            {
                scaleDelta = minScale;
            }
            else
            {
                scaleDelta = maxScale - (absX / scaleAreaWidth) * (maxScale - minScale);
            }

            showObj.scaleDelta = scaleDelta;
            tran.localScale = new Vector3(scaleDelta, scaleDelta, 1.0f);
            scaleList.Add(showObj);

        }
        scaleList.Sort((a, b) =>
        {
            if (a.scaleDelta == b.scaleDelta)
            {
                return 0;
            }
            if (a.scaleDelta < b.scaleDelta)
            {
                return 1;
            }
            return -1;
        });

        for (int i = 0; i < scaleList.Count; i++)
        {
            var showObj = scaleList[i];
            showObj.gameObject.transform.SetAsFirstSibling();
        }

        //scroll move
        if (currTargetIndex >= 0)
        {
            var rootRectTran = root.GetComponent<RectTransform>();
            var currRootX = rootRectTran.anchoredPosition.x;
            var targetX = -(currTargetIndex * xSpace);

            rootRectTran.anchoredPosition = Vector2.Lerp(rootRectTran.anchoredPosition,
                new Vector2(targetX, rootRectTran.anchoredPosition.y), Time.deltaTime * 10.0f);
        }
    }
}
