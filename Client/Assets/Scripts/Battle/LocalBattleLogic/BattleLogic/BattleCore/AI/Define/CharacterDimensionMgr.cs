using System;
using System.Collections.Generic;

namespace Battle
{
    //性格维度管理
    public class CharacterDimensionMgr
    {
        private Dictionary<CharacterDimensionType, CharacterDimension> dimensionDic;

        public void Init()
        {
            dimensionDic = new Dictionary<CharacterDimensionType, CharacterDimension>();
            List<CharacterDimensionType> types = new List<CharacterDimensionType>()
            {
                CharacterDimensionType.Bravery,
                CharacterDimensionType.Compassion,
                // CharacterDimensionType.Bloodthirsty,
                // CharacterDimensionType.Hatred,
                CharacterDimensionType.Tenacity
            };
            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                dimensionDic.Add(type, new CharacterDimension()
                {
                    type = type,
                    value = 0.0f
                });
            }
        }

        public void SetValue(CharacterDimensionType type, float value)
        {
            dimensionDic[type].value = value;
        }

        public float GetValue(CharacterDimensionType type)
        {
            return dimensionDic[type].value;
        }
    }
}