using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;
	private float heightOffset = 2;

	private void Update()
	{
		float horizontalPosition = playerTransform.position.x;
		float verticalPosition = playerTransform.position.y + heightOffset;

		if (playerTransform.position.x < -3.5f)
		{
			horizontalPosition = -3.5f;
		}

		// Temporary
		if (playerTransform.position.x > 6.5f)
		{
			horizontalPosition = 6.5f;
		}

		transform.position = new Vector3(horizontalPosition, verticalPosition, transform.position.z);
	}
}
