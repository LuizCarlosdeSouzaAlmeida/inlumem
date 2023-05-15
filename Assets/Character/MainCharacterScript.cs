using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{
	public Rigidbody2D mainCharacterRb;
	public SpriteRenderer spriteRenderer;

	public float jumpSpeed = 5f;
	public float movementSpeed = 0.01f;
	private bool IsJumpRequested
	{
		get =>
			// Input.GetAxisRaw("Vertical") > 0 ||
			Input.GetButtonDown("Jump");
	}

	private bool CharacterTurnedDirection
	{
		get
		{
			float movement = Input.GetAxis("Horizontal");
			return (movement < 0 && !spriteRenderer.flipX) || (movement > 0 && spriteRenderer.flipX);
		}
	}

	void Start()
	{
		mainCharacterRb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		HandleMovement();
		HandleJump();
	}

	void HandleMovement()
	{
		float movement = Input.GetAxis("Horizontal") * movementSpeed;

		if (CharacterTurnedDirection)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		transform.Translate(movement, 0, 0);
	}

	void HandleJump()
	{
		if (IsJumpRequested)
		{
			mainCharacterRb.velocity = Vector2.up * jumpSpeed;
		}
	}
}
