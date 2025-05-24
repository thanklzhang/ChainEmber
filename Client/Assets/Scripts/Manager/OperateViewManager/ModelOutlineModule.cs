using NetProto;
using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using UnityEngine;

public class ModelOutline
{
    public GameObject gameObject;
    Material outlineMat;

    public Renderer[] renderers;
    List<Material[]> materials;

    public void Init(GameObject gameObject, Material outlineMat)
    {
        this.gameObject = gameObject;

        this.outlineMat = outlineMat;

        renderers = this.gameObject.GetComponentsInChildren<Renderer>();

        materials = new List<Material[]>();
        foreach (var render in renderers)
        {
            var mats = render.sharedMaterials;
            materials.Add(mats);
        }
    }

    public void AddOutline()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            var mats = materials[i];
            var matList = mats.ToList();
            matList.Add(outlineMat);
            renderers[i].sharedMaterials = matList.ToArray();
        }
    }

    public void RemoveOutline()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            var mats = materials[i];
            var matList = mats.ToList();
            matList.Remove(outlineMat);
            renderers[i].sharedMaterials = matList.ToArray();
        }
    }

    public void Release()
    {
    }
}

public class ModelOutlineModule
{
    Material outlineMat;
    Dictionary<int, ModelOutline> targetGoDic;

    public void Init()
    {
        ResourceManager.Instance.GetObject<Shader>((int)ResIds.OutlineEffect, (res) =>
        {
            outlineMat = new Material(res);
            outlineMat.SetColor("_OutlineColor", new Color(0.5f, 1, 0.5f));

            targetGoDic = new Dictionary<int, ModelOutline>();
        });
    }

    public void OpenOutline(GameObject targetGo, Color color, bool isClearOther = false)
    {
        var insId = targetGo.GetInstanceID();

        ModelOutline modelOutline = null;
        if (!targetGoDic.ContainsKey(insId))
        {
            if (isClearOther)
            {
                this.CloseAllModelOutline();
            }

            modelOutline = new ModelOutline();
            modelOutline.Init(targetGo, this.outlineMat);
            targetGoDic[insId] = modelOutline;

            modelOutline.AddOutline();
            outlineMat.SetColor("_OutlineColor", color);
        }
        else
        {
            //Logx.Log("ModelOutlineModule : CloseOutline : the id is not exist : " + insId);
        }
    }


    public void CloseAllModelOutline()
    {
        List<GameObject> removeGos = new List<GameObject>();
        foreach (var item in targetGoDic)
        {
            var outline = item.Value;
            removeGos.Add(outline.gameObject);
        }

        foreach (var go in removeGos)
        {
            CloseOutline(go);
        }
    }

    public void CloseOutline(GameObject targetGo)
    {
        ModelOutline modelOutline = null;
        var insId = targetGo.GetInstanceID();
        if (targetGoDic.ContainsKey(insId))
        {
            modelOutline = targetGoDic[insId];

            modelOutline.RemoveOutline();
            modelOutline.Release();

            targetGoDic.Remove(insId);
        }
        else
        {
            //Logx.Log("ModelOutlineModule : CloseOutline : the id is not exist : " + insId);
        }
    }
}