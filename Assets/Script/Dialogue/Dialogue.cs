using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
	public string npcName;

	[TextArea(3, 10)]
	public string[] lines;
}
