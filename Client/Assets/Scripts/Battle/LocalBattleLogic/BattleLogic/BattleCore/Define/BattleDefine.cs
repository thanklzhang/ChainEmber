using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;



namespace Battle
{
   public class BattleDefine
   {
      //实体物品栏最大数量
      // public static int maxItemBarCellCount = 6;
      public static int maxStar = 5;
      public static int ultimateUnlockStar = 3;
      // public static int maxWarehouseItemBarCellCount = 100;
   }
   
   public enum RewardQuality
   {
      Green = 0,
      Blue = 1,
      Purple = 2,
      Orange = 3,
      Red = 4
   }

   public enum BattleProcessState
   {
      Ready = 0,
      Battle = 1,
      Boss = 2
   }

   public enum BattleWaveType
   {
      Normal = 0,
      Elite = 1,
      Boss = 2
   }
     
   public enum BattleWavePassType
   {
      //击杀 boss
      KillBoss = 0,
      //击杀所有
      KillAllInTime = 1,
      //至少有一个友方存活
      AliveAtLeast = 2
   }
      
   public enum CreatePosType
   {
      MapPosIndexRand = 0
   }
   
   public enum EntityLocationType
   {
      Battle = 0,
      UnderStudy = 1
   }

   // 0：普通攻击
   // 1：小招
   // 2：大招
   // 3：天赋技能
   // 4：队长技能
   // 5: 道具技能
   public enum SkillCategory
   {
      NormalAttack = 0,
      MinorSkill = 1,
      LeaderSkill = 2,
      UltimateSkill = 3,
      TalentSkill = 4,
      ItemSkill = 5
   }
   
   public enum ItemLocationType
   {
      //仓库
      Warehouse = 0,

      //实体道具栏
      EntityItemBar = 1
   }

   public class MoveItemOpLocation
   {
      public ItemLocationType type;
      public int index;
      public int entityGuid;
   }
   
   //实体间的关系
   public enum EntityRelationType
   {
      //自己
      Self = 0,
      //友军
      Friend = 1,
      //敌人
      Enemy = 2,
      //召唤者 - 召唤兽
      Master_SummonEntity = 3,
      //召唤兽 - 召唤者
      SummonEntity_Master = 4
   }
   
   
   //叠层类型
   public enum BuffAddLayerType
   {
      //替换
      Replace = 0,
      //叠加层数且叠加效果
      AddLayerAndEffect = 1,
      //叠加层数但是不叠加效果
      AddLayerWithoutEffect = 2,
   }

   //buff 后续效果的目标类型(通用)
   public enum BuffEffectTargetType
   {
      //技能释放着
      SkillReleaser = 0,
      //buff 目标者
      BuffTarget = 1,
   }
   
   public enum MoveEndPosType
   {
      TargetEntityPos = 0,
      SkillTargetPos = 1,
      //自己 到 目标点 的 方向
      SelfToSkillTargetPos_DirMaxDistancePos = 2,
      //释放者 到 自己 的 方向
      ReleaserToSelf_DirMaxDistancePos = 3,
      //自己 到 鼠标点 的 方向
      SelfToMousePos_DirMaxDistancePos = 4,
      //施法者坐标点
      ReleaserPos = 5

   }
}

