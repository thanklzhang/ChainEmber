using Battle;

public class BattleCommonConfigHelper
{
    public static IBattleCommonParam GetConfig()
    {
        var configId = 1;
        return BattleConfigManager.Instance.GetById<IBattleCommonParam>(configId);
    }

   
}