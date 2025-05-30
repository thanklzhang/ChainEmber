using System;

namespace GameData
{
    [Serializable]
    public class UserData
    {
        public int uid;
        
        public string deviceId;
        
        public int level;
        
        public string name;
        
        public string avatarURL;
        
        
        public BagData bagData;
        public HeroListData heroListData;

    }
}