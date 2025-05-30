using System;
using GameData;

public class ItemIds
{
    public const int CoinId = 22000001;
}

public class BagService : Singleton<BagService>
{
    public BagData bagData => UserService.Instance.userData.bagData;

    //public void SetData(BagData bagData)
    // {
    //     this.bagData = bagData;
    // }
    //
    public void AddItem(int itemId, int count, bool isSave = true)
    {
        var item = bagData.bagItemList.Find(x => x.configId == itemId);

        if (item != null)
        {
            item.count += count;
        }
        else
        {
            bagData.bagItemList.Add(new BagItem() { configId = itemId, count = count });
        }

        if (isSave)
        {
            SaveData();
        }


        EventDispatcher.Broadcast(EventIDs.OnRefreshItemData, itemId);
    }

    public void RemoveItem(int itemId, int count, bool isSave = true)
    {
        var item = bagData.bagItemList.Find(x => x.configId == itemId);

        if (item != null)
        {
            if (item.count >= count)
            {
                item.count -= count;
            }
            else
            {
                item.count = 0;
            }

            if (isSave)
            {
                SaveData();
            }

            EventDispatcher.Broadcast(EventIDs.OnRefreshItemData, itemId);
        }
    }

    public BagItem GetItem(int itemId)
    {
        var item = bagData.bagItemList?.Find(x => x.configId == itemId);
        return item;
    }

    public int GetCountByItemId(int itemId)
    {
        var item = GetItem(itemId);
        return item?.count ?? 0;
    }

    public void SaveData()
    {
        UserService.Instance.SaveData();
    }
}