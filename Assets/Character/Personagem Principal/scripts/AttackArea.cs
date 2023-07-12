using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	[SerializeField] private float damage = 3f;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		Health playerHealth = collider.GetComponent<Health>();
		if (playerHealth != null)
		{
			playerHealth.TakeDamage(damage);
		}
	}
}
