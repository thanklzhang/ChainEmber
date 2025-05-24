using NetProto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OperateViewManager : Singleton<OperateViewManager>
{
    public CursorModule cursorModule;
    public ModelOutlineModule modelOutlineModule;
    public void Init()
    {
        cursorModule = new CursorModule();
        cursorModule.Init();

        modelOutlineModule = new ModelOutlineModule();
        modelOutlineModule.Init();
    }
    //------------------------
    public void StartLoad()
    {
        cursorModule.StartLoad();
    }
    public void SetCursor(CursorType type)
    {
        cursorModule.SetCursor(type);
    }

    //-------------------
    // public void OpenOutline(GameObject go)
    // {
    //     modelOutlineModule.OpenOutline(go);
    // }
}
