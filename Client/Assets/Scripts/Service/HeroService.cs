using System;
using GameData;

public class HeroService : Singleton<HeroService>
{
    public HeroData heroData;

    public void Init(HeroData heroData)
    {
        this.heroData = heroData;
    }

    public void AddHero(int configId, int level = 1, int star = 1)
    {
        var hero = heroData.heroList.Find(x => x.configId == configId);
        if (hero != null)
        {
            hero.level = level;
            hero.star = star;
        }
        else
        {
            heroData.heroList.Add(new HeroItemData() { configId = configId, level = level, star = star });
        }

        SaveData();
    }

    public void RemoveHero(int configId)
    {
        var hero = heroData.heroList.Find(x => x.configId == configId);
        if (hero != null)
        {
            heroData.heroList.Remove(hero);
            SaveData();
        }
    }

    public void SaveData()
    {
        UserService.Instance.SaveData();
    }
}