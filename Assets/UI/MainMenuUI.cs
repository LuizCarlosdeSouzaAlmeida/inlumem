using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        newGameButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.SampleScene);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
