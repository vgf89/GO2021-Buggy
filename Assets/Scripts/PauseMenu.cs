using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
