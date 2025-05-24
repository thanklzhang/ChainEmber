using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    //属性组
    public class EntityAttrGroup
    {
        Dictionary<EntityAttrType, EntityAttrObj> attrObjDic = new Dictionary<EntityAttrType, EntityAttrObj>();
    
        public Dictionary<EntityAttrType, EntityAttrObj> GetAttrObjs()
        {
            return attrObjDic;
        }

        public void AddAttr(AttrOption _attrOption,Battle battle)
        {
            var dic = attrObjDic;
            var type = _attrOption.entityAttrType;
            EntityAttrObj attrOption = null;
            if (dic.ContainsKey(type))
            {
                attrOption = dic[type];
                attrOption.Add(_attrOption,battle);
            }
            else
            {
                attrOption = new EntityAttrObj();
                attrOption.Init(_attrOption.entityAttrType);
                dic[type] = attrOption;
                attrOption.Add(_attrOption,battle);
            }
        }

        public void RemoveAttr(EntityAttrType type, int id)
        {
            EntityAttrObj attrOption = null;
            var dic = attrObjDic;
            if (dic.ContainsKey(type))
            {
                attrOption = dic[type];
                attrOption.Remove(id);
            }
        }

        public float GetTotalCalculateValue(EntityAttrType type,bool isIncludeContinuous = true)
        {
            var dic = attrObjDic;
            if (dic.ContainsKey(type))
            {
                var option = dic[type];
                return option.GetTotalCalculateValue(isIncludeContinuous);
            }

            return 0;
        }
    }
}
