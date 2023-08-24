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

    public void Resume()
    {
        AudioSource.PlayOneShot(MenuClickSound);

        creditsUI.SetActive(false);

        Time.timeScale = 1f;

        GameAudio.UnPause();
        GameIsPaused = false;
    }

    public void ShowCredits()
    {
        creditsUI.SetActive(true);

        Time.timeScale = 0f;

        GameAudio.Pause();
        GameIsPaused = true;
    }

    public void triggerSoundOnHover()
    {
        AudioSource.PlayOneShot(MenuHoverSound);
    }
}


