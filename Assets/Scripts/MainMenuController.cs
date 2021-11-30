using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    public List<Canvas> canvasList;

    public string nextSceneName;

    // Start is called before the first frame update
    void Awake()
    {
        canvasList[0].enabled = true;
        canvasList[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        StartCoroutine("LoadAsyncScene", "SampleScene");
    }

    IEnumerator LoadAsyncScene(string _sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);

        while (!asyncLoad.isDone)
            yield return null;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwitchCanvas()
    {
        /*
        if (canvasList[0].enabled)
        {
            canvasList[0].enabled = false;
            canvasList[1].enabled = true;
        }
        else
        {
            canvasList[0].enabled = false;
            canvasList[1].enabled = true;
        }*/

        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].enabled = !canvasList[i].enabled;
        }
    }
}
