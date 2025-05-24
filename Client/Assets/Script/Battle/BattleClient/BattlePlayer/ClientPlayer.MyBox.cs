using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        //拥有的宝箱配置id list
        // public List<int> boxConfigIdList;
        // private List<BattleClientMsg_BattleBox> boxList;
        private Dictionary<RewardQuality, MyBoxGroup> boxDic;

        //当前打开的宝箱
        public BattleClientMsg_BattleBox currOpenBox;

        public void InitMyBox()
        {
            
        }

        public void SetMyBoxList(Dictionary<RewardQuality,MyBoxGroup> boxDic)
        {
            // if (BattleManager.Instance.GetLocalPlayer().playerIndex == this.playerIndex)
            // {
            //     Logx.Log(LogxType.BattleBox, "BattlePlayer_Client : SetBoxList : count : " + boxDic.Count);
            // }

            this.boxDic = boxDic;
            EventDispatcher.Broadcast(EventIDs.OnUpdateMyBoxInfo);
        }

        public void OnOpenBox(BattleClientMsg_BattleBox box)
        {
            Logx.Log(LogxType.BattleBox,
                "BattlePlayer_Client : OnOpenBox : box.selections?.Count : " + box.selections?.Count);
            currOpenBox = box;

            EventDispatcher.Broadcast(EventIDs.OnBoxOpen);
        }

        // public void OnSelectBox(int index)
        // {
        //     
        // }

        public int GetBoxTotalCount()
        {
            if (null == boxDic)
            {
                return 0;
            }

            var totalCount = 0;
            foreach (var kv in boxDic)
            {
                var count = GetMyBoxCount(kv.Key);
                totalCount += count;
            }

            return totalCount;
        }

        public int GetMyBoxCount(RewardQuality quality)
        {
            if (null == boxDic)
            {
                return 0;
            }

            if (!boxDic.ContainsKey(quality))
            {
                return 0;
            }

            var boxGroup = boxDic[quality];
            if (null == boxGroup)
            {
                return 0;
            }


            return boxGroup.count;
        }


        public void TryOpenBox(RewardQuality quality)
        {
            if (boxDic.ContainsKey(quality))
            {
                var boxGroup = boxDic[quality];
                if (boxGroup.count > 0)
                {
                    Logx.Log(LogxType.BattleBox, "BattlePlayer_Client : TryOpenBox : quality : " + quality);

                    BattleManager.Instance.MsgSender.Send_OpenBox(quality);
                }
                else
                {
                    Logx.Log(LogxType.BattleBox, "BattlePlayer_Client : TryOpenBox : box list count is 0 : ");
                }
            }
            else
            {
                Logx.Log(LogxType.BattleBox, "BattlePlayer_Client : TryOpenBox : no contain quality : " + quality);
            }
        }
        //获取所有商店宝箱信息
        public Dictionary<RewardQuality, MyBoxGroup> GetAllMyBoxItems()
        {
            return boxDic;
        }
    }
    
  
}