using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject deathMenuUI;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip MenuHoverSound;
    [SerializeField] private AudioClip MenuClickSound;
    private Health playerHealth;

    void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        if (playerHealth.GetDead() == true)
        {
            Pause();
            GameIsPaused = true;
            deathMenuUI.SetActive(true);
        }
    }

    public void Resume()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        deathMenuUI.SetActive(false);
        Time.timeScale = 1f;

        GameIsPaused = false;
    }

    void Pause()
    {
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;

        GameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        StartCoroutine(WaitForAudioToLoad(() => {
            GameIsPaused = false;

            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.MainMenuScene);
        }));
    }

    public void RestartGame()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        StartCoroutine(WaitForAudioToLoad(() => {
            GameIsPaused = false;

            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.SceneBoss);
        }));
    }

    public void QuitGame()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        StartCoroutine(WaitForAudioToLoad(() => {
            GameIsPaused = false;

            Application.Quit();
        }));
    }

    private IEnumerator WaitForAudioToLoad(Action afterWait)
    {
        yield return new WaitWhile(() => AudioSource.isPlaying);

        afterWait?.Invoke();
    }

    public void triggerSoundOnHover()
    {
        AudioSource.PlayOneShot(MenuHoverSound);
    }
}
