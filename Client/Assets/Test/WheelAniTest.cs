using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WheelAniTest : MonoBehaviour
{

    public Transform entityTran;
    public Transform startPos;
    public Transform endPos;

    
    //
    public string constStr = "constStr";
    public float targetS1 = 360 * 2 + 180f;
    public float targetTime1 = 2;

    //public float targetS2 = 360.0f * 6;
    public float targetTime2 = 4;

    public float targetS3 = 360.0f * 5;
    public float targetTime3 = 3.5f;
    public float targetIndex = 3;
        
    //
    public string runtimeStr = "rntimeStr";
    private int state = 0;
    public float a;
    public float currAngle;
    public float currTime;
    public float v;
    
    public Button playBtn;

    void Awake()
    {
        playBtn.onClick.AddListener(() =>
        {
            OnClickPlayBtn();
        });
    }

    void ResetInfo()
    {
        state = 1;
        a = 2 * targetS1 / (targetTime1 * targetTime1);
        currAngle = 0;
        currTime = 0;
        v = 0;
        
    }

    void Start()
    {
       

    }

    public float currIndexS;
    public float s;
    public void OnClickPlayBtn()
    {
        ResetInfo();
        
        state = 1;
        
        // state = 3;
        // v = 360f;
        //
        // currAngle = 4 * 360.0f + 66.0f;
        //
        // entityTran.rotation = Quaternion.Euler(0, 0, currAngle);
        //
        //
        // currIndexS = currAngle % 360.0f;
        // s = targetS3 + (360 + targetIndex * (360.0f / 8) - currIndexS) ;
        // //a = (s - v * targetTime3) / (0.5f * targetTime3 * targetTime3);
        // a = -v * v / (2 * s);
    }


    // Update is called once per frame
    void Update()
    {
        currTime = currTime + Time.deltaTime;

        if (1 == state && currTime >= targetTime1)
        {
            state = 2;
            currTime = 0;
            return;
        }
        
        if (2 == state && currTime >= targetTime2)
        {
            state = 3;
            currTime = currTime - targetTime2;

            var currIndexS = currAngle % 360.0f;
            var s = targetS3 + (360 + targetIndex * (360.0f / 8) - currIndexS) ;
            // a = (s - v * targetTime3) / (0.5f * targetTime3 * targetTime3);
            a = -v * v / (2 * s);
            return;
        }


        if (1 == state)
        {
            currAngle = currAngle + v * Time.deltaTime;
            
            v = v + a * Time.deltaTime;

            entityTran.rotation = Quaternion.Euler(0, 0, currAngle);
        }
        else if (2 == state)
        {
            currAngle = currAngle + v * Time.deltaTime;
            
            entityTran.rotation = Quaternion.Euler(0, 0, currAngle);
        }
        else if (3 == state)
        {
            currAngle = currAngle + v * Time.deltaTime;
            
            v = v + a * Time.deltaTime;
            
            if (v <= 0)
            {
                v = 0;
                state = 4;
                return;
            }
            
            entityTran.rotation = Quaternion.Euler(0, 0, currAngle);
            
          
            
            

           

            
        }


        




    }
}