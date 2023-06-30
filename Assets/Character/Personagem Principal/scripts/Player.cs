using UnityEngine;

public class Player : MonoBehaviour
{
	private enum AnimationState
	{
		Idle,
		Walking,
		Jumping,
		Falling,
		Attacking
	}

	private Rigidbody2D mainCharacterRb;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private GameObject attackArea = default;

	[SerializeField] private float health = 5f;
	[SerializeField] private float jumpSpeed = 3f;
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private LayerMask groundLayer;
	private bool isAttacking = false;
	private float timeToAttack = 0.6f;
	private float timer = 0f;
	private AnimationState animationState = AnimationState.Idle;
	private float movementValue = 0;
	private int remainingMidAirJumps = 1;
	private float groundThreshold = .1f;
	private float fallAnimationThreshold = 1f;

	private void Start()
	{
		mainCharacterRb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		groundLayer = LayerMask.GetMask("Ground");
		attackArea = transform.Find("AttackArea").gameObject;
	}

	private void Update()
	{
		HandleActions();
		HandleStates();
	}

	private void HandleActions()
	{
		void HandleMovement()
		{
			// Decidir entre GetAxis ou GetAxisRaw. No primeiro o personagem vai "escorregando" ao parar, no último ele para diretamente
			movementValue = Input.GetAxisRaw("Horizontal");

			if (CharacterTurnedDirection(movementValue))
			{
				spriteRenderer.flipX = !spriteRenderer.flipX;
			}

			mainCharacterRb.velocity = new Vector2(movementValue * movementSpeed, mainCharacterRb.velocity.y);
		}

		void HandleJump()
		{
			if (IsJumpRequested())
			{
				mainCharacterRb.velocity = new Vector2(mainCharacterRb.velocity.x, jumpSpeed * (remainingMidAirJumps + 1));
				remainingMidAirJumps--;
			}

			if (CharacterIsCloseToGround(groundThreshold))
			{
				remainingMidAirJumps = 1;
			}
		}

		void HandleAttack()
		{
			if (Input.GetButtonDown("MeleeAttack"))
			{
				isAttacking = true;
				attackArea.SetActive(isAttacking);
			}

			if (isAttacking)
			{
				timer += Time.deltaTime;
				if (timer >= timeToAttack)
				{
					timer = 0;
					isAttacking = false;
					attackArea.SetActive(isAttacking);

				}
			}
		}

		void HandleDeath()
		{
			if (health <= 0f)
			{
				Debug.Log("Foi de comes e bebes");
				Destroy(gameObject);
			}
		}

		HandleMovement();
		HandleJump();
		HandleAttack();
		HandleDeath();
	}

	private void HandleStates()
	{
		animationState = movementValue == 0 ? AnimationState.Idle : AnimationState.Walking;

		if (isAttacking)
		{
			animationState = AnimationState.Attacking;
		}
		else if (mainCharacterRb.velocity.y > groundThreshold)
		{
			animationState = AnimationState.Jumping;
		}
		else if (mainCharacterRb.velocity.y < -groundThreshold)
		{
			if (animationState == AnimationState.Jumping)
			{
				ResumeAnimation();
			}

			animationState = AnimationState.Falling;
		}

		if (CharacterIsCloseToGround(fallAnimationThreshold))
		{
			ResumeAnimation();
		}

		animator.SetInteger("State", (int)animationState);
	}

	private void FreezeAnimation()
	{
		animator.speed = 0;
	}

	private void ResumeAnimation()
	{
		animator.speed = 1;
	}

	private bool IsJumpRequested() => (CharacterIsCloseToGround(groundThreshold) || remainingMidAirJumps > 0) && Input.GetButtonDown("Jump");


	private bool CharacterTurnedDirection(float axisValue) =>
		 (axisValue < 0 && !spriteRenderer.flipX) || (axisValue > 0 && spriteRenderer.flipX);

	private bool CharacterIsCloseToGround(float distance) =>
		Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, distance, groundLayer);

	public void TakeDamage(float damage)
	{
		float newHealth = health - damage;
		health = newHealth < 0 ? 0 : newHealth;
	}
}