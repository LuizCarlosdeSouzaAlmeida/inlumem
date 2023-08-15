using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrapScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeDetection;
    [SerializeField] private float rangeDamage;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistanceDetection;
    [SerializeField] private float colliderDistanceDamage;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        //Attack only when player in sight
        if(PlayerInSightDetection()) {
            if(cooldownTimer >= attackCooldown) {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
    }
    private bool PlayerInSightDetection() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeDetection * transform.localScale.x * colliderDistanceDetection, 
            new Vector3(boxCollider.bounds.size.x * rangeDetection, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);        
        return hit.collider != null;
    }
    private bool PlayerInSightDamage() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeDamage * transform.localScale.x * colliderDistanceDamage, 
            new Vector3(boxCollider.bounds.size.x * rangeDamage, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeDamage * transform.localScale.x * colliderDistanceDamage,
            new Vector3(boxCollider.bounds.size.x * rangeDamage, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeDetection * transform.localScale.x * colliderDistanceDetection,
            new Vector3(boxCollider.bounds.size.x * rangeDetection, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer() {
        // if player in sight, damage player
        if (PlayerInSightDamage()){
            playerHealth.TakeDamage(damage);
        }
    }
}
