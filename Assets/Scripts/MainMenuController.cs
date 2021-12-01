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
        SwitchCanvas("Main Title");
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

    //Canvas does not need to be included in the canvasName.
    public void SwitchCanvas(string canvasName)
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
            if (canvasList[i].name.Contains(canvasName))
                canvasList[i].enabled = true;
            else
                canvasList[i].enabled = false;
        }


    }
}
