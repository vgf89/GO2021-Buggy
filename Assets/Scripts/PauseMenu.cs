using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused;

    public Canvas pauseMenuCanvas, bugUICanvas;

    private void Start()
    {
        pauseMenuCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                Resume();
            else
                Pause();
        }
            
    }

    public void Resume()
    {
        //Activate Bug UI
        bugUICanvas.enabled = true;
        //Deactivate Pause Menu UI 
        pauseMenuCanvas.enabled = false;
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Pause()
    {
        //Activate Pause Menu UI
        pauseMenuCanvas.enabled = true;
        //Deactivate Bug UI 
        bugUICanvas.enabled = false;
        //Stops the game
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void GoToMainMenu()
    {
        StartCoroutine("LoadAsyncScene", "TitleScreen");
    }

    IEnumerator LoadAsyncScene(string _sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);

        while (!asyncLoad.isDone)
            yield return null;
    }

}
