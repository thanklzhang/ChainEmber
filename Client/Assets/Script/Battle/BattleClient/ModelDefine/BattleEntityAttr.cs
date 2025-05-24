using System.Collections.Generic;
using System.Linq;
using Battle;
using Config;

namespace Battle_Client
{
    public class AttrOption_Client
    {
        public EntityAttrType type;
        public float value;
    }

    public class BattleEntityAttr
    {
        //public List<AttrOption_Client> attrList = new List<AttrOption_Client>();
        public Dictionary<EntityAttrType, AttrOption_Client> attrDic;

        public void Init()
        {
            //attrList = new List<AttrOption_Client>();
            attrDic = new Dictionary<EntityAttrType, AttrOption_Client>();
        }

        public float GetValue(EntityAttrType attrType)
        {
            var option = GetOption(attrType);

            if (option != null)
            {
                return option.value;
            }

            return 0;
        }

        public AttrOption_Client GetOption(EntityAttrType attrType)
        {
            if (attrDic.ContainsKey(attrType))
            {
                return attrDic[attrType];
            }

            return null;
        }

        public void SetAttr(EntityAttrType attrType, float value)
        {
            AttrOption_Client option = null;
            if (attrDic.ContainsKey(attrType))
            {
                option = attrDic[attrType];
            }
            else
            {
                option = new AttrOption_Client();
                option.type = attrType;
                attrDic.Add(attrType, option);
            }

            option.value = value;
        }

        public List<AttrOption_Client> GetAttrList()
        {
            var list = attrDic.Select(attr => attr.Value).ToList();
            list.Sort((a, b) => { return a.type.CompareTo(b.type); });
            return list;
        }


        // public float attack;
        // public float defence;
        // public float maxHealth;
        // public float attackSpeed;
        // public float attackRange;
        // public float moveSpeed;
        //
        // //????
        // public float attack_Permillage;
        // public float defence_Permillage;
        // public float maxHealth_Permillage;
        // public float attackSpeed_Permillage;
        // public float attackRange_Permillage;
        // public float moveSpeed_Permillage;
        //
        // public float GetValue_pre(EntityAttrType type)
        // {
        //     if (type == EntityAttrType.Attack)
        //     {
        //         // return this.attack * (1 + attack_Permillage / 1000.0f);
        //         return this.attack;
        //     }
        //     else if (type == EntityAttrType.Defence)
        //     {
        //         return this.defence;
        //     }
        //     else if (type == EntityAttrType.MaxHealth)
        //     {
        //         return this.maxHealth;
        //     }
        //     else if (type == EntityAttrType.AttackSpeed)
        //     {
        //         return this.attackSpeed;
        //     }
        //     else if (type == EntityAttrType.AttackRange)
        //     {
        //         return this.attackRange;
        //     }
        //     else if (type == EntityAttrType.MoveSpeed)
        //     {
        //         return this.moveSpeed;
        //     }
        //     else
        //     {
        //         return 0;
        //     }
        // }
    }
}