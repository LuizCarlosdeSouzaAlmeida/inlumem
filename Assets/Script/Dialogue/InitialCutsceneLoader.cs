using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCutsceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        Loader.LoadWithNoLoadingScreen(Loader.Scene.SampleScene);
    }
}
