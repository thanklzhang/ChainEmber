using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Battle
{
    public class LinkGroupEffect : SkillEffect
    {
        public ILinkGroupEffect tableConfig;
        Battle battle;

        public List<BattleEntity> linkEntityList;

        private float maxLastTime;
        private float currLastTimer;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = BattleConfigManager.Instance.GetById<ILinkGroupEffect>(this.configId);

            linkEntityList = new List<BattleEntity>();

            var maxLinkCount = this.tableConfig.MaxLinkEntityCount;
            if (1 == tableConfig.IsAddReleaser)
            {
                linkEntityList.Add(this.context.fromSkill.releaser);
            }

            var needAddCount = maxLinkCount - linkEntityList.Count;
            if (needAddCount > 0)
            {
                if (this.context.selectEntities.Count > 1)
                {
                    //需要增加的实体数目在 selectEntities 不一定够用
                    var canGetCount = needAddCount;
                    if (this.context.selectEntities.Count < needAddCount)
                    {
                        canGetCount = this.context.selectEntities.Count;
                    }

                    var indexList = BattleRandom.GetRandIndexes(0, this.context.selectEntities.Count, canGetCount,
                        this.battle);
                    for (int i = 0; i < indexList.Count; i++)
                    {
                        var index = indexList[i];
                        linkEntityList.Add(this.context.selectEntities[index]);
                    }
                }
                else
                {
                    //1 个单位或者以下不能链接
                }
            }

            this.maxLastTime = this.tableConfig.LastTime;
            currLastTimer = this.maxLastTime;
        }

        public override CreateEffectInfo ToCreateEffectInfo()
        {
            CreateEffectInfo createInfo = new CreateEffectInfo();
            createInfo.guid = guid;
            // createInfo.resId = tableConfig.LinkResId;
            createInfo.effectPosType = EffectPosType.Custom_Pos;
            createInfo.followEntityGuid = -1;
            if (createInfo.resId > 0)
            {
                createInfo.isAutoDestroy = true;
            }

            // var groupGuidList = linkEntityList.Select(item => item.guid).ToList<int>();
            // createInfo.groupEntityGuidList = groupGuidList;
            createInfo.isSync = false;

            return createInfo;
        }

        public override void OnStart()
        {
            base.OnStart();

            //创建 link
            CreateLinks();
        }

        private void CreateLinks()
        {
            
            for (int i = 0; i < linkEntityList.Count; i++)
            {
               
                
                var aEntity = linkEntityList[i];
                BattleEntity bEntity = null;
                if (i + 1 < linkEntityList.Count)
                {
                    bEntity = linkEntityList[i + 1];
                }

                if (this.tableConfig.StartEffectList != null)
                {
                    //这里目前理论上只有一个 effect 进行相连，先按照多个来写
                    
                    var newContext = context.Copy();
                    newContext.selectEntities = new List<BattleEntity>();
                    newContext.selectEntities.Add(aEntity);
                    if (bEntity != null)
                    {
                        newContext.selectEntities.Add(bEntity);
                    }
                    newContext.linkGroupEffect = this;
                    
                    
                    // for (int j = 0; j < this.tableConfig.StartEffectList.Count; j++)
                    // {
                    //     var eftConfigId = this.tableConfig.StartEffectList[j];
                    //
                    //     var newContext = context.Copy();
                    //     newContext.selectEntities = new List<BattleEntity>();
                    //     newContext.selectEntities.Add(aEntity);
                    //     if (bEntity != null)
                    //     {
                    //         newContext.selectEntities.Add(bEntity);
                    //     }
                    //
                    //     // Debug.Log("zxy : aEntity :" + aEntity.guid);
                    //     // Debug.Log("zxy : bEntity :" + bEntity.guid);
                    //     newContext.linkGroupEffect = this;
                    //     var effect = this.battle.AddSkillEffect(eftConfigId, newContext);
                    // }
                    
                    battle.AddSkillEffectGroup(tableConfig.StartEffectList,this.context);

                    
                }
            }
        }

        public override void OnUpdate(float timeDelta)
        {
            currLastTimer -= timeDelta;
            if (currLastTimer <= 0)
            {
                this.SetWillEndState();
            }
        }


        public override void OnEnd()
        {
            
        }
    }


    public enum LinkType
    {
        //连续不间断的 并且不会重复链接
        Continuous = 0,
    }

    public enum LinkEffectType
    {
        ShareDamage = 0,
        CopyDamage = 1,
    }
}