using UnityEngine;

public class Attack : MonoBehaviour
{
	private GameObject attackArea = default;
	private float attackDurationInSeconds = 0.2f;
	private float attackAnimationDurationInSeconds = 0.6f;
	private float timer = 0f;
	private float animationTimer = 0f;
	[HideInInspector] public bool isAttacking = false;

	private void Start()
	{
		attackArea = transform.Find("AttackArea").gameObject;
		attackArea.SetActive(false);
	}

	private void Update()
	{
		HandleAttack();
	}

	private void HandleAttack()
	{
		if (Input.GetButtonDown("MeleeAttack"))
		{
			isAttacking = true;
			attackArea.SetActive(true);
		}

		if (isAttacking)
		{
			timer += Time.deltaTime;
			animationTimer += Time.deltaTime;

			if (timer >= attackDurationInSeconds)
			{
				timer = 0;
				attackArea.SetActive(false);
			}
			if (animationTimer >= attackAnimationDurationInSeconds)
			{
				animationTimer = 0;
				isAttacking = false;
			}
		}
	}
}