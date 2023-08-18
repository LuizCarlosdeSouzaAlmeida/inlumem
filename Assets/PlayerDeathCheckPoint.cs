using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCheckPoint : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsCheckpoint;
    public Vector2 SafeGroundLocation { get; private set; } = new Vector2(0, 0);
    private void Start() {
        SafeGroundLocation = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision) {  
        if((whatIsCheckpoint.value & (1 << collision.gameObject.layer)) > 0){
            SafeGroundLocation = new Vector2(collision.bounds.center.x, collision.bounds.min.y);
        }
    }
    public void WarpToSafeGround()
    {
        transform.position = SafeGroundLocation;
    }
}
