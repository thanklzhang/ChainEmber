using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrintFPS : MonoBehaviour
{

    private float time;
    private float updateTime = 0.5f;
    private float count = 0;
    private int frame = 0;
    private float fps = 0;
    protected void Start()
    {

        time = updateTime;
    }

    void Update()
    {
        Controller();
    }
    private void Controller()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = updateTime;
            // 每次加上 时间比例除以每帧的事件，得到每秒的频率
            count += Time.timeScale / Time.deltaTime;
            frame++;
            // 总帧数除以相加次数，得到当天帧率的平均值
            fps = (float)(count / frame);
        }

        if (frame > 100)
        {
            count = 0;
            frame = 0;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(450, 10, 500, 400), "FPS: " + fps.ToString());
    }

}
