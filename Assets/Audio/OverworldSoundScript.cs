using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSoundScript : MonoBehaviour
{
	public static OverworldSoundScript instance;
	private AudioSource AudioSource;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			// Set the current instance and mark it as not to be destroyed when loading new scenes
			instance = this;
			DontDestroyOnLoad(gameObject);

			AudioSource = GetComponent<AudioSource>();
			AudioSource.Play();
		}
	}
}
