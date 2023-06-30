using UnityEngine;

public class Movement : MonoBehaviour
{
	private Rigidbody2D rigidBody;
	private BoxCollider2D boxCollider;

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float jumpSpeed = 5f;
	[SerializeField] private float movementSpeed = 5f;

	private int remainingMidAirJumps = 1;
	[HideInInspector] public float distance = 0;
	[HideInInspector] public float groundThreshold = .1f;
	[HideInInspector] public float fallAnimationThreshold = 1f;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		groundLayer = LayerMask.GetMask("Ground");
	}

	private void Update()
	{
		HandleMovement();
		HandleJump();
	}

	void HandleMovement()
	{
		// Decidir entre GetAxis ou GetAxisRaw. No primeiro o personagem vai "escorregando" ao parar, no último ele para diretamente
		distance = Input.GetAxisRaw("Horizontal");

		if (CharacterTurnedDirection(distance))
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		rigidBody.velocity = new Vector2(distance * movementSpeed, rigidBody.velocity.y);
	}

	void HandleJump()
	{
		if (IsJumpRequested())
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed * (remainingMidAirJumps + 1));
			remainingMidAirJumps--;
		}

		if (CharacterIsCloseToGround(groundThreshold))
		{
			remainingMidAirJumps = 1;
		}
	}

	private bool IsJumpRequested() => (CharacterIsCloseToGround(groundThreshold) || remainingMidAirJumps > 0) && Input.GetButtonDown("Jump");

	private bool CharacterTurnedDirection(float axisValue) =>
		 (axisValue < 0 && transform.localScale.x > 0) || (axisValue > 0 && transform.localScale.x < 0);

	public bool CharacterIsCloseToGround(float distance) =>
		Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, distance, groundLayer);
}