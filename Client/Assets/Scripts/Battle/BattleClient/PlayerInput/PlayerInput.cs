using System.Collections.Generic;
using Battle;
using GameData;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    //战斗中的用户输入
    public partial class PlayerInput
    {
        //技能释放指示模块
        SkillDirectorModule skillDirectModule;

        //技能释放后的轨道模块
        SkillTrackModule skillTrackModule;

        public int willUseItemIndex;
        public int willReleaserSkillId;
        private Color enemyOutlineColor = new Color(1, 0.2f, 0.2f, 1);
        private Color myOutlineColor = new Color(0.5f, 1.0f, 0.5f, 1);
        private Color friendOutlineColor = new Color(0.2f, 0.6f, 1.0f, 1);

        public void InitPlayerInput()
        {
            skillDirectModule = new SkillDirectorModule();
            skillDirectModule.Init();

            skillTrackModule = new SkillTrackModule();
            skillTrackModule.Init();
        }

        public bool CheckLocalHeroSkillRelease(int skillId)
        {
            //检测 cd
            var skill = BattleManager.Instance.FindLocalHeroSkill(skillId);
            if (skill != null)
            {
                if (skill.currCDTime <= 0)
                {
                    return true;
                }
            }

            //CtrlManager.Instance.globalCtrl.ShowTips("这个技能还不能释放");

            var tips = "这个技能还不能释放";
            Tips.ShowSkillTipText(tips);
            return false;
        }

        //用户点击地面的时候(右键)
        void OnPlayerClickGround(Vector3 clickPos)
        {
            //Logx.Log("OnPlayerClickGround : clickPos : " + clickPos);
            var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();

            var myUid = GameDataManager.Instance.UserData.Uid;

            var guid = 1; //目前这个不用发 因为 1 个玩家只控制一个英雄实体 服务器已经记录 这里先保留 entity guid
            //battleNet.SendMoveEntity(guid, clickPos);
            BattleManager.Instance.MsgSender.Send_MoveEntity(guid, clickPos);
        }


        //根据 tag 获取碰撞物体
        public bool TryToGetRayTargetPos(out RaycastHit hit, List<string> tags)
        {
            hit = new RaycastHit();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    var currHit = hits[i];
                    var tag = currHit.collider.tag;
                    if (tags.Contains(tag))
                    {
                        //Logx.Log("hit ground : " + currHit.collider.gameObject.name);
                        // pos = currHit.point;
                        // return true;
                        hit = currHit;
                        return true;

                        //this.OnPlayerClickGround(currHit.point);
                    }
                }
            }

            return false;
        }

        //得到射线检测的目标
        public bool TryToGetRayTargetPos(out RaycastHit hit, string targetTag = GlobalConfig.Ground)
        {
            var list = new List<string>() { targetTag };
            return TryToGetRayTargetPos(out hit, list);
        }

        public bool TryToGetRayOnEntity(out List<int> entityGuidList)
        {
            entityGuidList = new List<int>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default") );
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    var currHit = hits[i];
                    var currHitGo = currHit.collider.gameObject;

                    // Logx.Log("currHitGo : " + currHitGo.name);
                    var entity =
                        BattleEntityManager.Instance.FindEntityByColliderInstanceId(currHitGo.GetInstanceID());
                    if (entity != null)
                    {
                        entityGuidList.Add(entity.guid);
                    }
                }
            }

            return entityGuidList.Count > 0;
        }

        //使用道具
        public void OnUseItem(int index)
        {
            willUseItemIndex = index;

            //TODO 根据 index 找到道具 然后从配置中找到 skillId
            var skillId = 2005001; //BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
            // willReleaserSkillId = skillId;


            //willReleaserSkillIndex = index;
            int targetGuid = 0;
            Vector3 targetPos = Vector3.zero;

            var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);

            var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
            var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

            //TODO 判断道具释放条件 现在本地检测一下
            // var isNormalAttack = 0 == index;
            // if (!isNormalAttack)
            // {
            //     var isCanRelease = CheckLocalHeroSkillRelease(skillId);
            //     if (!isCanRelease)
            //     {
            //         return;
            //     }
            // }

            var releaseTargetType = (SkillReleaseTargetType)skillConfig.SkillReleaseTargeType;
            if (releaseTargetType == SkillReleaseTargetType.Point)
            {
                this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
            }
            else if (releaseTargetType == SkillReleaseTargetType.Entity)
            {
                this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
            }
            else if (releaseTargetType == SkillReleaseTargetType.NoTarget)
            {
                var arg = new ItemUseArg_Client()
                {
                    itemIndex = willUseItemIndex,
                    releaserGuid = localEntity.guid,
                    targetGuid = targetGuid,
                    targetPos = targetPos
                };

                Logx.Log(LogxType.Battle, "Use item : itemIndex : " + arg.itemIndex);

                BattleManager.Instance.MsgSender.Send_UseItem(arg);
            }
        }

        public void OnSkillInput(PlayerInputType inputType)
        {
            var player = BattleManager.Instance.GetLocalPlayer();

            var commandModel = player.GetCommandModelByInputType(inputType);
            if (null == commandModel)
            {
                return;
            }

            var skillModel = commandModel as SkillCommandModel;
            var skillId = skillModel.skillConfigId;

            // var skill = BattleManager.Instance.FindLocalHeroSkill(skillId);

            willReleaserSkillId = skillId;
            int targetGuid = 0;
            Vector3 targetPos = Vector3.zero;

            // var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            if (skillId <= 0)
            {
                return;
            }

            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
            if (null == skillConfig)
            {
                return;
            }

            var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
            var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

            //判断释放条件 现在本地检测一下
            //普通攻击不提示 


            var isNormalAttack = (SkillCategory.NormalAttack) == (SkillCategory)skillConfig.SkillCategory;
            if (!isNormalAttack)
            {
                var isCanRelease = CheckLocalHeroSkillRelease(skillId);
                if (!isCanRelease)
                {
                    return;
                }
            }

            var releaseTargetType = (SkillReleaseTargetType)skillConfig.SkillReleaseTargeType;
            if (releaseTargetType == SkillReleaseTargetType.Point)
            {
                this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
            }
            else if (releaseTargetType == SkillReleaseTargetType.Entity)
            {
                this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
            }
            else if (releaseTargetType == SkillReleaseTargetType.NoTarget)
            {
                //技能
                Vector3 mousePos = Vector3.zero;
                if (TryToGetRayTargetPos(out var hit))
                {
                    mousePos = hit.point;
                }
                
                BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos,mousePos);
            }

            //Logx.Log("use skill : skillId : " + skillId + " targetGuid : " + targetGuid + " targetPos : " + targetPos);
        }

        // public void OnUseSkill(int index)
        // {
        //     var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
        //
        //     willReleaserSkillIndex = index;
        //     int targetGuid = 0;
        //     Vector3 targetPos = Vector3.zero;
        //
        //     var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        //     var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
        //
        //     var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
        //     var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
        //     var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);
        //
        //     //判断释放条件 现在本地检测一下
        //     //普通攻击不提示
        //
        //
        //     var isNormalAttack = (SkillCategory.NormalAttack) == (SkillCategory)skillConfig.SkillCategory;
        //     if (!isNormalAttack)
        //     {
        //         var isCanRelease = CheckLocalHeroSkillRelease(skillId);
        //         if (!isCanRelease)
        //         {
        //             return;
        //         }
        //     }
        //
        //     var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
        //     if (releaseTargetType == SkillReleaseTargeType.Point)
        //     {
        //         this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
        //     }
        //     else if (releaseTargetType == SkillReleaseTargeType.Entity)
        //     {
        //         this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
        //     }
        //     else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
        //     {
        //         BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
        //     }
        //
        //     //Logx.Log("use skill : skillId : " + skillId + " targetGuid : " + targetGuid + " targetPos : " + targetPos);
        // }

        public void OnSkillTrackStart(TrackBean trackBean)
        {
            this.skillTrackModule.AddTrack(trackBean);
        }

        public void OnSkillTrackEnd(int entityGuid, int trackId)
        {
            this.skillTrackModule.DeleteTrack(entityGuid, trackId);
        }


        public void Update(float deltaTime)
        {
            CheckInput();
            this.skillDirectModule?.Update(deltaTime);
            skillTrackModule?.Update(deltaTime);
        }

        public void OnBattleEnd()
        {
            skillTrackModule.OnBattleEnd();
        }
    }
}