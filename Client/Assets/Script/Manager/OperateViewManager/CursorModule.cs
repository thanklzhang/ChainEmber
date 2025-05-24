using NetProto;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType
{
    Normal = 0,
    Attack = 1,
    Select = 2,
    SelectAttack = 3
}

//懒得再写类了 先这样吧
public class CursorModule
{
    //正常光标
    Texture2D normalCursor;
    //攻击敌人的光标
    Texture2D attackCursor;
    //技能等操作选择的光标
    Texture2D selectCursor;
    //技能等操作选择的时候目标是敌人的光标
    Texture2D selectAttackCursor;

    public CursorType currType;

    bool isFinishLoad;
    int finishLoadCount;
    public void Init()
    {
        isFinishLoad = false;
        finishLoadCount = 0;
        currType = CursorType.Normal;
    }

    public void StartLoad()
    {
        ResourceManager.Instance.GetObject<Texture2D>((int)Config.ResIds.cursor_normal_005, (tex) =>
        {
            if (tex != null)
            {
                normalCursor = tex;

                finishLoadCount += 1;
                CheckIsLoadFinish();
            }
        });
        ResourceManager.Instance.GetObject<Texture2D>((int)Config.ResIds.cursor_attack_001, (tex) =>
        {
            if (tex != null)
            {
                attackCursor = tex;

                finishLoadCount += 1;
                CheckIsLoadFinish();
            }
        });
        ResourceManager.Instance.GetObject<Texture2D>((int)Config.ResIds.cursor_002, (tex) =>
        {
            if (tex != null)
            {
                selectCursor = tex;

                finishLoadCount += 1;
                CheckIsLoadFinish();
            }
        });
        ResourceManager.Instance.GetObject<Texture2D>((int)Config.ResIds.cursor_select_attack_001, (tex) =>
        {
            if (tex != null)
            {
                selectAttackCursor = tex;

                finishLoadCount += 1;
                CheckIsLoadFinish();
            }
        });
    }

    public void CheckIsLoadFinish()
    {
        if (finishLoadCount >= 4)
        {
            isFinishLoad = true;
            OnAllFinishLoad();

        }
    }

    public void OnAllFinishLoad()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);

        SetCursor(currType);
    }

    public void SetCursor(CursorType type)
    {
        if (currType == type)
        {
            return;
        }
        currType = type;

        if (!isFinishLoad)
        {
            return;
        }

        if (currType == CursorType.Normal)
        {
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (currType == CursorType.Attack)
        {
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (currType == CursorType.Select)
        {
            Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (currType == CursorType.SelectAttack)
        {
            Cursor.SetCursor(selectAttackCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
