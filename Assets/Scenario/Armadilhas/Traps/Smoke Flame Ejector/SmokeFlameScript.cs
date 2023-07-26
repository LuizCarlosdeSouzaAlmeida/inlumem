using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeFlameScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range1;
    [SerializeField] private float range2;
    [SerializeField] private float range3;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance1;
    [SerializeField] private float colliderDistance2;
    [SerializeField] private float colliderDistance3;
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
        if(cooldownTimer >= attackCooldown) {
            cooldownTimer = 0;
            anim.SetTrigger("attack");
        }
    }
    private bool PlayerInSight1() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range1 * transform.localScale.x * colliderDistance1, 
            new Vector3(boxCollider.bounds.size.x * range1, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private bool PlayerInSight2() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range2 * transform.localScale.x * colliderDistance2, 
            new Vector3(boxCollider.bounds.size.x * range2, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private bool PlayerInSight3() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range3 * transform.localScale.x * colliderDistance3, 
            new Vector3(boxCollider.bounds.size.x * range3, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range1 * transform.localScale.x * colliderDistance1,
            new Vector3(boxCollider.bounds.size.x * range1, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range2 * transform.localScale.x * colliderDistance2,
            new Vector3(boxCollider.bounds.size.x * range2, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range3 * transform.localScale.x * colliderDistance3,
            new Vector3(boxCollider.bounds.size.x * range3, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer1() {
        // if player in sight, damage player
        if (PlayerInSight1()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void DamagePlayer2() {
        // if player in sight, damage player
        if (PlayerInSight2()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void DamagePlayer3() {
        // if player in sight, damage player
        if (PlayerInSight3()){
            playerHealth.TakeDamage(damage);
        }
    }
}
