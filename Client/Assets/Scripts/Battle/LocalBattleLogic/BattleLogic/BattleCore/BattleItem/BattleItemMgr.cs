using System.Collections.Generic;

namespace Battle
{
    //战斗道具
    public class BattleItemMgr
    {
        private int maxGuid = 1;

        public Dictionary<int, BattleItem> itemDic;

        //初始化
        public void Init()
        {
            itemDic = new Dictionary<int, BattleItem>();
        }

        public void Update(float deltaTime)
        {
            List<BattleItem> willDelList = new List<BattleItem>();

            var keys = new List<int>(itemDic.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var item = itemDic[key];
                item.Update(deltaTime);
                if (item.isWillDestroy)
                {
                    willDelList.Add(item);
                }
            }

            foreach (var item in willDelList)
            {
                item.Remove();
                item.OnDestroy();
                itemDic.Remove(item.guid);
            }
        }

        public List<BattleItem> GenerateItems(int configId, int count = 1)
        {
            List<BattleItem> itemList = new List<BattleItem>();
            for (int i = 0; i < count; i++)
            {
                BattleItem item = new BattleItem();

                item.Init(configId, maxGuid);
                itemList.Add(item);

                itemDic.Add(maxGuid, item);
                maxGuid += 1;
            }

            return itemList;
        }

        public BattleItem GenerateItem(int configId)
        {
            var list = GenerateItems(configId, 1);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        public BattleItem FindItem(int guid)
        {
            if (itemDic.ContainsKey(guid))
            {
                return itemDic[guid];
            }

            return null;
        }

        public void Release()
        {
        }
    }
}