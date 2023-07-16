using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    // Must match the exactly Scene file name
    public enum Scene {
        MainMenuScene,
        SampleScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // Load the target scene at least 1 frame after the loading scene
    public static void LoaderCallback() 
    {
        SceneManager.LoadScene(targetScene.ToString()); 
    }
}
