using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// public enum NextAction
// {
// 	Cutscene,
// 	Dialogue,
// 	Scene,
// 	Play
// }

public class DialogueManager : MonoBehaviour
{
	public TMP_Text npcName;
	public TMP_Text dialogueText;

	public Animator animator;

	public Queue<string> lines;

	private UnityEvent onEnd;

	private bool isOpen = false;
	private bool isTyping = false;
	private string currentLine = "";

	void Start()
	{
		lines = new Queue<string>();
	}

	void Update()
	{
		if (isOpen && Input.GetKeyDown(KeyCode.Space))
		{
			DisplayNext();
		}
	}

	public void StartDialogue(Dialogue dialogue)
	{
		dialogueText.text = "";
		onEnd = dialogue.onEndEvents;

		isOpen = true;
		animator.SetBool("IsOpen", isOpen);

		npcName.text = dialogue.npcName;

		lines.Clear();

		foreach (string line in dialogue.lines)
		{
			lines.Enqueue(line);
		}


		StartCoroutine(WaitDialogueAnimation());
	}

	public void DisplayNext()
	{
		if (isTyping)
		{
			StopAllCoroutines();
			dialogueText.text = currentLine;
			isTyping = false;
		}
		else if (!lines.Any())
		{
			EndDialogue();
		}
		else
		{
			currentLine = lines.Dequeue();
			StartCoroutine(TypeLine());
		}
	}

	void EndDialogue()
	{
		isOpen = false;
		animator.SetBool("IsOpen", isOpen);
		StartCoroutine(WaitToEnd());
	}

	private IEnumerator TypeLine()
	{
		isTyping = true;
		dialogueText.text = "";
		foreach (char character in currentLine)
		{
			dialogueText.text += character;
			for (int i = 0; i < 3; i++)
			{
				yield return null;
			}
		}
		isTyping = false;
	}

	private IEnumerator WaitDialogueAnimation()
	{
		yield return new WaitForSeconds(0.5f);
		DisplayNext();
	}

	private IEnumerator WaitToEnd()
	{
		yield return new WaitForSeconds(1f);
		onEnd.Invoke();
	}
}
