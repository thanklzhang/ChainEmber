using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Battle
{
    public partial class BattleEntity
    {
        public float Attack => attrMgr.GetFinalAttrValue(EntityAttrType.Attack);
        public float Defence => attrMgr.GetFinalAttrValue(EntityAttrType.Defence);
        public float MaxHealth => attrMgr.GetFinalAttrValue(EntityAttrType.MaxHealth);
        public float AttackSpeed => attrMgr.GetFinalAttrValue(EntityAttrType.AttackSpeed);
        public float MoveSpeed => attrMgr.GetFinalAttrValue(EntityAttrType.MoveSpeed);
        public float AttackRange => attrMgr.GetFinalAttrValue(EntityAttrType.AttackRange);
        public float InputDamageRate => attrMgr.GetFinalAttrValue(EntityAttrType.InputDamageRate);
        public float OutputDamageRate => attrMgr.GetFinalAttrValue(EntityAttrType.OutputDamageRate);
        public float CritRate => attrMgr.GetFinalAttrValue(EntityAttrType.CritRate);
        public float CritDamage => attrMgr.GetFinalAttrValue(EntityAttrType.CritDamage);
        public float SkillCD => attrMgr.GetFinalAttrValue(EntityAttrType.SkillCD);
        public float TreatmentRate => attrMgr.GetFinalAttrValue(EntityAttrType.TreatmentRate);
        public float HealthRecoverSpeed => attrMgr.GetFinalAttrValue(EntityAttrType.HealthRecoverSpeed);

        public EntityAttrMgr attrMgr;

        void InitAttrs()
        {
            this.attrMgr = new EntityAttrMgr();
            this.attrMgr.Init(this);
        }

        //添加一组属性（所有属性都属于一个来源地 也就是说 ， guid 都一样）
        public List<AttrOption> AddAttrs(AttrHelper.AttrCreateArg createArg)
        {
            //根据配置创建属性
            var attrOptions = AttrHelper.GetAttrOptions(createArg, this.battle);
            if (attrOptions.Count > 0)
            {
                this.AddAttrs(createArg.attrGroupType, attrOptions);
                return attrOptions;
            }

            return null;
        }

        //所有增加 attr 都会经过这个函数
        public void AddAttrs(EntityAttrGroupType type, List<AttrOption> buffAttrList)
        {
            if (buffAttrList != null)
            {
                var preMaxHP = (int)this.MaxHealth;
                var preHpRatio = this.CurrHealth / this.MaxHealth;
                for (int i = 0; i < buffAttrList.Count; i++)
                {
                    var buffAttr = buffAttrList[i];
                    this.attrMgr.AddAttrValue(type, buffAttr);
                }

                var nowMaxHp = (int)this.MaxHealth;

                if (nowMaxHp != preMaxHP)
                {
                    //前后保持当前生命百分比相同
                    var result = nowMaxHp * preHpRatio;
                    if (result < 1) result = 1;
                    SetCurrHp(result);
                }

                this.SyncAllAttr();
            }
        }

        public void RemoveAttrs(EntityAttrGroupType type, int guid, List<AttrOption> attrList)
        {
            for (int i = 0; i < attrList.Count; i++)
            {
                var buffAttr = attrList[i];

                this.attrMgr.RemoveAttrValue(type, buffAttr.entityAttrType, guid);
            }

            this.SyncAllAttr();
        }


        public float GetEntityAttrFinalValue(EntityAttrType type)
        {
            float resultValue = 0;
            switch (type)
            {
                case EntityAttrType.Attack:
                    resultValue = this.Attack;
                    break;
                case EntityAttrType.Defence:
                    resultValue = this.Defence;
                    break;
                // case EntityAttrType.CurrHealth:
                //     resultValue = this.CurrHealth;
                //     break;
                case EntityAttrType.MaxHealth:
                    resultValue = this.MaxHealth;
                    break;
                case EntityAttrType.AttackSpeed:
                    resultValue = this.AttackSpeed;
                    break;
                case EntityAttrType.AttackRange:
                    resultValue = this.AttackRange;
                    break;
                case EntityAttrType.InputDamageRate:
                    resultValue = this.InputDamageRate;
                    break;
                case EntityAttrType.OutputDamageRate:
                    resultValue = this.OutputDamageRate;
                    break;
                case EntityAttrType.CritRate:
                    resultValue = this.CritRate;
                    break;
                case EntityAttrType.CritDamage:
                    resultValue = this.CritDamage;
                    break;
                case EntityAttrType.SkillCD:
                    resultValue = this.SkillCD;
                    break;
                case EntityAttrType.HealthRecoverSpeed:
                    resultValue = this.HealthRecoverSpeed;
                    break;
            }

            return resultValue;
        }

        public float GetCurrHpRatio()
        {
            var ratio = (float)this.CurrHealth / this.MaxHealth;

            ratio = MathTool.Clamp(ratio, 0, 1);
            return ratio;
        }

        private float currRecoverPerSecondTimer = 0.0f;

        private float recoverPerSecondTotalTime = 1.0f;

        //关于恢复状态的逻辑 如生命恢复
        public void RecoverStateUpdate(float deltaTime)
        {
            currRecoverPerSecondTimer += deltaTime;
            if (currRecoverPerSecondTimer >= recoverPerSecondTotalTime)
            {
                currRecoverPerSecondTimer = 0;
                HandleRecoverLogicPerSecond();
            }
        }

        public void HandleRecoverLogicPerSecond()
        {
            //生命恢复
            var hpRecoverValuePerSecond = this.HealthRecoverSpeed;
            this.AddCurrHp(hpRecoverValuePerSecond);
        }
    }
}