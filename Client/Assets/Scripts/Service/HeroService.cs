using System;
using GameData;

public class HeroService : Singleton<HeroService>
{
    public HeroListData heroListData;

    public void Init(HeroListData heroListData)
    {
        this.heroListData = heroListData;
    }

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

    public void SaveData()
    {
        UserService.Instance.SaveData();
    }
}