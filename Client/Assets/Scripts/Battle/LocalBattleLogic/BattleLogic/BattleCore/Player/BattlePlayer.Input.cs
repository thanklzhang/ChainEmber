using System.Collections.Generic;
using System.Linq;
namespace Battle
{


    public enum BattlePlayerInputKey
    {
        Null = 0,
        KeyCode_A = 97,
        KeyCode_Q = 113,
        KeyCode_W = 119,
        KeyCode_E = 101,
        KeyCode_R = 114,
        KeyCode_D = 100,
        KeyCode_F = 102,
    }

    public enum BattlePlayerInputValue
    {
        Null = 0,
        NormalAttack = 10,
        //默认 q
        Skill1 = 101,
        //默认 w
        Skill2 = 102,
        //默认 e
        Skill3 = 103,
        //默认 r
        Skill4 = 104,
        //默认 d
        Skill5 = 105,
        //默认 f
        Skill6 = 106,
        Skill7 = 107,
        
    }

    public class BattleInputModel
    {
        public BattlePlayerInputKey key;
        public BattlePlayerInputValue value;

        public int skillConfigId;
    }

    public partial class BattlePlayer
    {
        public Dictionary<BattlePlayerInputKey, BattleInputModel> inputDic;
        public void InitBattlePlayerInput()
        {
            inputDic = new Dictionary<BattlePlayerInputKey, BattleInputModel>();

            FillInput();
        }

        public void SetSkillInput(BattlePlayerInputKey key,BattlePlayerInputValue value,int skillConfigId)
        {
            SetInput(key,new BattleInputModel()
            {
                key = key,
                value = value,
                skillConfigId = skillConfigId,
            });
        }

        public void SetSkillInputByInputValue(BattlePlayerInputValue value,int skillConfigId)
        {
            foreach (var kv in inputDic)
            {
                var model = kv.Value;
                if (model.value == value)
                {
                    model.skillConfigId = skillConfigId;
                }
            }
        }

        public void SetInput(BattlePlayerInputKey key,BattleInputModel model)
        {
            if (inputDic.ContainsKey(key))
            {
                inputDic[key] = model;
            }
            else
            {
                inputDic.Add(key,model);
            }
        }

        public void FillInput()
        {
            inputDic.Add(BattlePlayerInputKey.KeyCode_A,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_A,
                value = BattlePlayerInputValue.NormalAttack
            });
            
            inputDic.Add(BattlePlayerInputKey.KeyCode_Q,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_Q,
                value = BattlePlayerInputValue.Skill1
            });
            
            inputDic.Add(BattlePlayerInputKey.KeyCode_W,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_W,
                value = BattlePlayerInputValue.Skill2
            });
            
            inputDic.Add(BattlePlayerInputKey.KeyCode_E,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_E,
                value = BattlePlayerInputValue.Skill3
            });
            
            inputDic.Add(BattlePlayerInputKey.KeyCode_R,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_R,
                value = BattlePlayerInputValue.Skill4
            });
            
            inputDic.Add(BattlePlayerInputKey.KeyCode_D,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_D,
                value = BattlePlayerInputValue.Skill5
            });
            
            inputDic.Add(BattlePlayerInputKey.KeyCode_F,new BattleInputModel()
            { 
                key = BattlePlayerInputKey.KeyCode_F,
                value = BattlePlayerInputValue.Skill6
            });
        }
    }


}

