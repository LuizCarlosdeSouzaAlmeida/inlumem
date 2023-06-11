using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{
	private Rigidbody2D mainCharacterRb;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	[SerializeField] private float jumpSpeed = 5f;
	[SerializeField] private float movementSpeed = 5f;

	private void Start()
	{
		mainCharacterRb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		HandleMovement();
		HandleJump();
	}

	private void HandleMovement()
	{
		// Decidir entre GetAxis ou GetAxisRaw. No primeiro o personagem vai "escorregando" ao parar, no último ele para diretamente
		float axisValue = Input.GetAxisRaw("Horizontal");

		animator.SetBool("IsWalking", axisValue != 0);

		if (CharacterTurnedDirection(axisValue))
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		mainCharacterRb.velocity = new Vector2(axisValue * movementSpeed, mainCharacterRb.velocity.y);
	}

	private void HandleJump()
	{
		if (IsJumpRequested())
		{
			animator.SetBool("IsJumping", true);
			mainCharacterRb.velocity = new Vector2(mainCharacterRb.velocity.x, jumpSpeed);
		}
	}

	private bool IsJumpRequested() =>
		// Input.GetAxisRaw("Vertical") > 0 ||
		Input.GetButtonDown("Jump");


	private bool CharacterTurnedDirection(float axisValue) =>
		 (axisValue < 0 && !spriteRenderer.flipX) || (axisValue > 0 && spriteRenderer.flipX);
}
