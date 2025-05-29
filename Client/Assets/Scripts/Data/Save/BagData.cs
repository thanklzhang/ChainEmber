using System;
using System.Collections.Generic;

namespace GameData
{
    public class BagItem
    {
        public int configId;
        public int count;
    }

    [Serializable]
    public class BagData
    {
        public List<BagItem> bagItemList;
    }
}