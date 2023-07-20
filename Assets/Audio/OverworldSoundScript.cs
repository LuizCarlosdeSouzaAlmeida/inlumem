using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSoundScript : MonoBehaviour
{
	private AudioSource AudioSource;

	void Awake()
	{
		AudioSource = GetComponent<AudioSource>();
		AudioSource.Play();
	}
}
