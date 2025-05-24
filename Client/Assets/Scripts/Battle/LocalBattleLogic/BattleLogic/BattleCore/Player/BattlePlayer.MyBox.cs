using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    //该玩家的宝箱仓库
    public partial class BattlePlayer
    {
        public Dictionary<RewardQuality, List<BattleBox>> myBoxDic =
            new Dictionary<RewardQuality, List<BattleBox>>();

        public void InitMyBox()
        {
        }

        //根据配置增加一个宝箱
        public void GainBoxByConfigId(int configId, bool isSync = true)
        {
            // Battle_Log.Log(200, "BattleBox : GainBox : configId : " + configId);

            BattleBox box = new BattleBox();
            box.Init(configId, this);

            List<BattleBox> list = new List<BattleBox>() { box };
            GainBoxes(list, isSync);
        }

        public void GainBoxes(List<int> configIds, bool isSync = true)
        {
            for (int i = 0; i < configIds.Count; i++)
            {
                var configId = configIds[i];
                this.GainBoxByConfigId(configId, isSync);
            }
        }

        public void GainBoxes(List<int> configIds, List<int> counts, bool isSync = true)
        {
            for (int i = 0; i < configIds.Count; i++)
            {
                var configId = configIds[i];
                var count = 0;
                if (i < counts.Count)
                {
                    count = counts[i];
                }

                for (int j = 0; j < count; j++)
                {
                    //因为数量可能很多 所以这里总是不同步 最后再统一判断是否同步
                    GainBoxByConfigId(configId, false);
                }
            }

            if (isSync)
            {
                this.SyncBoxInfo();
            }
        }

        public void GainBoxes(List<BattleBox> boxList, bool isSync = true)
        {
            for (int i = 0; i < boxList.Count; i++)
            {
                var box = boxList[i];
                this.GainBox(box);
            }

            if (isSync)
            {
                this.SyncBoxInfo();
            }
        }

        private void GainBox(BattleBox box)
        {
            //根据品质进行存放 box
            var quality = (RewardQuality)box.boxConfig.Quality;
            var qualityBoxList = new List<BattleBox>();
            if (!myBoxDic.ContainsKey(quality))
            {
                qualityBoxList = new List<BattleBox>();
                myBoxDic.Add(quality, qualityBoxList);
            }
            else
            {
                qualityBoxList = myBoxDic[quality];
            }

            qualityBoxList.Add(box);
        }


        public void GainReward(BaseBattleReward reward)
        {   
            //增加奖励到奖励列表中
            this.AddBattleReward(reward);
            
            //触发奖励效果
            reward.ImmediatelyGainRewardEffect();
            
            //同步奖励数据
            SyncBattleReward(reward);
        }

        
        //开宝箱
        public void OpenBox(RewardQuality quality)
        {
            // Battle_Log.LogZxy("BattleBox : OpenBox : quality : " + quality);

            // if (boxList.Count > 0)
            // {
            //     var box = boxList[0];
            //     box.GenSelectionReward();
            //     
            //     this.SyncOpenBox(box);
            // }


            if (myBoxDic.ContainsKey(quality))
            {
                var currBoxList = myBoxDic[quality];
                if (currBoxList.Count > 0)
                {
                    var box = currBoxList[0];
                    box.GenSelectionReward();

                    this.SyncOpenBox(box);
                }
            }
            else
            {
                BattleLog.LogZxy("BattleBox : OpenBox : no containKey : quality : " + quality);
            }
        }

        //选择宝箱
        public void SelectBoxReward(RewardQuality quality, int index)
        {
            // Battle_Log.Log(200, "BattleBox : SelectBoxReward : try to select box : index : " + index);

            if (myBoxDic.ContainsKey(quality))
            {
                var currBoxList = myBoxDic[quality];
                if (currBoxList.Count > 0)
                {
                    var box = currBoxList[0];
                    if (box.IsOpened())
                    {
                        // Battle_Log.Log(200,
                        //     "BattleBox : SelectBoxReward : success to SelectBoxReward : index : " + index);

                        box.Select(index);
                        currBoxList.RemoveAt(0);

                        SyncBoxInfo();
                    }
                }
            }
            else
            {
                BattleLog.Log(200, "BattleBox : SelectBoxReward : no containKey : quality : " + quality);
            }
        }

        public void SyncBoxInfo()
        {
            this.battle.OnUpdateBoxInfo(this.playerIndex, this.myBoxDic);
        }

        //同步开箱子操作
        public void SyncOpenBox(BattleBox box)
        {
            //Logx.Log(LogxType.BattleItem,"entity OnSyncItemInfo : init item list : count : " + itemList.Count);

            this.battle.OnNotifyOpenBox(box);
        }
    }
}