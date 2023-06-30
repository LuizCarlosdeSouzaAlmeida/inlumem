using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	private int damage = 3;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		Player character = collider.GetComponent<Player>();
		if (character != null)
		{
			character.TakeDamage(damage);
		}

		// Implementar dano aqui
	}
}
