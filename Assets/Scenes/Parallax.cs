using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float assetWidth;
    private float startPosition;

    private Transform cameraTransform;

    public float speed;

    void Start()
    {
        startPosition = transform.position.x;
        assetWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float reposition = cameraTransform.transform.position.x * (1 - speed);
        float distance = cameraTransform.position.x * speed;

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (reposition > startPosition + assetWidth / 2)
        {
            startPosition += assetWidth;

        }
        else if (reposition < startPosition - assetWidth / 2)
        {
            startPosition -= assetWidth;
        }
    }
}
