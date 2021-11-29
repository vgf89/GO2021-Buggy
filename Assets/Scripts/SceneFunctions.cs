using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneFunctions
{
    public static void LoadSceneAdditiveNoDuplicates(string scenePath)
    {
        Scene loadedadditive = SceneManager.GetSceneByPath(scenePath);
        if (!loadedadditive.IsValid()) { // Instiate additive if it is not already loaded
            SceneManager.LoadScene(scenePath, LoadSceneMode.Additive);
        }
    }
}
