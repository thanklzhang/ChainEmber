using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniTest : MonoBehaviour
{
    public GameObject target;

    private Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        ani = target.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.SetInteger("Action",1);
        }
        else
        if (Input.GetKeyDown(KeyCode.W))
        {
            ani.SetInteger("Action",2);
        }
        else
        if (Input.GetKeyDown(KeyCode.E))
        {
            ani.SetInteger("Action",3);
        }
        else
        if (Input.GetKeyDown(KeyCode.R))
        {
            ani.SetInteger("Action",4);
        }
        
    }
}
