using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;
    public SceneField nextLevel;

    private MapManager mapManager;
    private FrustrationBarUI frustrationBarUI;

    static private float originalTimeScale = 1;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        frustrationBarUI = FindObjectOfType<FrustrationBarUI>();
        if (Time.timeScale > 0) {
            originalTimeScale = Time.timeScale;
        } else {
            Time.timeScale = originalTimeScale;
        }
    }
    
    private void Update()
    {
        if (mapManager.condition == MapManager.conditionsOfTheWoeld.AllChestsHaveBeenOpened) {
            loseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else if (frustrationBarUI.currentFrustration >= frustrationBarUI.maxFrustration) {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void goNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }


}
