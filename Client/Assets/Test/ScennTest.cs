using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScennTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private AsyncOperation loadOp;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
           StartCoroutine( Load("LoginScene"));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine( Load("LobbyScene"));
            
            // loadOp = SceneManager.LoadSceneAsync("LobbyScene");
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(Unload(loginScene));
            // SceneManager.UnloadSceneAsync(loginScene);
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            loadOp = SceneManager.UnloadSceneAsync("LobbyScene");
        }
        
    }

    IEnumerator Unload(Scene scene)
    {
        yield return   SceneManager.UnloadSceneAsync(scene);
        Debug.Log("unload success : " + scene.name);
    }

    private Scene loginScene;
    private Scene lobbyScene;
    IEnumerator Load(string sceneName)
    {
        Debug.Log("start load " + sceneName);
        loadOp = SceneManager.LoadSceneAsync(sceneName);
      
        while (true)
        {
            yield return null;
            if (loadOp.isDone)
            {
                loadOp.allowSceneActivation = true;
                break;
            }
        }
        
        
        Debug.Log("finish load " + sceneName);
        
        var scene = SceneManager.GetSceneByName(sceneName);

        if (sceneName == "LoginScene")
        {
            loginScene = scene;
        }else if (sceneName == "LobbyScene")
        {
            lobbyScene = scene;
        }

        Debug.Log(scene.isLoaded);
        Debug.Log(scene.name);

        var acScene = SceneManager.GetActiveScene();

        SceneManager.SetActiveScene(acScene);
        Debug.Log(acScene.isLoaded);
        Debug.Log(acScene .name);
    }
}
