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
    public void AddItem(int itemId, int count)
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

        SaveData();

        EventDispatcher.Broadcast(EventIDs.OnRefreshItemData,  itemId);
    }

    public void RemoveItem(int itemId, int count)
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

            SaveData();
            
            EventDispatcher.Broadcast(EventIDs.OnRefreshItemData,  itemId);
        }
    }
    
    public BagItem GetItem(int itemId)
    {
        var item = bagData.bagItemList?.Find(x => x.configId == itemId);
        return item;
    }

    public void SaveData()
    {
        UserService.Instance.SaveData();
    }

}