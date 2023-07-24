using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WarpScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;
    private Health myHealth;
    public AIPath aIPath;
    void Start()
    {
        anim = GetComponent<Animator>();
        myHealth = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aIPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if(aIPath.desiredVelocity.x <= -0.01f){
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        cooldownTimer += Time.deltaTime;
        //Attack only when player in sight
        if(PlayerInSight()) {
            if(cooldownTimer >= attackCooldown) {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
    }
     private bool PlayerInSight() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer() {
        // if player in sight, damage player
        if (PlayerInSight()){
            playerHealth.TakeDamage(damage);
        }
    }
    public void Desactivate() {
        Debug.Log("Desactivate");
        //Desactivate enemy when health is 0
        gameObject.SetActive(false);
        aIPath.enabled = false;
        aIPath.canMove = false;
    }
    public void AttackDesativateMovement() {
        aIPath.canMove = false;
    }
    public void AttackActivateMovement() {
        aIPath.canMove = true;
    }
}
