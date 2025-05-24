using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GameData
{
    public class HeroConvert
    {
        public static HeroData ToHeroData(NetProto.HeroProto serverHero)
        {
            HeroData hero = new HeroData();
            hero.guid = serverHero.Guid;
            hero.configId = serverHero.ConfigId;
            hero.level = serverHero.Level;

            return hero;
        }
    }
}