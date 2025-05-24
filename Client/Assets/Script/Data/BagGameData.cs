using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameData
{

    public class BagItemData
    {
        public int configId;
        public int count;
    }


    public class BagGameData : BaseGameData
    {
        //public int flag;//待定
        List<BagItemData> bagItemList = new List<BagItemData>();
        Dictionary<int, BagItemData> bagItemDic = new Dictionary<int, BagItemData>();

        public List<BagItemData> BagItemList { get => bagItemList; set => bagItemList = value; }
        public Dictionary<int, BagItemData> BagItemDic { get => bagItemDic; }

        public void SetBagItemList(RepeatedField<NetProto.Item> serverItems)
        {
            for (int i = serverItems.Count - 1; i >= 0; --i)
            {
                var serverItem = serverItems[i];
                //Logx.Log("bag : serverItem " + serverItem.ConfigId + " " + serverItem.Count);
                UpdateItem(serverItem);
            }

            EventDispatcher.Broadcast(EventIDs.OnRefreshBagData);
        }

        public void UpdateItem(NetProto.Item serverItem)
        {
            var configId = serverItem.ConfigId;
            var count = serverItem.Count;
            if (count > 0)
            {
                var localItem = GetByConfig(configId);
                if (null == localItem)
                {
                    localItem = new BagItemData();
                    BagItemDic.Add(configId, localItem);
                    BagItemList.Add(localItem);
                }

                localItem.configId = serverItem.ConfigId;
                localItem.count = serverItem.Count;
            }
            else
            {
                //del

                bagItemList.RemoveAll(it => it.configId == configId);
                BagItemDic.Remove(configId);

                //Logx.Log("bag : del " + bagItemList.Count + " " + BagItemDic.Count);
            }

        }

        public BagItemData GetByConfig(int configId)
        {
            if (BagItemDic.ContainsKey(configId))
            {
                return BagItemDic[configId];
            }

            return null;
        }

        public int GetCountByConfigId(int configId)
        {
            var count = 0;
            var item = GetByConfig(configId);
            if (item != null)
            {
                count = item.count;
            }

            return count;
        }
    }

}
