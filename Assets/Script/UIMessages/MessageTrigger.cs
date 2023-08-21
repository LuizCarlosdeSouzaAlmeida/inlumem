using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageTrigger : MonoBehaviour
{
	public TMP_Text text;
	public bool waitOnFirstTime;
	private bool isFirstTime = true;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(Activate());
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			text.gameObject.SetActive(false);
		}
	}

	private IEnumerator Activate()
	{
		if (waitOnFirstTime && isFirstTime)
		{
			yield return new WaitForSeconds(0.5f);
		}

		text.gameObject.SetActive(true);
		isFirstTime = false;
	}
}