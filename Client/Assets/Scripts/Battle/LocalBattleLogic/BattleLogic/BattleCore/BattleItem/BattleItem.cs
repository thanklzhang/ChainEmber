using System.Collections.Generic;


namespace Battle
{
    //战斗道具
    public class BattleItem
    {
        public int configId;
        public IBattleItem tableConfig;
        public int guid = -1;
        public ItemSkill skill;
        public BattleEntity owner;

        public bool isDiscard;

        //道具数目（叠加数目）
        public int count = 1;
        private List<AttrOption> attrs = new List<AttrOption>();

        //初始化
        public void Init(int configId, int guid)
        {
            this.configId = configId;
            this.guid = guid;
            tableConfig = BattleConfigManager.Instance.GetById<IBattleItem>(this.configId);
        }

        public void OnGain(BattleEntity entity, bool isAddLayer = false)
        {
            owner = entity;
            this.isDiscard = false;
            if (!isAddLayer)
            {
                if (this.tableConfig.SkillId > 0)
                {
                    AddSkill();
                }

                AddAttr();
            }
            else
            {
                this.count += 1;
            }
        }


        //增加道具带有的技能效果 包括主动和被动
        public void AddSkill()
        {
            if (null == this.owner)
            {
                return;
            }

            skill = new ItemSkill();
            skill.SetItem(this);
            skill.Init(new CreateSkillBean()
            {
                releaser = this.owner,
                configId = tableConfig.SkillId
            });
        }

        //增加道具带有的属性
        public void AddAttr()
        {
            if (null == this.owner)
            {
                return;
            }

            GenAttrs();
        }

        private void GenAttrs()
        {
            attrs = new List<AttrOption>();
            if (this.tableConfig.AttrGroupConfigId > 0)
            {
                var attrArg = new AttrHelper.AttrCreateArg();
                attrArg.guid = this.guid;
                attrArg.attrGroupConfigId = this.tableConfig.AttrGroupConfigId;
                attrArg.attrGroupType = EntityAttrGroupType.Item;

                this.attrs = this.owner.AddAttrs(attrArg);
            }
        }

        //主动使用
        public void SuccessToUse(int targetGuid, Vector3 targetPos,Vector3 mousePos)
        {
            StartUseEffect(targetGuid, targetPos,mousePos);

            if (1 == this.tableConfig.IsDestroyAfterUse)
            {
                this.count -= 1;
                //如果之后有叠层效果还累加的需求 那么这里应该还要去除技能效果和属性的减计算
            }

            if (this.count <= 0)
            {
                //销毁道具
                SetDestroyState();
            }
        }

        public virtual void StartUseEffect(int targetGuid, Vector3 targetPos,Vector3 mousePos)
        {
            skill.Start(targetGuid, targetPos,mousePos);
        }

        public void Update(float deltaTime)
        {
            if (null == this.owner)
            {
                return;
            }

            skill?.Update(deltaTime);
        }

        //当被移除的时候 ， 不是销毁
        public void Remove()
        {
            RemoveSkill();
            RemoveAttr();
            owner = null;
        }

        // //当丢弃的时候（如果叠层 那就是全部丢弃）
        // public void OnDiscard()
        // {
        //     RemoveSkill();
        //     RemoveAttr();
        //
        //     owner = null;
        // }

        //移除技能效果 目前只移除被动技能 主动技能可能需要改周期待定（因为释放后可能会丢弃道具，
        //这时候需要在别处进行生命周期运行下去）
        void RemoveSkill()
        {
            // var passiveEftId = 0;
            // owner.DeletePassiveSkill(passiveEftId);
            skill?.Remove();
            skill = null;
        }

        //移除属性
        void RemoveAttr()
        {
            if (attrs != null && attrs.Count > 0)
            {
                this.owner.RemoveAttrs(EntityAttrGroupType.Item, this.guid, attrs);
            }
        }

        public bool isWillDestroy;

        //即将销毁
        public virtual void SetDestroyState()
        {
            // this.owner?.DiscardItem(this);
            isWillDestroy = true;
        }

        public void OnDestroy()
        {
            this.count = 0;
            this.owner = null;
        }
    }
}