using Battle;

public class EntityTool
{
    public static bool IsSameOriginConfigId(int aId,int bId)
    {
        var aConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(aId);
        var bConfig = BattleConfigManager.Instance.GetById<IEntityInfo>(bId);

        return aConfig.OriginEntityId == bConfig.OriginEntityId;

    }

    
}