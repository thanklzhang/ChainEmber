//using Assets.Script.Combat;
//using NetCommon;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class GameJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
//{


//    Transform roll;

//    public Button upBtn;
//    public Button downBtn;
//    float joyX;
//    float joyY;

//    float maxR = 100;
//    Canvas canvas;
//    CombatHero currHero;


//    // Use this for initialization
//    void Start()
//    {
//        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
//        roll = transform.Find("bg/roll");
//        currHero = CombatManager.Instance.GetLocalCombatPlayer();
//        Debug.Log("game joystick currHero : " + currHero);

//        //upBtn.onClick.AddListener(() =>
//        //{

//        //    hero.SetJoystickMoveInfo(0, 1);



//        //    Debug.Log("run ...");
//        //    var x = (int)Math.Round(joyX * Const.floatMul);
//        //    var y = (int)Math.Round(joyY * Const.floatMul);
//        //    CombatManager.Instance.MoveOperate(hero.seat, x, y);




//        //});

//        //downBtn.onClick.AddListener(() =>
//        //{
//        //    var hero = CombatManager.Instance.heroes[0];
//        //    float x = 0;
//        //    float y = -0.87875333f;
//        //    hero.SetJoystickMoveInfo(0, -1);

//        //});

//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        //Vector2 pos = Vector2.zero;
//        //RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

//        //joyX = pos.x / pos.magnitude;
//        //joyY = pos.y / pos.magnitude;

//        //var sqrtNum = pos.x * pos.x + pos.y * pos.y;
//        //if (sqrtNum >= maxR * maxR)
//        //{
//        //    var limitX = pos.x / pos.magnitude * maxR;
//        //    var limitY = pos.y / pos.magnitude * maxR;

//        //    roll.localPosition = new Vector3(limitX, limitY, 0);
//        //}
//        //else
//        //{
//        //    roll.localPosition = new Vector3(pos.x, pos.y, 0);
//        //}

//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        // roll.localPosition = Vector3.zero;
//    }


//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            Vector2 pos = Vector2.zero;

//            if (Input.mousePosition.x < 360 && Input.mousePosition.y < 410)
//            {
//                //move

//                //Debug.Log("roll : " + roll.transform.position + " mouse pos : " + Input.mousePosition);


//                //RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

//                pos = Input.mousePosition - roll.parent.position;

//                joyX = pos.x / pos.magnitude;
//                joyY = pos.y / pos.magnitude;

//                var sqrtNum = pos.x * pos.x + pos.y * pos.y;
//                if (sqrtNum >= maxR * maxR)
//                {
//                    var limitX = pos.x / pos.magnitude * maxR;
//                    var limitY = pos.y / pos.magnitude * maxR;

//                    roll.localPosition = new Vector3(limitX, limitY, 0);
//                }
//                else
//                {
//                    roll.localPosition = new Vector3(pos.x, pos.y, 0);
//                }

//                var x = (int)Math.Round(joyX * Const.floatMul);
//                var y = (int)Math.Round(joyY * Const.floatMul);
//                if (Math.Abs(x) < 0.001f && Math.Abs(y) < 0.001f)
//                {

//                }
//                else
//                {
//                    CombatManager.Instance.MoveOperate(currHero.seat, x, y);
//                    joyX = 0;
//                    joyY = 0;
//                }

//            }
//            else
//            {
//                roll.localPosition = Vector3.zero;
//                CombatManager.Instance.MoveOperate(currHero.seat, 0, 0);


//            }

//        }
//        else
//        {
//            roll.localPosition = Vector3.zero;
//            CombatManager.Instance.MoveOperate(currHero.seat, 0, 0);
//        }




//    }
//}
