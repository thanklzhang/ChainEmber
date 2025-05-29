using System;
using System.Collections.Generic;

namespace GameData
{
    public class HeroItemData
    {
        public int configId;
        public int level;
        public int star;
    }

    [Serializable]
    public class HeroData
    {
        public List<HeroItemData> heroList;
    }
}