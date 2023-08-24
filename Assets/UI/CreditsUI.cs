using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject creditsUI;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip MenuHoverSound;
    [SerializeField] private AudioClip MenuClickSound;

    private AudioSource GameAudio;

    void Awake()
    {
        GameAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioSource.PlayOneShot(MenuClickSound);

            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        GameAudio.UnPause();
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        GameAudio.Pause();
        GameIsPaused = true;
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


