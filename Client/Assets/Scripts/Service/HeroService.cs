using System;
using GameData;

public class HeroService : Singleton<HeroService>
{
    public HeroListData heroListData =>  UserService.Instance.userData.heroListData;

    // public void SetData(HeroListData heroListData)
    // {
    //     this.heroListData = heroListData;
    // }

    public void AddHero(int configId, int level = 1, int star = 1)
    {
        var hero = heroListData.heroList.Find(x => x.configId == configId);
        if (hero != null)
        {
            hero.level = level;
            hero.star = star;
        }
        else
        {
            var guid = heroListData.maxGuid + 1;
            heroListData.heroList.Add(new HeroData() { guid = guid, configId = configId, level = level, star = star });

            heroListData.maxGuid++;
        }

        SaveData();
    }

    public void RemoveHero(int guid)
    {
        var hero = heroListData.heroList.Find(x => x.guid == guid);
        if (hero != null)
        {
            heroListData.heroList.Remove(hero);
            SaveData();
        }
    }

    public HeroData GetHero(int guid)
    {
        var hero = heroListData.heroList.Find(x => x.guid == guid);
        return hero;
    }
    
    public void UpgradeHero(int guid,Action<ServiceResponse> callback,bool isSave = true)
    {
        var hero = heroListData.heroList.Find(x => x.guid == guid);
        if (hero != null)
        {
            //判断消耗
            var consumeCount = GetUpgradeConsumeCount(hero.level);
            var myCount =  BagService.Instance.GetCountByItemId(ItemIds.CoinId);            
            if (myCount < consumeCount)
            {
                callback?.Invoke(ServiceResponse.New(ErrorCode.UpgradeHeroNotEnoughConsume));
                return;
            }
            
            if (hero.level >= HeroConst.MaxHeroLevel)
            {
                callback?.Invoke(ServiceResponse.New(ErrorCode.UpgradeHeroMaxLevel));
                return;
            }
            hero.level++;
            
            BagService.Instance.RemoveItem(ItemIds.CoinId, consumeCount,false);

            if (isSave)
            {
                SaveData();    
            }

            EventDispatcher.Broadcast(EventIDs.OnRefreshHeroData,  hero.guid);
        
            callback?.Invoke(ServiceResponse.Success);
        }

      
    }

    public void SaveData()
    {
        UserService.Instance.SaveData();
    }

    public int GetUpgradeConsumeCount(int heroDataLevel)
    {
        //弄个简单的就行
        return heroDataLevel * 80;
    }
}

public class HeroConst
{
    public const int MaxHeroLevel = 30;
}