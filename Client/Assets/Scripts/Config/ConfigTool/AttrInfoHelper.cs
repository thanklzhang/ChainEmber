using Battle_Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Battle;

namespace Config
{
    public class AttrInfoOption
    {
        public EntityAttrType type;
        public string name;
        public string describe;
        public int iconResId;
    }

    public class AttrInfoHelper : Singleton<AttrInfoHelper>
    {
        Dictionary<EntityAttrType, AttrInfoOption> attrInfoDic;

        public void Init()
        {
            attrInfoDic = new Dictionary<EntityAttrType, AttrInfoOption>();
            var list = Config.ConfigManager.Instance.GetList<Config.Attribute>();

            foreach (var attrConfig in list)
            {
                AttrInfoOption option = new AttrInfoOption();
                option.type = (EntityAttrType)attrConfig.Type;
                option.name = attrConfig.Name;
                option.describe = attrConfig.Describe;
                option.iconResId = attrConfig.IconResId;

                attrInfoDic.Add(option.type, option);
            }
        }

        public AttrInfoOption GetAttrInfo(EntityAttrType type)
        {
            var option = attrInfoDic[type];
            return option;
        }

        //public static AttrDesOption GetAttrDes(EntityAttrType attrType)
        //{
        //    Config.ConfigManager.Instance.GetDic<>
        //    AttrDesOption option = new AttrDesOption();
        //}
    }
}