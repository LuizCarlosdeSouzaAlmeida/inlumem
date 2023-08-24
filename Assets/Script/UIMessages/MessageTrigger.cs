using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageTrigger : MonoBehaviour
{
	public TMP_Text text;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			text.gameObject.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			text.gameObject.SetActive(false);
		}
	}
}