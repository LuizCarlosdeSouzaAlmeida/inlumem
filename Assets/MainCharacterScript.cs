using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{
	public Rigidbody2D mainCharacterRb;
	public float jumpSpeed;
	public float movementSpeed;
	private bool IsJumpRequested
	{
		get =>
			// Input.GetAxisRaw("Vertical") > 0 ||
			Input.GetButtonDown("Jump");
	}

	void Start()
	{

	}

	void Update()
	{
		HandleMovement();
		HandleJump();
	}

	void HandleMovement()
	{
		float movement = Input.GetAxis("Horizontal") * movementSpeed;

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
