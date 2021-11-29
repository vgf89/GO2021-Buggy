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
        SceneFunctions.LoadSceneAdditiveNoDuplicates("Scenes/_preload"); // Load AudioManager and other persistent things
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
            AudioManager.PlaySFX("lose");
            Time.timeScale = 0;
        }
        else if (AdventurerFrustrationTracker.currentFrustation >= frustrationBarUI.maxFrustration) {
            winScreen.SetActive(true);
            AudioManager.PlaySFX("win");
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
