using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleEntityManager
    {
        /// <summary>
        /// 返回当前战斗中所有实体加载请求
        /// </summary>
        /// <param name="finishCallback"></param>
        /// <returns></returns>
        public List<LoadObjectRequest> MakeCurrBattleAllEntityLoadRequests(
            Action<BattleEntity_Client, GameObject> finishCallback)
        {
            var battleEntityDic = this.entityDic;
            return this.MakeMoreBattleEntityLoadRequests(battleEntityDic, finishCallback);
        }

        /// <summary>
        /// 返回多个战斗实体的加载请求
        /// </summary>
        public List<LoadObjectRequest> MakeMoreBattleEntityLoadRequests(
            Dictionary<int, BattleEntity_Client> battleEntityDic,
            Action<BattleEntity_Client, GameObject> finishCallback)
        {
            List<LoadObjectRequest> list = new List<LoadObjectRequest>();
            foreach (var item in battleEntityDic)
            {
                var entity = item.Value;
                var req = MakeBattleEntityLoadRequest(entity, finishCallback);
                list.Add(req);
            }

            return list;
        }

        /// <summary>
        /// 加载现在所有实体 一般再战斗开始加载的时候调用
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadInitEntities()
        {
            int total = entityDic.Count;
            int finishCount = 0;
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                var path = entity.path;
                bool isFinish = true;
                ResourceManager.Instance.GetObject<GameObject>(path, (gameObject) =>
                {
                    //viewEntity.OnLoadModelFinish(gameObject);
                    // this.OnFinishLoadEntityObj(battleEntity, gameObject);
                    entity.OnLoadModelFinish(gameObject);
                    finishCount += 1;
                });
            }

            if (total > 0)
            {
                while (finishCount < total)
                {
                    yield return null;

                    //0.5 -> 0.9
                    var resultProgress = 0.5f + 0.4f * (finishCount / (float)total);
                    EventSender.SendLoadingProgress(resultProgress, "加载 战斗实体 中");
                }
            }

            yield return null;
        }


        /// <summary>
        /// 返回一个战斗实体的加载请求
        /// </summary>
        public LoadObjectRequest MakeBattleEntityLoadRequest(BattleEntity_Client battleEntity,
            Action<BattleEntity_Client, GameObject> finishCallback)
        {
            var req = new LoadBattleViewEntityRequest(battleEntity)
            {
                selfFinishCallback = finishCallback
            };
            return req;
        }
    }
}