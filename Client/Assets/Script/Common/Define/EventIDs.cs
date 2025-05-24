using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 1 - 100000 网络协议  100000+ 是自定义事件
/// </summary>
public enum EventIDs
{
    Null = 0,

    //自定义事件--------------
    OnUpgradeHeroLevel = 100001,
    OnRefreshAllMainTaskData = 100051,
    OnPlotEnd = 100071,
    OnRefreshHeroListData = 100101,
    OnRefreshBagData = 100102,

    //team
    OnPlayerEnterTeamRoom = 100131,
    OnPlayerChangeInfoInTeamRoom = 100132,
    OnPlayerLeaveTeamRoom = 100133,

    //title
    OnTitleBarClickCloseBtn = 100900,

    //battle-------------
    OnCreateBattle = 101001,
    OnAllPlayerLoadFinish = 1010002,
    OnBattleStart = 1010003,
    OnPlayerReadyState = 1010004,

    OnCreateEntity = 101101,
    OnChangeEntityBattleData = 101102,
    OnEntityDead = 101103, //实体死亡的时候
    OnEntityDestroy = 101104, //实体销毁(死亡之后的一段时间后的销毁时间点)
    OnBattleEnd = 101105,
    OnEntityChangeShowState = 101106, //实体改变了显隐时
    OnEntityAbnormalEffect = 101107, //单位异常效果触发（躲避 无敌等）

    OnProcessReadyStateEnter = 101110, //进入准备阶段
    OnProcessBattleStateEnter = 101111, //进入战斗阶段
    OnProcessBossStateEnter = 101112, //进入boss阶段
    OnProcessWavePass = 101113, //回合结算，当前波通过了
    OnUpdateProcessStateInfo = 101115, //更新战斗流程中的状态信息

    OnSetEntityPosition = 101201,
    OnSkillInfoUpdate = 101202, //当有技能信息改变的时候
    OnBuffInfoUpdate = 101203, //当有buff信息改变的时候
    OnSkillTrackStart = 101204, //当有技能轨迹开始的时候
    OnSkillTrackEnd = 101205, //当有技能轨迹结束的时候
    OnEntityItemsUpdate = 101206, //当有实体单位道具信息改变的时候

    // OnSkillItemInfoUpdate = 101207,     //当有技能书道具信息改变的时候
    // OnUpdateBoxInfo = 101208,           //同步宝箱信息
    OnEntityItemInfoUpdate = 101207, //当有实体单位道具信息改变的时候(单个)
    OnBoxOpen = 101209, //有宝箱打开了
    OnUpdateBattleCurrencyInfo = 101210, //同步玩家资源
    OnUpdateShopBoxInfo = 101211, //同步宝箱商店信息
    OnUpdateMyBoxInfo = 101212, //同步我的宝箱信息
    OnUpdateBattleReward = 101213, //同步战斗奖励

    //OnReplaceSkillResult = 101214,       //更换技能的消息返回同步（例如：技能满了要替换的时候）
    OnSkillRefreshAll = 101215, //当有技能有增加或者删除等的时候需要刷新全部
    OnUpdateWarehouseItemData = 101220, //仓库的道具数据刷新
    OnUpdatePlayerTeamMembersInfo = 101225, //玩家的队伍发生改变
    OnSyncIsUseReviveCoin = 101226,//同步玩家是否是用复活币
    OnUpdatePlayerBuyInfo = 101227,

    OnSkillTips = 101250, //技能提示
    OnItemTips = 101251, //道具提示
    OnSelectEntity = 101270, //当选择了一个单位（相当于左键）
    OnCancelSelectEntity = 101271, //当取消选择了一个单位

    _Battle_Flag_End = 101300, //战斗消息的结束符号
    //

    //UI-------------
    On_UIAttrOption_PointEnter = 101501,
    On_UIAttrOption_PointExit = 101502,
    On_UISkillOption_PointEnter = 101503,
    On_UISkillOption_PointExit = 101504,
    On_UIBuffOption_PointEnter = 101505,
    On_UIBuffOption_PointExit = 101506,
    On_UIItemOption_PointEnter = 101507,
    On_UIItemOption_PointExit = 101508,
    On_UISkillItemOption_PointEnter = 101509,
    On_UISkillItemOption_PointExit = 101510,

    OnChangeLoadingProgress = 101601,
    //
}

public class EventSender
{
    public static void SendLoadingProgress(float curr, float max, string str = "")
    {
        SendLoadingProgress(new LoadingUICtrlArg()
        {
            curr = curr,
            max = curr,
            progress = curr / max,
            text = str
        });
    }

    public static void SendLoadingProgress(float progress, string str = "")
    {
        SendLoadingProgress(new LoadingUICtrlArg()
        {
            progress = progress,
            text = str
        });
    }

    public static void SendLoadingProgress(LoadingUICtrlArg arg)
    {
        EventDispatcher.Broadcast<LoadingUICtrlArg>(EventIDs.OnChangeLoadingProgress, arg);
    }
}