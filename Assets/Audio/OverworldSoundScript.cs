using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSoundScript : MonoBehaviour
{
	private static OverworldSoundScript instance;
	private AudioSource AudioSource;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			// Set the current instance and mark it as not to be destroyed when loading new scenes
			instance = this;
			DontDestroyOnLoad(this.gameObject);

			AudioSource = GetComponent<AudioSource>();
			AudioSource.Play();
		}
	}
}
