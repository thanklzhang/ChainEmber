//using Assets.Script.Combat;
//using FixedPointy;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

//public class GameSkillOperate : MonoBehaviour
//{
//    CombatHero currHero;

//    Button attackBtn;
//    List<Button> skillBtns;

//    private void Awake()
//    {
//        attackBtn = transform.Find("attackBtn").GetComponent<Button>();
//        skillBtns = transform.Find("skillBtns").GetComponentsInChildren<Button>().ToList();

//        attackBtn.onClick.AddListener(() =>
//        {
//            var skill = currHero.GetAttackSkill();
//            ClickSkill(skill.config.SN);
//        });
//        for (int i = 0; i < skillBtns.Count; ++i)
//        {
//            var skill = currHero.GetSkill(i);
//            skillBtns[i].onClick.AddListener(() =>
//            {
//                ClickSkill(skill.config.SN);
//            });
//        }
//    }

//    // Use this for initialization
//    void Start()
//    {
//        //寻找放技能控制范围的标记物体 之后可能会动态加载
//        var worldRoot = GameObject.Find("WorldRoot").transform;
//        rangeShowObj = worldRoot.Find("Plane/rangeShowObj").gameObject;

//        currHero = CombatManager.Instance.GetLocalCombatPlayer();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isInSelectTargetPos)
//        {
//            //show
//            UpdateSelectSkillObj();

//            //这里是由 view 层传递
//            //currSkill = null;
//            //currSkillPos = Input.mousePosition;

//            //release
//            if (Input.GetMouseButtonDown(0))
//            {
//                StartCurrSkill();
//                FinishSelectSkillTargetPos();
//            }
//        }

//        if (isInSelectTargetUnit)
//        {
//            //show
//            UpdateSelectSkillUnitObj();

//            //release
//            if (Input.GetMouseButtonDown(0))
//            {
//                //currTargetUnit = null;//这里由 view 层传递
//                StartCurrSkill();
//                FinishSelectSkillTargetUnit();
//            }
//        }
//    }

//    //void ClickAttack()
//    //{
//    //    var findEntity = FindNearestEntity();
//    //    CombatManager.Instance.SkillOperate(currHero.seat, OperateType.Attack, 0, 0, findEntity?.guid ?? 0);
//    //}

//    //CombatLogicEntity FindNearestEntity()
//    //{
//    //    //目前是寻找最近的单位
//    //    var entityList = CombatLogicEntityManager.Instance.entityList;
//    //    Fix min = Fix.MaxValue;
//    //    int minIndex = 0;
//    //    bool isFind = false;
//    //    for (int i = 0; i < entityList.Count; ++i)
//    //    {
//    //        var currEntity = entityList[i];
//    //        if (currEntity.guid == currHero.guid)
//    //        {
//    //            continue;
//    //        }
//    //        var dis = (currHero.position - currEntity.position).GetMagnitude();
//    //        Debug.Log("dis : " + currEntity.guid + " " + currHero.guid + " " + dis + " " + currHero.attackRange + " " + min);
//    //        if (dis <= currHero.attackRange)
//    //        {
//    //            if (dis < min)
//    //            {
//    //                min = dis;
//    //                minIndex = i;
//    //                isFind = true;
//    //            }
//    //        }

//    //    }
//    //    if (isFind)
//    //    {
//    //        var findEntity = entityList[minIndex];
//    //        Debug.Log("find succuss : " + findEntity.guid);
//    //        return findEntity;
//    //        //CombatManager.Instance.SkillOperate(currHero.seat, PlayerOperateType.Attack, 0, 0, findEntity.guid);

//    //    }
//    //    else
//    //    {
//    //        Debug.Log("find fail , no entity that in attack range");
//    //    }
//    //    return null;
//    //}

//    public Vector3 currSkillPos = Vector3.zero;
//    public CombatLogicEntity currTargetUnit;

//    public GameObject rangeShowObj;
//    public GameObject unitShowObj;//单位选择图标对象

//    bool isInSelectTargetPos;//是否在选择技能目标点状态中
//    bool isInSelectTargetUnit;//是否在选择技能单位状态中

//    void SelectSkillTargetPos()
//    {
//        isInSelectTargetPos = true;
//        rangeShowObj.SetActive(true);
//    }

//    void FinishSelectSkillTargetPos()
//    {
//        isInSelectTargetPos = false;
//        rangeShowObj.SetActive(false);
//    }

//    void UpdateSelectSkillObj()
//    {
//        //将要增加位置显示
//        rangeShowObj.transform.localScale = Vector3.one * (float)currSkill.config.skillReleaseSelectRange;
//    }

//    void SelectSkillTargetUnit()
//    {
//        isInSelectTargetUnit = true;
//        unitShowObj.SetActive(true);
//    }

//    void FinishSelectSkillTargetUnit()
//    {
//        isInSelectTargetUnit = false;
//        unitShowObj.SetActive(false);
//    }

//    void UpdateSelectSkillUnitObj()
//    {
//        unitShowObj.transform.localScale = Vector3.one * (float)currSkill.config.skillReleaseSelectRange;
//    }

//    //int currSkillIndex;
//    SkillModel currSkill;
//    void ClickSkill(int skillSN)
//    {
//        // currSkillIndex = index;
//        currSkill = currHero.GetSkill(skillSN);
//        var releaseType = (SkillReleaseTargetType)currSkill.config.skillEffectSelectType;
//        if (releaseType == SkillReleaseTargetType.Pos)
//        {
//            SelectSkillTargetPos();
//        }

//        if (releaseType == SkillReleaseTargetType.Unit)
//        {
//            SelectSkillTargetUnit();
//        }
//    }

//    void StartCurrSkill()
//    {
//        var x = (int)Math.Round(currSkillPos.x * Const.floatMul);
//        var y = (int)Math.Round(currSkillPos.y * Const.floatMul);
//        CombatManager.Instance.SkillOperate(currHero.seat, OperateType.Skill, x, y, currTargetUnit?.guid ?? 0);
//    }
//}
