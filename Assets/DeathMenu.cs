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

    private AudioSource GameAudio;

    void Awake()
    {
        GameAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        AudioSource = GetComponent<AudioSource>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        if (playerHealth.GetDead())
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

        GameAudio.Pause();

        GameIsPaused = true;
    }

    public void MenuAppear()
    {
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartCheckpoint()
    {
        Health[] objectsWithHealth = FindObjectsOfType<Health>();
        Health playerHealt = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealt.RevivePlayer();
        // Chama a função em cada objeto com o script
        foreach (Health enemy in objectsWithHealth)
        {
            enemy.ReviveEnemy();
        }
        Resume();
        //Time.timeScale = 0f;
    }
    public void LoadMainMenu()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        StartCoroutine(WaitForAudioToLoad(() =>
        {
            GameIsPaused = false;

            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.MainMenuScene);
        }));
    }

    public void RestartGame()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        StartCoroutine(WaitForAudioToLoad(() =>
        {
            GameIsPaused = false;

            Time.timeScale = 1f;
            GameAudio.Stop();
            Loader.Reload();
            GameAudio.Play();
        }));
    }

    public void QuitGame()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        StartCoroutine(WaitForAudioToLoad(() =>
        {
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
