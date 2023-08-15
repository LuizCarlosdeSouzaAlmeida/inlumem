using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeGroundSaver : MonoBehaviour
{
    [SerializeField] private float saveFrequency = 1f;

    public Vector2 SafeGroundLocation { get; private set; } = new Vector2(0, 0);

    private Coroutine safeGroundCoroutine;

    private PlayerMovement playerMovement;

    private void Start()
    {
        safeGroundCoroutine = StartCoroutine(SafeGroundLocationCoroutine());
        SafeGroundLocation = transform.position;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private IEnumerator SafeGroundLocationCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < saveFrequency)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (playerMovement.isGrounded()){
            SafeGroundLocation = transform.position;
        }
        safeGroundCoroutine = StartCoroutine(SafeGroundLocationCoroutine());
    }
    public void WarpToSafeGround()
    {
        transform.position = SafeGroundLocation;
    }
}
