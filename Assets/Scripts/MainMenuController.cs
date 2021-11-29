using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    public List<Button> buttons;

    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
