using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        Loader.LoadWithNoLoadingScreen(Loader.Scene.SceneBoss);
    }
}
