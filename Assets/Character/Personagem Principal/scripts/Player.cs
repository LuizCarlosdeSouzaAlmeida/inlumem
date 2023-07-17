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

	private Animator animator;
	private Rigidbody2D rigidBody;
	private Movement movement;
	private Attack attack;

	private AnimationState animationState = AnimationState.Idle;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		movement = GetComponent<Movement>();
		attack = GetComponent<Attack>();
	}

	private void Update()
	{
		HandleStates();
	}

	private void HandleStates()
	{
		animationState = movement.distance == 0 ? AnimationState.Idle : AnimationState.Walking;

		if (attack.isAttacking)
		{
			animationState = AnimationState.Attacking;
		}
		else if (rigidBody.velocity.y > movement.groundThreshold)
		{
			animationState = AnimationState.Jumping;
		}
		else if (rigidBody.velocity.y < -movement.groundThreshold)
		{
			if (animationState == AnimationState.Jumping)
			{
				ResumeAnimation();
			}

			animationState = AnimationState.Falling;
		}

		if (movement.CharacterIsCloseToGround(movement.fallAnimationThreshold))
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
}