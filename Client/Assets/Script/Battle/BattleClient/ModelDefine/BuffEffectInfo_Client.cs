namespace Battle_Client
{
    public class BuffEffectInfo_Client
    {
        public int guid;
        public int configId;
        public int targetEntityGuid;
        public float currCDTime;
        public float maxCDTime;
        public int linkTargetEntityGuid;
        public int stackCount;

        //public int iconResId;
        public bool isRemove;
        public bool isCreate;

        public void SetRemoveState()
        {
            this.isRemove = true;
            this.isCreate = false;
        }

        public void SetCrateState()
        {
            this.isCreate = true;
            this.isRemove = false;
        }

        public static BuffEffectInfo_Client  ToBuffClient(Battle.BuffEffectInfo buffInfo)
        {
            var buffInfo_client = new BuffEffectInfo_Client()
            {
                guid = buffInfo.guid,
                targetEntityGuid = buffInfo.targetEntityGuid,
                currCDTime = buffInfo.currCDTime / 1000.0f,
                maxCDTime = buffInfo.maxCDTime / 1000.0f,
                stackCount = buffInfo.statckCount,
                linkTargetEntityGuid = buffInfo.linkTargetEntityGuid,
                configId = buffInfo.configId,
                //iconResId = buffInfo.iconResId
            };

            return buffInfo_client;
        }
        
    }
    
  
}