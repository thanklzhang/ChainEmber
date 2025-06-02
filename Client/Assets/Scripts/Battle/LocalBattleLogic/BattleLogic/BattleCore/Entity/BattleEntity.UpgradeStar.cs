using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Config;


namespace Battle
{
    //升星部分
    public partial class BattleEntity
    {
        //这个根据经验也能算出是什么星级，但方便取就独立了
        public int starLevel = 1;
        public int currStarExp = 1;

        public void InitStar()
        {
            currStarExp = 1;
        }

        public void AddInitStarExp()
        {
            if (willAddStarExp > 0)
            {
                this.AddStarExp(willAddStarExp);    
            }
        }

        public void AddStarExp(int exp)
        {
            var preStar = starLevel;

            this.currStarExp += exp;

            // BattleLog.LogZxy($"{this.infoConfig.Name} add {exp} exp");

            var nowStar = EntityUpgradeConfigHelper.GetLevelByExp(this.currStarExp);
            this.starLevel = nowStar;

            var upgradeStar = nowStar - preStar;

            // BattleLog.LogZxy($"{this.infoConfig.Name} now star level : {nowStar}");

            SyncStateData();

            // UpgradeStarLevel(upgradeStar);

            if (upgradeStar > 0)
            {
                //装备栏相关--------
                //升星解锁装备栏
                var paramConfig = BattleCommonConfigHelper.GetConfig();
                var unlockList = paramConfig.EntityItemBarCellUnlockStarLevel;
                for (int i = 0; i < unlockList.Count; i++)
                {
                    var unlockLevel = unlockList[i];
                    if (nowStar >= unlockLevel
                        && preStar < unlockLevel)
                    {
                        UnlockItemBarCell(1);
                    }
                }
                //---------------
                
                //大招相关------
                // if (nowStar >= BattleDefine.ultimateUnlockStar
                //     && preStar < BattleDefine.ultimateUnlockStar)
                // {
                //     // //解锁大招
                //     // var skillConfigId = GetUltimateSkillConfigId();
                //     // if (skillConfigId > 0)
                //     // {
                //     //     this.AddSkill(new CreateSkillBean()
                //     //     {
                //     //         configId = skillConfigId,
                //     //         releaser = this
                //     //     });
                //     // }
                //
                //     UnlockUltimateSkill();
                // }
                //------------
                
                // if (this.starLevel == BattleDefine.maxStar
                if (3 == this.starLevel)
                {
                    //大招升级
                    var skillConfigId = GetUltimateSkillConfigId();
                    if (skillConfigId > 0)
                    {
                        this.AddExpToSkill(skillConfigId, 3);                        
                    }

                    
                }
                
                if (4 == this.starLevel)
                {
                    //大招升级
                    var skillConfigId = GetUltimateSkillConfigId();
                    if (skillConfigId != 0)
                    {
                        this.AddExpToSkill(skillConfigId, 99999);
                    }
                }

                this.attrMgr.RefreshEntityStarAttr();
                SyncAllAttr();
            }
        }
        public void UnlockUltimateSkill()
        {
            //解锁大招
            var skillConfigId = GetUltimateSkillConfigId();
            if (skillConfigId > 0)
            {
                this.AddSkill(new CreateSkillBean()
                {
                    configId = skillConfigId,
                    releaser = this
                });
            }
        }
        
        //
        // public void UpgradeStarLevel(int upgradeStarCount)
        // {
        //     if (upgradeStarCount > 0)
        //     {
        //         
        //         if (this.starLevel == BattleDefine.maxStar)
        //         {
        //             //满星
        //         }
        //
        //         this.attrMgr.RefreshEntityStarAttr();
        //         SyncAllAttr();
        //     }
        // }
    }
 
}