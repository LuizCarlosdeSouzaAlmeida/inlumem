using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{
	private enum State
	{
		Idle,
		Walking,
		Jumping,
		Falling
	}

	private Rigidbody2D mainCharacterRb;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	[SerializeField] private float jumpSpeed = 3f;
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private LayerMask groundLayer;
	private State state = State.Idle;
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

		HandleMovement();
		HandleJump();
	}

	private void HandleStates()
	{
		state = movementValue == 0 ? State.Idle : State.Walking;

		if (mainCharacterRb.velocity.y > groundThreshold)
		{
			state = State.Jumping;
		}
		else if (mainCharacterRb.velocity.y < -groundThreshold)
		{
			if (state == State.Jumping)
			{
				ResumeAnimation();
			}

			state = State.Falling;
		}

		if (CharacterIsCloseToGround(fallAnimationThreshold))
		{
			ResumeAnimation();
		}

		animator.SetInteger("State", (int)state);
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
}
