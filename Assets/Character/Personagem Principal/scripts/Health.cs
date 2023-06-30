using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private float health = 5f;

	private void Start()
	{
	}

	private void Update()
	{
		HandleDeath();
	}

	private void HandleDeath()
	{
		if (health <= 0f)
		{
			Debug.Log("Foi de comes e bebes");
			Destroy(gameObject);
		}
	}

	public void TakeDamage(float damage)
	{
		float newHealth = health - damage;
		health = newHealth < 0 ? 0 : newHealth;
		Debug.Log("Tomou dano, vida atual: " + health);
	}
}