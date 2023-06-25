using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private float heightOffset = 1;
    private float smoothSpeed = 5f;
    private float yOffset = 0f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        float horizontalPosition = playerTransform.position.x;
        float targetVerticalPosition = playerTransform.position.y + heightOffset;

        // Evitar que a câmera suba a cada pulo (opcional)
        if (playerTransform.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            yOffset = Mathf.Lerp(yOffset, 0f, Time.deltaTime * smoothSpeed);
        }
        else
        {
            yOffset = Mathf.Lerp(yOffset, heightOffset, Time.deltaTime * smoothSpeed);
        }

        float verticalPosition = playerTransform.position.y + yOffset;

        if (playerTransform.position.x < -3.5f)
        {
            horizontalPosition = -3.5f;
        }


        Vector3 targetPosition = new Vector3(horizontalPosition, targetVerticalPosition, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
