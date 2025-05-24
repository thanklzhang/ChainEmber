using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerOperationManager : Singleton<PlayerOperationManager>
{
    //public void StartAction(IEnumerator ie)
    //{
    //    StartCoroutine(ie);
    //}

    public void Update(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ////这里之后提取出来
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //var hitInfo = Physics.RaycastAll(ray, 1000, 1 << LayerMask.NameToLayer("Entity"));
            //if (hitInfo != null && hitInfo.Length > 0)
            //{
            //    var hitEntity = hitInfo[0];//目前先总是选择第一个
            //    var go = hitEntity.transform.gameObject;
            //    Debug.Log("zxy : click obj : " + go.name);

            //    EventManager.Broadcast((int)GameEvent.PlayerClickEntity, go);

            //    //var instanceId = hitEntity.transform.gameObject.GetInstanceID();
            //    //var entity = CombatManager.Instance.GetEntityByInstanceId(instanceId);
            //    //if (entity != null)
            //    //{

            //    //    EventManager.Broadcast((int)GameEvent.CombatEnd);
            //    //}
            //    //else
            //    //{
            //    //    Debug.LogWarning("zxy : " + "the entity doesnt exist , instanceId : " + instanceId);
            //    //}
            //}

        }
    }
}

