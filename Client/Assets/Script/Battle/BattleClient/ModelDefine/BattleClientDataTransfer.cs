using Google.Protobuf.Collections;
using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    //这些都是战斗相关的输出输入的统一收敛数据

    //客户端创建战斗所需参数战斗
    public class BattleClient_CreateBattleArgs
    {
        public int guid;
        public int configId;
        public int roomId;
        public List<BattleClient_ClientPlayer> clientPlayers;
        public List<BattleClientMsg_Entity> entityList;
        public int mapSizeX;
        public int mapSizeZ;
    }

    public class BattleClient_ClientPlayer
    {
        public int playerIndex;
        public int team;
        public long uid;
        public int ctrlHeroGuid;


        public BattleClientMsg_BoxShop boxShop;
        public BattleClient_Currency currency;
        public BattleClientMsg_BattleMyBox myBox;
    }

    
    
    public class BattleClient_Currency
    {
        public Dictionary<int, BattleClient_CurrencyItem> currencyDic;
    }

    public class BattleClient_CurrencyItem
    {
        public int itemId;
        public int count;
    }


    // public class BattleClientMsg_InitArg
    // {
    //     public int battleGuid;
    //     public int battleTableId;
    //     public int battleRoomId;
    //
    //     public Dictionary<int, BattleClientMsg_ClientPlayer> players;
    //
    // }

    // public class BattleClientMsg_ClientPlayer
    // {
    //     public int playerIndex;
    //     public int team;
    //     public int uid;
    //     public int ctrlHeroGuid;
    // }

    public class BattleClientMsg_BoxShop
    {
        public Dictionary<RewardQuality, BattleClientMsg_BoxShopItem> shopItems;
    }

    public class BattleClientMsg_BoxShopItem
    {
        public int configId;

        // public int quality;
        public int canBuyCount;
        public int maxBuyCount;
        public int costItemId;
        public int costCount;
    }

    public class BattleClientMsg_Entity
    {
        public int guid;
        public int configId;
        public int playerIndex;
        public int level;
        public UnityEngine.Vector3 position;
        public List<BattleClientMsg_Skill> skills;
        // public List<BattleClientMsg_Item> itemList;

        public EntityLocationType localType;
    }

    public class BattleClientMsg_Skill
    {
        public int configId;
        public int level;
        internal float maxCDTime;
    }

    public class BattleClientMsg_Item
    {
        public int configId;
        public int count;
    }

    public class BattleClientMsg_BattleAttr
    {
        public EntityAttrType type;
        public float value;
    }

    public class BattleClientMsg_BattleStateValue
    {
        public EntityStateValueType type;
        public float value;

        //来源实体 如伤害来源
        public int fromEntityGuid;
    }

    public class BattleClientMsg_CreateSkillTrack
    {
        //public List<int> strackIdList;
        public int trackConfigId;
        public int releaserEntityGuid;
        public Vector3 targetPos;
        public int targetEntityGuid;
    }

    //玩家箱子部分
    public class BattleClientMsg_BattleMyBox
    {
        //public Dictionary<RewardQuality, BattleClientMsg_BattleBox> boxDic;
        public Dictionary<RewardQuality, BattleClientMsg_MyBoxQualityGroup> boxGroupDic;
    }

    //玩家箱子品质列表项(只包含品质和数目)
    public class BattleClientMsg_MyBoxQualityGroup
    {
        public RewardQuality quality;
        public int count;
    }

    //开箱子之后的详细箱子信息
    public class BattleClientMsg_BattleBox
    {
        public int playerIndex;
        public int configId;
        public List<BattleClientMsg_BattleBoxSelection> selections;
    }

    public class BattleClientMsg_BattleBoxSelection
    {
        public int rewardConfigId;

        //实际奖励值
        //根据奖励类型 得到不同的实际奖励意义 如技能id 等
        // public List<int> intValueList;

        public BattleReward_Client battleReward;
    }
}