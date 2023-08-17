using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;
            
            StartCoroutine(LoadTargetSceneAfterWaiting()); 
        }    
    }

    IEnumerator LoadTargetSceneAfterWaiting ()
    {
        yield return new WaitForEndOfFrame();

        AsyncOperation operation = SceneManager.LoadSceneAsync(Loader.targetScene.ToString()); 
        
        yield return new WaitUntil(() => operation.isDone);
    }
}
