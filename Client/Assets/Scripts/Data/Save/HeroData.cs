using System;
using System.Collections.Generic;

namespace GameData
{
    public class HeroData
    {
        public int guid;
        public int configId;
        public int level;
        public int star;
    }

    [Serializable]
    public class HeroListData
    {
        public List<HeroData> heroList;
        public int maxGuid;
    }
}