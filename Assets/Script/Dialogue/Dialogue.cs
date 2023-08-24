using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialogue
{
	public string npcName;

	[TextArea(3, 10)]
	public string[] lines;

	public UnityEvent onEndEvents;
}
