using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimedMessageTrigger : MonoBehaviour
{
	public TMP_Text text;
	public float time;
	public bool canActivateAgain;
	private bool wasActivatedOnce = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if ((canActivateAgain || !wasActivatedOnce) && other.CompareTag("Player"))
		{
			wasActivatedOnce = true;
			text.gameObject.SetActive(true);
			StartCoroutine(WaitToDismiss());
		}
	}

	IEnumerator WaitToDismiss()
	{
		yield return new WaitForSeconds(time);
		text.gameObject.SetActive(false);

	}
}