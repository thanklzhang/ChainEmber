using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResTst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var abPath = "Assets/StreamingAssets/Assets/BuildRes/Prefabs/UI/LoginUI.ab";
        var abPath2 = "Assets/StreamingAssets/Assets/BuildRes/Atlas/Common/Other.ab";
        
        
        var assetPath = "Assets/BuildRes/Prefabs/UI/LoginUI.prefab";
        var loginAB2 = AssetBundle.LoadFromFile(abPath2);
        
        
        var loginAB = AssetBundle.LoadFromFile(abPath);
       
        var loginUI = loginAB.LoadAsset<GameObject>(assetPath);
        GameObject.Instantiate(loginUI,this.transform);
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
