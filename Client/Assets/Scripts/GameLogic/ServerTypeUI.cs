using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerTypeUI : MonoBehaviour
{
    public Button remoteStartBtn;
    public Button localStartBtn;
    public InputField localServerIp;
    public InputField localServerPort;

    public Text tips;

    public GameStartup startUp;

    private void Awake()
    {
        remoteStartBtn = transform.Find("remote").GetComponent<Button>();
        localStartBtn = transform.Find("local").GetComponent<Button>();
        localServerIp = transform.Find("Ip").GetComponent<InputField>();
        localServerPort = transform.Find("port").GetComponent<InputField>();
        tips = transform.Find("tips").GetComponent<Text>();

        remoteStartBtn.onClick.AddListener(() =>
        {
            tips.text = "暂未开放";
            GlobalConfig.isLANServer = false;
        });

        localStartBtn.onClick.AddListener(() =>
        {
            //var ip = localServerIp.text;
            //var port = int.Parse(localServerPort.text);


            //check ip
            if (!NetTool.IsIpFormat(localServerIp.text))
            {
                this.tips.text = "不是 ip 地址";
                return;
            }

            GlobalConfig.isLANServer = true;
            this.gameObject.SetActive(false);

            GlobalConfig.LANServerIP = localServerIp.text;
            //var startUp = GameObject.Find("GameStartup").GetComponent<GameStartup>();
            startUp.Startup();
        });
    }


    // Start is called before the first frame update
    void Start()
    {
        //localServerIp.text = "192.168.3.13";
        //localServerPort.text = "" + 5556;
    }


    private void OnEnable()
    {
        localServerIp.text = NetTool.GetHostIp();
    }

    // Update is called once per frame
    void Update()
    {
    }
}